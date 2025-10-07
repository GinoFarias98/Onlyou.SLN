using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Microsoft.IdentityModel.Tokens;
using Onlyou.Client.Servicios;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Onlyou.Shared.DTOS.Usuario;

namespace Onlyou.Client.Autorizacion
{
    public class ProveedorAutenticacionJWT : AuthenticationStateProvider, ILoginService
    {
        public static readonly string TOKENKEY = "TOKENKEY";
        public static readonly string EXPIRATIONTOKENKEY = "EXPIRATIONTOKENKEY";

        private readonly IJSRuntime js;
        private readonly HttpClient httpClient;

        private AuthenticationState Anonimo => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public ProveedorAutenticacionJWT(IJSRuntime js, HttpClient httpClient)
        {
            this.js = js;
            this.httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await js.ObtenerDeLocalStorage(TOKENKEY);
            if (token == null)
            {
                return Anonimo;
            }

            return ConstruirAuthenticationState(token.ToString());
        }

        private AuthenticationState ConstruirAuthenticationState(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var claims = ParsearClaimsDelJWT(token);
            return new AuthenticationState(new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));

        }

        private IEnumerable<Claim> ParsearClaimsDelJWT(string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenDeserializado = jwtSecurityTokenHandler.ReadJwtToken(token);

            var claims = new List<Claim>();

            // Copiamos todos los claims que no sean roles
            claims.AddRange(tokenDeserializado.Claims.Where(c => c.Type != "role" && c.Type != "roles"));

            // Agregamos roles correctamente como ClaimTypes.Role
            var roleClaims = tokenDeserializado.Claims.Where(c => c.Type == "role" || c.Type == "roles");
            foreach (var rc in roleClaims)
            {
                if (rc.Value.Contains(","))
                {
                    foreach (var r in rc.Value.Split(","))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, r.Trim()));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, rc.Value));
                }
            }

            return claims;
        }


        public async Task Login(UserTokenDTO userTokenDTO)
        {
            await js.GuardarEnLocalStorage(TOKENKEY, userTokenDTO.Token);
            await js.GuardarEnLocalStorage(EXPIRATIONTOKENKEY, userTokenDTO.Expiracion.ToString());
            var authState = ConstruirAuthenticationState(userTokenDTO.Token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));  

        }

        public async Task Logout()
        {
            await js.RemoverDelLocalStorage(TOKENKEY);
            await js.RemoverDelLocalStorage(EXPIRATIONTOKENKEY);
            httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(Anonimo));

        }
    }
}

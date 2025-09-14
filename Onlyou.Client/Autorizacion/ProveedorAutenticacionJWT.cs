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
            return tokenDeserializado.Claims;
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

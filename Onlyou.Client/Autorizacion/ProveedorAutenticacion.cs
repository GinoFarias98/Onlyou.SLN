using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Onlyou.Client.Autorizacion
{
    public class ProveedorAutenticacion : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(1000);
            var anonimo = new ClaimsIdentity();

            var UsuarioAutorizado = new ClaimsIdentity(
                new List<Claim>
                { 
                    new Claim(ClaimTypes.Name, "Gino Farias"),
                    new Claim(ClaimTypes.Role, "admin"),
                    new Claim("DNI", "456465656"),
                    

                },
                authenticationType: "ok");
    
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonimo)));



        }
    }
}

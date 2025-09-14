using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Onlyou.Shared.DTOS.Usuario;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        public UsuarioController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<UserTokenDTO>> CrearUsuario([FromBody] UserInfoDTO modelo)
        {
            var usuario = new IdentityUser { UserName = modelo.Email, Email = modelo.Email };
            var  resultado = await userManager.CreateAsync(usuario, modelo.Password);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(modelo);
            }
            else
            {
                return BadRequest(resultado.Errors.First());
            }
        }

        private async Task<UserTokenDTO> ConstruirToken(UserInfoDTO userInfoDTO)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, userInfoDTO.Email)
                // Codigo para agregar claims custom: new new Claim("customClaim", "valorDelClaim")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]!));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddMonths(1);

            var token = new JwtSecurityToken(

                issuer: null,
                audience: null,
                claims: claims,
                expires: expiracion,
                signingCredentials: credenciales

                );

            return new UserTokenDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiracion
            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserTokenDTO>> LogIn([FromBody] UserInfoDTO userInfoDTO)
        {
            var resultado = await signInManager.PasswordSignInAsync(userInfoDTO.Email, userInfoDTO.Password, isPersistent:false, lockoutOnFailure:false);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(userInfoDTO);
            }
            else
            {
                return BadRequest("LogIn Incorrecto");
            }

        }

    }
}

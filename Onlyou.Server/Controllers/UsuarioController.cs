using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Onlyou.Shared.DTOS.Usuario;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("usuarios")]
[AllowAnonymous]
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
        var usuario = new IdentityUser
        {
            UserName = modelo.Email,
            Email = modelo.Email,
        };

        // Crear el usuario
        var resultado = await userManager.CreateAsync(usuario, modelo.Password);

        if (!resultado.Succeeded)
            return BadRequest(resultado.Errors.First());

        // 🔹 Asignar rol Admin automáticamente
        await userManager.AddToRoleAsync(usuario, "Admin");

        // Construir token incluyendo el rol
        return await ConstruirToken(usuario);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserTokenDTO>> LogIn([FromBody] UserInfoDTO userInfoDTO)
    {
        var resultado = await signInManager.PasswordSignInAsync(
            userInfoDTO.Email, userInfoDTO.Password, isPersistent: false, lockoutOnFailure: false);

        if (!resultado.Succeeded)
            return BadRequest("LogIn Incorrecto");

        var usuario = await userManager.FindByEmailAsync(userInfoDTO.Email);
        return await ConstruirToken(usuario);
    }

    private async Task<UserTokenDTO> ConstruirToken(IdentityUser usuario)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.UserName!),
            new Claim(ClaimTypes.Email, usuario.Email!)
        };

        // 🔹 Agregar roles del usuario
        var roles = await userManager.GetRolesAsync(usuario);
        foreach (var rol in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, rol));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiracion = DateTime.UtcNow.AddMonths(1);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expiracion,
            signingCredentials: creds
        );

        return new UserTokenDTO
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiracion = expiracion
        };
    }
}

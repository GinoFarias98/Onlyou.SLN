using Onlyou.Shared.DTOS.Usuario;

namespace Onlyou.Client.Autorizacion
{
    public interface ILoginService
    {
        Task Login(UserTokenDTO tokenDTO);
        Task Logout();
    }
}

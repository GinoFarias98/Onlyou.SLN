namespace Onlyou.Server.Services
{
    public interface IImagenValidator
    {
        bool ValidarBase64Extencion(string base64, string extencion);
    }
}
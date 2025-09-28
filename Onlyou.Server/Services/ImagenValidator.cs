namespace Onlyou.Server.Services
{
    public class ImagenValidator : IImagenValidator
    {
        public bool ValidarBase64Extencion(string base64, string extencion)
        {
            if (string.IsNullOrEmpty(base64) || string.IsNullOrEmpty(extencion))
            {
                return false;
            }
            byte[] bytes;

            try
            {
                bytes = Convert.FromBase64String(base64);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrio un error al guarda la imagen: {ex.Message}");
                return false;
            }

            return extencion.ToLower() switch
            {
                ".jpg" or ".jpeg" => bytes.Length > 3 && bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[^2] == 0xFF && bytes[^1] == 0xD9,
                ".png" => bytes.Length > 8 && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47,
                ".gif" => bytes.Length > 6 && bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46,
                _ => false

            };
        }
    }
}

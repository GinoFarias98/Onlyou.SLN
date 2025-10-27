namespace Onlyou.Client.Helpers
{
    public class SelectorMultipleModel
    {
        public SelectorMultipleModel(string llave, string valor , string? hex = null)
        {
            Llave = llave;
            Valor = valor;
            Hexadecimal = hex;
        }

        public string Llave { get; set; }
        public string Valor { get; set; }
        public string? Hexadecimal { get; set; } // solo para colores
    }
}

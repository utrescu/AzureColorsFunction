namespace FunctionColorApp.Models
{
    public static class ColorConversion
    {
        public static ColorItem MapToTable(this Color color)
        {
            return new ColorItem(color.Id, color.Traduccio.Idioma)
            {                
                Rgb = color.Rgb,
                Paraula = color.Traduccio.Paraula
            };
        }


        public static Color MapFromTable(this ColorItem colorItem)
        {
            return new Color
            {
                Id = colorItem.PartitionKey,
                Rgb = colorItem.Rgb,
                Traduccio = new Traduccio
                {
                    Idioma = colorItem.RowKey,
                    Paraula = colorItem.Paraula
                }
            };
        }
    }
}

namespace FunctionColorApp.Models
{
    /// <summary>
    /// Classes d'extensió dels objectes que es fan servir per
    /// convertir d'un objecte DTO a un objecte de la base de 
    /// dades (en aquest cas una Table )
    /// </summary>
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

        public static Idiom MapFromTable(this IdiomItem item)
        {
            return new Idiom()
            {
                idioma = item.RowKey,
                quantitat = item.quantitat
            };
        }

    }
}

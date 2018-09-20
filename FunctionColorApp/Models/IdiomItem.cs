using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionColorApp.Models
{
    /// <summary>
    /// Classes Model de la font de dades
    /// en aquest cas són Tables
    /// </summary>
    public class IdiomItem: TableEntity
    {
        public IdiomItem() { }
        public IdiomItem(string Idioma)
        {
            PartitionKey = "Idiomes";
            RowKey = Idioma;
        }

        public int quantitat { get; set; }
    }
}
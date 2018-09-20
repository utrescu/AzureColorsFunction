using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;


namespace FunctionColorApp.Models
{
    /// <summary>
    /// Classes Model de la font de dades
    /// en aquest cas són Tables
    /// </summary>
    public class ColorItem: TableEntity
    {
        public ColorItem() { }
        public ColorItem(string Id, string Idioma)
        {
            PartitionKey = Id;
            RowKey = Idioma;
        }

        public string Rgb { get; set; }
        public string Paraula { get; set;  }
    }
}

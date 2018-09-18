using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;


namespace FunctionColorApp.Models
{
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

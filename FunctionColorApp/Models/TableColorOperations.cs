using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FunctionColorApp.Models
{
    public static class TableColorOperations
    {
        public static void AddOrUpdateColorToTable(this CloudTable table, Color color)
        {
            if (string.IsNullOrEmpty(color.Id))
            {
                color.Id = color.Rgb;
            }
            var item = color.MapToTable();
            var saveOperation = TableOperation.InsertOrReplace(item);
            table.ExecuteAsync(saveOperation);
        }

        public static List<Color> GetAllColorsFromTable(this CloudTable table)
        {
            var colors = new List<Color>();
            var querySegment = table.ExecuteQuerySegmentedAsync(new TableQuery<ColorItem>(), null);
            foreach (ColorItem item in querySegment.Result)
            {
                colors.Add(item.MapFromTable());
            }
            return colors;
        }

        public static Color GetColorFromTable(this CloudTable table, string id, string Idioma)
        {
            var retrieveOperation = TableOperation.Retrieve<ColorItem>(id, Idioma);
            TableResult resultat = table.ExecuteAsync(retrieveOperation).Result;
            if (resultat.Result != null)
            {
                ColorItem item = ((ColorItem)resultat.Result);
                return item.MapFromTable();
            }
            return null;
        }

        public static List<Color> GetColorTranslationFromTable(this CloudTable table, string rgb)
        {
            var colors = new List<Color>();

            TableQuery<ColorItem> query = new TableQuery<ColorItem>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, rgb));
            var resultat = table.ExecuteQuerySegmentedAsync(query, null);
            foreach (ColorItem item in resultat.Result)
            {
                colors.Add(item.MapFromTable());
            }
            return colors;
        }

        public static void DeleteColorFromTable(this CloudTable table, string id, string idioma)
        {
            var item = new ColorItem(id, idioma) { ETag = "*" };
            var deleteOperation = TableOperation.Delete(item);
            table.ExecuteAsync(deleteOperation);
        }
    }
}

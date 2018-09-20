using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Repositori d'accés a la base de dades
/// - No implementa un 'interface' perquè els mètodes han de ser static
/// </summary>
namespace FunctionColorApp.Models
{
    /// <summary>
    /// Operacions amb la base de dades
    /// </summary>
    /// <remarks>
    /// En general sempre es rep la base de dades amb la que treballar
    /// que és CloudTable
    /// <remarks>
    public static class TableColorOperations
    {
        /// <summary>
        /// Afegeix o actualitza un color a la base de dades
        /// <summary>
        /// <remarks>
        /// Color és l'objecte DTO
        /// </remarks>
        public static async Task<int> AddColorToTable(this CloudTable table, Color color)
        {
            if (string.IsNullOrEmpty(color.Id))
            {
                color.Id = color.Rgb;
            }
            var item = color.MapToTable();
            var saveOperation = TableOperation.Insert(item);
            TableResult resultat = await table.ExecuteAsync(saveOperation);
            return resultat.HttpStatusCode;
        }

        public static async Task<int> ModifyColorFromTable(this CloudTable table, Color color)
        {
            if (string.IsNullOrEmpty(color.Id))
            {
                color.Id = color.Rgb;
            }
            var retrieveOperation = TableOperation.Retrieve<ColorItem>(color.Rgb, color.Traduccio.Idioma);
            TableResult resultat = await table.ExecuteAsync(retrieveOperation);
            ColorItem item = ((ColorItem)resultat.Result);
            if (item != null)
            {                
                item = color.MapToTable();
                item.ETag = "*";
                var saveOperation = TableOperation.Replace(item);
                TableResult resultatDesar = await table.ExecuteAsync(saveOperation);
                return resultatDesar.HttpStatusCode;
            }
            return 400;
        }

        /// <summary>
        /// Obtenir tots els colors de la base de dades
        /// </summary>
        /// <returns>
        /// Llista amb tots els colors de la base de dades
        /// </returns>
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

        /// <summary>
        /// Obtenir tots els colors de la base de dades
        /// </summary>
        /// <returns>
        /// Color amb el rgb i l'idioma demanats per paràmetres
        /// </returns>
        public static Color GetColorFromTable(this CloudTable table, string rgb, string Idioma)
        {
            var retrieveOperation = TableOperation.Retrieve<ColorItem>(rgb, Idioma);
            TableResult resultat = table.ExecuteAsync(retrieveOperation).Result;
            if (resultat.Result != null)
            {
                ColorItem item = ((ColorItem)resultat.Result);
                return item.MapFromTable();
            }
            return null;
        }

        /// <summary>
        /// Obtenir totes les traduccions d'un determinat rgb
        /// </summary>
        /// <returns>
        /// Llista de objectes DTO Color amb el rgb especificat
        /// </returns>
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

        /// <summary>
        /// Esborra una determinada traducció de la base de dades
        /// </summary>
        /// <remarks>
        /// S'hi especifica el rgb i l'idioma de la traducció a eliminar
        /// </remarks>
        public static void DeleteColorFromTable(this CloudTable table, string rgb, string idioma)
        {
            var item = new ColorItem(rgb, idioma) { ETag = "*" };
            var deleteOperation = TableOperation.Delete(item);
            table.ExecuteAsync(deleteOperation);
        }

        /// <summary>
        /// Obtenir la quantitat de traduccions d'un determinat idioma
        /// es pot fer de forma més eficient, però és simplement per fer proves
        /// </summary>
        public static async Task<int> GetNumTraductionsFromTable(this CloudTable table, string idioma)
        {
            int comptador = 0;
            TableQuery<ColorItem> query = new TableQuery<ColorItem>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, idioma));
            var resultat = await table.ExecuteQuerySegmentedAsync(query, null);
            foreach (ColorItem c in resultat)
            {
                comptador++;
            }
            return comptador;
        }
    }
}

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
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
    public static class TableStatisticsOperations
    {
        /// <summary>
        /// Afegeix o actualitza un color a la base de dades
        /// <summary>
        /// <remarks>
        /// Color és l'objecte DTO
        /// </remarks>
        public static async Task<int> AddOrUpdateStatisticsToTable(this CloudTable table, IdiomItem item)
        {
            var saveOperation = TableOperation.InsertOrReplace(item);
            TableResult resultat = await table.ExecuteAsync(saveOperation);
            return resultat.HttpStatusCode;
        }


        public static int GetIdiomQuantitatFromTable(this CloudTable table, string idioma) {
            
            var retrieveOperation = TableOperation.Retrieve<IdiomItem>("Idiomes", idioma);
            TableResult resultat = table.ExecuteAsync(retrieveOperation).Result;
            if (resultat.Result != null)
            {
                IdiomItem item = ((IdiomItem)resultat.Result);
                return item.quantitat;
            }
            return 0;
        }
        /// <summary>
        /// Obtenir tots els colors de la base de dades
        /// </summary>
        /// <returns>
        /// Llista amb tots els colors de la base de dades
        /// </returns>
        public static List<Idiom> GetAllStatistics(this CloudTable table)
        {
            var idioms = new List<Idiom>();
            var querySegment = table.ExecuteQuerySegmentedAsync(new TableQuery<IdiomItem>(), null);
            foreach (IdiomItem item in querySegment.Result)
            {
                idioms.Add(item.MapFromTable());
            }
            return idioms;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CsvHelper;
using System.IO;
using System.Data;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace EEP.Controllers
{
    
    
    public class MetadataController : ControllerBase
    {
        
        [HttpGet("[controller]/{movieId:int}")]
        public IActionResult GetMovieById([FromRoute] int movieId)
        {
            string jsonString = Csv_Reader.ReadCSVFile("metadata.csv");
            List<Metadata> metadata = (List<Metadata>)JsonConvert.DeserializeObject(jsonString, (typeof(List<Metadata>)));
            
            
            List<Metadata> matchingMetadata = metadata.FindAll(x => x.MovieId == movieId);
            if(matchingMetadata.Count == 0)
            {
                return NotFound();
            }
            
            List<Metadata> uniqueLanguageEntries = matchingMetadata.GroupBy(x => x.Language).Select(x => x.Last()).ToList();
            List<Metadata> sortedLanguageEntries = uniqueLanguageEntries.OrderBy(x => x.Language).ToList();
            List<Metadata> validEntries = new List<Metadata>();

                foreach(var entry in sortedLanguageEntries)
                {
                bool invalidEntry = entry.GetType().GetProperties()
                .Where(pi => pi.PropertyType == typeof(string))
                .Select(pi => (string)pi.GetValue(entry))
                .Any(value => string.IsNullOrEmpty(value));
                if(!invalidEntry)
                {
                    validEntries.Add(entry);
                }
                
                }
            return Ok(JsonConvert.SerializeObject(validEntries, Formatting.Indented));
            
        }

    [HttpPost("[controller]")]
    public IActionResult Post([FromBody] Metadata metadata)
    {
        List<Metadata> database = new List<Metadata>();

        database.Add(metadata);
        return Ok(JsonConvert.SerializeObject(metadata, Formatting.Indented));
        
    }
    

    }

    

    

    public class Csv_Reader
    {

    
    public static string ReadCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            string jsonString = string.Empty;
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields;
                    bool tableCreated = false;
                    while (tableCreated == false)
                    {
                        colFields = csvReader.ReadFields();
                        foreach (string column in colFields)
                        {
                            DataColumn datecolumn = new DataColumn(column);
                            datecolumn.AllowDBNull = true;
                            csvData.Columns.Add(datecolumn);
                        }
                        tableCreated = true;
                    }
                    while (!csvReader.EndOfData)
                    {
                        csvData.Rows.Add(csvReader.ReadFields());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Error:Parsing CSV";
            }
            //if everything goes well, serialize csv to json 
            jsonString = JsonConvert.SerializeObject(csvData);
 
            return jsonString;
        }
    }
}

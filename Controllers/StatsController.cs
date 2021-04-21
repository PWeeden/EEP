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
    
    
    public class StatsController : ControllerBase
    {
        [HttpGet("movies/stats")]
        public IActionResult GetStats()
        {
            List<StatsDto> stats1 = new List<StatsDto>();

            string statsString = Csv_Reader.ReadCSVFile("stats.csv");
            string metadataString = Csv_Reader.ReadCSVFile("metadata.csv");

            List<Metadata> data = (List<Metadata>)JsonConvert.DeserializeObject(metadataString, (typeof(List<Metadata>)));

            List<Stats> stats = (List<Stats>)JsonConvert.DeserializeObject(statsString, (typeof(List<Stats>)));

            List<StatsDto> sortedStats = new List<StatsDto>();
            
            var orderedMovies = data.GroupBy(x => x.MovieId);

            var orderedStats = stats.GroupBy(x => x.MovieId);

            long watchTimeMilliseconds = 0;

            List<double> TotalWatchTimeMs = new List<double>();

            int count = 0;
            
            foreach(var group in orderedStats)
            {
                watchTimeMilliseconds = 0;

                foreach(var stat in group)
                {
                    
                    watchTimeMilliseconds += stat.WatchDurationMs;
                    count++;
                    
                }
                StatsDto statistic = new StatsDto();

                var movie = data.Find(x => x.MovieId == group.Key); 
                if(movie != null)
                {
                
                statistic.MovieId = group.Key;
                statistic.Title = movie.Title;
                statistic.ReleaseYear = movie.ReleaseYear;
                

                long averageWatchTime = watchTimeMilliseconds/count;

                statistic.AverageWatchDurationS = averageWatchTime;
                statistic.Watches = count;


                TotalWatchTimeMs.Add(averageWatchTime);
                stats1.Add(statistic);
                sortedStats = stats1.OrderByDescending(x => x.Watches).ThenBy(x =>x.ReleaseYear).ToList();
                }
            }

            return Ok(JsonConvert.SerializeObject(sortedStats, Formatting.Indented));
        }
    }
}

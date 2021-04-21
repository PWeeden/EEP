using System;
using Microsoft.VisualBasic.FileIO;
namespace EEP
{
    public class StatsDto
    {
        public int MovieId {get; set;}

        public string Title { get; set; }

        public long AverageWatchDurationS { get; set; }

        public int Watches { get; set; }

        public int ReleaseYear { get; set; }
    }
}

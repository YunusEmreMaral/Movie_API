using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EntityLayer.Concrete
{
        public class Movie
        {
            [Key]
            public int MovieID { get; set; }
            public string? MovieName { get; set; }
            public int MovieRelaseYear { get; set; }
            public string? MovieDirector { get; set; }
            public int MovieDuration { get; set; }
            public bool IsWatched { get; set; }
             public int MovieGenreID { get; set; }

        [JsonIgnore]
        public virtual List<Watched>? Watcheds { get; set; }

        [JsonIgnore]

        public virtual MovieGenre? MovieGenre { get; set; }
    }
}

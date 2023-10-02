using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class MovieGenre
    {
        [Key]
        public int MovieGenreID { get; set; }
        public string? GenreName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Watched
    {
        [Key]
        public int WatchedID { get; set; }
        public int UserID { get; set; }
        public int MovieID { get; set; }

        [JsonIgnore]
        public virtual Movie? Movie { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}

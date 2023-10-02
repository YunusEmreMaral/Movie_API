using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? UserPassword { get; set; }
        public string? UserFullName { get; set; }


        [JsonIgnore]
        public virtual List<Watched>? Watcheds { get; set; } 

        



    }
}

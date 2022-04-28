using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteApi.Models
{
    public class Quote
    {
        
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [Required]
        [StringLength(30)]
        public string Author { get; set; }
        [Required]
        [StringLength(600)]
        public string Description { get; set; }
        [Required]        
        public string Type { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }


    }
}

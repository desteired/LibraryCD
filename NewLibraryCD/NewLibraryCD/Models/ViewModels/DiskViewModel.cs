using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewLibraryCD.Models
{
    public class DiskViewModel
    {
        public int DiskId { get; set; }

        [Required]
        [StringLength(70)]
        public string Singer { get; set; }

        [Required]
        public string NameCD { get; set; }


        [Range(1950, 2017, ErrorMessage = "Год издания должен быть не менее 1950 и не более 2017")]
        public int Year { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Rating { get; set; }

        [Required]
        public int YourRating { get; set; }

        public string DirectionName { get; set; }

        public Photo Photo { get; set; }
    }
}
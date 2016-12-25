namespace NewLibraryCD.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

   
    public partial class Disk
    {
        
        public int DiskId { get; set; }

        
        public string Singer { get; set; }

       
        public string NameCD { get; set; }

        
        public int Year { get; set; }
        
        public double TotalRating { get; set; }
        public string Description { get; set; }
        
        public virtual Direction Direction { get; set; }

        public virtual Photo Photo { get; set; }
    }
}

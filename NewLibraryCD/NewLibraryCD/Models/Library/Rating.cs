namespace NewLibraryCD.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Rating
    {
        
        public int RatingId { get; set; }

        public virtual Disk Disk { get; set; }

        public virtual User User { get; set; }

        public int UserRating { get; set; }
    }
}

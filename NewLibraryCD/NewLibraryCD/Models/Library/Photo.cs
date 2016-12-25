using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewLibraryCD.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public byte[] ImageData { get; set; }
        public string Mime { get; set; }
    }


}
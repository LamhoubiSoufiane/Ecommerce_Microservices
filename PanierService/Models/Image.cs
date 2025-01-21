using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanierService.Models
{
    public class Image
    {
        public int id { get; set; }

        public string url { get; set; }

        public Boolean imagePrincipale { get; set; }
    }
}

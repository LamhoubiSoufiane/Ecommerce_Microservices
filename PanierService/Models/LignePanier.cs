using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PanierService.Models
{
    public class LignePanier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int quantite_ligne { get; set; }
        public double prixdevente { get; set; }
        public int id_produit { get; set; }
        public Produit produit { get; set; }
        public int id_achat { get; set; }
        public Achat achat { get; set; }
    }
}

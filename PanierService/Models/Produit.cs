using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PanierService.Models
{
    public class Produit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string nomProduit { get; set; }
        public string description { get; set; }
        public double prixProduit { get; set; }
        public int qteStock { get; set; }
        public DateTime dateAjout { get; set; }
        public int categorieId { get; set; }
        public Categorie categorie { get; set; }
        public ICollection<LignePanier> lignesPanier { get; set; } = new List<LignePanier>();

        public ICollection<Image> images { get; set; } = new List<Image>();
        public string imageUrl { get; set; }
    }
}

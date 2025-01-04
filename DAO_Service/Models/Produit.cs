using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DAO_Service.Models
{
    [Table("Produits")]
    public class Produit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(30)]
        public string nomProduit { get; set; }
        public string description { get; set; }
        [Required]
        public double prixProduit { get; set; }
        [Required]
        public int qteStock { get; set; }
        [DataType(DataType.Date)]
        public DateTime dateAjout { get; set; }
        [ForeignKey("Categories")]
        public int categorieId { get; set; }
        public virtual Categorie categorie { get; set; }
        public ICollection<LignePanier> lignesPanier { get; set; } = new List<LignePanier>();

        public ICollection<Image> images { get; set; } = new List<Image>();
        public string imageUrl { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProduitService
{
    [Table("Produits")]
    public class Produit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string nomProduit { get; set; }

        public string description { get; set; }

        [Required]
        public double prixProduit { get; set; }

        [Required]
        public int qteStock { get; set; }

        public DateTime dateAjout { get; set; } = DateTime.Now;

        public string imageUrl { get; set; }

        public int CategorieId { get; set; }

        //public virtual Categorie Categorie { get; set; }
    }
}

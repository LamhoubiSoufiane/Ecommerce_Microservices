using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAO_Service.Models
{
    [Table("LignesPanier")]
    public class LignePanier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int quantite_ligne { get; set; }
        public double prixdevente { get; set; }
        [ForeignKey("Produits")]
        public int id_produit { get; set; }
        public virtual Produit produit { get; set; }
        [ForeignKey("Achats")]
        public int id_achat { get; set; }
        public virtual Achat achat { get; set; }
    }
}

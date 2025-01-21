using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CategorieService.Models
{
    [Table("Categories")]
    public class Categorie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [StringLength(30)]
        public string nomCategorie { get; set; }
        [StringLength(100)]
        public string imageUrl { get; set; }

        public virtual ICollection<Produit> produits { get; set; } = new List<Produit>();
    }
}

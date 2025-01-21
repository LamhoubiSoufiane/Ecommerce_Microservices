using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchatService.Models
{
    public class LignePanier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int quantite_ligne { get; set; }
        public double prixdevente { get; set; }
        public int id_produit { get; set; }
        public int id_achat { get; set; }
        [ForeignKey("id_achat")]
        public Achat achat { get; set; }
    }
}

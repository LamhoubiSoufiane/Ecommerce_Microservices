using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PanierService.Models
{
    public class Achat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public DateTime DateAchat { get; set; }
        public string user_Id { get; set; }
        public IdentityUser utilisateur { get; set; }
        public ICollection<LignePanier> lignesPanier { get; set; } = new List<LignePanier>();
        public string Status { get; set; }
    }
}

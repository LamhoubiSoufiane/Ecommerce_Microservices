using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AchatService.Models
{
    public class Achat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public DateTime DateAchat { get; set; }
        public string user_Id { get; set; }
        public ICollection<LignePanier> lignesPanier { get; set; } = new List<LignePanier>();
        public string Status { get; set; }
    }
}

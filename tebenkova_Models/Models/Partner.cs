using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tebenkova_Models.Models
{
    [Table("Partners", Schema = "app")]
    public class Partner
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("PartnerType")]
        public int TypeId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public string LegalAddress { get; set; }
        public string INN { get; set; }
        public string DirectorName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string LogoPath { get; set; }
        public int Rating { get; set; }

        public PartnerType PartnerType { get; set; }
        public ICollection<SaleHistory> SalesHistories { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tebenkova_Models.Models
{
    [Table("StockIssues", Schema = "app")]
    public class StockIssue
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата списания")]
        public DateTime IssueDate { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Количество")]
        public decimal Quantity { get; set; }

        [MaxLength(200)]
        [Display(Name = "Причина")]
        public string Reason { get; set; }

        [MaxLength(50)]
        [Display(Name = "Номер документа")]
        public string DocumentNumber { get; set; }

        [MaxLength(500)]
        [Display(Name = "Примечание")]
        public string Notes { get; set; }

        // Навигационное свойство
        public virtual Product Product { get; set; }
    }
}

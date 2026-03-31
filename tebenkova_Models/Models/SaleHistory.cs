using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tebenkova_Models.Models
{
    [Table("SalesHistories", Schema = "app")]
    public class SaleHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Partner")]
        public int PartnerId { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата продажи")]
        public DateTime SaleDate { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть больше 0")]
        [Display(Name = "Количество")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
        [Display(Name = "Цена продажи")]
        public decimal SalePrice { get; set; }

        // Вычисляемое поле - общая сумма
        [NotMapped]
        [Display(Name = "Сумма")]
        public decimal TotalAmount => Quantity * SalePrice;

        // Навигационные свойства
        public virtual Partner Partner { get; set; }
        public virtual Product Product { get; set; }
    }
}

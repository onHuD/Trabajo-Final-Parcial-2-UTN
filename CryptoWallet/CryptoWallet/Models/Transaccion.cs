using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoWallet.Models
{
    public class Transaccion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CryptoCode { get; set; }

        [Required]
        public string Action { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,8)")]
        public decimal CryptoAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Money { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
    }
}
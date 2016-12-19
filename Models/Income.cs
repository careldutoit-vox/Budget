using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EntityModels
{
    public class Income : EntityBase
    {
        [Required]
        public double Salary { get; set; }
        
        [Required]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }
    }
}

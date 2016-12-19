using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModels
{
    public class Expenses : EntityBase
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public double Amount { get; set; }
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }
    }

    public enum Category{
        House,
        Morgage,
        Insurance,

    }
}

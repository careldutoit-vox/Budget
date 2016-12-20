using EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Budget.Models
{
    public class ReportViewModel : EntityBase
    {
        public List<Expenses> Expenses { get; set; }
        public Income Income { get; set; }
    }
}
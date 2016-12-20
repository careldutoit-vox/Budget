namespace Budget.Controllers
{
    using EntityModels;
    using Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.AspNet.Identity;
    using Budget.Models;
    using System.Threading.Tasks;

    public class ReportsController : Controller
    {
        private DocumentDBRepository<ReportViewModel> documentDb;
        public ReportsController()
        {
           
        }
        private static string _userId;
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                _userId = requestContext.HttpContext.User.Identity.GetUserId();
                
            }

        }
        //
        // GET: /Reports/
        public async Task<ActionResult> Index()
        {
            var incomeDb = new DocumentDBRepository<Income>("Income", _userId);
            var expensesDb = new DocumentDBRepository<Expenses>("Expenses", _userId);
            var report = new ReportViewModel();
            report.Income = await incomeDb.GetUserItemAsync();
            report.Expenses = await expensesDb.GetItemsAsync(func => func.Amount > 0);
            return View(report);
        }
	}
}
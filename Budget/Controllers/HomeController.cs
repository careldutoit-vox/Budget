
using EntityModels;
using Google.Apis.Services;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;

namespace Budget.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
      
        //public async Task<ActionResult> IndexAsync(CancellationToken cancellationToken)
        //{
        //    var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
        //        AuthorizeAsync(cancellationToken);

        //    if (result.Credential != null)
        //    {
        //        //var service = new DriveService(new BaseClientService.Initializer
        //        //{
        //        //    HttpClientInitializer = result.Credential,
        //        //    ApplicationName = "ASP.NET MVC Sample"
        //        //});
        //        var service = new GmailService(new BaseClientService.Initializer()
        //        {
        //            HttpClientInitializer = result.Credential,
        //            ApplicationName = "Budget",
        //        });
        //        //var list = await service.Files.List().ExecuteAsync();
        //        //ViewBag.Message = "FILE COUNT IS: " + list.Items.Count();
                
        //        UsersResource.LabelsResource.ListRequest request = service.Users.Labels.List("fnb");

        //        // List labels.
        //        IList<Label> labels = request.Execute().Labels;
        //        Console.WriteLine("Labels:");
        //        if (labels != null && labels.Count > 0)
        //        {
        //            foreach (var labelItem in labels)
        //            {
        //                Console.WriteLine("{0}", labelItem.Name);
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("No labels found.");
        //        }
        //        Console.Read();
        //        return View();
        //    }
        //    else
        //    {
        //        return new RedirectResult(result.RedirectUri);
        //    }
        //}

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
namespace Budget.Controllers
{
    using EntityModels;
    using Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.AspNet.Identity;

    [Authorize]
    public class ExpensesController : Controller
    {
        private DocumentDBRepository<Expenses> documentDb;
        public ExpensesController()
        {
           
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = requestContext.HttpContext.User.Identity.GetUserId();
                documentDb = new DocumentDBRepository<Expenses>(this.GetType().Name.Replace("Controller", ""), userId);
            }

        }
         public async Task<ActionResult> GetFromMail()
        {
            //var mailrepo = new MailRepository();
            //mailrepo.ReadAllMails("fnb");
            string _client_id = "157077024985-hdi3jkdmppbl40l3qr6ma8h22ggve0hp.apps.googleusercontent.com";
            string _client_secret = "QXWDdsOQDl7gb7McGUqWCuya";

            var mails = new CustomMailService().GetMails(_client_id, _client_secret);
            //string userId = HttpContext.User.Identity.GetUserId();
            //var EmaildocumentDb = new DocumentDBRepository<EmailModel>("Mails", userId);
            foreach (var item in mails)
            {
                await documentDb.CreateItemAsync(item);    
            }
            
            
            return RedirectToAction("Index");
            return null; ;
        }
        
        //
        // GET: /Income/
        public async Task<ActionResult> Index()
        {
            var items = await documentDb.GetItemsAsync(func => func.Amount > 0);
            return View(items);
        }

#pragma warning disable 1998
        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            return View();
        }
#pragma warning restore 1998

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Expenses item)
        {
            if (ModelState.IsValid)
            {
                await documentDb.CreateItemAsync(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Name,Description,Completed")] Expenses item)
        {
            if (ModelState.IsValid)
            {
                await documentDb.UpdateItemAsync(item.Id, item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var item = await documentDb.GetItemAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var item = await documentDb.GetItemAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind(Include = "Id")] string id)
        {
            await documentDb.DeleteItemAsync(id);
            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            var item = await documentDb.GetItemAsync(id);
            return View(item);
        }
    }
}

using ZipNTuck.UI.MVC.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace ZipNTuck.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //[Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }

            string emailBody = $"You have recieved an email from {cvm.Name} with a subject {cvm.Subject}. Please respond to {cvm.Email} with your response to the following message: <br /> <br />{cvm.Message}";

            MailMessage msg = new MailMessage(
                //From
                "no-reply@christinazuniga.com",
                //To (Where the message is sent)
                "czuniga3177@gmail.com",
                //Subject
                "Email from christinazuniga.com",
                //Body
                emailBody
                );

            //3) (Optional) Customize the MailMessage
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.High;

            //4) Create the SmtpClient
            SmtpClient client = new SmtpClient("mail.christinazuniga.com");//has to be your domain
            client.Credentials = new NetworkCredential("no-reply@christinazuniga.com", "Madrid!");

            //5) Attempt to send the emali
            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {

                ViewBag.ErrorMessage = $"Sorry, something went wrong. Error message {ex.Message}<br />{ex.StackTrace}";
                return View(cvm);
            }

            return View("EmailConfirmation", cvm);
        }
    }
}

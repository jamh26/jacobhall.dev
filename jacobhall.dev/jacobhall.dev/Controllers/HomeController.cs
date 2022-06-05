using jacobhall.dev.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace jacobhall.dev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            var contactViewModel = new ContactViewModel();
            contactViewModel.ContactFormEmail = new ContactFormModel();

            var contactOptions = new List<ContactFormOptionsModel>()
            {
                new ContactFormOptionsModel { Id = "1", Category = "Category 1"},
                new ContactFormOptionsModel { Id = "2", Category = "Category 2"}
            };

            contactViewModel.Categories = new SelectList(contactOptions, "Id", "Category");

            return View("Contact", contactViewModel);
        }

        [HttpPost]
        public IActionResult Contact(ContactFormModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Info - JacobHall.dev", _config.GetValue<string>("Smtp:FromAddress")));
                    message.To.Add(new MailboxAddress($"{vm.Name}", $"{vm.Email}"));
                    message.Subject = $"Contact from JacobHall.dev - Category: {vm.Category} - Email: {vm.Email}";

                    message.Body = new TextPart("plain")
                    {
                        Text = @$"{vm.Message}"
                    };

                    using (var client = new SmtpClient())
                    {
                        client.Connect(_config.GetValue<string>("Smtp:Server"), _config.GetValue<int>("Smtp:Port"), SecureSocketOptions.StartTls);

                        // Note: only needed if the SMTP server requires authentication
                        client.Authenticate(_config.GetValue<string>("Smtp:UserName"), _config.GetValue<string>("Smtp:Password"));

                        client.Send(message);
                        client.Disconnect(true);
                    }

                    ModelState.Clear();
                    ViewBag.Message = "Thank you for Contacting us ";
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    ViewBag.Message = $" Sorry we are facing Problem here {ex.Message}";
                }
            }
            return View("ThankYou");
        }

        public IActionResult ThankYou()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
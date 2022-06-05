using Microsoft.AspNetCore.Mvc.Rendering;

namespace jacobhall.dev.Models
{
    public class ContactViewModel
    {
        public ContactFormModel ContactFormEmail { get; set; }
        public SelectList Categories { get; set; }
    }
}
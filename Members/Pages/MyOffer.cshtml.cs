using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Members.Pages
{
    public class MyOfferModel : PageModel
    {

        [BindProperty]
        public string OfferTitle { get; set; }

        [BindProperty]
        public string ShortDescription { get; set; }
        [BindProperty]
        public string Category { get; set; }
        [BindProperty]
        public string PersonName { get; set; }
        [BindProperty]
        public string ContactPerson { get; set; }
        [BindProperty]
        public string Email { get; set; }



        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public string YoutubeId { get; set; }
        [BindProperty]
        public string Address { get; set; }
        [BindProperty]
        public string Phone { get; set; }
        [BindProperty]
        public string Web { get; set; }
        [BindProperty]
        public string Instagram { get; set; }
        [BindProperty]
        public string Facebook { get; set; }
        [BindProperty]
        public string LinkedIn { get; set; }


        public async Task OnGetAsync()
        {
        }
    }
}

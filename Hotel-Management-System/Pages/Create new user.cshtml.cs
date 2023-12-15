using Microsoft.AspNetCore.Mvc;
using Hotel_Management_System.Pages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Hotel_Management_System.Pages.Models;

namespace Hotel_Management_System.Pages
{
    public class Create_new_userModel : PageModel
    {
        [BindProperty]
        public Guest new_guest { get; set; } = new Guest();
        private readonly DB DB;

        public Create_new_userModel(DB db)
        {
            DB = db;
        }
        public void OnGet()
        {
            // nothing to do here
        }
        public IActionResult OnPost() {
            DB.AddGuest(new_guest);
            return RedirectToPage("/index");
        }
    }
}

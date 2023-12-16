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
  if (string.IsNullOrEmpty(new_guest.first_name) && string.IsNullOrEmpty(new_guest.last_name) &&
      string.IsNullOrEmpty(new_guest.city_code) && string.IsNullOrEmpty(new_guest.country_code) &&
      string.IsNullOrEmpty(new_guest.street_number) && string.IsNullOrEmpty(new_guest.email))
  {

      return RedirectToPage("/Error"); ;
  }
  else
  {
      return RedirectToPage("/index");
  }
            
        }
    }
}

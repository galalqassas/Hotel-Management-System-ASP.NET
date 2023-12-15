using Hotel_Management_System.Pages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel_Management_System.Pages
{
    public class UpdateModel : PageModel
    {
        [BindProperty]
        public int guest_id { get; set; }
        [BindProperty] 
        public Guest new_guest { get; set; } = new Guest();
        private readonly DB DB;
        public UpdateModel(DB db)
        {
            this.DB = db;
        }

        public void OnGet(int id)
        {
            // query the database for the guest with the given id
            Guest guest = DB.GetGuestById(id);
            // assign the guest properties to the model properties
            guest_id = guest.guest_id;
            new_guest.first_name = guest.first_name;
            new_guest.last_name = guest.last_name;
            new_guest.city_code = guest.city_code;
            new_guest.country_code = guest.country_code;
            new_guest.street_number = guest.street_number;
            new_guest.email = guest.email;
            new_guest.ssn = guest.ssn;
        }
        public IActionResult OnPost(int id, Guest new_guest)
        {
            new_guest.guest_id = id;
            DB.UpdateGuestInfo(new_guest);
            return RedirectToPage("/Update");
        }
    }
    
}


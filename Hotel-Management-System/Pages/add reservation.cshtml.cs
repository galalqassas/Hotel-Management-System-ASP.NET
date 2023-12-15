using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Hotel_Management_System.Pages.Models;

namespace Hotel_Management_System.Pages
{
    public class add_reservationModel : PageModel
    {
        private readonly DB DB;

        [BindProperty]
        public Reservation new_reservation { get; set; } = new Reservation();
        public string room_type{ get; set; }
        public add_reservationModel(DB db)
        {
              this.DB = db;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            DB.AddReservation(new_reservation);
            return RedirectToPage("/index");
        }
    }
}
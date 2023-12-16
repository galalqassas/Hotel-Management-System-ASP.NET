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
        [BindProperty]
        public string room_type { get; set; }
        [BindProperty]
        public string has_ac { get; set; }
        public add_reservationModel(DB db)
        {
            this.DB = db;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            DB.AddReservation(new_reservation, room_type, has_ac);
            if (new_reservation.check_in_date == null && new_reservation.num_guests <= 0)
            {

                return RedirectToPage("/Error");
            }
            else
            {
                return RedirectToPage("/index");
            }
        }
    }
}
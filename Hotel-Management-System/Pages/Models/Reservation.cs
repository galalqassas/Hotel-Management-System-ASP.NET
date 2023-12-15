using Microsoft.AspNetCore.Components.Forms;

namespace Hotel_Management_System.Pages.Models
{
    public class Reservation
    {
        public int reservation_id { get; set; }
        public string is_cancelled { get; set; }
        public int guest_arrived { get; set; }
        public string check_in_date { get; set; }
        public int num_guests { get; set; }
    }
}

namespace Hotel_Management_System.Pages.Models
{
    public class Guest
    {
        public int guest_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string city_code { get; set; }
        public string country_code { get; set; }
        public string street_number { get; set; }
        public string email { get; set; }
        public int not_showingup;
        public int ssn;
    }
}

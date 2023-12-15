using Hotel_Management_System.Pages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel_Management_System.Pages
{
    public class AddRatingModel : PageModel
    {
        private readonly DB DB;
        [BindProperty]
        public Rating new_rating{ get; set; } = new Rating();
        public AddRatingModel(DB db)
        {
            DB = db;
        }
        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            DB.AddRating(new_rating);
            return RedirectToPage("/index");
        }
    }
}

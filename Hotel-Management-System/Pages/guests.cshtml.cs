using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Hotel_Management_System.Pages;
using Hotel_Management_System.Pages.Models;
using System.Data;
using Microsoft.Extensions.Logging;

namespace Hotel_Management_System.Pages
{
    public class guestsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DB db;
        public DataTable dt { get; set; }
        public guestsModel(ILogger<IndexModel> logger, DB database)
        {
            _logger = logger;
            this.db = database;
        }



        public void OnGet()
        {
            dt = db.ReadTable("Guest");
        }
    }
}

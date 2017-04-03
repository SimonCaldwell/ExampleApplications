using PROACTIS.ExampleApplications.CustomTabMVC.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PROACTIS.ExampleApplications.CustomTabMVC.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index(string token, string title, string url)
        {
            // Token is in the format databaseTitle@SessionID
            var parts = token.Split('@');
            if (parts.Length != 2) throw new System.Exception("Token should be in the format databaseTitle@SessionID");
            var databaseTitle = parts[0];
            var sessionID = parts[1];

            // Get the information for this session from the database
            using (var db = new Database(databaseTitle))
            {
                var sessionDetails = await db.GetSessionDetailsFromTokenAsync(token);

                return View(new LandingDetails()
                {
                    Title = title,
                    Token = token,
                    URL = url,
                    DatabaseTitle = databaseTitle,
                    SessionID = sessionID,
                    SessionDetails = sessionDetails 
                });
            }
        }

    }
}
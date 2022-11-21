using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TieRenTournament.Models;
using Newtonsoft.Json;

namespace TieRenTournament.Pages.Events
{
    public class ResultsModel : PageModel
    {
       
        public List<Competitor> Results { get; set; }
        public void OnGet()
        {
            Results = JsonConvert.DeserializeObject<List<Competitor>>(TempData["compList"].ToString());
        }
    }
}

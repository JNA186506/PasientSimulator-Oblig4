using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace CaseSetup.Pages
{
    public class IndexModel : PageModel
    {
        public Context Context { get; set; }
        public CaseService CaseService { get; set; }
        public List<Case> Cases { get; set; }
        public IndexModel() {
            Context = new Context();
            CaseService = new CaseService(Context);
            Cases = CaseService.GetAllCases();
        }
    }
}

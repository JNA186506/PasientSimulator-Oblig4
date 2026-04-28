using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace CaseSetup.Pages
{
    public class IndexModel : PageModel
    {
        public Context context { get; set; }
        public CaseService caseService { get; set; }
        public List<Case> cases { get; set; }
        public IndexModel() {
            context = new Context();
            caseService = new CaseService(context);
            cases = caseService.GetAllCases();
        }
    }
}

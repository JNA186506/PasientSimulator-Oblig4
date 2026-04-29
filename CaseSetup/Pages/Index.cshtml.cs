using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace CaseSetup.Pages
{
    public class IndexModel : PageModel
    {
        private readonly CaseService _caseService;
        public List<Case> Cases { get; set; } = new();

        public IndexModel(CaseService caseService) {
            _caseService = caseService;
        }

        public async Task OnGetAsync()
        {
            Cases = await _caseService.GetAllCases();
        }
    }
}

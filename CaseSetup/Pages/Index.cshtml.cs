using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace CaseSetup.Pages
{
    public class IndexModel : PageModel
    {
        private readonly CaseService _caseService;
        private readonly PatientService _patientService;
        private readonly UserService _userService;
        private readonly MedicationService _medicationService;
        public List<Case> Cases { get; set; } = new();

        public IndexModel(CaseService caseService, PatientService patientService, UserService userService, MedicationService medicationService) {
            _caseService = caseService;
            _patientService = patientService;
            _userService = userService;
            _medicationService = medicationService;
        }

        public async Task OnGetAsync()
        {
            Cases = await _caseService.GetAllCases();
        }
    }
}

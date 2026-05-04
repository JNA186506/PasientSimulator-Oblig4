using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace CaseSetup.Pages;

public class IndexModel : PageModel
{
    private readonly CaseService _caseService;
    private readonly MedicationService _medicationService;
    private readonly PatientService _patientService;
    private readonly UserService _userService;

    public IndexModel(CaseService caseService, PatientService patientService, UserService userService,
        MedicationService medicationService)
    {
        _caseService = caseService;
        _patientService = patientService;
        _userService = userService;
        _medicationService = medicationService;
    }

    public List<Case> Cases { get; set; } = new();

    public async Task OnGetAsync()
    {
        Cases = await _caseService.GetAllCases();
    }
    public IActionResult OnPostSendToViewScenario(String scenarioId)
    {
        int.TryParse(scenarioId, out var idInt);
        return RedirectToPage("/ViewScenario", new { idInt = idInt });
    }
    public IActionResult OnPostSendToChangeScenario(String scenarioId)
    {
        int.TryParse(scenarioId, out var idInt);
        return RedirectToPage("/ChangeScenario", new { idInt = idInt });
    }
}
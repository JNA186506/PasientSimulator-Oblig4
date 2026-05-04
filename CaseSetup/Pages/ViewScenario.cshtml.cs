using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace CaseSetup.Pages;

public class ViewScenarioModel : PageModel
{
    private CaseService _caseService { get; set; }
    private PatientService _patientService { get; set; }
    [BindProperty]
    public Case _case { get; set; }
    [BindProperty]
    public Patient _patient { get; set; }
    public ViewScenarioModel(CaseService caseService, PatientService patientService) {
        _caseService = caseService;
        _patientService = patientService;
    }
    public async Task OnGet(int idInt)
    {
        _case = await _caseService.GetCaseById(idInt);
        _patient = _case.CasePatient;
    }
}
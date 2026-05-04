using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;
using PasientSimulator.lib.Services.Interfaces;

namespace CaseSetup.Pages
{
    public class ChangeScenarioModel : PageModel
    {
        public ChangeScenarioModel(PatientService patientService, UserService userService, CaseService caseService, MedicationService medicationService)
        {
            PatientService = patientService;
            UserService = userService;
            CaseService = caseService;
            MedicationService = medicationService;
        }
        public List<Medication> Allergies { get; set; }
        public List<Illness> Diagnoses { get; set; }
        public List<User> Students { get; set; }
        public List<Goal> Goals { get; set; }
        public PatientService PatientService { get; set; }
        public UserService UserService { get; set; }
        public CaseService CaseService { get; set; }
        public MedicationService MedicationService { get; set; }
        [BindProperty] public List<Illness> patientDiagnoses { get; set; }

        [BindProperty] public List<Medication> patientAllergies { get; set; }

        [BindProperty] public User Student { get; set; }

        [BindProperty] public List<Goal> CaseGoals { get; set; }
        [BindProperty] public Case _case { get; set; }
        public async Task OnGetAsync(int scenarioId)
        {
            Allergies = await PatientService.GetAllAllergies();
            Diagnoses = await PatientService.GetAllDiagnoses();
            Students = await UserService.GetAllStudents();
            Goals = await CaseService.GetAllGoals();

            _case = await CaseService.GetCaseById(scenarioId);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace CaseSetup.Pages
{
    public class AddScenarioModel : PageModel
    {
        public Context Context { get; set; }
        public PatientService PatientService { get; set; }
        public UserService UserService { get; set; }
        public CaseService CaseService { get; set; }
        public List<Medication> Allergies { get; set; }
        public List<Illness> Diagnosis { get; set; }
        public List<User> Students { get; set; }
        public List<Goal> Goals { get; set; }
        public AddScenarioModel() {
            Context = new Context();
            PatientService = new PatientService(Context);
            UserService = new UserService(Context);
            CaseService = new CaseService(Context);
            Allergies = PatientService.GetAllAllergies();
            Diagnosis = PatientService.GetAllDiagnosis();
            Students = UserService.GetAllStudents();
            Goals = CaseService.GetAllGoals();
        }
    }
}

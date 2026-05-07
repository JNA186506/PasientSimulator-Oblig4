using CaseSetup.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace CaseSetup.Pages
{
    public class ChangeScenarioModel : PageModel {
        private IHubContext<CaseHub> _hubContext; 
        public ChangeScenarioModel(PatientService patientService, UserService userService, CaseService caseService, MedicationService medicationService, IHubContext<CaseHub> hubContext)
        {
            PatientService = patientService;
            UserService = userService;
            CaseService = caseService;
            MedicationService = medicationService;
            _hubContext = hubContext;
        }
        public List<Medication> Allergies { get; set; }
        public List<Illness> Diagnoses { get; set; }
        public List<User> Students { get; set; }
        public List<Goal> Goals { get; set; }
        public PatientService PatientService { get; set; }
        public UserService UserService { get; set; }
        public CaseService CaseService { get; set; }
        public MedicationService MedicationService { get; set; }
        public Case Case { get; set; }
        [BindProperty]
        public int StudentId { get; set; }
        [BindProperty]
        public String Status { get; set; }
        [BindProperty]
        public String PatientName { get; set; }
        [BindProperty]
        public int PatientWeight { get; set; }
        [BindProperty]
        public int PatientAge { get; set; }
        [BindProperty]
        public int Sex { get; set; }
        [BindProperty]
        public int Heartrate { get; set; }
        [BindProperty]
        public int Systolic { get; set; }
        [BindProperty]
        public int Diastolic { get; set; }
        [BindProperty]
        public int RespiratoryRate { get; set; }
        [BindProperty]
        public double OxygenSaturation { get; set; }
        [BindProperty]
        public double Temperature { get; set; }
        public async Task OnGetAsync(int idInt)
        {
            Allergies = await PatientService.GetAllAllergies();
            Diagnoses = await PatientService.GetAllDiagnoses();
            Students = await UserService.GetAllStudents();
            Goals = await CaseService.GetAllGoals();

            Case = await CaseService.GetCaseById(idInt);
        }
        public async Task<IActionResult> OnPostUpdateCase()
        {
            Allergies = await PatientService.GetAllAllergies();
            Diagnoses = await PatientService.GetAllDiagnoses();
            Students = await UserService.GetAllStudents();
            Goals = await CaseService.GetAllGoals();

            int.TryParse(Request.Form["caseId"], out int id);
            Case = await CaseService.GetCaseById(id);
            
            if (Case == null) return RedirectToPage("/Index");

            User? Student = await UserService.FindStudent(StudentId);
            if (Student != null) Case.Student = Student;

            List<Goal> CaseGoals = new List<Goal>();
            var GoalsStr = Request.Form["goals"].ToString().Split(',').ToList();
            foreach (var s in GoalsStr)
                if (int.TryParse(s, out var num))
                    CaseGoals.Add(await CaseService.FindGoal(num));
            Case.Goals = CaseGoals;
            await CaseService.UpdateCase(Case);
            await _hubContext.Clients.All.SendAsync("CaseUpdated", Case.CaseId);
            return RedirectToPage(new { idInt = id });
        }
        public async Task<IActionResult> OnPostUpdatePatient()
        {
            Allergies = await PatientService.GetAllAllergies();
            Diagnoses = await PatientService.GetAllDiagnoses();
            Students = await UserService.GetAllStudents();
            Goals = await CaseService.GetAllGoals();

            int.TryParse(Request.Form["caseId"], out int id);
            Case = await CaseService.GetCaseById(id);
            if (Status != null) Case.CasePatient.Status = (Patient.StatusEnum)Enum.Parse(typeof(Patient.StatusEnum), Status);
            if (PatientName != null) Case.CasePatient.PatientName = PatientName;
            if (PatientWeight > 0) Case.CasePatient.Weight = PatientWeight;
            if (PatientAge > 0) Case.CasePatient.Age = PatientAge;
            if (Sex > 0 && Sex <= 4) Case.CasePatient.Sex = (Patient.SexEnum)Sex;
            if (Heartrate > 0) Case.CasePatient.Heartrate = Heartrate;
            if (Case.CasePatient.BloodPressure == null) Case.CasePatient.BloodPressure = new BloodPressure() { Systolic = 0, Diastolic = 0 };
            if (Systolic > 0) Case.CasePatient.BloodPressure.Systolic = Systolic;
            if (Diastolic > 0) Case.CasePatient.BloodPressure.Diastolic = Diastolic;
            if (RespiratoryRate > 0) Case.CasePatient.RespiratoryRate = RespiratoryRate;
            if (OxygenSaturation > 0) Case.CasePatient.OxygenSaturation = OxygenSaturation;
            Case.CasePatient.Temperature = Temperature;
            List<Illness> patientDiagnoses = new List<Illness>();
            List<Medication> patientAllergies = new List<Medication>();
            var DiagnosesStr = Request.Form["diagnoses"].ToString().Split(',').ToList();
            foreach (var s in DiagnosesStr)
                if (int.TryParse(s, out var num))
                {
                    var illness = Diagnoses.FirstOrDefault(d => d.IllnessId == num);
                    if (illness != null) patientDiagnoses.Add(Diagnoses.First(d => d.IllnessId == num));
                }
            Case.CasePatient.Diagnoses = patientDiagnoses;
            var AllergiesStr = Request.Form["allergies"].ToString().Split(',').ToList();
            foreach (var s in AllergiesStr)
                if (int.TryParse(s, out var num))
                {
                    var medication = Allergies.FirstOrDefault(d => d.MedicationId == num);
                    if (medication != null) patientAllergies.Add(Allergies.First(a => a.MedicationId == num));
                }
            Case.CasePatient.Allergies = patientAllergies;
            await PatientService.UpdatePatient(Case.CasePatient);
            await _hubContext.Clients.All.SendAsync("CaseUpdated", Case.CaseId);
            return RedirectToPage(new { idInt = id });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;
using System.ComponentModel;
using System.Linq;

namespace CaseSetup.Pages
{
    public class AddScenarioModel : PageModel
    {
        public Context Context { get; set; }
        public PatientService PatientService { get; set; }
        public UserService UserService { get; set; }
        public CaseService CaseService { get; set; }
        public MedicationService MedicationService { get; set; }
        public List<Medication> Allergies { get; set; }
        public List<Illness> Diagnoses { get; set; }
        public List<User> Students { get; set; }
        public List<Goal> Goals { get; set; }
        [BindProperty]
        public String PatientName { get; set; }
        [BindProperty]
        public int PatientWeight { get; set; }
        [BindProperty]
        public int PatientAge { get; set; }
        [BindProperty]
        public int PatientSex { get; set; }
        [BindProperty]
        public int SelectStatus { get; set; }
        [BindProperty]
        public int Heartrate { get; set; }
        [BindProperty]
        public int RespiratoryRate { get; set; }
        [BindProperty]
        public int Temperature { get; set; }
        [BindProperty]
        public List<Illness> PatientDiagnoses { get; set; }
        [BindProperty]
        public List<Medication> PatientAllergies { get; set; }
        [BindProperty]
        public User Student { get; set; }
        [BindProperty]
        public List<Goal> CaseGoals { get; set; }
        public AddScenarioModel() {
            Context = new Context();
            PatientService = new PatientService(Context);
            UserService = new UserService(Context);
            CaseService = new CaseService(Context);
            MedicationService = new MedicationService(Context);
        }
        public async Task OnGetAsync()
        {
            Allergies = await PatientService.GetAllAllergies();
            Diagnoses = await PatientService.GetAllDiagnoses();
            Students = await UserService.GetAllStudents();
            Goals = await CaseService.GetAllGoals();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Allergies = await PatientService.GetAllAllergies();
            Diagnoses = await PatientService.GetAllDiagnoses();
            Students = await UserService.GetAllStudents();
            Goals = await CaseService.GetAllGoals();

            PatientName = Request.Form["patientName"];
            if (int.TryParse(Request.Form["patientWeight"], out int weight)) {
                PatientWeight = weight;
            }
            if (int.TryParse(Request.Form["patientAge"], out int age)) {
                PatientAge = age;
            }
            if (int.TryParse(Request.Form["selectSex"], out int sex)) {
                PatientSex = sex;
            }
            if (int.TryParse(Request.Form["selectStatus"], out int status)) {
                SelectStatus = status;
            }
            if (int.TryParse(Request.Form["heartrate"], out int heartrate)) {
                Heartrate = heartrate;
            }
            if (int.TryParse(Request.Form["respiratoryRate"], out int respiratoryRate)) {
                RespiratoryRate = respiratoryRate;
            }
            if (int.TryParse(Request.Form["temperature"], out int temperature)) {
                Temperature = temperature;
            }
            PatientDiagnoses = new List<Illness>();
            PatientAllergies = new List<Medication>();
            CaseGoals = new List<Goal>();

            List<string> DiagnosesStr = Request.Form["diagnoses"].ToString().Split(',').ToList();
            foreach (string s in DiagnosesStr)
            {
                if (int.TryParse(s, out int num)) {
                    var illness = Diagnoses.FirstOrDefault(d => d.IllnessId == num);
                    if (illness != null) {
                        PatientDiagnoses.Add(Diagnoses.First(d => d.IllnessId == num));
                    }
                    // PatientDiagnoses.Add(await PatientService.FindIllness(num));
                }
            }
            Console.WriteLine(DiagnosesStr);
            List<string> AllergiesStr = Request.Form["allergies"].ToString().Split(',').ToList();
            foreach (string s in AllergiesStr)
            {
                if (int.TryParse(s, out int num))
                {
                    var illness = Diagnoses.FirstOrDefault(d => d.IllnessId == num);
                    if (illness != null) {
                        PatientAllergies.Add(Allergies.First(a => a.MedicationId == num));
                    }
                    // PatientAllergies.Add(await MedicationService.FindMedication(num));
                }
            }
            if (int.TryParse(Request.Form["selectStudent"], out int studentId))
            {
                Student = await UserService.FindStudent(studentId);
            }
            List<string> GoalsStr = Request.Form["goals"].ToString().Split(',').ToList();
            foreach (string s in GoalsStr)
            {
                if (int.TryParse(s, out int num))
                {
                    CaseGoals.Add(await CaseService.FindGoal(num));
                }
            }

            if (new object?[] { PatientName, PatientWeight, PatientAge, PatientSex, SelectStatus, Heartrate, RespiratoryRate, Temperature, PatientDiagnoses, PatientAllergies, Student, CaseGoals }.Any(x => x is null)) return Page();
            Patient Patient = await PatientService.AddNewPatient(PatientName, PatientWeight, PatientAge, PatientSex, SelectStatus, Heartrate, RespiratoryRate, Temperature, PatientDiagnoses, PatientAllergies);
            await CaseService.AddNewCase(Patient, Student, CaseGoals);

            return RedirectToPage();
        }
    }
}

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
        public PatientService PatientService { get; set; }
        public UserService UserService { get; set; }
        public CaseService CaseService { get; set; }
        public MedicationService MedicationService { get; set; }
        public List<Medication> Allergies { get; set; }
        public List<Illness> Diagnoses { get; set; }
        public List<User> Students { get; set; }
        public List<Goal> Goals { get; set; }
        [BindProperty]
        public String patientName { get; set; }
        [BindProperty]
        public int patientWeight { get; set; }
        [BindProperty]
        public int patientAge { get; set; }
        [BindProperty]
        public int patientSex { get; set; }
        [BindProperty]
        public int selectStatus { get; set; }
        [BindProperty]
        public int heartrate { get; set; }
        [BindProperty]
        public BloodPressure bloodPressure { get; set; }
        [BindProperty]
        public int respiratoryRate { get; set; }
        [BindProperty]
        public int temperature { get; set; }
        [BindProperty]
        public List<Illness> patientDiagnoses { get; set; }
        [BindProperty]
        public List<Medication> patientAllergies { get; set; }
        [BindProperty]
        public User Student { get; set; }
        [BindProperty]
        public List<Goal> CaseGoals { get; set; }
        public AddScenarioModel(PatientService patientService, UserService userService, CaseService caseService, MedicationService medicationService) {
            PatientService = patientService;
            UserService = userService;
            CaseService = caseService;
            MedicationService = medicationService;
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

            patientName = Request.Form["patientName"];
            if (int.TryParse(Request.Form["patientWeight"], out int weight)) {
                patientWeight = weight;
            }
            if (int.TryParse(Request.Form["patientAge"], out int age)) {
                patientAge = age;
            }
            if (int.TryParse(Request.Form["selectSex"], out int sex)) {
                patientSex = sex;
            }
            if (int.TryParse(Request.Form["selectStatus"], out int status)) {
                selectStatus = status;
            }
            if (int.TryParse(Request.Form["heartrate"], out int parsedHeartrate)) {
                heartrate = parsedHeartrate;
            }
            if (int.TryParse(Request.Form["bloodpressureSystolic"], out int bloodpressureSystolic) && int.TryParse(Request.Form["bloodpressureDiastolic"], out int bloodpressureDiastolic)) {
                bloodPressure = new BloodPressure { Systolic = bloodpressureSystolic, Diastolic = bloodpressureDiastolic };
            }
            if (int.TryParse(Request.Form["respiratoryRate"], out int parsedRespiratoryRate)) {
                respiratoryRate = respiratoryRate;
            }
            if (int.TryParse(Request.Form["temperature"], out int parsedTemperature)) {
                temperature = parsedTemperature;
            }
            patientDiagnoses = new List<Illness>();
            patientAllergies = new List<Medication>();
            CaseGoals = new List<Goal>();

            List<string> DiagnosesStr = Request.Form["diagnoses"].ToString().Split(',').ToList();
            foreach (string s in DiagnosesStr)
            {
                if (int.TryParse(s, out int num)) {
                    var illness = Diagnoses.FirstOrDefault(d => d.IllnessId == num);
                    if (illness != null) {
                        patientDiagnoses.Add(Diagnoses.First(d => d.IllnessId == num));
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
                        patientAllergies.Add(Allergies.First(a => a.MedicationId == num));
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

            if (new object?[] { patientName, patientWeight, patientAge, patientSex, selectStatus, parsedHeartrate, parsedRespiratoryRate, parsedTemperature, patientDiagnoses, patientAllergies, Student, CaseGoals }.Any(x => x is null)) return Page();
            Patient patient = new Patient {
                PatientName = patientName,
                Weight = patientWeight,
                Age = patientAge,
                Sex = (Patient.SexEnum)patientSex,
                Status = (Patient.StatusEnum)selectStatus,
                Heartrate = parsedHeartrate,
                Allergies = patientAllergies,
                Diagnoses = patientDiagnoses,
                BloodPressure = bloodPressure
            };
            
            Patient newPatient = await PatientService.AddNewPatient(patient);
            await CaseService.AddNewCase(newPatient, Student, CaseGoals);

            return RedirectToPage();
        }
    }
}

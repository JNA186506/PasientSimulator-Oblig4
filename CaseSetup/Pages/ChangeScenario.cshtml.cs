using CaseSetup.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;
using PasientSimulator.lib.Services.Interfaces;

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

            User? Student = null;
            List<Goal> CaseGoals = new List<Goal>();
            if (Case == null) return RedirectToPage("/Index");
            if (int.TryParse(Request.Form["selectStudent"], out var studentId))
                Student = await UserService.FindStudent(studentId);
            if (Student != null) Case.Student = Student;
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

            int selectStatus = -1;
            String? patientName = Request.Form["patientName"];
            int patientWeight = -1;
            int patientAge = -1;
            Patient.SexEnum? selectSex = null;
            int heartrate = -1;
            BloodPressure? bloodPressure = null;
            int respiratoryRate = -1;
            double oxygenSaturation = -1;
            double? temperature = null;
            List<Illness> patientDiagnoses = new List<Illness>();
            List<Medication> patientAllergies = new List<Medication>();
            if (Case == null || Case.CasePatient == null) return RedirectToPage("/Index");
            if (int.TryParse(Request.Form["selectStatus"], out int status)) selectStatus = status;
            if (selectStatus >= 0) Case.CasePatient.Status = (Patient.StatusEnum) selectStatus;
            if (patientName != null) Case.CasePatient.PatientName = patientName;
            if (int.TryParse(Request.Form["patientWeight"], out int weight)) patientWeight = weight;
            if (patientWeight >= 0) Case.CasePatient.Weight = patientWeight;
            if (int.TryParse(Request.Form["patientAge"], out int age)) patientAge = age;
            if (patientAge >= 0) Case.CasePatient.Age = patientAge;
            if (int.TryParse(Request.Form["selectSex"], out int sex)) selectSex = (Patient.SexEnum)sex;
            if (selectSex != null) Case.CasePatient.Sex = (Patient.SexEnum)selectSex;
            if (int.TryParse(Request.Form["heartrate"], out int hr)) heartrate = hr;
            if (heartrate >= 0) Case.CasePatient.Heartrate = heartrate;
            if (int.TryParse(Request.Form["bloodpressureSystolic"], out var bloodpressureSystolic) &&
                int.TryParse(Request.Form["bloodpressureDiastolic"], out var bloodpressureDiastolic))
                    if(bloodpressureSystolic >= 0 && bloodpressureDiastolic >= 0) bloodPressure = new BloodPressure { Systolic = bloodpressureSystolic, Diastolic = bloodpressureDiastolic };
            if (bloodPressure != null) Case.CasePatient.BloodPressure = bloodPressure;
            if(int.TryParse(Request.Form["respiratoryRate"], out int rr)) respiratoryRate = rr;
            if(respiratoryRate >= 0) Case.CasePatient.RespiratoryRate = respiratoryRate;
            if (double.TryParse(Request.Form["oxygenSaturation"], out double os)) oxygenSaturation = os;
            if(oxygenSaturation >= 0) Case.CasePatient.OxygenSaturation = oxygenSaturation;
            if (double.TryParse(Request.Form["temperature"], out double temp)) temperature = temp;
            if(temperature != null) Case.CasePatient.Temperature = (double)temperature;
            var DiagnosesStr = Request.Form["diagnoses"].ToString().Split(',').ToList();
            foreach (var s in DiagnosesStr)
                if (int.TryParse(s, out var num))
                {
                    var illness = Diagnoses.FirstOrDefault(d => d.IllnessId == num);
                    if (illness != null) patientDiagnoses.Add(Diagnoses.First(d => d.IllnessId == num));
                    // PatientDiagnoses.Add(await PatientService.FindIllness(num));
                }
            Case.CasePatient.Diagnoses = patientDiagnoses;
            var AllergiesStr = Request.Form["allergies"].ToString().Split(',').ToList();
            foreach (var s in AllergiesStr)
                if (int.TryParse(s, out var num))
                {
                    var medication = Allergies.FirstOrDefault(d => d.MedicationId == num);
                    if (medication != null) patientAllergies.Add(Allergies.First(a => a.MedicationId == num));
                    // PatientAllergies.Add(await MedicationService.FindMedication(num));
                }
            Case.CasePatient.Allergies = patientAllergies;
            await PatientService.UpdatePatient(Case.CasePatient);
            await _hubContext.Clients.All.SendAsync("CaseUpdated", Case.CaseId);
            return RedirectToPage(new { idInt = id });
        }
    }
}

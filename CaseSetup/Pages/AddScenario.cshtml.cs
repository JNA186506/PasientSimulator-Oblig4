using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;
using System.ComponentModel;

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
        public List<Illness> Diagnosis { get; set; }
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
        public List<Illness> Diagnoses { get; set; }
        [BindProperty]
        public List<Medication> PatientAllergies { get; set; }
        [BindProperty]
        public int CaseId { get; set; }
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
            Allergies = PatientService.GetAllAllergies();
            Diagnosis = PatientService.GetAllDiagnosis();
            Students = UserService.GetAllStudents();
            Goals = CaseService.GetAllGoals();
        }
        public void OnPost()
        {
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
            List<String> DiagnosesStr = Request.Form["diagnoses"].ToString().Split(',').ToList();
            foreach (String s in DiagnosesStr) {
                if (int.TryParse(s, out int num)) {
                    Diagnoses.Add(PatientService.FindIllness(num));
                }
            }
            List<String> AllergiesStr = Request.Form["allergies"].ToString().Split(',').ToList();
            foreach (String s in AllergiesStr)
            {
                if (int.TryParse(s, out int num))
                {
                    Diagnoses.Add(MedicationService.FindMedication(num));
                }
            }
            if (int.TryParse(Request.Form["caseId"], out int caseId)) {
                CaseId = caseId;
            }
            if (int.TryParse(Request.Form["studentId"], out int studentId)) {
                Student = UserService.FindStudent(studentId);
            }
            List<String> GoalsStr = Request.Form["goals"].ToString().Split(',').ToList();
            foreach (String s in AllergiesStr)
            {
                if (int.TryParse(s, out int num))
                {
                    CaseGoals.Add(CaseService.FindGoal(num));
                }
            }
            Patient Patient = PatientService.AddNewPatient(PatientName, PatientWeight, PatientAge, PatientSex, SelectStatus, Heartrate, RespiratoryRate, Temperature, Diagnoses, Allergies);
            CaseService.AddNewCase(CaseId, Patient, Student, CaseGoals);
        }
    }
}

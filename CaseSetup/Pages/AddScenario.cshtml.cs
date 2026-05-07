using CaseSetup.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace CaseSetup.Pages;

public class AddScenarioModel : PageModel
{
    private IHubContext<CaseHub> _hubContext;
    public AddScenarioModel(PatientService patientService, UserService userService, CaseService caseService,
        MedicationService medicationService, IHubContext<CaseHub> hubContext)
    {
        PatientService = patientService;
        UserService = userService;
        CaseService = caseService;
        MedicationService = medicationService;
        _hubContext = hubContext;
    }

    public PatientService PatientService { get; set; }
    public UserService UserService { get; set; }
    public CaseService CaseService { get; set; }
    public MedicationService MedicationService { get; set; }
    public List<Medication> Allergies { get; set; }
    public List<Illness> Diagnoses { get; set; }
    public List<User> Students { get; set; }
    public List<Goal> Goals { get; set; }

    [BindProperty] public string patientName { get; set; }

    [BindProperty] public int patientWeight { get; set; }

    [BindProperty] public int patientAge { get; set; }

    [BindProperty] public int patientSex { get; set; }

    [BindProperty] public int selectStatus { get; set; }

    [BindProperty] public int heartrate { get; set; }

    [BindProperty] public BloodPressure bloodPressure { get; set; }

    [BindProperty] public int respiratoryRate { get; set; }

    [BindProperty] public int temperature { get; set; }

    [BindProperty] public List<Illness> patientDiagnoses { get; set; }

    [BindProperty] public List<Medication> patientAllergies { get; set; }

    [BindProperty] public User Student { get; set; }

    [BindProperty] public List<Goal> CaseGoals { get; set; }

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
        if (int.TryParse(Request.Form["patientWeight"], out var weight)) patientWeight = weight;
        if (int.TryParse(Request.Form["patientAge"], out var age)) patientAge = age;
        if (int.TryParse(Request.Form["selectSex"], out var sex)) patientSex = sex;
        if (int.TryParse(Request.Form["selectStatus"], out var status)) selectStatus = status;
        if (int.TryParse(Request.Form["heartrate"], out var parsedHeartrate)) heartrate = parsedHeartrate;
        if (int.TryParse(Request.Form["bloodpressureSystolic"], out var bloodpressureSystolic) &&
            int.TryParse(Request.Form["bloodpressureDiastolic"], out var bloodpressureDiastolic))
            bloodPressure = new BloodPressure { Systolic = bloodpressureSystolic, Diastolic = bloodpressureDiastolic };
        if (int.TryParse(Request.Form["respiratoryRate"], out var parsedRespiratoryRate))
            respiratoryRate = respiratoryRate;
        if (int.TryParse(Request.Form["temperature"], out var parsedTemperature)) temperature = parsedTemperature;
        patientDiagnoses = new List<Illness>();
        patientAllergies = new List<Medication>();
        CaseGoals = new List<Goal>();

        var DiagnosesStr = Request.Form["diagnoses"].ToString().Split(',').ToList();
        foreach (var s in DiagnosesStr)
            if (int.TryParse(s, out var num))
            {
                var illness = Diagnoses.FirstOrDefault(d => d.IllnessId == num);
                if (illness != null) patientDiagnoses.Add(Diagnoses.First(d => d.IllnessId == num));
                // PatientDiagnoses.Add(await PatientService.FindIllness(num));
            }

        var AllergiesStr = Request.Form["allergies"].ToString().Split(',').ToList();
        foreach (var s in AllergiesStr)
            if (int.TryParse(s, out var num))
            {
                var illness = Diagnoses.FirstOrDefault(d => d.IllnessId == num);
                if (illness != null) patientAllergies.Add(Allergies.First(a => a.MedicationId == num));
                // PatientAllergies.Add(await MedicationService.FindMedication(num));
            }

        if (int.TryParse(Request.Form["selectStudent"], out var studentId))
            Student = await UserService.FindStudent(studentId);
        var GoalsStr = Request.Form["goals"].ToString().Split(',').ToList();
        foreach (var s in GoalsStr)
            if (int.TryParse(s, out var num))
                CaseGoals.Add(await CaseService.FindGoal(num));

        if (new object?[]
            {
                patientName, patientWeight, patientAge, patientSex, selectStatus, parsedHeartrate, bloodPressure,
                parsedRespiratoryRate, parsedTemperature, patientDiagnoses, patientAllergies, Student, CaseGoals
            }.Any(x => x is null)) return Page();
        var patient = new Patient
        {
            PatientName = patientName,
            Weight = patientWeight,
            Age = patientAge,
            Sex = (Patient.SexEnum)patientSex,
            Status = (Patient.StatusEnum)selectStatus,
            Heartrate = parsedHeartrate,
            RespiratoryRate = respiratoryRate,
            Temperature = temperature,
            OxygenSaturation = 96.5,
            Allergies = patientAllergies,
            Diagnoses = patientDiagnoses,
            BloodPressure = bloodPressure
        };

        var newPatient = await PatientService.AddNewPatient(patient);
        var newCase = await CaseService.AddNewCase(newPatient, Student, CaseGoals);
        await _hubContext.Clients.All.SendAsync("CaseUpdated", newCase.CaseId);

        return RedirectToPage();
    }
}
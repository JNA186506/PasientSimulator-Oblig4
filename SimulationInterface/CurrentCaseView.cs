using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;
using PasientSimulator.lib.Services.Interfaces;

namespace SimulationInterface;

public partial class CurrentCaseView : Form {

    private readonly IUserService _userService;
    private readonly IPatientService _patientService;
    private readonly ICaseService _caseService;
    
    public CurrentCaseView(IPatientService patientService, ICaseService caseService, IUserService userService) {
        _patientService = patientService;
        _caseService = caseService;
        _userService = userService;

        this.Load += CurrentCaseView_Load;

        InitializeComponent();
    }

    private async void CurrentCaseView_Load(object sender, EventArgs e) {
        var currCase = await _caseService.GetCaseById(1);

        if (currCase == null)
        {
            MessageBox.Show("Case not found.");
            return;
        }

        var p = currCase.CasePatient;
        if (p == null)
        {
            MessageBox.Show("Patient data missing from case.");
            return;
        }
        // Header
        labelCaseNo.Text      = $"CASE #{currCase.CaseId}";
        labelPatientName.Text = p.PatientName;

        // Demographics
        labelAge.Text    = $"Age: {p.Age}";
        labelSex.Text    = $"Sex: {p.Sex}";
        labelWeight.Text = $"Weight: {p.Weight} kg";
        labelStatus.Text = $"Status: {p.Status}";

        // Vitals
        labelHeartrate.Text    = $"Heart Rate: {p.Heartrate} bpm";
        labelBP.Text           = $"Blood Pressure: {p.BloodPressure.Systolic}/{p.BloodPressure.Diastolic} mmHg";
        labelRespRate.Text     = $"Respiratory Rate: {p.RespiratoryRate} /min";
        labelOxygen.Text       = $"O₂ Saturation: {p.OxygenSaturation:F1}%";
        labelTemperature.Text  = $"Temperature: {p.Temperature:F1} °C";

        // Diagnoses & Medical History
        listBoxDiagnoses.DataSource    = p.Diagnoses?.Select(d => d.IllnessName).ToList();
        listBoxMedHistory.DataSource   = p.MedicalHistory?.Select(d => d.IllnessName).ToList();

        // Medications & Allergies
        listBoxMedications.DataSource  = p.Medications?.Select(m => m.MedicationName).ToList();
        listBoxAllergies.DataSource    = p.Allergies?.Select(a => a.MedicationName).ToList();
    }


}
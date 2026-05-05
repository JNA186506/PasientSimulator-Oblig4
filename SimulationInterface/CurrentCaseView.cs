using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services.Interfaces;

namespace SimulationInterface;

public partial class CurrentCaseView : Form
{
    private readonly ICaseService _caseService;
    private readonly IMedicationService _medicationService;
    private readonly IUserService _userService;
    private readonly IAdministerTreatment _treatmentService;

    private Case? _currCase;

    public CurrentCaseView(IMedicationService medicationService, ICaseService caseService, IUserService userService, IAdministerTreatment treatmentService)
    {
        _medicationService = medicationService;
        _caseService = caseService;
        _userService = userService;
        _treatmentService = treatmentService;

        InitializeComponent();
        
        Load += CurrentCaseView_Load;
        administerTreatmentButton.Click += AdministerTreatmentButton_OnClicked;
    }

    private void RefreshView(Patient p)
    {    
        labelStatus.Text      = $"Status: {p.Status}";
        labelHeartrate.Text   = $"Heart Rate: {p.Heartrate} bpm";
        labelBP.Text          = $"Blood Pressure: {p.BloodPressure.Systolic}/{p.BloodPressure.Diastolic} mmHg";
        labelOxygen.Text      = $"O₂ Saturation: {p.OxygenSaturation:F1}%";
        labelTemperature.Text = $"Temperature: {p.Temperature:F1} °C";
        listBoxMedications.DataSource = p.Medications?.Select(m => m.MedicationName).ToList();
        
    }

    private async void CurrentCaseView_Load(object sender, EventArgs e)
    {
        _currCase = await _caseService.GetCaseById(1);
        await LoadEventLog();

        if (_currCase == null)
        {
            MessageBox.Show("Case not found.");
            return;
        }

        var p = _currCase.CasePatient;
        if (p == null)
        {
            MessageBox.Show("Patient data missing from case.");
            return;
        }

        // Header
        labelCaseNo.Text = $"CASE #{_currCase.CaseId}";
        labelPatientName.Text = p.PatientName;

        // Demographics
        labelAge.Text = $"Age: {p.Age}";
        labelSex.Text = $"Sex: {p.Sex}";
        labelWeight.Text = $"Weight: {p.Weight} kg";
        labelStatus.Text = $"Status: {p.Status}";

        // Vitals
        labelHeartrate.Text = $"Heart Rate: {p.Heartrate} bpm";
        labelBP.Text = $"Blood Pressure: {p.BloodPressure.Systolic}/{p.BloodPressure.Diastolic} mmHg";
        labelRespRate.Text = $"Respiratory Rate: {p.RespiratoryRate} /min";
        labelOxygen.Text = $"O₂ Saturation: {p.OxygenSaturation:F1}%";
        labelTemperature.Text = $"Temperature: {p.Temperature:F1} °C";

        // Diagnoses & Medical History
        listBoxDiagnoses.DataSource = p.Diagnoses?.Select(d => d.IllnessName).ToList();
        listBoxMedHistory.DataSource = p.MedicalHistory?.Select(d => d.IllnessName).ToList();

        // Medications & Allergies
        listBoxMedications.DataSource = p.Medications?.Select(m => m.MedicationName).ToList();
        listBoxAllergies.DataSource = p.Allergies?.Select(a => a.MedicationName).ToList();
    }

    private async Task LoadEventLog()
    {
        var events = await _caseService.GetEventsById(_currCase.CaseId);

        listViewEvents.Items.Clear();

        foreach (var ev in events.OrderByDescending(ev => ev.Timeadded))
        {
            var item = new ListViewItem(ev.Timeadded.ToLocalTime().ToString("HH:mm:ss"));
            item.SubItems.Add(ev.EventType == EventEnum.Comment ? "Comment" : "Intervention");
            item.SubItems.Add(ev.Description ?? "");
            item.ForeColor = ev.EventType == EventEnum.Comment ? Color.SteelBlue : Color.Black;
            listViewEvents.Items.Add(item);
        }
    }

    private async void AdministerTreatmentButton_OnClicked(object? sender, EventArgs e)
    {
        Patient? currentPatient = _currCase.CasePatient;
        List<Medication> allMedications = await _medicationService.GetAllMedications();
        if (_currCase == null)
        {
            MessageBox.Show("Case data is not loaded yet.");
            return;
        }
        if (currentPatient == null)
        {
            MessageBox.Show("Patient data is not loaded yet.");
            return;
        }

        using var treatmentView =
            new AdministerTreatmentView(currentPatient, _treatmentService, allMedications);
        var result = treatmentView.ShowDialog(this);
        
        await _caseService.AddEvent(new Event
        {
            CaseId = _currCase.CaseId,
            UserId = _currCase.UserId,
            EventType = EventEnum.MedicalIntervention,
            Description = $"Administered {treatmentView.SelectedTreatment}"
        });
        await LoadEventLog();
        RefreshView(currentPatient);
        
    }
}
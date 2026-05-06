using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace SimulationInterface;

public partial class CurrentCaseView : Form
{
    private readonly ICaseService _caseService;
    private readonly IMedicationService _medicationService;
    private readonly IUserService _userService;
    private readonly IAdministerTreatment _treatmentService;
    private HubConnection _hubConnection;

    private Case? _currCase;

    public CurrentCaseView(IMedicationService medicationService, 
        ICaseService caseService, IUserService userService, IAdministerTreatment treatmentService,
        HubConnection hubConnection)
    {
        _medicationService = medicationService;
        _caseService = caseService;
        _userService = userService;
        _treatmentService = treatmentService;
        _hubConnection = hubConnection;

        InitializeComponent();
        Load += CurrentCaseView_Load;
        Load += async (_, _) =>
        {
            await ConnectToHub();
        };
        administerTreatmentButton.Click += AdministerTreatmentButton_OnClicked;
        menuItemActiveCases.Click += OnActiveCasesClicked;
    }

    private async Task ConnectToHub()
    {
 
        _hubConnection.On<int>("CaseUpdated", async (caseId) =>
        {
            if (_currCase != null && _currCase.CaseId == caseId) {
                Invoke(() => _ = LoadCase(caseId));
            }
        });

        _hubConnection.On<int, string, string>("CommentRecieved", (caseId, comment, author) =>
        {
            Invoke(() =>
            {
                labelStatus.Text = $"New comment on case {caseId} by {author}: {comment}";
            });
        });

        await _hubConnection.StartAsync();
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
    
    private async Task LoadCase(int id)
    {
        _currCase = await _caseService.GetCaseById(id);
        if (_currCase == null)
            return;

        await LoadEventLog();
        PopulateView(_currCase);
    }

    private void PopulateView(Case c)
    {
        
        var p = c.CasePatient;
        if (p == null)
        {
            MessageBox.Show("Patient data missing from case.");
            return;
        }

        labelCaseNo.Text = $"CASE #{_currCase.CaseId}";
        labelPatientName.Text = p.PatientName;

        labelAge.Text = $"Age: {p.Age}";
        labelSex.Text = $"Sex: {p.Sex}";
        labelWeight.Text = $"Weight: {p.Weight} kg";
        labelStatus.Text = $"Status: {p.Status}";

        labelHeartrate.Text = $"Heart Rate: {p.Heartrate} bpm";
        labelBP.Text = $"Blood Pressure: {p.BloodPressure.Systolic}/{p.BloodPressure.Diastolic} mmHg";
        labelRespRate.Text = $"Respiratory Rate: {p.RespiratoryRate} /min";
        labelOxygen.Text = $"O₂ Saturation: {p.OxygenSaturation:F1}%";
        labelTemperature.Text = $"Temperature: {p.Temperature:F1} °C";

        listBoxDiagnoses.DataSource = p.Diagnoses?.Select(d => d.IllnessName).ToList();
        listBoxMedHistory.DataSource = p.MedicalHistory?.Select(d => d.IllnessName).ToList();

        listBoxMedications.DataSource = p.Medications?.Select(m => m.MedicationName).ToList();
        listBoxAllergies.DataSource = p.Allergies?.Select(a => a.MedicationName).ToList();    }

    private async void CurrentCaseView_Load(object sender, EventArgs e)
    {
        await LoadCase(1);
        
        if (_currCase == null)
        {
            MessageBox.Show("Case not found.");
            return;
        }

        await LoadEventLog();
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
        if (_currCase == null)
        {
            MessageBox.Show("Case data is not loaded yet.");
            return;
        }
       
        Patient? currentPatient = _currCase.CasePatient;
        if (currentPatient == null)
        {
            MessageBox.Show("Patient data is not loaded yet.");
            return;
        }
        
        List<Medication> allMedications = await _medicationService.GetAllMedications();
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

    private void OnActiveCasesClicked(object? sender, EventArgs e)
    {
        if (_currCase == null)
            return;

        var view = new ActiveCasesView(_caseService, _currCase.UserId, async id =>
            await LoadCase(id));
        view.ShowDialog(this);
    }
}
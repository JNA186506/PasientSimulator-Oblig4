using System.Reflection.Metadata;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services.Interfaces;

namespace SimulationInterface;

public partial class AdministerTreatmentView : Form
{

    private readonly Patient _patient;
    private readonly IAdministerTreatment _treatmentService;
    private readonly List<Medication> _medications;

    public string? SelectedTreatment { get; private set; }

    public AdministerTreatmentView(Patient patient, IAdministerTreatment treatmentService, List<Medication> medications)
    {
        _patient = patient;
        _treatmentService = treatmentService;
        _medications = medications;
        
        InitializeComponent();
        WireUpTreatmentButtons();
    }

    private void WireUpTreatmentButtons()
    {
        var btnOxygen = CreateTreatmentButton("Give oxygen");
        btnOxygen.Click += async (_, _) =>
        {
            var success = await _treatmentService.AdministerOxygen(_patient);
            ShowFeedbackAndClose(success, "Oxygen");
        };
        flowPanelLifeSupport.Controls.Add(btnOxygen);

        foreach (var med in _medications ?? Enumerable.Empty<Medication>())
        {
            var btn = CreateTreatmentButton(med.MedicationName);
            btn.Click += async (_, _) =>
            {
                bool success = await _treatmentService.AdministerMedicine(med, _patient);
                ShowFeedbackAndClose(success, med.MedicationName);
            };
            flowPanelMedications.Controls.Add(btn);
        }

    }

    private void ShowFeedbackAndClose(bool success, string description)
    {
        var (msg, title, icon) = success
            ? ($"{description} administered.", "Treatment Applied", MessageBoxIcon.Information)
            : ($"{description} could not be applied.", "Treatment Failed", MessageBoxIcon.Warning);

        MessageBox.Show(msg, title, MessageBoxButtons.OK, icon);
        DialogResult = success ? DialogResult.OK : DialogResult.Abort;
        Close();
    }

    private static Button CreateTreatmentButton(string label) => new()
    {
        Text = label,
        Size = new Size(160, 40),
        Margin = new Padding(6),
        FlatStyle = FlatStyle.Flat
    };

}
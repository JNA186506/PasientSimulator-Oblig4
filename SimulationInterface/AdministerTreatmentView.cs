using System.Reflection.Metadata;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services.Interfaces;

namespace SimulationInterface;

public partial class AdministerTreatmentView : Form
{

    private readonly Patient _patient;
    private readonly IAdministerTreatment _treatmentService;
    
    public string? SelectedTreatment { get; private set; }
    public AdministerTreatmentView(Patient patient, IAdministerTreatment treatmentService)
    {
        _patient = patient;
        _treatmentService = treatmentService;
        InitializeComponent();
        WireUpTreatmentButtons();
    }

    private void WireUpTreatmentButtons()
    {
        var treatmentButtons = new[]
        {
            buttonOxygen
        };

        foreach (var btn in treatmentButtons)
            btn.Click += TreatmentButton_OnClicked;
    }

    private void TreatmentButton_OnClicked(object? sender, EventArgs e)
    {
        if (sender is not Button btn) return;

        SelectedTreatment = btn.Text;
        DialogResult = DialogResult.OK;
        Close();
    }
}
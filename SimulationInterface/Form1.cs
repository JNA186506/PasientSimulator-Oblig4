using PasientSimulator.lib.Services;

namespace SimulationInterface;

public partial class Form1 : Form {

    private readonly PatientService _patientService;
    private readonly CaseService _caseService;
    public Form1(PatientService patientService, CaseService caseService) {
        _patientService = patientService;
        _caseService = caseService;
        InitializeComponent();
    }
}
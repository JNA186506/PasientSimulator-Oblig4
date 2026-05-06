using PasientSimulator.lib.Services.Interfaces;

namespace SimulationInterface;

public partial class ActiveCasesView : Form
{
    private readonly ICaseService _caseService;
    private readonly int _userId;
    private readonly Func<int, Task> _switchCase;

    public ActiveCasesView(ICaseService caseService, int userId, Func<int, Task> switchCase)
    {
        _caseService = caseService;
        _userId = userId;
        _switchCase = switchCase;

        InitializeComponent();

        Load += ActiveCasesView_Load;
        listViewCases.SelectedIndexChanged += (_, _) =>
            buttonSwitch.Enabled = listViewCases.SelectedItems.Count > 0;
        buttonSwitch.Click += ButtonSwitch_Click;
    }

    private async void ActiveCasesView_Load(object sender, EventArgs e)
    {
        var cases = await _caseService.GetAllCasesByUserId(_userId);
        listViewCases.Items.Clear();

        foreach (var c in cases)
        {
            var item = new ListViewItem(c.CaseId.ToString());
            item.SubItems.Add(c.CasePatient?.PatientName ?? "Unknown");
            item.SubItems.Add(c.CasePatient?.Status.ToString() ?? "-");
            item.Tag = c.CaseId;
            listViewCases.Items.Add(item);
        }
    }

    private async void ButtonSwitch_Click(object sender, EventArgs e)
    {
        if (listViewCases.SelectedItems.Count == 0) return;
        int caseId = (int)listViewCases.SelectedItems[0].Tag;
        await _switchCase(caseId);
        Close();
    }
}

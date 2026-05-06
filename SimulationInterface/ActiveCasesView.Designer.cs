namespace SimulationInterface;

partial class ActiveCasesView
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        listViewCases = new ListView
        {
            Location = new Point(12, 12),
            Size = new Size(560, 350),
            View = View.Details,
            FullRowSelect = true,
            GridLines = true,
            MultiSelect = false
        };
        listViewCases.Columns.Add("Case #", 80);
        listViewCases.Columns.Add("Patient", 230);
        listViewCases.Columns.Add("Status", 230);

        buttonSwitch = new Button
        {
            Text = "Switch to Case",
            Location = new Point(448, 375),
            Size = new Size(124, 35),
            Enabled = false
        };

        Controls.AddRange(new Control[] { listViewCases, buttonSwitch });

        Text = "My Active Cases";
        ClientSize = new Size(584, 425);
        Font = new Font("Segoe UI", 10F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
    }

    private ListView listViewCases;
    private Button buttonSwitch;
}

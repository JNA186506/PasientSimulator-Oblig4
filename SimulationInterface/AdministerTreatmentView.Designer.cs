using System.ComponentModel;

namespace SimulationInterface;

partial class AdministerTreatmentView
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "AdministerTreatmentView";

        labelTitle = new Label
        {
            Text = "Administer treatment",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            Location = new Point(20, 18),
            Size = new Size(460, 32)
        };

        var groupMedicationBox = new GroupBox
        {
            Text = "Medications",
            Location = new Point(20, 65),
            Size = new Size(460, 130)
        };

        flowPanelMedications = new FlowLayoutPanel
        {
            Location = new Point(8, 22),
            Size = new Size(440, 58),
            WrapContents = true
        };
        groupMedicationBox.Controls.Add(flowPanelMedications);

        var groupLifeSupportBox = new GroupBox
        {
            Text = "Life support",
            Location = new Point(20, 210),
            Size = new Size(460, 90)
        };

        flowPanelLifeSupport = new FlowLayoutPanel
        {
            Location = new Point(8, 22),
            Size = new Size(440, 58),
            WrapContents = true
        };
        groupLifeSupportBox.Controls.Add(flowPanelLifeSupport);

        buttonCancel = new Button
        {
            Text = "Cancel",
            Location = new Point(350, 400),
            Size = new Size(110, 36),
            DialogResult = DialogResult.Cancel
        };
        
        Controls.AddRange(new Control[]
        {
            labelTitle,
            buttonCancel,
            groupMedicationBox,
            groupLifeSupportBox
        });
        
        Text = "Administer Treatment";
        ClientSize = new Size(484, 455);
        Font = new Font("Segoe UI", 10F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        AcceptButton = null;
        CancelButton = buttonCancel;

    }

    private Button buttonCancel;
    private Label labelTitle;
    private FlowLayoutPanel flowPanelMedications, flowPanelLifeSupport;

    #endregion
}
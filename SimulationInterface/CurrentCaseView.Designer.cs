using PasientSimulator.lib.Services;

namespace SimulationInterface;

partial class CurrentCaseView {

    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    labelCaseNo = new Label { 
        Text = "CASE #", Font = new Font("Segoe UI", 16F, FontStyle.Bold),
        Location = new Point(20, 20), Size = new Size(300, 40) 
    };
    labelPatientName = new Label { 
        Text = "Patient Name", Font = new Font("Segoe UI", 13F),
        Location = new Point(20, 65), Size = new Size(300, 30) 
    };

    var groupDemographics = new GroupBox {
        Text = "Demographics", Location = new Point(20, 110), Size = new Size(300, 160)
    };
    labelAge    = new Label { Location = new Point(10, 30),  Size = new Size(270, 25) };
    labelSex    = new Label { Location = new Point(10, 60),  Size = new Size(270, 25) };
    labelWeight = new Label { Location = new Point(10, 90),  Size = new Size(270, 25) };
    labelStatus = new Label { Location = new Point(10, 120), Size = new Size(270, 25) };
    groupDemographics.Controls.AddRange(new Control[] { labelAge, labelSex, labelWeight, labelStatus });

    var groupVitals = new GroupBox {
        Text = "Vitals", Location = new Point(20, 290), Size = new Size(300, 175)
    };
    labelHeartrate   = new Label { Location = new Point(10, 30),  Size = new Size(270, 25) };
    labelBP          = new Label { Location = new Point(10, 60),  Size = new Size(270, 25) };
    labelRespRate    = new Label { Location = new Point(10, 90),  Size = new Size(270, 25) };
    labelOxygen      = new Label { Location = new Point(10, 120), Size = new Size(270, 25) };
    labelTemperature = new Label { Location = new Point(10, 150), Size = new Size(270, 25) };
    groupVitals.Controls.AddRange(new Control[] { labelHeartrate, labelBP, labelRespRate, labelOxygen, labelTemperature });

    var groupDiagnoses = new GroupBox {
        Text = "Diagnoses", Location = new Point(340, 110), Size = new Size(250, 175)
    };
    listBoxDiagnoses = new ListBox { Location = new Point(10, 25), Size = new Size(225, 135) };
    groupDiagnoses.Controls.Add(listBoxDiagnoses);

    var groupMedHistory = new GroupBox {
        Text = "Medical History", Location = new Point(340, 300), Size = new Size(250, 175)
    };
    listBoxMedHistory = new ListBox { Location = new Point(10, 25), Size = new Size(225, 135) };
    groupMedHistory.Controls.Add(listBoxMedHistory);

    var groupMedications = new GroupBox {
        Text = "Medications", Location = new Point(610, 110), Size = new Size(250, 175)
    };
    listBoxMedications = new ListBox { Location = new Point(10, 25), Size = new Size(225, 135) };
    groupMedications.Controls.Add(listBoxMedications);

    var groupAllergies = new GroupBox {
        Text = "Allergies", Location = new Point(610, 300), Size = new Size(250, 175)
    };
    listBoxAllergies = new ListBox { Location = new Point(10, 25), Size = new Size(225, 135) };
    groupAllergies.Controls.Add(listBoxAllergies);

    administerTreatmentButton = new Button { Location = new Point(760, 485), Size = new Size(100, 50) };
    administerTreatmentButton.Text = "Administer Treatment";

    Controls.AddRange(new Control[] {
        labelCaseNo, labelPatientName,
        groupDemographics, groupVitals,
        groupDiagnoses, groupMedHistory,
        groupMedications, groupAllergies,
        administerTreatmentButton
    });

    Text = "Simulation";
    ClientSize = new Size(1000, 620);
    Font = new Font("Segoe UI", 10F);
}

private Label labelCaseNo, labelPatientName;
private Label labelAge, labelSex, labelWeight, labelStatus;
private Label labelHeartrate, labelBP, labelRespRate, labelOxygen, labelTemperature;
private ListBox listBoxDiagnoses, listBoxMedHistory, listBoxMedications, listBoxAllergies;
private Button administerTreatmentButton;

#endregion
}
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

        buttonOxygen = new Button
        {
            Text = "Give oxygen",
            Location = new Point (20,350),
            Size = new Size(130,36)
        };

        buttonCancel = new Button
        {
            Text = "Cancel",
            Location = new Point(350, 400),
            Size = new Size(110, 36),
            DialogResult = DialogResult.Cancel
        };
        
        Controls.AddRange(new Control[]
        {
            buttonOxygen, buttonCancel
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

    private Button buttonOxygen, buttonCancel;

    #endregion
}
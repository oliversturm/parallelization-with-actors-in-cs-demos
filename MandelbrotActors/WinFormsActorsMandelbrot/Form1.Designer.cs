namespace WinFormsActorsMandelbrot {
  partial class Form1 {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

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
    private void InitializeComponent() {
      this.drawButton = new System.Windows.Forms.Button();
      this.panel = new System.Windows.Forms.Panel();
      this.SuspendLayout();
      // 
      // drawButton
      // 
      this.drawButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.drawButton.Location = new System.Drawing.Point(488, 12);
      this.drawButton.Name = "drawButton";
      this.drawButton.Size = new System.Drawing.Size(75, 23);
      this.drawButton.TabIndex = 0;
      this.drawButton.Text = "Draw!";
      this.drawButton.UseVisualStyleBackColor = true;
      this.drawButton.Click += new System.EventHandler(this.drawButton_Click);
      // 
      // panel
      // 
      this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panel.Location = new System.Drawing.Point(13, 12);
      this.panel.Name = "panel";
      this.panel.Size = new System.Drawing.Size(469, 360);
      this.panel.TabIndex = 1;
      this.panel.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_Paint);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(575, 384);
      this.Controls.Add(this.panel);
      this.Controls.Add(this.drawButton);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button drawButton;
    private System.Windows.Forms.Panel panel;
  }
}



namespace ImageTransform
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSoSwirlySteps = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSoSmartySteps = new System.Windows.Forms.TextBox();
            this.txtNodedataF = new System.Windows.Forms.TextBox();
            this.txtPredictionsF = new System.Windows.Forms.TextBox();
            this.btnSelect1 = new System.Windows.Forms.Button();
            this.btnSelect2 = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Node data:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Node QoS predictions:";
            // 
            // txtSoSwirlySteps
            // 
            this.txtSoSwirlySteps.Location = new System.Drawing.Point(25, 160);
            this.txtSoSwirlySteps.Multiline = true;
            this.txtSoSwirlySteps.Name = "txtSoSwirlySteps";
            this.txtSoSwirlySteps.Size = new System.Drawing.Size(195, 268);
            this.txtSoSwirlySteps.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "SoSwirly QoS per step:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(276, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "SoSmarty QoS per step:";
            // 
            // txtSoSmartySteps
            // 
            this.txtSoSmartySteps.Location = new System.Drawing.Point(279, 160);
            this.txtSoSmartySteps.Multiline = true;
            this.txtSoSmartySteps.Name = "txtSoSmartySteps";
            this.txtSoSmartySteps.Size = new System.Drawing.Size(195, 268);
            this.txtSoSmartySteps.TabIndex = 4;
            // 
            // txtNodedataF
            // 
            this.txtNodedataF.Location = new System.Drawing.Point(152, 26);
            this.txtNodedataF.Name = "txtNodedataF";
            this.txtNodedataF.Size = new System.Drawing.Size(178, 20);
            this.txtNodedataF.TabIndex = 6;
            // 
            // txtPredictionsF
            // 
            this.txtPredictionsF.Location = new System.Drawing.Point(152, 64);
            this.txtPredictionsF.Name = "txtPredictionsF";
            this.txtPredictionsF.Size = new System.Drawing.Size(178, 20);
            this.txtPredictionsF.TabIndex = 7;
            // 
            // btnSelect1
            // 
            this.btnSelect1.Location = new System.Drawing.Point(357, 24);
            this.btnSelect1.Name = "btnSelect1";
            this.btnSelect1.Size = new System.Drawing.Size(75, 23);
            this.btnSelect1.TabIndex = 8;
            this.btnSelect1.Text = "Load";
            this.btnSelect1.UseVisualStyleBackColor = true;
            this.btnSelect1.Click += new System.EventHandler(this.btnSelect1_Click);
            // 
            // btnSelect2
            // 
            this.btnSelect2.Location = new System.Drawing.Point(357, 64);
            this.btnSelect2.Name = "btnSelect2";
            this.btnSelect2.Size = new System.Drawing.Size(75, 23);
            this.btnSelect2.TabIndex = 9;
            this.btnSelect2.Text = "Load";
            this.btnSelect2.UseVisualStyleBackColor = true;
            this.btnSelect2.Click += new System.EventHandler(this.btnSelect2_Click);
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(193, 103);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(123, 23);
            this.btnGo.TabIndex = 10;
            this.btnGo.Text = "Fuckin pray it works";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 452);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.btnSelect2);
            this.Controls.Add(this.btnSelect1);
            this.Controls.Add(this.txtPredictionsF);
            this.Controls.Add(this.txtNodedataF);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSoSmartySteps);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSoSwirlySteps);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form3";
            this.Text = "Form3";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSoSwirlySteps;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSoSmartySteps;
        private System.Windows.Forms.TextBox txtNodedataF;
        private System.Windows.Forms.TextBox txtPredictionsF;
        private System.Windows.Forms.Button btnSelect1;
        private System.Windows.Forms.Button btnSelect2;
        private System.Windows.Forms.Button btnGo;
    }
}
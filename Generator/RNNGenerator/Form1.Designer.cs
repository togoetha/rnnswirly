
namespace ImageTransform
{
    partial class Form1
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
            this.pnlTopo = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtWaypoints = new System.Windows.Forms.TextBox();
            this.txtPaths = new System.Windows.Forms.TextBox();
            this.btnGenerateNodes = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFogNodes = new System.Windows.Forms.TextBox();
            this.txtEdgeNodes = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDraw = new System.Windows.Forms.Button();
            this.txtTimesteps = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSim = new System.Windows.Forms.Button();
            this.txtVectors = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTimestep = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pnlTopo
            // 
            this.pnlTopo.Location = new System.Drawing.Point(192, 48);
            this.pnlTopo.Name = "pnlTopo";
            this.pnlTopo.Size = new System.Drawing.Size(512, 512);
            this.pnlTopo.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Waypoints (csv):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(198, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fog nodes:";
            // 
            // txtWaypoints
            // 
            this.txtWaypoints.Location = new System.Drawing.Point(15, 35);
            this.txtWaypoints.Multiline = true;
            this.txtWaypoints.Name = "txtWaypoints";
            this.txtWaypoints.Size = new System.Drawing.Size(146, 256);
            this.txtWaypoints.TabIndex = 3;
            // 
            // txtPaths
            // 
            this.txtPaths.Location = new System.Drawing.Point(15, 335);
            this.txtPaths.Multiline = true;
            this.txtPaths.Name = "txtPaths";
            this.txtPaths.Size = new System.Drawing.Size(146, 212);
            this.txtPaths.TabIndex = 4;
            // 
            // btnGenerateNodes
            // 
            this.btnGenerateNodes.Location = new System.Drawing.Point(742, 14);
            this.btnGenerateNodes.Name = "btnGenerateNodes";
            this.btnGenerateNodes.Size = new System.Drawing.Size(95, 23);
            this.btnGenerateNodes.TabIndex = 5;
            this.btnGenerateNodes.Text = "Generate nodes";
            this.btnGenerateNodes.UseVisualStyleBackColor = true;
            this.btnGenerateNodes.Click += new System.EventHandler(this.btnGenerateNodes_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 319);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Paths (csv):";
            // 
            // txtFogNodes
            // 
            this.txtFogNodes.Location = new System.Drawing.Point(264, 17);
            this.txtFogNodes.Name = "txtFogNodes";
            this.txtFogNodes.Size = new System.Drawing.Size(76, 20);
            this.txtFogNodes.TabIndex = 7;
            // 
            // txtEdgeNodes
            // 
            this.txtEdgeNodes.Location = new System.Drawing.Point(419, 17);
            this.txtEdgeNodes.Name = "txtEdgeNodes";
            this.txtEdgeNodes.Size = new System.Drawing.Size(77, 20);
            this.txtEdgeNodes.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(346, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Edge nodes:";
            // 
            // btnDraw
            // 
            this.btnDraw.Location = new System.Drawing.Point(843, 14);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(95, 23);
            this.btnDraw.TabIndex = 10;
            this.btnDraw.Text = "Draw all";
            this.btnDraw.UseVisualStyleBackColor = true;
            this.btnDraw.Click += new System.EventHandler(this.btnDraw_Click);
            // 
            // txtTimesteps
            // 
            this.txtTimesteps.Location = new System.Drawing.Point(575, 17);
            this.txtTimesteps.Name = "txtTimesteps";
            this.txtTimesteps.Size = new System.Drawing.Size(77, 20);
            this.txtTimesteps.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(502, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Timesteps:";
            // 
            // btnSim
            // 
            this.btnSim.Location = new System.Drawing.Point(742, 46);
            this.btnSim.Name = "btnSim";
            this.btnSim.Size = new System.Drawing.Size(95, 23);
            this.btnSim.TabIndex = 13;
            this.btnSim.Text = "Simulate";
            this.btnSim.UseVisualStyleBackColor = true;
            this.btnSim.Click += new System.EventHandler(this.btnSim_Click);
            // 
            // txtVectors
            // 
            this.txtVectors.Location = new System.Drawing.Point(742, 139);
            this.txtVectors.Multiline = true;
            this.txtVectors.Name = "txtVectors";
            this.txtVectors.Size = new System.Drawing.Size(196, 256);
            this.txtVectors.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(739, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(170, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Distance vectors at timestep (csv):";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(739, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Timestep:";
            // 
            // lblTimestep
            // 
            this.lblTimestep.AutoSize = true;
            this.lblTimestep.Location = new System.Drawing.Point(798, 96);
            this.lblTimestep.Name = "lblTimestep";
            this.lblTimestep.Size = new System.Drawing.Size(24, 13);
            this.lblTimestep.TabIndex = 17;
            this.lblTimestep.Text = "0/0";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(843, 46);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(95, 23);
            this.btnExport.TabIndex = 18;
            this.btnExport.Text = "Export metrics";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 585);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.lblTimestep);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtVectors);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnSim);
            this.Controls.Add(this.txtTimesteps);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnDraw);
            this.Controls.Add(this.txtEdgeNodes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtFogNodes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnGenerateNodes);
            this.Controls.Add(this.txtPaths);
            this.Controls.Add(this.txtWaypoints);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlTopo);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTopo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtWaypoints;
        private System.Windows.Forms.TextBox txtPaths;
        private System.Windows.Forms.Button btnGenerateNodes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFogNodes;
        private System.Windows.Forms.TextBox txtEdgeNodes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDraw;
        private System.Windows.Forms.TextBox txtTimesteps;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSim;
        private System.Windows.Forms.TextBox txtVectors;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblTimestep;
        private System.Windows.Forms.Button btnExport;
    }
}
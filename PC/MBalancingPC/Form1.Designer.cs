namespace MBalancingPC
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
            this.components = new System.ComponentModel.Container();
            this.timerUpdateGUI = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblRPM = new System.Windows.Forms.Label();
            this.lblSpeedHz = new System.Windows.Forms.Label();
            this.timerProcData = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblPhaseDiff = new System.Windows.Forms.Label();
            this.lblPhaseY = new System.Windows.Forms.Label();
            this.lblPhaseX = new System.Windows.Forms.Label();
            this.lblVibroY = new System.Windows.Forms.Label();
            this.lblVibroX = new System.Windows.Forms.Label();
            this.lblRatioVibro = new System.Windows.Forms.Label();
            this.chartControl1 = new MBalancingPC.ChartControl();
            this.serialCommControl1 = new MBalancingPC.SerialCommControl();
            this.radialPlot1 = new MBalancingPC.RadialPlot();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerUpdateGUI
            // 
            this.timerUpdateGUI.Tick += new System.EventHandler(this.timerUpdateGUI_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblRPM);
            this.groupBox1.Controls.Add(this.lblSpeedHz);
            this.groupBox1.Location = new System.Drawing.Point(1034, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(138, 80);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Speed";
            // 
            // lblRPM
            // 
            this.lblRPM.AutoSize = true;
            this.lblRPM.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblRPM.Location = new System.Drawing.Point(8, 23);
            this.lblRPM.Name = "lblRPM";
            this.lblRPM.Size = new System.Drawing.Size(78, 20);
            this.lblRPM.TabIndex = 1;
            this.lblRPM.Text = "RPM: N/A";
            // 
            // lblSpeedHz
            // 
            this.lblSpeedHz.AutoSize = true;
            this.lblSpeedHz.Location = new System.Drawing.Point(9, 48);
            this.lblSpeedHz.Name = "lblSpeedHz";
            this.lblSpeedHz.Size = new System.Drawing.Size(64, 13);
            this.lblSpeedHz.TabIndex = 0;
            this.lblSpeedHz.Text = "Speed: N/A";
            // 
            // timerProcData
            // 
            this.timerProcData.Interval = 50;
            this.timerProcData.Tick += new System.EventHandler(this.timerProcData_Tick);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.lblRatioVibro);
            this.groupBox2.Controls.Add(this.lblPhaseDiff);
            this.groupBox2.Controls.Add(this.lblPhaseY);
            this.groupBox2.Controls.Add(this.lblPhaseX);
            this.groupBox2.Controls.Add(this.lblVibroY);
            this.groupBox2.Controls.Add(this.lblVibroX);
            this.groupBox2.Location = new System.Drawing.Point(825, 97);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(347, 112);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Measurements";
            // 
            // lblPhaseDiff
            // 
            this.lblPhaseDiff.AutoSize = true;
            this.lblPhaseDiff.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPhaseDiff.Location = new System.Drawing.Point(121, 71);
            this.lblPhaseDiff.Name = "lblPhaseDiff";
            this.lblPhaseDiff.Size = new System.Drawing.Size(68, 20);
            this.lblPhaseDiff.TabIndex = 6;
            this.lblPhaseDiff.Text = "Diff: N/A";
            // 
            // lblPhaseY
            // 
            this.lblPhaseY.AutoSize = true;
            this.lblPhaseY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPhaseY.Location = new System.Drawing.Point(121, 47);
            this.lblPhaseY.Name = "lblPhaseY";
            this.lblPhaseY.Size = new System.Drawing.Size(99, 20);
            this.lblPhaseY.TabIndex = 5;
            this.lblPhaseY.Text = "PhaseY: N/A";
            // 
            // lblPhaseX
            // 
            this.lblPhaseX.AutoSize = true;
            this.lblPhaseX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPhaseX.Location = new System.Drawing.Point(120, 20);
            this.lblPhaseX.Name = "lblPhaseX";
            this.lblPhaseX.Size = new System.Drawing.Size(99, 20);
            this.lblPhaseX.TabIndex = 4;
            this.lblPhaseX.Text = "PhaseX: N/A";
            // 
            // lblVibroY
            // 
            this.lblVibroY.AutoSize = true;
            this.lblVibroY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblVibroY.Location = new System.Drawing.Point(8, 50);
            this.lblVibroY.Name = "lblVibroY";
            this.lblVibroY.Size = new System.Drawing.Size(91, 20);
            this.lblVibroY.TabIndex = 3;
            this.lblVibroY.Text = "VibroY: N/A";
            // 
            // lblVibroX
            // 
            this.lblVibroX.AutoSize = true;
            this.lblVibroX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblVibroX.Location = new System.Drawing.Point(8, 20);
            this.lblVibroX.Name = "lblVibroX";
            this.lblVibroX.Size = new System.Drawing.Size(91, 20);
            this.lblVibroX.TabIndex = 2;
            this.lblVibroX.Text = "VibroX: N/A";
            // 
            // lblRatioVibro
            // 
            this.lblRatioVibro.AutoSize = true;
            this.lblRatioVibro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblRatioVibro.Location = new System.Drawing.Point(11, 77);
            this.lblRatioVibro.Name = "lblRatioVibro";
            this.lblRatioVibro.Size = new System.Drawing.Size(69, 16);
            this.lblRatioVibro.TabIndex = 2;
            this.lblRatioVibro.Text = "Ratio: N/A";
            // 
            // chartControl1
            // 
            this.chartControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartControl1.Location = new System.Drawing.Point(12, 12);
            this.chartControl1.Name = "chartControl1";
            this.chartControl1.Size = new System.Drawing.Size(807, 542);
            this.chartControl1.TabIndex = 2;
            // 
            // serialCommControl1
            // 
            this.serialCommControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.serialCommControl1.Location = new System.Drawing.Point(825, 12);
            this.serialCommControl1.Name = "serialCommControl1";
            this.serialCommControl1.Size = new System.Drawing.Size(200, 79);
            this.serialCommControl1.TabIndex = 0;
            // 
            // radialPlot1
            // 
            this.radialPlot1.Location = new System.Drawing.Point(825, 215);
            this.radialPlot1.Name = "radialPlot1";
            this.radialPlot1.Size = new System.Drawing.Size(347, 334);
            this.radialPlot1.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 561);
            this.Controls.Add(this.radialPlot1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chartControl1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.serialCommControl1);
            this.Name = "Form1";
            this.Text = "MBalancing v1.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SerialCommControl serialCommControl1;
        private System.Windows.Forms.Timer timerUpdateGUI;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblSpeedHz;
        private System.Windows.Forms.Label lblRPM;
        private System.Windows.Forms.Timer timerProcData;
        private ChartControl chartControl1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblVibroX;
        private System.Windows.Forms.Label lblVibroY;
        private System.Windows.Forms.Label lblPhaseX;
        private System.Windows.Forms.Label lblPhaseY;
        private System.Windows.Forms.Label lblPhaseDiff;
        private System.Windows.Forms.Label lblRatioVibro;
        private RadialPlot radialPlot1;
    }
}


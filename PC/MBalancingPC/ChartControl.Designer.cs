namespace MBalancingPC
{
    partial class ChartControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.nudManualYRange = new System.Windows.Forms.NumericUpDown();
            this.chkManualYRange = new System.Windows.Forms.CheckBox();
            this.btnResetZoom = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudManualYRange)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.nudManualYRange);
            this.groupBox1.Controls.Add(this.chkManualYRange);
            this.groupBox1.Controls.Add(this.btnResetZoom);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(743, 451);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Plot";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(6, 278);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(731, 167);
            this.panel2.TabIndex = 1;
            // 
            // nudManualYRange
            // 
            this.nudManualYRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudManualYRange.DecimalPlaces = 1;
            this.nudManualYRange.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudManualYRange.Location = new System.Drawing.Point(676, 79);
            this.nudManualYRange.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.nudManualYRange.Name = "nudManualYRange";
            this.nudManualYRange.Size = new System.Drawing.Size(50, 20);
            this.nudManualYRange.TabIndex = 3;
            this.nudManualYRange.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkManualYRange
            // 
            this.chkManualYRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkManualYRange.AutoSize = true;
            this.chkManualYRange.Location = new System.Drawing.Point(663, 56);
            this.chkManualYRange.Name = "chkManualYRange";
            this.chkManualYRange.Size = new System.Drawing.Size(74, 17);
            this.chkManualYRange.TabIndex = 2;
            this.chkManualYRange.Text = "Manual Y:";
            this.chkManualYRange.UseVisualStyleBackColor = true;
            this.chkManualYRange.CheckedChanged += new System.EventHandler(this.chkManualYRange_CheckedChanged);
            // 
            // btnResetZoom
            // 
            this.btnResetZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetZoom.Location = new System.Drawing.Point(651, 19);
            this.btnResetZoom.Name = "btnResetZoom";
            this.btnResetZoom.Size = new System.Drawing.Size(75, 23);
            this.btnResetZoom.TabIndex = 1;
            this.btnResetZoom.Text = "Zoom Reset";
            this.btnResetZoom.UseVisualStyleBackColor = true;
            this.btnResetZoom.Click += new System.EventHandler(this.btnResetZoom_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(6, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(628, 162);
            this.panel1.TabIndex = 0;
            // 
            // ChartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "ChartControl";
            this.Size = new System.Drawing.Size(743, 451);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudManualYRange)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnResetZoom;
        private System.Windows.Forms.NumericUpDown nudManualYRange;
        private System.Windows.Forms.CheckBox chkManualYRange;
        private System.Windows.Forms.Panel panel2;
    }
}

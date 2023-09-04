namespace MBalancingPC
{
    partial class SerialCommControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBoxMain = new System.Windows.Forms.GroupBox();
            this.lblRun = new System.Windows.Forms.Label();
            this.lblPortState = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbSelectPort = new System.Windows.Forms.ComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBoxMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxMain
            // 
            this.groupBoxMain.Controls.Add(this.lblRun);
            this.groupBoxMain.Controls.Add(this.lblPortState);
            this.groupBoxMain.Controls.Add(this.label1);
            this.groupBoxMain.Controls.Add(this.cbSelectPort);
            this.groupBoxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxMain.Location = new System.Drawing.Point(0, 0);
            this.groupBoxMain.Name = "groupBoxMain";
            this.groupBoxMain.Size = new System.Drawing.Size(297, 77);
            this.groupBoxMain.TabIndex = 0;
            this.groupBoxMain.TabStop = false;
            this.groupBoxMain.Text = "Serial Port";
            // 
            // lblRun
            // 
            this.lblRun.BackColor = System.Drawing.Color.LightGray;
            this.lblRun.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblRun.Location = new System.Drawing.Point(8, 52);
            this.lblRun.Name = "lblRun";
            this.lblRun.Size = new System.Drawing.Size(60, 15);
            this.lblRun.TabIndex = 8;
            this.lblRun.Text = "STOPPED";
            this.lblRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPortState
            // 
            this.lblPortState.BackColor = System.Drawing.Color.Red;
            this.lblPortState.Location = new System.Drawing.Point(73, 52);
            this.lblPortState.Name = "lblPortState";
            this.lblPortState.Size = new System.Drawing.Size(60, 15);
            this.lblPortState.TabIndex = 7;
            this.lblPortState.Text = "CLOSED";
            this.lblPortState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Port:";
            // 
            // cbSelectPort
            // 
            this.cbSelectPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSelectPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectPort.FormattingEnabled = true;
            this.cbSelectPort.Location = new System.Drawing.Point(40, 21);
            this.cbSelectPort.Name = "cbSelectPort";
            this.cbSelectPort.Size = new System.Drawing.Size(251, 21);
            this.cbSelectPort.TabIndex = 4;
            this.cbSelectPort.DropDown += new System.EventHandler(this.cbSelectPort_DropDown);
            this.cbSelectPort.SelectedIndexChanged += new System.EventHandler(this.cbSelectPort_SelectedIndexChanged);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // SerialCommControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxMain);
            this.Name = "SerialCommControl";
            this.Size = new System.Drawing.Size(297, 77);
            this.groupBoxMain.ResumeLayout(false);
            this.groupBoxMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbSelectPort;
        private System.Windows.Forms.Label lblPortState;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblRun;
    }
}

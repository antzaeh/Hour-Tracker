using System;
using System.Windows.Forms;

namespace TuntiPorttiUser
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.txtStart = new System.Windows.Forms.TextBox();
            this.txtEnd = new System.Windows.Forms.TextBox();
            this.btnAddEntry = new System.Windows.Forms.Button();
            this.dgvEntries = new System.Windows.Forms.DataGridView();
            this.btnStartTimer = new System.Windows.Forms.Button();
            this.btnStopTimer = new System.Windows.Forms.Button();
            this.lblElapsedTime = new System.Windows.Forms.Label();
            this.btnDeleteEntry = new System.Windows.Forms.Button();
            this.lblTotalFlex = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.comboBoxNames = new System.Windows.Forms.ComboBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.ExelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEntries)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(17, 40);
            this.dtpDate.Margin = new System.Windows.Forms.Padding(2);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(151, 20);
            this.dtpDate.TabIndex = 0;
            // 
            // txtStart
            // 
            this.txtStart.Location = new System.Drawing.Point(172, 40);
            this.txtStart.Margin = new System.Windows.Forms.Padding(2);
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(76, 20);
            this.txtStart.TabIndex = 1;
            this.txtStart.Text = "HH:mm";
            // 
            // txtEnd
            // 
            this.txtEnd.Location = new System.Drawing.Point(252, 40);
            this.txtEnd.Margin = new System.Windows.Forms.Padding(2);
            this.txtEnd.Name = "txtEnd";
            this.txtEnd.Size = new System.Drawing.Size(76, 20);
            this.txtEnd.TabIndex = 2;
            this.txtEnd.Text = "HH:mm";
            // 
            // btnAddEntry
            // 
            this.btnAddEntry.Location = new System.Drawing.Point(332, 40);
            this.btnAddEntry.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddEntry.Name = "btnAddEntry";
            this.btnAddEntry.Size = new System.Drawing.Size(56, 19);
            this.btnAddEntry.TabIndex = 4;
            this.btnAddEntry.Text = "Add Entry";
            this.btnAddEntry.UseVisualStyleBackColor = true;
            this.btnAddEntry.Click += new System.EventHandler(this.btnAddEntry_Click);
            // 
            // dgvEntries
            // 
            this.dgvEntries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEntries.Location = new System.Drawing.Point(9, 73);
            this.dgvEntries.Margin = new System.Windows.Forms.Padding(2);
            this.dgvEntries.Name = "dgvEntries";
            this.dgvEntries.RowHeadersWidth = 51;
            this.dgvEntries.RowTemplate.Height = 24;
            this.dgvEntries.Size = new System.Drawing.Size(708, 223);
            this.dgvEntries.TabIndex = 7;
            // 
            // btnStartTimer
            // 
            this.btnStartTimer.Location = new System.Drawing.Point(12, 301);
            this.btnStartTimer.Name = "btnStartTimer";
            this.btnStartTimer.Size = new System.Drawing.Size(75, 23);
            this.btnStartTimer.TabIndex = 8;
            this.btnStartTimer.Text = "Start Timer";
            this.btnStartTimer.UseVisualStyleBackColor = true;
            this.btnStartTimer.Click += new System.EventHandler(this.btnStartTimer_Click);
            // 
            // btnStopTimer
            // 
            this.btnStopTimer.Location = new System.Drawing.Point(93, 301);
            this.btnStopTimer.Name = "btnStopTimer";
            this.btnStopTimer.Size = new System.Drawing.Size(75, 23);
            this.btnStopTimer.TabIndex = 9;
            this.btnStopTimer.Text = "Stop Timer";
            this.btnStopTimer.UseVisualStyleBackColor = true;
            this.btnStopTimer.Click += new System.EventHandler(this.btnStopTimer_Click);
            // 
            // lblElapsedTime
            // 
            this.lblElapsedTime.AutoSize = true;
            this.lblElapsedTime.Location = new System.Drawing.Point(12, 325);
            this.lblElapsedTime.Name = "lblElapsedTime";
            this.lblElapsedTime.Size = new System.Drawing.Size(0, 13);
            this.lblElapsedTime.TabIndex = 10;
            // 
            // btnDeleteEntry
            // 
            this.btnDeleteEntry.Location = new System.Drawing.Point(472, 301);
            this.btnDeleteEntry.Name = "btnDeleteEntry";
            this.btnDeleteEntry.Size = new System.Drawing.Size(120, 23);
            this.btnDeleteEntry.TabIndex = 10;
            this.btnDeleteEntry.Text = "Delete Selected Entry";
            this.btnDeleteEntry.UseVisualStyleBackColor = true;
            this.btnDeleteEntry.Click += new System.EventHandler(this.btnDeleteEntry_Click);
            // 
            // lblTotalFlex
            // 
            this.lblTotalFlex.AutoSize = true;
            this.lblTotalFlex.Location = new System.Drawing.Point(393, 43);
            this.lblTotalFlex.Name = "lblTotalFlex";
            this.lblTotalFlex.Size = new System.Drawing.Size(114, 13);
            this.lblTotalFlex.TabIndex = 11;
            this.lblTotalFlex.Text = "Cumulative Flex: 00:00";
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Location = new System.Drawing.Point(12, 9);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(0, 13);
            this.lblWelcome.TabIndex = 12;
            // 
            // comboBoxNames
            // 
            this.comboBoxNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNames.Location = new System.Drawing.Point(17, 370);
            this.comboBoxNames.Name = "comboBoxNames";
            this.comboBoxNames.Size = new System.Drawing.Size(200, 21);
            this.comboBoxNames.TabIndex = 13;
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // ExelButton
            // 
            this.ExelButton.Location = new System.Drawing.Point(252, 314);
            this.ExelButton.Name = "button1";
            this.ExelButton.Size = new System.Drawing.Size(75, 23);
            this.ExelButton.TabIndex = 14;
            this.ExelButton.Text = "ExelButton";
            this.ExelButton.UseVisualStyleBackColor = true;
            this.ExelButton.Click += new System.EventHandler(this.ExelButton_Click);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(748, 490);
            this.Controls.Add(this.ExelButton);
            this.Controls.Add(this.comboBoxNames);
            this.Controls.Add(this.lblElapsedTime);
            this.Controls.Add(this.lblTotalFlex);
            this.Controls.Add(this.btnStopTimer);
            this.Controls.Add(this.btnStartTimer);
            this.Controls.Add(this.dgvEntries);
            this.Controls.Add(this.btnAddEntry);
            this.Controls.Add(this.txtEnd);
            this.Controls.Add(this.txtStart);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.btnDeleteEntry);
            this.Controls.Add(this.lblWelcome);
            this.Name = "MainForm";
            this.Text = "Work Hours Tracker";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEntries)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.TextBox txtEnd;
        private System.Windows.Forms.Button btnAddEntry;
        private System.Windows.Forms.DataGridView dgvEntries;
        private System.Windows.Forms.Button btnStartTimer;
        private System.Windows.Forms.Button btnStopTimer;
        private System.Windows.Forms.Label lblElapsedTime;
        private System.Windows.Forms.Label lblTotalFlex;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button btnDeleteEntry;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.ComboBox comboBoxNames;  // ComboBox declaration
        private System.Windows.Forms.Button ExelButton;
    }
}

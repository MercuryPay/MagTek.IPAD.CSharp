namespace IPADDemo
{
    partial class Tester
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Tester));
            this.ddlCommands = new System.Windows.Forms.ComboBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.groupBoxParameters = new System.Windows.Forms.GroupBox();
            this.labelRangeParam6 = new System.Windows.Forms.Label();
            this.textBoxParam6 = new System.Windows.Forms.TextBox();
            this.labelParam6 = new System.Windows.Forms.Label();
            this.labelRangeParam5 = new System.Windows.Forms.Label();
            this.labelRangeParam4 = new System.Windows.Forms.Label();
            this.labelRangeParam3 = new System.Windows.Forms.Label();
            this.labelRangeParam2 = new System.Windows.Forms.Label();
            this.labelRangeParam1 = new System.Windows.Forms.Label();
            this.textBoxParam5 = new System.Windows.Forms.TextBox();
            this.textBoxParam4 = new System.Windows.Forms.TextBox();
            this.textBoxParam3 = new System.Windows.Forms.TextBox();
            this.labelParam5 = new System.Windows.Forms.Label();
            this.labelParam4 = new System.Windows.Forms.Label();
            this.labelParam3 = new System.Windows.Forms.Label();
            this.textBoxParam2 = new System.Windows.Forms.TextBox();
            this.labelParam2 = new System.Windows.Forms.Label();
            this.labelParam1 = new System.Windows.Forms.Label();
            this.textBoxParam1 = new System.Windows.Forms.TextBox();
            this.btnValidate = new System.Windows.Forms.Button();
            this.richTextBoxStatus = new System.Windows.Forms.RichTextBox();
            this.richTextBoxEventResult = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxClearEventWindow = new System.Windows.Forms.CheckBox();
            this.gBoxSelectReportType = new System.Windows.Forms.GroupBox();
            this.rBtnDispMsg = new System.Windows.Forms.RadioButton();
            this.rBtnReqUsrSelect = new System.Windows.Forms.RadioButton();
            this.textBoxMsg = new System.Windows.Forms.TextBox();
            this.btnAcceptMsg = new System.Windows.Forms.Button();
            this.lMsg = new System.Windows.Forms.Label();
            this.Underline = new System.Windows.Forms.CheckBox();
            this.BackgroundClr = new System.Windows.Forms.CheckBox();
            this.QwickCodes = new System.Windows.Forms.CheckBox();
            this.PANinPIN = new System.Windows.Forms.CheckBox();
            this.groupBoxParameters.SuspendLayout();
            this.gBoxSelectReportType.SuspendLayout();
            this.SuspendLayout();
            // 
            // ddlCommands
            // 
            this.ddlCommands.FormattingEnabled = true;
            this.ddlCommands.Items.AddRange(new object[] {
            "Display Msg",
            "Send MultiData",//HS 6/28/2011
            "Request Manual Card Entry",//HS 6/28/2011
            "Request PIN",
            "Request Card",
            "Get Response",
            "Confirm Amount",
            "Select Credit Debit",
            "Halt Operation",
            "End Session",
            "Send Amount",
            "Get Status"});
            this.ddlCommands.Location = new System.Drawing.Point(12, 12);
            this.ddlCommands.Name = "ddlCommands";
            this.ddlCommands.Size = new System.Drawing.Size(259, 21);
            this.ddlCommands.TabIndex = 0;
            this.ddlCommands.SelectedIndexChanged += new System.EventHandler(this.ddlCommands_SelectedIndexChanged);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(298, 12);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(84, 22);
            this.btnRun.TabIndex = 1;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(618, 624);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(84, 22);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(708, 624);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 22);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblStatus.Location = new System.Drawing.Point(12, 623);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(498, 21);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "...";
            // 
            // groupBoxParameters
            // 
            this.groupBoxParameters.Controls.Add(this.textBoxMsg);
            this.groupBoxParameters.Controls.Add(this.btnAcceptMsg);
            this.groupBoxParameters.Controls.Add(this.lMsg);
            this.groupBoxParameters.Controls.Add(this.Underline);
            this.groupBoxParameters.Controls.Add(this.BackgroundClr);
            this.groupBoxParameters.Controls.Add(this.QwickCodes);
            this.groupBoxParameters.Controls.Add(this.PANinPIN);
            this.groupBoxParameters.Controls.Add(this.gBoxSelectReportType);
            this.groupBoxParameters.Controls.Add(this.labelRangeParam6);
            this.groupBoxParameters.Controls.Add(this.textBoxParam6);
            this.groupBoxParameters.Controls.Add(this.labelParam6);
            this.groupBoxParameters.Controls.Add(this.labelRangeParam5);
            this.groupBoxParameters.Controls.Add(this.labelRangeParam4);
            this.groupBoxParameters.Controls.Add(this.labelRangeParam3);
            this.groupBoxParameters.Controls.Add(this.labelRangeParam2);
            this.groupBoxParameters.Controls.Add(this.labelRangeParam1);
            this.groupBoxParameters.Controls.Add(this.textBoxParam5);
            this.groupBoxParameters.Controls.Add(this.textBoxParam4);
            this.groupBoxParameters.Controls.Add(this.textBoxParam3);
            this.groupBoxParameters.Controls.Add(this.labelParam5);
            this.groupBoxParameters.Controls.Add(this.labelParam4);
            this.groupBoxParameters.Controls.Add(this.labelParam3);
            this.groupBoxParameters.Controls.Add(this.textBoxParam2);
            this.groupBoxParameters.Controls.Add(this.labelParam2);
            this.groupBoxParameters.Controls.Add(this.labelParam1);
            this.groupBoxParameters.Controls.Add(this.textBoxParam1);
            this.groupBoxParameters.Location = new System.Drawing.Point(12, 40);
            this.groupBoxParameters.Name = "groupBoxParameters";
            this.groupBoxParameters.Size = new System.Drawing.Size(780, 223);
            this.groupBoxParameters.TabIndex = 7;
            this.groupBoxParameters.TabStop = false;
            this.groupBoxParameters.Text = "Parameters";
            // 
            // labelRangeParam6
            // 
            this.labelRangeParam6.AutoSize = true;
            this.labelRangeParam6.Location = new System.Drawing.Point(235, 173);
            this.labelRangeParam6.Name = "labelRangeParam6";
            this.labelRangeParam6.Size = new System.Drawing.Size(35, 13);
            this.labelRangeParam6.TabIndex = 24;
            this.labelRangeParam6.Text = "label3";
            // 
            // textBoxParam6
            // 
            this.textBoxParam6.Location = new System.Drawing.Point(119, 170);
            this.textBoxParam6.Name = "textBoxParam6";
            this.textBoxParam6.Size = new System.Drawing.Size(97, 20);
            this.textBoxParam6.TabIndex = 23;
            // 
            // labelParam6
            // 
            this.labelParam6.AutoSize = true;
            this.labelParam6.Location = new System.Drawing.Point(6, 173);
            this.labelParam6.Name = "labelParam6";
            this.labelParam6.Size = new System.Drawing.Size(46, 13);
            this.labelParam6.TabIndex = 22;
            this.labelParam6.Text = "Param6:";
            // 
            // labelRangeParam5
            // 
            this.labelRangeParam5.AutoSize = true;
            this.labelRangeParam5.Location = new System.Drawing.Point(235, 138);
            this.labelRangeParam5.Name = "labelRangeParam5";
            this.labelRangeParam5.Size = new System.Drawing.Size(35, 13);
            this.labelRangeParam5.TabIndex = 21;
            this.labelRangeParam5.Text = "label3";
            // 
            // labelRangeParam4
            // 
            this.labelRangeParam4.AutoSize = true;
            this.labelRangeParam4.Location = new System.Drawing.Point(235, 107);
            this.labelRangeParam4.Name = "labelRangeParam4";
            this.labelRangeParam4.Size = new System.Drawing.Size(35, 13);
            this.labelRangeParam4.TabIndex = 20;
            this.labelRangeParam4.Text = "label2";
            // 
            // labelRangeParam3
            // 
            this.labelRangeParam3.AutoSize = true;
            this.labelRangeParam3.Location = new System.Drawing.Point(235, 76);
            this.labelRangeParam3.Name = "labelRangeParam3";
            this.labelRangeParam3.Size = new System.Drawing.Size(35, 13);
            this.labelRangeParam3.TabIndex = 19;
            this.labelRangeParam3.Text = "label1";
            // 
            // labelRangeParam2
            // 
            this.labelRangeParam2.AutoSize = true;
            this.labelRangeParam2.Location = new System.Drawing.Point(235, 46);
            this.labelRangeParam2.Name = "labelRangeParam2";
            this.labelRangeParam2.Size = new System.Drawing.Size(35, 13);
            this.labelRangeParam2.TabIndex = 18;
            this.labelRangeParam2.Text = "label1";
            // 
            // labelRangeParam1
            // 
            this.labelRangeParam1.AutoSize = true;
            this.labelRangeParam1.Location = new System.Drawing.Point(235, 18);
            this.labelRangeParam1.Name = "labelRangeParam1";
            this.labelRangeParam1.Size = new System.Drawing.Size(35, 13);
            this.labelRangeParam1.TabIndex = 17;
            this.labelRangeParam1.Text = "label1";
            // 
            // textBoxParam5
            // 
            this.textBoxParam5.Location = new System.Drawing.Point(119, 138);
            this.textBoxParam5.Name = "textBoxParam5";
            this.textBoxParam5.Size = new System.Drawing.Size(97, 20);
            this.textBoxParam5.TabIndex = 16;
            // 
            // textBoxParam4
            // 
            this.textBoxParam4.Location = new System.Drawing.Point(119, 107);
            this.textBoxParam4.Name = "textBoxParam4";
            this.textBoxParam4.Size = new System.Drawing.Size(97, 20);
            this.textBoxParam4.TabIndex = 15;
            // 
            // textBoxParam3
            // 
            this.textBoxParam3.Location = new System.Drawing.Point(119, 76);
            this.textBoxParam3.Name = "textBoxParam3";
            this.textBoxParam3.Size = new System.Drawing.Size(97, 20);
            this.textBoxParam3.TabIndex = 14;
            // 
            // labelParam5
            // 
            this.labelParam5.AutoSize = true;
            this.labelParam5.Location = new System.Drawing.Point(6, 138);
            this.labelParam5.Name = "labelParam5";
            this.labelParam5.Size = new System.Drawing.Size(46, 13);
            this.labelParam5.TabIndex = 13;
            this.labelParam5.Text = "Param5:";
            // 
            // labelParam4
            // 
            this.labelParam4.AutoSize = true;
            this.labelParam4.Location = new System.Drawing.Point(6, 107);
            this.labelParam4.Name = "labelParam4";
            this.labelParam4.Size = new System.Drawing.Size(46, 13);
            this.labelParam4.TabIndex = 12;
            this.labelParam4.Text = "Param4:";
            // 
            // labelParam3
            // 
            this.labelParam3.AutoSize = true;
            this.labelParam3.Location = new System.Drawing.Point(6, 76);
            this.labelParam3.Name = "labelParam3";
            this.labelParam3.Size = new System.Drawing.Size(46, 13);
            this.labelParam3.TabIndex = 11;
            this.labelParam3.Text = "Param3:";
            // 
            // textBoxParam2
            // 
            this.textBoxParam2.Location = new System.Drawing.Point(119, 46);
            this.textBoxParam2.Name = "textBoxParam2";
            this.textBoxParam2.Size = new System.Drawing.Size(97, 20);
            this.textBoxParam2.TabIndex = 10;
            // 
            // labelParam2
            // 
            this.labelParam2.AutoSize = true;
            this.labelParam2.Location = new System.Drawing.Point(6, 46);
            this.labelParam2.Name = "labelParam2";
            this.labelParam2.Size = new System.Drawing.Size(46, 13);
            this.labelParam2.TabIndex = 9;
            this.labelParam2.Text = "Param2:";
            // 
            // labelParam1
            // 
            this.labelParam1.AutoSize = true;
            this.labelParam1.Location = new System.Drawing.Point(6, 18);
            this.labelParam1.Name = "labelParam1";
            this.labelParam1.Size = new System.Drawing.Size(46, 13);
            this.labelParam1.TabIndex = 8;
            this.labelParam1.Text = "Param1:";
            // 
            // textBoxParam1
            // 
            this.textBoxParam1.Location = new System.Drawing.Point(119, 18);
            this.textBoxParam1.MaxLength = 3;
            this.textBoxParam1.Name = "textBoxParam1";
            this.textBoxParam1.Size = new System.Drawing.Size(97, 20);
            this.textBoxParam1.TabIndex = 7;
            // 
            // btnValidate
            // 
            this.btnValidate.Location = new System.Drawing.Point(440, 12);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(107, 22);
            this.btnValidate.TabIndex = 8;
            this.btnValidate.Text = "Validate";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // richTextBoxStatus
            // 
            this.richTextBoxStatus.Location = new System.Drawing.Point(12, 452);
            this.richTextBoxStatus.Name = "richTextBoxStatus";
            this.richTextBoxStatus.Size = new System.Drawing.Size(780, 150);
            this.richTextBoxStatus.TabIndex = 9;
            this.richTextBoxStatus.Text = "";
            // 
            // richTextBoxEventResult
            // 
            this.richTextBoxEventResult.Location = new System.Drawing.Point(12, 286);
            this.richTextBoxEventResult.Name = "richTextBoxEventResult";
            this.richTextBoxEventResult.Size = new System.Drawing.Size(780, 138);
            this.richTextBoxEventResult.TabIndex = 10;
            this.richTextBoxEventResult.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Location = new System.Drawing.Point(13, 269);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "Events:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Fuchsia;
            this.label2.Location = new System.Drawing.Point(14, 432);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "Tests:";
            // 
            // checkBoxClearEventWindow
            // 
            this.checkBoxClearEventWindow.AutoSize = true;
            this.checkBoxClearEventWindow.Location = new System.Drawing.Point(71, 269);
            this.checkBoxClearEventWindow.Name = "checkBoxClearEventWindow";
            this.checkBoxClearEventWindow.Size = new System.Drawing.Size(270, 17);
            this.checkBoxClearEventWindow.TabIndex = 13;
            this.checkBoxClearEventWindow.Text = "Clear event window before starting a new command";
            this.checkBoxClearEventWindow.UseVisualStyleBackColor = true;
            // 
            // gBoxSelectReportType
            // 
            this.gBoxSelectReportType.Controls.Add(this.rBtnDispMsg);
            this.gBoxSelectReportType.Controls.Add(this.rBtnReqUsrSelect);
            this.gBoxSelectReportType.Location = new System.Drawing.Point(428, 19);
            this.gBoxSelectReportType.Name = "gBoxSelectReportType";
            this.gBoxSelectReportType.Size = new System.Drawing.Size(202, 56);
            this.gBoxSelectReportType.TabIndex = 33;
            this.gBoxSelectReportType.TabStop = false;
            this.gBoxSelectReportType.Text = "Select Report Type:";
            // 
            // rBtnDispMsg
            // 
            this.rBtnDispMsg.AutoSize = true;
            this.rBtnDispMsg.Location = new System.Drawing.Point(22, 31);
            this.rBtnDispMsg.Name = "rBtnDispMsg";
            this.rBtnDispMsg.Size = new System.Drawing.Size(105, 17);
            this.rBtnDispMsg.TabIndex = 32;
            this.rBtnDispMsg.TabStop = true;
            this.rBtnDispMsg.Text = "Display Message";
            this.rBtnDispMsg.UseVisualStyleBackColor = true;
            // 
            // rBtnReqUsrSelect
            // 
            this.rBtnReqUsrSelect.AutoSize = true;
            this.rBtnReqUsrSelect.Location = new System.Drawing.Point(22, 15);
            this.rBtnReqUsrSelect.Name = "rBtnReqUsrSelect";
            this.rBtnReqUsrSelect.Size = new System.Drawing.Size(137, 17);
            this.rBtnReqUsrSelect.TabIndex = 31;
            this.rBtnReqUsrSelect.TabStop = true;
            this.rBtnReqUsrSelect.Text = "Request User Selection";
            this.rBtnReqUsrSelect.UseVisualStyleBackColor = true;
            // 
            // textBoxMsg
            // 
            this.textBoxMsg.Location = new System.Drawing.Point(428, 182);
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.Size = new System.Drawing.Size(225, 20);
            this.textBoxMsg.TabIndex = 40;
            // 
            // btnAcceptMsg
            // 
            this.btnAcceptMsg.Location = new System.Drawing.Point(659, 178);
            this.btnAcceptMsg.Name = "btnAcceptMsg";
            this.btnAcceptMsg.Size = new System.Drawing.Size(107, 27);
            this.btnAcceptMsg.TabIndex = 39;
            this.btnAcceptMsg.Text = "Accept MSG";
            this.btnAcceptMsg.UseVisualStyleBackColor = true;
            this.btnAcceptMsg.Click += new System.EventHandler(this.btnAcceptMsg_Click);
            // 
            // lMsg
            // 
            this.lMsg.AutoSize = true;
            this.lMsg.Font = new System.Drawing.Font("Verdana", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lMsg.Location = new System.Drawing.Point(425, 162);
            this.lMsg.Name = "lMsg";
            this.lMsg.Size = new System.Drawing.Size(195, 17);
            this.lMsg.TabIndex = 38;
            this.lMsg.Text = "Display Text Message # ";
            // 
            // Underline
            // 
            this.Underline.AutoSize = true;
            this.Underline.Location = new System.Drawing.Point(428, 134);
            this.Underline.Name = "Underline";
            this.Underline.Size = new System.Drawing.Size(71, 17);
            this.Underline.TabIndex = 37;
            this.Underline.Text = "Underline";
            this.Underline.UseVisualStyleBackColor = true;
            // 
            // BackgroundClr
            // 
            this.BackgroundClr.AutoSize = true;
            this.BackgroundClr.Location = new System.Drawing.Point(428, 111);
            this.BackgroundClr.Name = "BackgroundClr";
            this.BackgroundClr.Size = new System.Drawing.Size(123, 17);
            this.BackgroundClr.TabIndex = 36;
            this.BackgroundClr.Text = "Background Cleared";
            this.BackgroundClr.UseVisualStyleBackColor = true;
            // 
            // QwickCodes
            // 
            this.QwickCodes.AutoSize = true;
            this.QwickCodes.Location = new System.Drawing.Point(508, 81);
            this.QwickCodes.Name = "QwickCodes";
            this.QwickCodes.Size = new System.Drawing.Size(86, 17);
            this.QwickCodes.TabIndex = 35;
            this.QwickCodes.Text = "QwickCodes";
            this.QwickCodes.UseVisualStyleBackColor = true;
            // 
            // PANinPIN
            // 
            this.PANinPIN.AutoSize = true;
            this.PANinPIN.Location = new System.Drawing.Point(428, 81);
            this.PANinPIN.Name = "PANinPIN";
            this.PANinPIN.Size = new System.Drawing.Size(74, 17);
            this.PANinPIN.TabIndex = 34;
            this.PANinPIN.Text = "PANinPIN";
            this.PANinPIN.UseVisualStyleBackColor = true;
            // 
            // Tester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 662);
            this.Controls.Add(this.checkBoxClearEventWindow);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBoxEventResult);
            this.Controls.Add(this.richTextBoxStatus);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.ddlCommands);
            this.Controls.Add(this.groupBoxParameters);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Tester";
            this.Text = "IPAD Tester";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBoxParameters.ResumeLayout(false);
            this.groupBoxParameters.PerformLayout();
            this.gBoxSelectReportType.ResumeLayout(false);
            this.gBoxSelectReportType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ddlCommands;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox groupBoxParameters;
        private System.Windows.Forms.TextBox textBoxParam2;
        private System.Windows.Forms.Label labelParam2;
        private System.Windows.Forms.Label labelParam1;
        private System.Windows.Forms.TextBox textBoxParam1;
        private System.Windows.Forms.Label labelParam5;
        private System.Windows.Forms.Label labelParam4;
        private System.Windows.Forms.Label labelParam3;
        private System.Windows.Forms.TextBox textBoxParam5;
        private System.Windows.Forms.TextBox textBoxParam4;
        private System.Windows.Forms.TextBox textBoxParam3;
        private System.Windows.Forms.Label labelRangeParam5;
        private System.Windows.Forms.Label labelRangeParam4;
        private System.Windows.Forms.Label labelRangeParam3;
        private System.Windows.Forms.Label labelRangeParam2;
        private System.Windows.Forms.Label labelRangeParam1;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.RichTextBox richTextBoxStatus;
        private System.Windows.Forms.RichTextBox richTextBoxEventResult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxClearEventWindow;
        private System.Windows.Forms.Label labelRangeParam6;
        private System.Windows.Forms.TextBox textBoxParam6;
        private System.Windows.Forms.Label labelParam6;
        private System.Windows.Forms.TextBox textBoxMsg;
        public System.Windows.Forms.Button btnAcceptMsg;
        private System.Windows.Forms.Label lMsg;
        private System.Windows.Forms.CheckBox Underline;
        private System.Windows.Forms.CheckBox BackgroundClr;
        private System.Windows.Forms.CheckBox QwickCodes;
        private System.Windows.Forms.CheckBox PANinPIN;
        private System.Windows.Forms.GroupBox gBoxSelectReportType;
        private System.Windows.Forms.RadioButton rBtnDispMsg;
        private System.Windows.Forms.RadioButton rBtnReqUsrSelect;
    }
}


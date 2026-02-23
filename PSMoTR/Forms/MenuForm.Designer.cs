namespace PSMoTR.Forms
{
    partial class MenuForm
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
            this.B_Start = new System.Windows.Forms.Button();
            this.L_TimerMove = new System.Windows.Forms.Label();
            this.B_RoutesEditForm = new System.Windows.Forms.Button();
            this.CB_ShowPointsSpeed = new System.Windows.Forms.CheckBox();
            this.CB_ShowRoutes = new System.Windows.Forms.CheckBox();
            this.TB_WavesCount = new System.Windows.Forms.TextBox();
            this.L_WavesCount = new System.Windows.Forms.Label();
            this.B_PauseContinue = new System.Windows.Forms.Button();
            this.B_Stop = new System.Windows.Forms.Button();
            this.CB_ShowAutoWay = new System.Windows.Forms.CheckBox();
            this.TB_AutoNumber = new System.Windows.Forms.TextBox();
            this.GB_ControlPanel = new System.Windows.Forms.GroupBox();
            this.CLB_Models = new System.Windows.Forms.CheckedListBox();
            this.L_CurrentProjectName = new System.Windows.Forms.Label();
            this.CB_NewGenerateAutos = new System.Windows.Forms.CheckBox();
            this.B_EditProject = new System.Windows.Forms.Button();
            this.GB_DisplayOptions = new System.Windows.Forms.GroupBox();
            this.CB_ShowOverlook = new System.Windows.Forms.CheckBox();
            this.B_RoutesConstructor = new System.Windows.Forms.Button();
            this.B_TrafficLightEditForm = new System.Windows.Forms.Button();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.GB_EditPanel = new System.Windows.Forms.GroupBox();
            this.GB_ControlPanel.SuspendLayout();
            this.GB_DisplayOptions.SuspendLayout();
            this.GB_EditPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // B_Start
            // 
            this.B_Start.Location = new System.Drawing.Point(6, 100);
            this.B_Start.Name = "B_Start";
            this.B_Start.Size = new System.Drawing.Size(46, 23);
            this.B_Start.TabIndex = 0;
            this.B_Start.Text = "Start";
            this.B_Start.UseVisualStyleBackColor = true;
            this.B_Start.Click += new System.EventHandler(this.B_Start_Click);
            // 
            // L_TimerMove
            // 
            this.L_TimerMove.AutoSize = true;
            this.L_TimerMove.Location = new System.Drawing.Point(6, 84);
            this.L_TimerMove.Name = "L_TimerMove";
            this.L_TimerMove.Size = new System.Drawing.Size(80, 13);
            this.L_TimerMove.TabIndex = 1;
            this.L_TimerMove.Text = "{L_TimerMove}";
            // 
            // B_RoutesEditForm
            // 
            this.B_RoutesEditForm.Location = new System.Drawing.Point(6, 48);
            this.B_RoutesEditForm.Name = "B_RoutesEditForm";
            this.B_RoutesEditForm.Size = new System.Drawing.Size(277, 23);
            this.B_RoutesEditForm.TabIndex = 13;
            this.B_RoutesEditForm.Text = "Edit routes";
            this.B_RoutesEditForm.UseVisualStyleBackColor = true;
            this.B_RoutesEditForm.Click += new System.EventHandler(this.B_RoutesEditForm_Click);
            // 
            // CB_ShowPointsSpeed
            // 
            this.CB_ShowPointsSpeed.AutoSize = true;
            this.CB_ShowPointsSpeed.Location = new System.Drawing.Point(6, 42);
            this.CB_ShowPointsSpeed.Name = "CB_ShowPointsSpeed";
            this.CB_ShowPointsSpeed.Size = new System.Drawing.Size(123, 17);
            this.CB_ShowPointsSpeed.TabIndex = 8;
            this.CB_ShowPointsSpeed.Text = "Display points speed";
            this.CB_ShowPointsSpeed.UseVisualStyleBackColor = true;
            this.CB_ShowPointsSpeed.CheckedChanged += new System.EventHandler(this.CB_ShowPointsSpeed_CheckedChanged);
            // 
            // CB_ShowRoutes
            // 
            this.CB_ShowRoutes.AutoSize = true;
            this.CB_ShowRoutes.Location = new System.Drawing.Point(6, 19);
            this.CB_ShowRoutes.Name = "CB_ShowRoutes";
            this.CB_ShowRoutes.Size = new System.Drawing.Size(92, 17);
            this.CB_ShowRoutes.TabIndex = 9;
            this.CB_ShowRoutes.Text = "Display routes";
            this.CB_ShowRoutes.UseVisualStyleBackColor = true;
            this.CB_ShowRoutes.CheckedChanged += new System.EventHandler(this.CB_ShowLines_CheckedChanged);
            // 
            // TB_WavesCount
            // 
            this.TB_WavesCount.Location = new System.Drawing.Point(255, 123);
            this.TB_WavesCount.Name = "TB_WavesCount";
            this.TB_WavesCount.Size = new System.Drawing.Size(24, 20);
            this.TB_WavesCount.TabIndex = 3;
            this.TB_WavesCount.Text = "10";
            // 
            // L_WavesCount
            // 
            this.L_WavesCount.AutoSize = true;
            this.L_WavesCount.Location = new System.Drawing.Point(3, 126);
            this.L_WavesCount.Name = "L_WavesCount";
            this.L_WavesCount.Size = new System.Drawing.Size(242, 13);
            this.L_WavesCount.TabIndex = 14;
            this.L_WavesCount.Text = "The number of auto generations every 2 seconds:";
            // 
            // B_PauseContinue
            // 
            this.B_PauseContinue.Location = new System.Drawing.Point(58, 100);
            this.B_PauseContinue.Name = "B_PauseContinue";
            this.B_PauseContinue.Size = new System.Drawing.Size(120, 23);
            this.B_PauseContinue.TabIndex = 1;
            this.B_PauseContinue.Text = "Pause / Continue";
            this.B_PauseContinue.UseVisualStyleBackColor = true;
            this.B_PauseContinue.Click += new System.EventHandler(this.B_PauseContinue_Click);
            // 
            // B_Stop
            // 
            this.B_Stop.Location = new System.Drawing.Point(184, 100);
            this.B_Stop.Name = "B_Stop";
            this.B_Stop.Size = new System.Drawing.Size(46, 23);
            this.B_Stop.TabIndex = 2;
            this.B_Stop.Text = "Stop";
            this.B_Stop.UseVisualStyleBackColor = true;
            this.B_Stop.Click += new System.EventHandler(this.B_Stop_Click);
            // 
            // CB_ShowAutoWay
            // 
            this.CB_ShowAutoWay.AutoSize = true;
            this.CB_ShowAutoWay.Location = new System.Drawing.Point(6, 65);
            this.CB_ShowAutoWay.Name = "CB_ShowAutoWay";
            this.CB_ShowAutoWay.Size = new System.Drawing.Size(158, 17);
            this.CB_ShowAutoWay.TabIndex = 10;
            this.CB_ShowAutoWay.Text = "Display auto way by number";
            this.CB_ShowAutoWay.UseVisualStyleBackColor = true;
            this.CB_ShowAutoWay.CheckedChanged += new System.EventHandler(this.CB_AutoWay_CheckedChanged);
            // 
            // TB_AutoNumber
            // 
            this.TB_AutoNumber.Location = new System.Drawing.Point(170, 63);
            this.TB_AutoNumber.Name = "TB_AutoNumber";
            this.TB_AutoNumber.Size = new System.Drawing.Size(23, 20);
            this.TB_AutoNumber.TabIndex = 11;
            // 
            // GB_ControlPanel
            // 
            this.GB_ControlPanel.Controls.Add(this.CLB_Models);
            this.GB_ControlPanel.Controls.Add(this.L_CurrentProjectName);
            this.GB_ControlPanel.Controls.Add(this.CB_NewGenerateAutos);
            this.GB_ControlPanel.Controls.Add(this.B_EditProject);
            this.GB_ControlPanel.Controls.Add(this.B_Start);
            this.GB_ControlPanel.Controls.Add(this.B_PauseContinue);
            this.GB_ControlPanel.Controls.Add(this.L_TimerMove);
            this.GB_ControlPanel.Controls.Add(this.B_Stop);
            this.GB_ControlPanel.Controls.Add(this.L_WavesCount);
            this.GB_ControlPanel.Controls.Add(this.TB_WavesCount);
            this.GB_ControlPanel.Location = new System.Drawing.Point(12, 12);
            this.GB_ControlPanel.Name = "GB_ControlPanel";
            this.GB_ControlPanel.Size = new System.Drawing.Size(289, 231);
            this.GB_ControlPanel.TabIndex = 19;
            this.GB_ControlPanel.TabStop = false;
            this.GB_ControlPanel.Text = "Control panel";
            // 
            // CLB_Models
            // 
            this.CLB_Models.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CLB_Models.CheckOnClick = true;
            this.CLB_Models.ColumnWidth = 200;
            this.CLB_Models.Items.AddRange(new object[] {
            "Model 1",
            "Model 2",
            "Model 3"});
            this.CLB_Models.Location = new System.Drawing.Point(6, 161);
            this.CLB_Models.Name = "CLB_Models";
            this.CLB_Models.Size = new System.Drawing.Size(273, 64);
            this.CLB_Models.TabIndex = 15;
            this.CLB_Models.SelectedIndexChanged += new System.EventHandler(this.CLB_Models_SelectedIndexChanged);
            // 
            // L_CurrentProjectName
            // 
            this.L_CurrentProjectName.AutoSize = true;
            this.L_CurrentProjectName.Location = new System.Drawing.Point(3, 24);
            this.L_CurrentProjectName.Name = "L_CurrentProjectName";
            this.L_CurrentProjectName.Size = new System.Drawing.Size(122, 13);
            this.L_CurrentProjectName.TabIndex = 21;
            this.L_CurrentProjectName.Text = "{L_CurrentProjectName}";
            // 
            // CB_NewGenerateAutos
            // 
            this.CB_NewGenerateAutos.AutoSize = true;
            this.CB_NewGenerateAutos.Checked = true;
            this.CB_NewGenerateAutos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_NewGenerateAutos.Location = new System.Drawing.Point(6, 142);
            this.CB_NewGenerateAutos.Name = "CB_NewGenerateAutos";
            this.CB_NewGenerateAutos.Size = new System.Drawing.Size(142, 17);
            this.CB_NewGenerateAutos.TabIndex = 15;
            this.CB_NewGenerateAutos.Text = "New generation of autos";
            this.CB_NewGenerateAutos.UseVisualStyleBackColor = true;
            this.CB_NewGenerateAutos.CheckedChanged += new System.EventHandler(this.CB_NewGenerateAutos_CheckedChanged);
            // 
            // B_EditProject
            // 
            this.B_EditProject.Location = new System.Drawing.Point(136, 19);
            this.B_EditProject.Name = "B_EditProject";
            this.B_EditProject.Size = new System.Drawing.Size(143, 23);
            this.B_EditProject.TabIndex = 16;
            this.B_EditProject.Text = "Project management";
            this.B_EditProject.UseVisualStyleBackColor = true;
            this.B_EditProject.Click += new System.EventHandler(this.B_EditProject_Click);
            // 
            // GB_DisplayOptions
            // 
            this.GB_DisplayOptions.Controls.Add(this.CB_ShowOverlook);
            this.GB_DisplayOptions.Controls.Add(this.CB_ShowPointsSpeed);
            this.GB_DisplayOptions.Controls.Add(this.TB_AutoNumber);
            this.GB_DisplayOptions.Controls.Add(this.CB_ShowRoutes);
            this.GB_DisplayOptions.Controls.Add(this.CB_ShowAutoWay);
            this.GB_DisplayOptions.Location = new System.Drawing.Point(12, 249);
            this.GB_DisplayOptions.Name = "GB_DisplayOptions";
            this.GB_DisplayOptions.Size = new System.Drawing.Size(289, 108);
            this.GB_DisplayOptions.TabIndex = 20;
            this.GB_DisplayOptions.TabStop = false;
            this.GB_DisplayOptions.Text = "Display options";
            // 
            // CB_ShowOverlook
            // 
            this.CB_ShowOverlook.AutoSize = true;
            this.CB_ShowOverlook.Location = new System.Drawing.Point(6, 88);
            this.CB_ShowOverlook.Name = "CB_ShowOverlook";
            this.CB_ShowOverlook.Size = new System.Drawing.Size(128, 17);
            this.CB_ShowOverlook.TabIndex = 12;
            this.CB_ShowOverlook.Text = "Display auto overlook";
            this.CB_ShowOverlook.UseVisualStyleBackColor = true;
            this.CB_ShowOverlook.CheckedChanged += new System.EventHandler(this.CB_AutoOverlook_CheckedChanged);
            // 
            // B_RoutesConstructor
            // 
            this.B_RoutesConstructor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.B_RoutesConstructor.Location = new System.Drawing.Point(6, 19);
            this.B_RoutesConstructor.Name = "B_RoutesConstructor";
            this.B_RoutesConstructor.Size = new System.Drawing.Size(277, 23);
            this.B_RoutesConstructor.TabIndex = 15;
            this.B_RoutesConstructor.Text = "Construct routes";
            this.B_RoutesConstructor.UseVisualStyleBackColor = true;
            this.B_RoutesConstructor.Click += new System.EventHandler(this.B_RoutesConstructor_Click);
            // 
            // B_TrafficLightEditForm
            // 
            this.B_TrafficLightEditForm.Location = new System.Drawing.Point(6, 77);
            this.B_TrafficLightEditForm.Name = "B_TrafficLightEditForm";
            this.B_TrafficLightEditForm.Size = new System.Drawing.Size(277, 23);
            this.B_TrafficLightEditForm.TabIndex = 14;
            this.B_TrafficLightEditForm.Text = "Edit traffic lights";
            this.B_TrafficLightEditForm.UseVisualStyleBackColor = true;
            this.B_TrafficLightEditForm.Click += new System.EventHandler(this.B_TrafficLightEditForm_Click);
            // 
            // Timer
            // 
            this.Timer.Interval = 1000;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // GB_EditPanel
            // 
            this.GB_EditPanel.Controls.Add(this.B_TrafficLightEditForm);
            this.GB_EditPanel.Controls.Add(this.B_RoutesConstructor);
            this.GB_EditPanel.Controls.Add(this.B_RoutesEditForm);
            this.GB_EditPanel.Location = new System.Drawing.Point(12, 363);
            this.GB_EditPanel.Name = "GB_EditPanel";
            this.GB_EditPanel.Size = new System.Drawing.Size(289, 104);
            this.GB_EditPanel.TabIndex = 21;
            this.GB_EditPanel.TabStop = false;
            this.GB_EditPanel.Text = "Edit panel";
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 506);
            this.Controls.Add(this.GB_EditPanel);
            this.Controls.Add(this.GB_DisplayOptions);
            this.Controls.Add(this.GB_ControlPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MenuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Model menu";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MenuForm_FormClosing);
            this.Load += new System.EventHandler(this.MenuForm_Load);
            this.GB_ControlPanel.ResumeLayout(false);
            this.GB_ControlPanel.PerformLayout();
            this.GB_DisplayOptions.ResumeLayout(false);
            this.GB_DisplayOptions.PerformLayout();
            this.GB_EditPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button B_Start;
        protected internal System.Windows.Forms.Label L_TimerMove;
        private System.Windows.Forms.Button B_RoutesEditForm;
        protected internal System.Windows.Forms.CheckBox CB_ShowPointsSpeed;
        protected internal System.Windows.Forms.CheckBox CB_ShowRoutes;
        protected internal System.Windows.Forms.CheckBox CB_ShowAutoWay;
        private System.Windows.Forms.Label L_WavesCount;
        private System.Windows.Forms.TextBox TB_WavesCount;
        private System.Windows.Forms.Button B_PauseContinue;
        protected internal System.Windows.Forms.TextBox TB_AutoNumber;
        private System.Windows.Forms.GroupBox GB_ControlPanel;
        private System.Windows.Forms.GroupBox GB_DisplayOptions;
        protected internal System.Windows.Forms.Button B_Stop;
        protected internal System.Windows.Forms.CheckBox CB_ShowOverlook;
        private System.Windows.Forms.Button B_TrafficLightEditForm;
        private System.Windows.Forms.Timer Timer;
        protected internal System.Windows.Forms.CheckBox CB_NewGenerateAutos;
        private System.Windows.Forms.Button B_EditProject;
        protected internal System.Windows.Forms.Label L_CurrentProjectName;
        protected internal System.Windows.Forms.CheckedListBox CLB_Models;
        private System.Windows.Forms.Button B_RoutesConstructor;
        private System.Windows.Forms.GroupBox GB_EditPanel;
    }
}
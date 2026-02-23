namespace PSMoTR.Forms
{
    partial class TrafficLightsEditForm
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
            this.DGV_TrafficLights = new System.Windows.Forms.DataGridView();
            this.B_SaveFile = new System.Windows.Forms.Button();
            this.BS = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_TrafficLights)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BS)).BeginInit();
            this.SuspendLayout();
            // 
            // DGV_TrafficLights
            // 
            this.DGV_TrafficLights.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_TrafficLights.GridColor = System.Drawing.SystemColors.ControlLightLight;
            this.DGV_TrafficLights.Location = new System.Drawing.Point(18, 63);
            this.DGV_TrafficLights.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DGV_TrafficLights.Name = "DGV_TrafficLights";
            this.DGV_TrafficLights.RowHeadersWidth = 62;
            this.DGV_TrafficLights.Size = new System.Drawing.Size(1023, 531);
            this.DGV_TrafficLights.TabIndex = 0;
            this.DGV_TrafficLights.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_TrafficLights_CellClick);
            this.DGV_TrafficLights.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_TrafficLights_CellMouseLeave);
            this.DGV_TrafficLights.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DGV_TrafficLights_CellParsing);
            this.DGV_TrafficLights.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DGV_TrafficLights_DataError);
            this.DGV_TrafficLights.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.DGV_TrafficLights_EditingControlShowing);
            this.DGV_TrafficLights.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_TrafficLights_RowEnter);
            this.DGV_TrafficLights.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.DGV_TrafficLights_RowPrePaint);
            this.DGV_TrafficLights.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DGV_TrafficLights_KeyDown);
            // 
            // B_SaveFile
            // 
            this.B_SaveFile.Location = new System.Drawing.Point(18, 18);
            this.B_SaveFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.B_SaveFile.Name = "B_SaveFile";
            this.B_SaveFile.Size = new System.Drawing.Size(162, 35);
            this.B_SaveFile.TabIndex = 2;
            this.B_SaveFile.Text = "Save";
            this.B_SaveFile.UseVisualStyleBackColor = true;
            this.B_SaveFile.Click += new System.EventHandler(this.B_SaveFile_Click);
            // 
            // BS
            // 
            this.BS.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.BS_AddingNew);
            this.BS.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.BS_ListChanged);
            // 
            // TrafficLightsEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1059, 612);
            this.Controls.Add(this.B_SaveFile);
            this.Controls.Add(this.DGV_TrafficLights);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "TrafficLightsEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Traffic lights editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TrafficLightsEditForm_FormClosed);
            this.Load += new System.EventHandler(this.TrafficLightsEditForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_TrafficLights)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV_TrafficLights;
        private System.Windows.Forms.Button B_SaveFile;
        private System.Windows.Forms.BindingSource BS;
    }
}
namespace PSMoTR.Forms
{
    partial class RoutesEditForm
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
            this.DGV_Routes = new System.Windows.Forms.DataGridView();
            this.B_SaveFile = new System.Windows.Forms.Button();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Routes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // DGV_Routes
            // 
            this.DGV_Routes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Routes.GridColor = System.Drawing.SystemColors.ControlLightLight;
            this.DGV_Routes.Location = new System.Drawing.Point(18, 63);
            this.DGV_Routes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DGV_Routes.Name = "DGV_Routes";
            this.DGV_Routes.RowHeadersWidth = 62;
            this.DGV_Routes.Size = new System.Drawing.Size(818, 531);
            this.DGV_Routes.TabIndex = 0;
            this.DGV_Routes.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DGV_CellParsing);
            this.DGV_Routes.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_RowEnter);
            this.DGV_Routes.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.DGV_RowPrePaint);
            this.DGV_Routes.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.DGV_UserAddedRow);
            this.DGV_Routes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DGV_KeyDown);
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
            // bindingSource
            // 
            this.bindingSource.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.BS_AddingNew);
            this.bindingSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.BS_ListChanged);
            // 
            // RoutesEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 612);
            this.Controls.Add(this.B_SaveFile);
            this.Controls.Add(this.DGV_Routes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "RoutesEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Routes editor";
            this.Load += new System.EventHandler(this.RoutesEditForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Routes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV_Routes;
        private System.Windows.Forms.Button B_SaveFile;
        private System.Windows.Forms.BindingSource bindingSource;
    }
}
namespace PSMoTR.Forms
{
    partial class ProjectEditForm
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
            this.DGV_Projects = new System.Windows.Forms.DataGridView();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Projects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // DGV_Projects
            // 
            this.DGV_Projects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Projects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Projects.Location = new System.Drawing.Point(12, 46);
            this.DGV_Projects.Name = "DGV_Projects";
            this.DGV_Projects.RowHeadersWidth = 62;
            this.DGV_Projects.Size = new System.Drawing.Size(348, 483);
            this.DGV_Projects.TabIndex = 0;
            this.DGV_Projects.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.DGV_Projects_CellParsing);
            this.DGV_Projects.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_Projects_RowEnter);
            this.DGV_Projects.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DGV_Projects_KeyDown);
            // 
            // bindingSource
            // 
            this.bindingSource.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.BS_AddingNew);
            this.bindingSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.BS_ListChanged);
            // 
            // ProjectEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 541);
            this.Controls.Add(this.DGV_Projects);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectEditForm";
            this.Text = "Редактор проектов";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProjectEditForm_FormClosed);
            this.Load += new System.EventHandler(this.ProjectEditForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Projects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV_Projects;
        private System.Windows.Forms.BindingSource bindingSource;
    }
}
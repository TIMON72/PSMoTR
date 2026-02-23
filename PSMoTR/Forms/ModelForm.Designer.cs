namespace PSMoTR.Forms
{
    partial class ModelForm
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PB_Model = new System.Windows.Forms.PictureBox();
            this.TimerMove = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PB_Model)).BeginInit();
            this.SuspendLayout();
            // 
            // PB_Model
            // 
            this.PB_Model.BackColor = System.Drawing.Color.Transparent;
            this.PB_Model.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PB_Model.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PB_Model.Location = new System.Drawing.Point(0, 0);
            this.PB_Model.Margin = new System.Windows.Forms.Padding(0);
            this.PB_Model.Name = "PB_Model";
            this.PB_Model.Size = new System.Drawing.Size(1686, 799);
            this.PB_Model.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PB_Model.TabIndex = 0;
            this.PB_Model.TabStop = false;
            this.PB_Model.Paint += new System.Windows.Forms.PaintEventHandler(this.PB_Model_Paint);
            this.PB_Model.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PB_Model_MouseClick);
            this.PB_Model.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PB_Model_MouseMove);
            this.PB_Model.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.PB_Model_MouseWheel);
            // 
            // TimerMove
            // 
            this.TimerMove.Interval = 40;
            this.TimerMove.Tick += new System.EventHandler(this.TimerMove_Tick);
            // 
            // ModelForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1686, 799);
            this.ControlBox = false;
            this.Controls.Add(this.PB_Model);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModelForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Model";
            this.Load += new System.EventHandler(this.ModelForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ModelForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.PB_Model)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        protected internal System.Windows.Forms.Timer TimerMove;
        protected internal System.Windows.Forms.PictureBox PB_Model;
    }
}
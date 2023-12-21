namespace DJMAXPlus.MainForm
{
    partial class MainForm
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
            hotKeyController = new HotKeyController();
            SuspendLayout();
            // 
            // hotKeyController
            // 
            hotKeyController.Location = new Point(0, 0);
            hotKeyController.Name = "hotKeyController";
            hotKeyController.Size = new Size(0, 0);
            hotKeyController.TabIndex = 0;
            hotKeyController.HotKeyDown += hotKeyController_HotKeyDown;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(hotKeyController);
            Name = "MainForm";
            Text = "MainForm";
            Load += MainForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private HotKeyController hotKeyController;
    }
}
namespace AssetGUI
{
    partial class AssetForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.typeGroupBox = new System.Windows.Forms.GroupBox();
            this.isFolderCheckBox = new System.Windows.Forms.CheckBox();
            this.isEncryptCheckBox = new System.Windows.Forms.CheckBox();
            this.formatComboBox = new System.Windows.Forms.ComboBox();
            this.inputButton = new System.Windows.Forms.Button();
            this.outputButton = new System.Windows.Forms.Button();
            this.outputTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.inputLabel = new System.Windows.Forms.Label();
            this.inputGroupBox = new System.Windows.Forms.GroupBox();
            this.typeGroupBox.SuspendLayout();
            this.inputGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // typeGroupBox
            // 
            this.typeGroupBox.Controls.Add(this.isFolderCheckBox);
            this.typeGroupBox.Controls.Add(this.isEncryptCheckBox);
            this.typeGroupBox.Controls.Add(this.formatComboBox);
            this.typeGroupBox.Location = new System.Drawing.Point(12, 12);
            this.typeGroupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.typeGroupBox.Name = "typeGroupBox";
            this.typeGroupBox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.typeGroupBox.Size = new System.Drawing.Size(377, 50);
            this.typeGroupBox.TabIndex = 2;
            this.typeGroupBox.TabStop = false;
            this.typeGroupBox.Text = "Operation";
            // 
            // isFolderCheckBox
            // 
            this.isFolderCheckBox.AutoSize = true;
            this.isFolderCheckBox.Location = new System.Drawing.Point(313, 19);
            this.isFolderCheckBox.Name = "isFolderCheckBox";
            this.isFolderCheckBox.Size = new System.Drawing.Size(57, 19);
            this.isFolderCheckBox.TabIndex = 2;
            this.isFolderCheckBox.Text = "File(s)";
            this.isFolderCheckBox.UseVisualStyleBackColor = true;
            this.isFolderCheckBox.CheckedChanged += new System.EventHandler(this.inputTypeCheckBox_CheckedChanged);
            // 
            // isEncryptCheckBox
            // 
            this.isEncryptCheckBox.AutoSize = true;
            this.isEncryptCheckBox.Location = new System.Drawing.Point(240, 19);
            this.isEncryptCheckBox.Name = "isEncryptCheckBox";
            this.isEncryptCheckBox.Size = new System.Drawing.Size(67, 19);
            this.isEncryptCheckBox.TabIndex = 1;
            this.isEncryptCheckBox.Text = "Decrypt";
            this.isEncryptCheckBox.UseVisualStyleBackColor = true;
            this.isEncryptCheckBox.CheckedChanged += new System.EventHandler(this.opTypeCheckBox_CheckedChanged);
            // 
            // formatComboBox
            // 
            this.formatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formatComboBox.FormattingEnabled = true;
            this.formatComboBox.Location = new System.Drawing.Point(7, 17);
            this.formatComboBox.Name = "formatComboBox";
            this.formatComboBox.Size = new System.Drawing.Size(227, 23);
            this.formatComboBox.TabIndex = 0;
            // 
            // inputButton
            // 
            this.inputButton.Location = new System.Drawing.Point(7, 22);
            this.inputButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.inputButton.Name = "inputButton";
            this.inputButton.Size = new System.Drawing.Size(88, 27);
            this.inputButton.TabIndex = 4;
            this.inputButton.Text = "Input";
            this.inputButton.UseVisualStyleBackColor = true;
            this.inputButton.Click += new System.EventHandler(this.inputPathButton_Click);
            // 
            // outputButton
            // 
            this.outputButton.Location = new System.Drawing.Point(7, 55);
            this.outputButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.outputButton.Name = "outputButton";
            this.outputButton.Size = new System.Drawing.Size(88, 27);
            this.outputButton.TabIndex = 4;
            this.outputButton.Text = "Output";
            this.outputButton.UseVisualStyleBackColor = true;
            this.outputButton.Click += new System.EventHandler(this.outputPathButton_Click);
            // 
            // outputTextBox
            // 
            this.outputTextBox.Location = new System.Drawing.Point(102, 58);
            this.outputTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.Size = new System.Drawing.Size(269, 23);
            this.outputTextBox.TabIndex = 5;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(158, 161);
            this.startButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(88, 26);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // inputLabel
            // 
            this.inputLabel.AutoSize = true;
            this.inputLabel.Location = new System.Drawing.Point(101, 28);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(0, 15);
            this.inputLabel.TabIndex = 7;
            // 
            // inputGroupBox
            // 
            this.inputGroupBox.Controls.Add(this.inputButton);
            this.inputGroupBox.Controls.Add(this.inputLabel);
            this.inputGroupBox.Controls.Add(this.outputButton);
            this.inputGroupBox.Controls.Add(this.outputTextBox);
            this.inputGroupBox.Location = new System.Drawing.Point(12, 68);
            this.inputGroupBox.Name = "inputGroupBox";
            this.inputGroupBox.Size = new System.Drawing.Size(378, 87);
            this.inputGroupBox.TabIndex = 8;
            this.inputGroupBox.TabStop = false;
            this.inputGroupBox.Text = "Input/Output";
            // 
            // AssetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 194);
            this.Controls.Add(this.inputGroupBox);
            this.Controls.Add(this.typeGroupBox);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "AssetForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Asset";
            this.typeGroupBox.ResumeLayout(false);
            this.typeGroupBox.PerformLayout();
            this.inputGroupBox.ResumeLayout(false);
            this.inputGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox typeGroupBox;
        private System.Windows.Forms.Button inputButton;
        private System.Windows.Forms.Button outputButton;
        private System.Windows.Forms.TextBox outputTextBox;
        private System.Windows.Forms.Button startButton;
        private Label inputLabel;
        private ComboBox formatComboBox;
        private CheckBox isFolderCheckBox;
        private CheckBox isEncryptCheckBox;
        private GroupBox inputGroupBox;
    }
}
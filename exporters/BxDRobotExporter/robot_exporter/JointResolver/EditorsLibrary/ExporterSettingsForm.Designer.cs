﻿namespace EditorsLibrary
{
    partial class PluginSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginSettingsForm));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.ChildHighlight = new System.Windows.Forms.Button();
            this.ChildLabel = new System.Windows.Forms.Label();
            this.MainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.MainLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(3, 35);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(160, 36);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(169, 35);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(160, 36);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // ChildHighlight
            // 
            this.ChildHighlight.BackColor = System.Drawing.Color.Black;
            this.ChildHighlight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChildHighlight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChildHighlight.Location = new System.Drawing.Point(170, 4);
            this.ChildHighlight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ChildHighlight.Name = "ChildHighlight";
            this.ChildHighlight.Size = new System.Drawing.Size(158, 25);
            this.ChildHighlight.TabIndex = 9;
            this.ChildHighlight.UseVisualStyleBackColor = false;
            this.ChildHighlight.Click += new System.EventHandler(this.ChildHighlight_Click);
            // 
            // ChildLabel
            // 
            this.ChildLabel.AutoSize = true;
            this.ChildLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.ChildLabel.ForeColor = System.Drawing.Color.Black;
            this.ChildLabel.Location = new System.Drawing.Point(4, 4);
            this.ChildLabel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ChildLabel.Name = "ChildLabel";
            this.ChildLabel.Size = new System.Drawing.Size(100, 25);
            this.ChildLabel.TabIndex = 6;
            this.ChildLabel.Text = "Highlight Color";
            this.ChildLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainLayout
            // 
            this.MainLayout.AutoSize = true;
            this.MainLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MainLayout.ColumnCount = 2;
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.MainLayout.Controls.Add(this.ChildHighlight, 1, 0);
            this.MainLayout.Controls.Add(this.buttonCancel, 0, 1);
            this.MainLayout.Controls.Add(this.ChildLabel, 0, 0);
            this.MainLayout.Controls.Add(this.buttonOK, 1, 1);
            this.MainLayout.Location = new System.Drawing.Point(4, 4);
            this.MainLayout.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MainLayout.Name = "MainLayout";
            this.MainLayout.RowCount = 2;
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainLayout.Size = new System.Drawing.Size(332, 73);
            this.MainLayout.TabIndex = 13;
            // 
            // PluginSettingsForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(369, 142);
            this.Controls.Add(this.MainLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PluginSettingsForm";
            this.Text = "Exporter Settings";
            this.MainLayout.ResumeLayout(false);
            this.MainLayout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button ChildHighlight;
        private System.Windows.Forms.Label ChildLabel;
        private System.Windows.Forms.TableLayoutPanel MainLayout;
    }
}
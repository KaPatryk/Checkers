
namespace Checkers
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.greyPointsLabel = new System.Windows.Forms.Label();
            this.redPointsLabel = new System.Windows.Forms.Label();
            this.turnIndicatorLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(455, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "POINTS:";
            // 
            // greyPointsLabel
            // 
            this.greyPointsLabel.AutoSize = true;
            this.greyPointsLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.greyPointsLabel.Location = new System.Drawing.Point(457, 34);
            this.greyPointsLabel.Name = "greyPointsLabel";
            this.greyPointsLabel.Size = new System.Drawing.Size(60, 19);
            this.greyPointsLabel.TabIndex = 1;
            this.greyPointsLabel.Text = "POINTS";
            // 
            // redPointsLabel
            // 
            this.redPointsLabel.AutoSize = true;
            this.redPointsLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.redPointsLabel.Location = new System.Drawing.Point(457, 59);
            this.redPointsLabel.Name = "redPointsLabel";
            this.redPointsLabel.Size = new System.Drawing.Size(60, 19);
            this.redPointsLabel.TabIndex = 2;
            this.redPointsLabel.Text = "POINTS";
            // 
            // turnIndicatorLabel
            // 
            this.turnIndicatorLabel.AutoSize = true;
            this.turnIndicatorLabel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.turnIndicatorLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.turnIndicatorLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.turnIndicatorLabel.Location = new System.Drawing.Point(457, 98);
            this.turnIndicatorLabel.Name = "turnIndicatorLabel";
            this.turnIndicatorLabel.Size = new System.Drawing.Size(167, 25);
            this.turnIndicatorLabel.TabIndex = 3;
            this.turnIndicatorLabel.Text = "turnIndicatorLabel";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 411);
            this.Controls.Add(this.turnIndicatorLabel);
            this.Controls.Add(this.redPointsLabel);
            this.Controls.Add(this.greyPointsLabel);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(700, 450);
            this.MinimumSize = new System.Drawing.Size(700, 450);
            this.Name = "Form1";
            this.Text = "Checkers";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label greyPointsLabel;
        private System.Windows.Forms.Label redPointsLabel;
        private System.Windows.Forms.Label turnIndicatorLabel;
    }
}


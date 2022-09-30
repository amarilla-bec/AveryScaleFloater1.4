namespace ScaleRfcApp
{
    partial class Floater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Floater));
            this.tare1 = new System.Windows.Forms.Label();
            this.weightStabilityHeader = new System.Windows.Forms.Label();
            this.grossWeight = new System.Windows.Forms.Label();
            this.lblnetWeight1 = new System.Windows.Forms.Label();
            this.lblNetWeight = new System.Windows.Forms.Label();
            this.lblTare = new System.Windows.Forms.Label();
            this.lblGrossWeight = new System.Windows.Forms.Label();
            this.lblNetWeightUOM = new System.Windows.Forms.Label();
            this.tmrCheckConnection = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tare1
            // 
            this.tare1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tare1.ForeColor = System.Drawing.Color.White;
            this.tare1.Location = new System.Drawing.Point(48, 96);
            this.tare1.Name = "tare1";
            this.tare1.Size = new System.Drawing.Size(96, 20);
            this.tare1.TabIndex = 1;
            this.tare1.Text = "-- Kg";
            this.tare1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // weightStabilityHeader
            // 
            this.weightStabilityHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weightStabilityHeader.ForeColor = System.Drawing.Color.White;
            this.weightStabilityHeader.Location = new System.Drawing.Point(8, 0);
            this.weightStabilityHeader.Name = "weightStabilityHeader";
            this.weightStabilityHeader.Size = new System.Drawing.Size(296, 24);
            this.weightStabilityHeader.TabIndex = 2;
            this.weightStabilityHeader.Text = "--";
            this.weightStabilityHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grossWeight
            // 
            this.grossWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grossWeight.ForeColor = System.Drawing.Color.White;
            this.grossWeight.Location = new System.Drawing.Point(208, 96);
            this.grossWeight.Name = "grossWeight";
            this.grossWeight.Size = new System.Drawing.Size(96, 20);
            this.grossWeight.TabIndex = 3;
            this.grossWeight.Text = "-- Kg";
            this.grossWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblnetWeight1
            // 
            this.lblnetWeight1.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblnetWeight1.ForeColor = System.Drawing.Color.White;
            this.lblnetWeight1.Location = new System.Drawing.Point(40, 24);
            this.lblnetWeight1.Name = "lblnetWeight1";
            this.lblnetWeight1.Size = new System.Drawing.Size(232, 58);
            this.lblnetWeight1.TabIndex = 4;
            this.lblnetWeight1.Text = "--";
            this.lblnetWeight1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblNetWeight
            // 
            this.lblNetWeight.AutoSize = true;
            this.lblNetWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNetWeight.ForeColor = System.Drawing.Color.White;
            this.lblNetWeight.Location = new System.Drawing.Point(0, 40);
            this.lblNetWeight.Name = "lblNetWeight";
            this.lblNetWeight.Size = new System.Drawing.Size(38, 20);
            this.lblNetWeight.TabIndex = 5;
            this.lblNetWeight.Text = "Net:";
            // 
            // lblTare
            // 
            this.lblTare.AutoSize = true;
            this.lblTare.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTare.ForeColor = System.Drawing.Color.White;
            this.lblTare.Location = new System.Drawing.Point(0, 96);
            this.lblTare.Name = "lblTare";
            this.lblTare.Size = new System.Drawing.Size(45, 20);
            this.lblTare.TabIndex = 6;
            this.lblTare.Text = "Tare:";
            // 
            // lblGrossWeight
            // 
            this.lblGrossWeight.AutoSize = true;
            this.lblGrossWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrossWeight.ForeColor = System.Drawing.Color.White;
            this.lblGrossWeight.Location = new System.Drawing.Point(152, 96);
            this.lblGrossWeight.Name = "lblGrossWeight";
            this.lblGrossWeight.Size = new System.Drawing.Size(56, 20);
            this.lblGrossWeight.TabIndex = 7;
            this.lblGrossWeight.Text = "Gross:";
            // 
            // lblNetWeightUOM
            // 
            this.lblNetWeightUOM.AutoSize = true;
            this.lblNetWeightUOM.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNetWeightUOM.ForeColor = System.Drawing.Color.White;
            this.lblNetWeightUOM.Location = new System.Drawing.Point(275, 56);
            this.lblNetWeightUOM.Name = "lblNetWeightUOM";
            this.lblNetWeightUOM.Size = new System.Drawing.Size(28, 20);
            this.lblNetWeightUOM.TabIndex = 8;
            this.lblNetWeightUOM.Text = "Kg";
            this.lblNetWeightUOM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tmrCheckConnection
            // 
            this.tmrCheckConnection.Enabled = true;
            this.tmrCheckConnection.Interval = 5000;
            this.tmrCheckConnection.Tick += new System.EventHandler(this.tmrCheckConnection_Tick);
            // 
            // Floater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(310, 118);
            this.Controls.Add(this.lblNetWeightUOM);
            this.Controls.Add(this.lblGrossWeight);
            this.Controls.Add(this.lblTare);
            this.Controls.Add(this.lblNetWeight);
            this.Controls.Add(this.lblnetWeight1);
            this.Controls.Add(this.grossWeight);
            this.Controls.Add(this.weightStabilityHeader);
            this.Controls.Add(this.tare1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(300, 300);
            this.MaximizeBox = false;
            this.Name = "Floater";
            this.Text = "Floater Scale";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label tare1;
        private System.Windows.Forms.Label weightStabilityHeader;
        private System.Windows.Forms.Label grossWeight;
        private System.Windows.Forms.Label lblnetWeight1;
        private System.Windows.Forms.Label lblNetWeight;
        private System.Windows.Forms.Label lblTare;
        private System.Windows.Forms.Label lblGrossWeight;
        private System.Windows.Forms.Label lblNetWeightUOM;
        private System.Windows.Forms.Timer tmrCheckConnection;
        //private System.Windows.Forms.Button btnTare;
        //private System.Windows.Forms.Button button1;
        //private System.Windows.Forms.Button button2;
    }
}


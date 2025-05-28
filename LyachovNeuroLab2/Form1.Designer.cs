namespace LyachovNeuroLab2
{
    partial class Form1
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
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.textBoxLocalPort = new System.Windows.Forms.TextBox();
            this.textBoxRemotePort = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnConnect = new System.Windows.Forms.Button();
            this.textBoxData = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.InformTextbox = new System.Windows.Forms.TextBox();
            this.btnLoadWeightsFromFile = new System.Windows.Forms.Button();
            this.btnSaveWeightsToFile = new System.Windows.Forms.Button();
            this.dataGridWeights = new System.Windows.Forms.DataGridView();
            this.btnShowWeights = new System.Windows.Forms.Button();
            this.btnApplyWeightsFromGrid = new System.Windows.Forms.Button();
            this.radioInputHidden = new System.Windows.Forms.RadioButton();
            this.BtnTrainNetwork = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridWeights)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(93, 13);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(100, 20);
            this.textBoxIP.TabIndex = 0;
            this.textBoxIP.Text = "127.0.0.1";
            // 
            // textBoxLocalPort
            // 
            this.textBoxLocalPort.Location = new System.Drawing.Point(92, 39);
            this.textBoxLocalPort.Name = "textBoxLocalPort";
            this.textBoxLocalPort.Size = new System.Drawing.Size(100, 20);
            this.textBoxLocalPort.TabIndex = 1;
            this.textBoxLocalPort.Text = "7777";
            // 
            // textBoxRemotePort
            // 
            this.textBoxRemotePort.Location = new System.Drawing.Point(92, 65);
            this.textBoxRemotePort.Name = "textBoxRemotePort";
            this.textBoxRemotePort.Size = new System.Drawing.Size(100, 20);
            this.textBoxRemotePort.TabIndex = 2;
            this.textBoxRemotePort.Text = "8888";
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 91);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(180, 23);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // textBoxData
            // 
            this.textBoxData.Location = new System.Drawing.Point(545, 37);
            this.textBoxData.Multiline = true;
            this.textBoxData.Name = "textBoxData";
            this.textBoxData.Size = new System.Drawing.Size(180, 401);
            this.textBoxData.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Local Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Remote Port";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(542, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Data from robot";
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(12, 120);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(180, 23);
            this.btnStartStop.TabIndex = 15;
            this.btnStartStop.Text = "Start/Stop recieving";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // InformTextbox
            // 
            this.InformTextbox.Location = new System.Drawing.Point(12, 212);
            this.InformTextbox.Multiline = true;
            this.InformTextbox.Name = "InformTextbox";
            this.InformTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.InformTextbox.Size = new System.Drawing.Size(180, 226);
            this.InformTextbox.TabIndex = 16;
            // 
            // btnLoadWeightsFromFile
            // 
            this.btnLoadWeightsFromFile.Location = new System.Drawing.Point(199, 11);
            this.btnLoadWeightsFromFile.Name = "btnLoadWeightsFromFile";
            this.btnLoadWeightsFromFile.Size = new System.Drawing.Size(340, 23);
            this.btnLoadWeightsFromFile.TabIndex = 17;
            this.btnLoadWeightsFromFile.Text = "Load wheights";
            this.btnLoadWeightsFromFile.UseVisualStyleBackColor = true;
            this.btnLoadWeightsFromFile.Click += new System.EventHandler(this.btnLoadWeightsFromFile_Click);
            // 
            // btnSaveWeightsToFile
            // 
            this.btnSaveWeightsToFile.Location = new System.Drawing.Point(199, 37);
            this.btnSaveWeightsToFile.Name = "btnSaveWeightsToFile";
            this.btnSaveWeightsToFile.Size = new System.Drawing.Size(340, 23);
            this.btnSaveWeightsToFile.TabIndex = 18;
            this.btnSaveWeightsToFile.Text = "Save wheights";
            this.btnSaveWeightsToFile.UseVisualStyleBackColor = true;
            this.btnSaveWeightsToFile.Click += new System.EventHandler(this.btnSaveWeightsToFile_Click);
            // 
            // dataGridWeights
            // 
            this.dataGridWeights.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridWeights.Location = new System.Drawing.Point(199, 144);
            this.dataGridWeights.Name = "dataGridWeights";
            this.dataGridWeights.Size = new System.Drawing.Size(340, 294);
            this.dataGridWeights.TabIndex = 19;
            // 
            // btnShowWeights
            // 
            this.btnShowWeights.Location = new System.Drawing.Point(199, 63);
            this.btnShowWeights.Name = "btnShowWeights";
            this.btnShowWeights.Size = new System.Drawing.Size(340, 23);
            this.btnShowWeights.TabIndex = 20;
            this.btnShowWeights.Text = "Show wheights";
            this.btnShowWeights.UseVisualStyleBackColor = true;
            this.btnShowWeights.Click += new System.EventHandler(this.btnShowWeights_Click);
            // 
            // btnApplyWeightsFromGrid
            // 
            this.btnApplyWeightsFromGrid.Location = new System.Drawing.Point(199, 92);
            this.btnApplyWeightsFromGrid.Name = "btnApplyWeightsFromGrid";
            this.btnApplyWeightsFromGrid.Size = new System.Drawing.Size(340, 23);
            this.btnApplyWeightsFromGrid.TabIndex = 21;
            this.btnApplyWeightsFromGrid.Text = "Apply wheights from grid";
            this.btnApplyWeightsFromGrid.UseVisualStyleBackColor = true;
            this.btnApplyWeightsFromGrid.Click += new System.EventHandler(this.btnApplyWeightsFromGrid_Click);
            // 
            // radioInputHidden
            // 
            this.radioInputHidden.AutoSize = true;
            this.radioInputHidden.Location = new System.Drawing.Point(329, 121);
            this.radioInputHidden.Name = "radioInputHidden";
            this.radioInputHidden.Size = new System.Drawing.Size(94, 17);
            this.radioInputHidden.TabIndex = 22;
            this.radioInputHidden.TabStop = true;
            this.radioInputHidden.Text = "Input→Hidden";
            this.radioInputHidden.UseVisualStyleBackColor = true;
            // 
            // BtnTrainNetwork
            // 
            this.BtnTrainNetwork.Location = new System.Drawing.Point(12, 149);
            this.BtnTrainNetwork.Name = "BtnTrainNetwork";
            this.BtnTrainNetwork.Size = new System.Drawing.Size(180, 23);
            this.BtnTrainNetwork.TabIndex = 23;
            this.BtnTrainNetwork.Text = "Train network";
            this.BtnTrainNetwork.UseVisualStyleBackColor = true;
            this.BtnTrainNetwork.Click += new System.EventHandler(this.BtnTrainNetwork_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 178);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(180, 23);
            this.button1.TabIndex = 24;
            this.button1.Text = "Restart";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 453);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BtnTrainNetwork);
            this.Controls.Add(this.radioInputHidden);
            this.Controls.Add(this.btnApplyWeightsFromGrid);
            this.Controls.Add(this.btnShowWeights);
            this.Controls.Add(this.dataGridWeights);
            this.Controls.Add(this.btnSaveWeightsToFile);
            this.Controls.Add(this.btnLoadWeightsFromFile);
            this.Controls.Add(this.InformTextbox);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxData);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.textBoxRemotePort);
            this.Controls.Add(this.textBoxLocalPort);
            this.Controls.Add(this.textBoxIP);
            this.Name = "Form1";
            this.Text = "Lyachov LAB2";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridWeights)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.TextBox textBoxLocalPort;
        private System.Windows.Forms.TextBox textBoxRemotePort;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox textBoxData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.TextBox InformTextbox;
        private System.Windows.Forms.Button btnLoadWeightsFromFile;
        private System.Windows.Forms.Button btnSaveWeightsToFile;
        private System.Windows.Forms.DataGridView dataGridWeights;
        private System.Windows.Forms.Button btnShowWeights;
        private System.Windows.Forms.Button btnApplyWeightsFromGrid;
        private System.Windows.Forms.RadioButton radioInputHidden;
        private System.Windows.Forms.Button BtnTrainNetwork;
        private System.Windows.Forms.Button button1;
    }
}


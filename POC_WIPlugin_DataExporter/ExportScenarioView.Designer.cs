namespace DataExporter
{
    partial class ExportScenarioView
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.Generate = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.folderPath = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(12, 40);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(234, 118);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // Generate
            // 
            this.Generate.Location = new System.Drawing.Point(12, 164);
            this.Generate.Name = "Generate";
            this.Generate.Size = new System.Drawing.Size(75, 23);
            this.Generate.TabIndex = 2;
            this.Generate.Text = "Generate";
            this.Generate.UseVisualStyleBackColor = true;
            this.Generate.Click += new System.EventHandler(this.generate_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(171, 164);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // folderPath
            // 
            this.folderPath.Location = new System.Drawing.Point(12, 4);
            this.folderPath.Name = "folderPath";
            this.folderPath.Size = new System.Drawing.Size(25, 23);
            this.folderPath.TabIndex = 5;
            this.folderPath.Text = "...";
            this.folderPath.UseVisualStyleBackColor = true;
            this.folderPath.Click += new System.EventHandler(this.folderPath_Click);
            // 
            // ExportScenarioView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 195);
            this.Controls.Add(this.folderPath);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Generate);
            this.Controls.Add(this.listView1);
            this.Name = "ExportScenarioView";
            this.Text = "Export scenario";
            this.Load += new System.EventHandler(this.exportScenarioView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button Generate;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button folderPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}


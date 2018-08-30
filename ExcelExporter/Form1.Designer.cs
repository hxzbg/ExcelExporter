namespace ExcelExporter
{
	partial class Form1
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
			this.checkJson = new System.Windows.Forms.CheckBox();
			this.checkXml = new System.Windows.Forms.CheckBox();
			this.checkCSharp = new System.Windows.Forms.CheckBox();
			this.checkSqlite = new System.Windows.Forms.CheckBox();
			this.checkMySql = new System.Windows.Forms.CheckBox();
			this.filePath = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.LogWindow = new System.Windows.Forms.TextBox();
			this.checkArray = new System.Windows.Forms.CheckBox();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.label2 = new System.Windows.Forms.Label();
			this.checkCSV = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// checkJson
			// 
			this.checkJson.AutoSize = true;
			this.checkJson.Location = new System.Drawing.Point(71, 112);
			this.checkJson.Name = "checkJson";
			this.checkJson.Size = new System.Drawing.Size(48, 16);
			this.checkJson.TabIndex = 0;
			this.checkJson.Text = "JSON";
			this.checkJson.UseVisualStyleBackColor = true;
			this.checkJson.CheckedChanged += new System.EventHandler(this.checkJson_CheckedChanged);
			// 
			// checkXml
			// 
			this.checkXml.AutoSize = true;
			this.checkXml.Location = new System.Drawing.Point(263, 112);
			this.checkXml.Name = "checkXml";
			this.checkXml.Size = new System.Drawing.Size(42, 16);
			this.checkXml.TabIndex = 1;
			this.checkXml.Text = "XML";
			this.checkXml.UseVisualStyleBackColor = true;
			this.checkXml.CheckedChanged += new System.EventHandler(this.checkXml_CheckedChanged);
			// 
			// checkCSharp
			// 
			this.checkCSharp.AutoSize = true;
			this.checkCSharp.Location = new System.Drawing.Point(356, 112);
			this.checkCSharp.Name = "checkCSharp";
			this.checkCSharp.Size = new System.Drawing.Size(36, 16);
			this.checkCSharp.TabIndex = 2;
			this.checkCSharp.Text = "C#";
			this.checkCSharp.UseVisualStyleBackColor = true;
			this.checkCSharp.CheckedChanged += new System.EventHandler(this.checkCSharp_CheckedChanged);
			// 
			// checkSqlite
			// 
			this.checkSqlite.AutoSize = true;
			this.checkSqlite.Location = new System.Drawing.Point(443, 112);
			this.checkSqlite.Name = "checkSqlite";
			this.checkSqlite.Size = new System.Drawing.Size(60, 16);
			this.checkSqlite.TabIndex = 3;
			this.checkSqlite.Text = "Sqlite";
			this.checkSqlite.UseVisualStyleBackColor = true;
			this.checkSqlite.CheckedChanged += new System.EventHandler(this.checkSqlite_CheckedChanged);
			// 
			// checkMySql
			// 
			this.checkMySql.AutoSize = true;
			this.checkMySql.Location = new System.Drawing.Point(554, 112);
			this.checkMySql.Name = "checkMySql";
			this.checkMySql.Size = new System.Drawing.Size(54, 16);
			this.checkMySql.TabIndex = 4;
			this.checkMySql.Text = "MySql";
			this.checkMySql.UseVisualStyleBackColor = true;
			this.checkMySql.CheckedChanged += new System.EventHandler(this.checkMySql_CheckedChanged);
			// 
			// filePath
			// 
			this.filePath.AllowDrop = true;
			this.filePath.Location = new System.Drawing.Point(71, 31);
			this.filePath.Name = "filePath";
			this.filePath.ReadOnly = true;
			this.filePath.Size = new System.Drawing.Size(538, 21);
			this.filePath.TabIndex = 5;
			this.filePath.Text = "拖拽文件或者文件夹到这里";
			this.filePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.filePath_DragDrop);
			this.filePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.filePath_DragEnter);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(71, 92);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 12);
			this.label1.TabIndex = 6;
			this.label1.Text = "导出格式：";
			// 
			// LogWindow
			// 
			this.LogWindow.Location = new System.Drawing.Point(71, 178);
			this.LogWindow.Multiline = true;
			this.LogWindow.Name = "LogWindow";
			this.LogWindow.ReadOnly = true;
			this.LogWindow.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.LogWindow.Size = new System.Drawing.Size(538, 348);
			this.LogWindow.TabIndex = 7;
			// 
			// checkArray
			// 
			this.checkArray.AutoSize = true;
			this.checkArray.Location = new System.Drawing.Point(73, 146);
			this.checkArray.Name = "checkArray";
			this.checkArray.Size = new System.Drawing.Size(84, 16);
			this.checkArray.TabIndex = 8;
			this.checkArray.Text = "导出为数组";
			this.checkArray.UseVisualStyleBackColor = true;
			this.checkArray.CheckedChanged += new System.EventHandler(this.checkArray_CheckedChanged);
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(71, 559);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(538, 15);
			this.progressBar.TabIndex = 9;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(71, 541);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 12);
			this.label2.TabIndex = 10;
			this.label2.Text = "整体进度:";
			// 
			// checkCSV
			// 
			this.checkCSV.AutoSize = true;
			this.checkCSV.Location = new System.Drawing.Point(170, 112);
			this.checkCSV.Name = "checkCSV";
			this.checkCSV.Size = new System.Drawing.Size(42, 16);
			this.checkCSV.TabIndex = 11;
			this.checkCSV.Text = "CSV";
			this.checkCSV.UseVisualStyleBackColor = true;
			this.checkCSV.CheckedChanged += new System.EventHandler(this.checkCSV_CheckedChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(688, 594);
			this.Controls.Add(this.checkCSV);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.checkArray);
			this.Controls.Add(this.LogWindow);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.filePath);
			this.Controls.Add(this.checkMySql);
			this.Controls.Add(this.checkSqlite);
			this.Controls.Add(this.checkCSharp);
			this.Controls.Add(this.checkXml);
			this.Controls.Add(this.checkJson);
			this.Name = "Form1";
			this.Text = "导出Excel";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox checkJson;
		private System.Windows.Forms.CheckBox checkXml;
		private System.Windows.Forms.CheckBox checkCSharp;
		private System.Windows.Forms.CheckBox checkSqlite;
		private System.Windows.Forms.CheckBox checkMySql;
		private System.Windows.Forms.TextBox filePath;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox LogWindow;
		private System.Windows.Forms.CheckBox checkArray;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox checkCSV;
	}
}


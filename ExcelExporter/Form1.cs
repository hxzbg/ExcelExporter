using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using excel2json;
using Excel;

namespace ExcelExporter
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			bool var = false;
			GetSetting("ExportJson", out var);
			this.checkJson.Checked = var;

			GetSetting("ExportCSV", out var);
			this.checkCSV.Checked = var;

			GetSetting("ExportXml", out var);
			this.checkXml.Checked = var;

			GetSetting("ExportCSharp", out var);
			this.checkCSharp.Checked = var;

			GetSetting("ExportSQlite", out var);
			this.checkSqlite.Checked = var;

			GetSetting("ExportMySQL", out var);
			this.checkMySql.Checked = var;

			GetSetting("ExportAsArray", out var);
			this.checkArray.Checked = var;
		}

		void GetSetting(string key, out bool var)
		{
			var = false;
			if(File.Exists("config.xml"))
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(@"config.xml");
				XmlElement node = doc.SelectSingleNode("/config") as XmlElement;
				if(node != null && node.HasAttribute(key))
				{
					string str = node.GetAttribute(key);
					if(string.IsNullOrEmpty(str) == false)
					{
						bool.TryParse(str, out var);
					}
				}
			}
		}

		void SetSetting(string key, bool var)
		{
			XmlElement node = null;
			XmlDocument doc = new XmlDocument();
			if(File.Exists("config.xml"))
			{
				doc.Load(@"config.xml");
				node = doc.SelectSingleNode("/config") as XmlElement;
			}
			
			if(node == null)
			{
				node = doc.CreateElement("config");
				doc.AppendChild(node);
			}
			node.SetAttribute(key, var.ToString());
			doc.Save("config.xml");
		}

		private void checkJson_CheckedChanged(object sender, EventArgs e)
		{
			SetSetting("ExportJson", this.checkJson.Checked);
		}

		private void checkCSV_CheckedChanged(object sender, EventArgs e)
		{
			SetSetting("ExportCSV", this.checkCSV.Checked);
		}

		private void checkXml_CheckedChanged(object sender, EventArgs e)
		{
			SetSetting("ExportXml", this.checkXml.Checked);
		}

		private void checkCSharp_CheckedChanged(object sender, EventArgs e)
		{
			SetSetting("ExportCSharp", this.checkCSharp.Checked);
		}

		private void checkSqlite_CheckedChanged(object sender, EventArgs e)
		{
			SetSetting("ExportSQlite", this.checkSqlite.Checked);
		}

		private void checkMySql_CheckedChanged(object sender, EventArgs e)
		{
			SetSetting("ExportMySQL", this.checkMySql.Checked);
		}

		private void checkArray_CheckedChanged(object sender, EventArgs e)
		{
			SetSetting("ExportAsArray", this.checkArray.Checked);
		}

		void PutLog(string txt)
		{
			this.LogWindow.Text += string.Format("{0}\r\n", txt);
		}
		delegate void PutLogDelegate(string txt);

		void ShowMessage(string txt)
		{
			MessageBox.Show(this, txt);
		}
		delegate void MessageBoxDelegate(string txt);

		void SetProgress(int percent)
		{
			this.progressBar.Value = percent;
		}
		delegate void ProgressBarDelegate(int percent);

		static void Export(Options options)
		{
			string excelPath = options.ExcelPath;
			int header = options.HeaderRows;

			Console.WriteLine(string.Format("export {0}", excelPath));
			// 加载Excel文件
			using (FileStream excelFile = File.Open(excelPath, FileMode.Open, FileAccess.Read))
			{
				IExcelDataReader excelReader = null;
				string extension = Path.GetExtension(excelPath);
				if (string.IsNullOrEmpty(extension) == false)
				{
					extension = extension.ToLower();
				}

				if (extension == ".xlsx")
				{
					excelReader = ExcelReaderFactory.CreateOpenXmlReader(excelFile);
				}
				else
				{
					throw new Exception("仅支持xlsx格式。");
				}
				string fileName = Path.GetFileNameWithoutExtension(excelPath);

				// The result of each spreadsheet will be created in the result.Tables
				excelReader.IsFirstRowAsColumnNames = true;
				DataSet book = excelReader.AsDataSet();

				// 数据检测
				if (book.Tables.Count < 1)
				{
					throw new Exception("Excel文件中没有找到Sheet");
				}

				// 取得数据
				DataTable sheet = book.Tables[0];
				if (sheet.Rows.Count <= 0)
				{
					throw new Exception("Excel Sheet中没有数据");
				}

				//-- 确定编码
				Encoding cd = new UTF8Encoding(false);
				if (options.Encoding != "utf8-nobom")
				{
					foreach (EncodingInfo ei in Encoding.GetEncodings())
					{
						Encoding e = ei.GetEncoding();
						if (e.EncodingName == options.Encoding)
						{
							cd = e;
							break;
						}
					}
				}

				//-- 导出JSON文件
				if (options.json || options.xml)
				{
					JsonExporter exporter = new JsonExporter(sheet, header, options.Lowcase);
					if (options.json)
					{
						exporter.SaveToJsonFile(string.Format("{0}/{1}.json", options.WorkOut, fileName), cd, options.ExportArray);
					}

					if (options.xml)
					{
						exporter.SaveToXmlFile(string.Format("{0}/{1}.xml", options.WorkOut, fileName), cd, options.ExportArray);
					}
				}

				//-- 导出SQL文件
				if (options.sqlite || options.mysql)
				{
					SQLExporter exporter = new SQLExporter(sheet, header);
					if (string.IsNullOrEmpty(options.SQLPath))
					{
						options.SQLPath = string.Format("{0}/{1}", options.WorkOut, fileName);
					}
					exporter.SaveToFile(options, cd, fileName);
				}

				//-- 生成C#定义文件
				if (options.csharp)
				{
					string excelName = Path.GetFileName(excelPath);
					if (string.IsNullOrEmpty(options.SQLPath))
					{
						options.XmlPath = string.Format("{0}/{1}.cs", options.WorkOut, fileName);
					}
					CSDefineGenerator exporter = new CSDefineGenerator(sheet);
					exporter.ClassComment = string.Format("// Generate From {0}", excelName);
					exporter.SaveToFile(options.CSharpPath, cd);
				}

				if (options.csv)
				{
					if (string.IsNullOrEmpty(options.CSVPath))
					{
						options.CSVPath = string.Format("{0}/{1}.csv", options.WorkOut, fileName);
					}
					CSVExporter.SaveToCSVFile(sheet, options.CSVPath);
				}
			}
		}

		private void filePath_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.filePath.DoDragDrop(null, DragDropEffects.Move);
		}

		private void filePath_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Move;
			}
		}

		void ProcessDrag(object obj)
		{
			PutLogDelegate putlog = PutLog;
			MessageBoxDelegate messagebox = ShowMessage;
			ProgressBarDelegate progress = SetProgress;

			DateTime curTime = System.DateTime.Now;
			string time = string.Format("{0}_{1}_{2}-{3}_{4}_{5}", curTime.Year, curTime.Month, curTime.Day, curTime.Hour, curTime.Minute, curTime.Second);

			Options option = new Options();
			option.HeaderRows = 1;
			option.WorkOut = "输出目录";
			option.json = this.checkJson.Checked;
			option.csv = this.checkCSV.Checked;
			option.xml = this.checkXml.Checked;
			option.sqlite = this.checkSqlite.Checked;
			option.mysql = this.checkMySql.Checked;
			option.csharp = this.checkCSharp.Checked;
			option.ExportArray = this.checkArray.Checked;
			option.SQLPath = string.Format("{0}/{1}", option.WorkOut, time);
			option.CSharpPath = string.Format("{0}/{1}.cs", option.WorkOut, time);

			if (Directory.Exists(option.WorkOut) == false)
			{
				Directory.CreateDirectory(option.WorkOut);
			}

			List<string> list = obj as List<string>;
			for (int i = 0; i < list.Count; i++)
			{
				option.ExcelPath = null;
				string path = list[i];
				if (File.Exists(path))
				{
					option.ExcelPath = path;
					try
					{
						Export(option);
					}
					catch (System.Exception e)
					{
						this.Invoke(putlog, string.Format("{0}:{1}", option.ExcelPath, e.Message));
					}
				}
				this.Invoke(progress, (int)((i + 1) / (float)list.Count * 100));
			}
			this.Invoke(messagebox, "导出完成");
		}

		private void filePath_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			this.LogWindow.Clear();
			System.Array pathArray = (System.Array)e.Data.GetData(DataFormats.FileDrop);
			if(pathArray != null && pathArray.Length > 0)
			{
				List<string> list = new List<string>();
				for (int i = 0; i < pathArray.Length; i++)
				{
					string path = pathArray.GetValue(i).ToString();
					if (File.Exists(path))
					{
						list.Add(path);
					}
					else if (Directory.Exists(path))
					{
						string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
						if (files != null && files.Length > 0)
						{
							for (int index = 0; index < files.Length; index++)
							{
								path = files[index].Replace('\\', '/');
								list.Add(path);
							}
						}
					}
				}

				if ((this.checkJson.Checked || this.checkXml.Checked || this.checkSqlite.Checked || this.checkCSharp.Checked || this.checkMySql.Checked || this.checkCSV.Checked) && list.Count > 0)
				{
					System.Threading.ParameterizedThreadStart ParStart = new System.Threading.ParameterizedThreadStart(ProcessDrag);
					System.Threading.Thread thread = new System.Threading.Thread(ProcessDrag);
					thread.Start(list);
				}
				else
				{
					ShowMessage("请先设置输出格式。客户端为SQlite，服务器为MySQL，不要弄错。");
				}				
			}
		}
	}
}

using System;
using System.IO;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace excel2json
{
    class SQLExporter
    {
		enum RowType
		{
			INTEGER = 0,
			TEXT,
		}
		RowType[] _types = null;
        DataTable m_sheet;
        int m_headerRows;

        /// <summary>
        /// 初始化内部数据
        /// </summary>
        /// <param name="sheet">Excel读取的一个表单</param>
        /// <param name="headerRows">表头有几行</param>
        public SQLExporter(DataTable sheet, int headerRows)
        {
            m_sheet = sheet;
            m_headerRows = headerRows;
			DataColumnCollection columns = sheet.Columns;
			if (columns != null && columns.Count > 0)
			{
				_types = new RowType[columns.Count];
				int firstDataRow = m_headerRows - 1;
				for (int i = firstDataRow; i < sheet.Rows.Count; i++)
				{
					DataRow row = sheet.Rows[i];
					for (int j = 0; j < columns.Count; j ++ )
					{
						if (_types[j] == RowType.TEXT)
						{
							continue;
						}

						DataColumn column = columns[j];
						object value = row[column];
						Type type = value.GetType();
						if (type == typeof(double))
						{
							double num = (double)value;
							if ((int)num != num)
							{
								_types[j] = RowType.TEXT;
							}
						}
						else if (type == typeof(string))
						{
							int var = 0;
							string str = value.ToString();
							if (string.IsNullOrEmpty(str))
							{
								continue;
							}
							else if (int.TryParse((string)value, out var) == false || var.ToString() != (string)value)
							{
								_types[j] = RowType.TEXT;
							}
						}
					}
				}
			}
        }

        /// <summary>
        /// 转换成SQL字符串，并保存到指定的文件
        /// </summary>
        /// <param name="filePath">存盘文件</param>
        /// <param name="encoding">编码格式</param>
		public void SaveToFile(Options options, Encoding encoding, string tableName)
        {
            //-- 转换成SQL语句
			string mysql_header = null;
			string sqlite_header = null;
            GetTabelStructSQL(m_sheet, tableName, out sqlite_header, out mysql_header);
            string tabelContent = GetTableContentSQL(m_sheet, tableName);

			bool[] build = { options.sqlite, options.mysql};
			string[] headers = { sqlite_header, mysql_header};
			string[] filePaths = { options.SQLPath + "_sqlite.sql", options.SQLPath + "_mysql.sql" };
			for (int i = 0; i < 2; i ++ )
			{
				//-- 保存文件
				if(build[i])
				{
					string filePath = filePaths[i];
					using (FileStream file = new FileStream(filePath, FileMode.Append, FileAccess.Write))
					{
						using (TextWriter writer = new StreamWriter(file, encoding))
						{
							writer.Write(headers[i]);
							writer.WriteLine();
							writer.Write(tabelContent);
							writer.WriteLine();
						}
					}
				}
			}
        }

        /// <summary>
        /// 将表单内容转换成INSERT语句
        /// </summary>
        private string GetTableContentSQL(DataTable sheet, string tabelName)
        {
            StringBuilder sbContent = new StringBuilder();
            StringBuilder sbNames = new StringBuilder();
            StringBuilder sbValues = new StringBuilder();

            //-- 字段名称列表
            foreach (DataColumn column in sheet.Columns)
            {
                sbNames.Append(column.ToString());
                sbNames.Append(", ");
            }

            //-- 逐行转换数据
            int firstDataRow = m_headerRows - 1;
            for (int i = firstDataRow; i < sheet.Rows.Count; i++ )
            {
                DataRow row = sheet.Rows[i];
				sbValues.Remove(0, sbValues.Length);

				int j = 0;
				DataColumnCollection columns = sheet.Columns;
				for (j = 0; j < columns.Count; j ++ )
				{
					DataColumn column = columns[j];
					if (sbValues.Length > 0)
					{
						sbValues.Append(", ");
					}

					object value = row[column];
					Type type = value.GetType();
					if (type == typeof(System.DBNull))
					{
						if(j == 0)
						{
							break;
						}
						sbValues.Append("NULL");
					}
					else
					{
						sbValues.AppendFormat("'{0}'", value.ToString());
					}
				}

				if(j > 0)
				{
					sbContent.AppendFormat("INSERT INTO `{0}` VALUES({1});\n", tabelName, sbValues.ToString());
				}
            }

            return sbContent.ToString();
        }

        /// <summary>
        /// 根据表头构造CREATE TABLE语句
        /// </summary>
        private void GetTabelStructSQL(DataTable sheet, string tabelName, out string sqlite_header, out string mysql_header)
        {
			sqlite_header = "";
			mysql_header = "";

            StringBuilder sqlite = new StringBuilder();
            sqlite.AppendFormat("DROP TABLE IF EXISTS `{0}`;\n", tabelName);
            sqlite.AppendFormat("CREATE TABLE `{0}` (\n", tabelName);

			StringBuilder mysql = new StringBuilder();
			mysql.AppendFormat("CREATE TABLE IF NOT EXISTS `{0}` (\n", tabelName);

			DataColumn column = null;
			DataColumnCollection columns = sheet.Columns;
			for (int i = 0; i < columns.Count; i ++ )
			{
				column = columns[i];
				string filedName = column.ToString();
				switch(_types[i])
				{
					case RowType.INTEGER:
						sqlite.AppendFormat("`{0}` INTEGER(11),\n", filedName);
						mysql.AppendFormat("`{0}` INTEGER(11),\n", filedName);
						break;

					case RowType.TEXT:
						sqlite.AppendFormat("`{0}` TEXT,\n", filedName);
						mysql.AppendFormat("`{0}` TEXT,\n", filedName);
						break;
				}
			}

			//检查是否存在有效主键
			column = columns[0];
			bool hasPrimaryKey = true;
			int firstDataRow = m_headerRows - 1;
			Dictionary<object, bool> keydict = new Dictionary<object, bool>();
			for (int i = firstDataRow; i < sheet.Rows.Count; i++)
			{
				object var = sheet.Rows[i][column];
				if(var is System.DBNull)
				{
					hasPrimaryKey = false;
					break;
				}

				object key = null;
				if(_types[0] == RowType.INTEGER)
				{
					int iv = 0;
					int.TryParse(var.ToString(), out iv);
					key = iv;
				}
				else
				{
					key = var.ToString();
				}

				if (keydict.ContainsKey(key))
				{
					hasPrimaryKey = false;
					break;
				}
				else
				{
					keydict[key] = true;
				}
			}

			if(hasPrimaryKey)
			{
				sqlite.AppendFormat("PRIMARY KEY (`{0}`) ", sheet.Columns[0].ToString());
				mysql.AppendFormat("PRIMARY KEY (`{0}` ASC)", sheet.Columns[0].ToString());
			}
			else
			{
				sqlite.Remove(sqlite.Length - 2, 2);
				mysql.Remove(mysql.Length - 2, 2);
			}

            sqlite.AppendLine("\n);");
			mysql.AppendFormat("\n)  ENGINE=InnoDB DEFAULT CHARSET=utf8;\nTRUNCATE {0};\n", tabelName);

			sqlite_header = sqlite.ToString();
			mysql_header = mysql.ToString();
        }
    }
}

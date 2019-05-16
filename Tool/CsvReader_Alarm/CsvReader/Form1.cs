using System;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using KgsCommon;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CsvReader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cv_BtStart.Click += new EventHandler(ReadCsv);
        }
        private void ReadCsv(object sender , EventArgs e)
        {
            string Inpath = cv_TbIn.Text.Trim();
            string sheetName = cv_TbSheetName.Text.Trim();
            string OutPath = cv_TbOut.Text.Trim();
            string strCon = " Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source = " + Inpath + ";Extended Properties='Excel 8.0;HDR=NO'";//連結字串中的HDR=YES，代表略過第一欄資料
            OleDbConnection oledb_con = new OleDbConnection(strCon);
            oledb_con.Open();
            OleDbCommand oledb_com = new OleDbCommand(" SELECT * FROM ["+sheetName+"$] ", oledb_con);
            OleDbDataReader oledb_dr = oledb_com.ExecuteReader();
            List<string> code = new List<string>();
            List<string> type = new List<string>();
            List<string> description = new List<string>();

            KXmlItem out_xml = new KXmlItem();
            out_xml.Text = @"<Data></Data>";
            while (oledb_dr.Read())
            {
                if (!string.IsNullOrEmpty(oledb_dr[2].ToString().Trim()))
                {
                    KXmlItem tmp = new KXmlItem();
                    tmp.Text = @"<Item No="""" SVID="""" SVNAME="""" Format="""" MaxSize="""" DataOperation="""" MinValue="""" MaxValue="""" Value=""""/>";

                    tmp.Attributes["No"] = int.Parse(oledb_dr[0].ToString().Trim()).ToString();
                    tmp.Attributes["SVID"] = "ZR"+oledb_dr[2].ToString().Trim();
                    tmp.Attributes["SVNAME"] = oledb_dr[4].ToString().Trim();
                    tmp.Attributes["Format"] = oledb_dr[6].ToString().Trim();
                    tmp.Attributes["MaxSize"] = oledb_dr[7].ToString().Trim();
                    tmp.Attributes["DataOperation"] = oledb_dr[8].ToString().Trim();
                    tmp.Attributes["MinValue"] = oledb_dr[9].ToString().Trim();
                    tmp.Attributes["MaxValue"] = oledb_dr[10].ToString().Trim();
                    //tmp.Attributes["Value"] = oledb_dr[6].ToString().Trim();
                    out_xml.AddItem(tmp);
                }
            }
            out_xml.SaveToFile(OutPath, true);
            oledb_dr.Close();
            oledb_con.Close();
        }
    }
}

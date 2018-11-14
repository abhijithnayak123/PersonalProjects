using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using Novacode;

namespace TCF.Zeo.Peripheral.Printer.EpsonTMS9000.Impl
{
    public class DocXCreator
    {
        List<String> labelList = new List<string>();
        List<String> valueList = new List<string>();
        
        char CHR_PIPE = '|';
        String STR_DATA = "{data}";
        String STR_BLANK = " ";
        String STR_PLUS = "+";

        public String PrepareDocument(String receiptData)
        {
            Trace.WriteLine("Epson.TMS9000.Impl:PrepareDocument()", DateTime.Now.ToString());

            String[] receiptDataList = receiptData.Split(CHR_PIPE);
            Trace.WriteLine("Epson.TMS9000.Impl:PrepareDocument() dataListCount = " + receiptDataList.Count(), DateTime.Now.ToString());
            for (int i = 0; i < receiptDataList.Count(); i++)
            {
                if (i % 2 == 0) //Labels
                {
                    Trace.WriteLine("Epson.TMS9000.Impl:Label = " + receiptDataList[i], DateTime.Now.ToString());
                    labelList.Add(receiptDataList[i]);
                }
                else //data
                {
                    if ( receiptDataList[i].Length < 100 )
                        Trace.WriteLine("Epson.TMS9000.Impl:Value = " + receiptDataList[i], DateTime.Now.ToString());
                    else
                        Trace.WriteLine("Epson.TMS9000.Impl:Value = " + "Lengthy Data Not Printed", DateTime.Now.ToString());
                    valueList.Add(receiptDataList[i]);
                }
            }
            Trace.WriteLine("Epson.TMS9000.Impl:PrepareDocument() Completed", DateTime.Now.ToString());
            return ReplaceWithMetaData(WriteDocX());
        }

        private String ReplaceWithMetaData(String docFile)
        {
            Trace.WriteLine("Epson.TMS9000.Impl:ReplaceMetaData()", DateTime.Now.ToString());
            DocX doc = DocX.Load(docFile);
            for (int i = 0; i < labelList.Count() - 1; i++)
            {
                String strLabel = labelList[i];
                doc.ReplaceText(labelList[i], GetValue(strLabel));
            }

            //Check if {RemoveLine} Tag is present
            List<int> remLine = doc.FindAll("{RemoveLine}");
            if (remLine.Count > 0)
            {
                Trace.WriteLine("Epson.TMS9000.Impl:ReplaceMetaData():Requires Line Removal", DateTime.Now.ToString());
                //Interate Tables and Rows
                List<Table> tables = doc.Tables;
                for (int i = 0; i < tables.Count; i++)
                {
                    List<Row> rows = tables[i].Rows;
                    for (int j = 0; j < rows.Count; j++)
                    {
                        List<int> delRow = rows[j].FindAll("{RemoveLine}");
                        if (delRow.Count > 0)
                        {
                            Trace.WriteLine("Epson.TMS9000.Impl:ReplaceMetaData():Found Row in table " + i + " and row " + j + " to be deleted", DateTime.Now.ToString());
                            rows[j].Remove();
                        }
                    }
                }
            }

            doc.Save();
            doc.Dispose();
            Trace.WriteLine("Epson.TMS9000.Impl:ReplaceMetaData() Completed", DateTime.Now.ToString());
            return docFile;
        }

        private String WriteDocX()
        {
            Trace.WriteLine("Epson.TMS9000.Impl:WriteDocx()", DateTime.Now.ToString());

			string applicationFolder = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
			string docXFile = string.Format(@"{0}\Temp\{1}.nxo", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.Ticks);
            string fStr = GetValue(STR_DATA);

			fStr = fStr.Replace(STR_BLANK, STR_PLUS);
            
			Trace.WriteLine("Epson.TMS9000.Impl: Data Value Length = \'" + fStr.Length + "\'", DateTime.Now.ToString());
            byte[] decodedByt = Convert.FromBase64String(fStr);

            BinaryWriter Writer = new BinaryWriter(File.OpenWrite(docXFile));
            Writer.Write(decodedByt);
            Writer.Flush();
            Writer.Close();
            Trace.WriteLine("Epson.TMS9000.Impl:WriteDocx() Completed File:" + docXFile, DateTime.Now.ToString());
            return docXFile;
        }

        public String GetValue(string label)
        {
            String str = String.Empty;
            for (int i = 0; i < labelList.Count(); i++)
            {
                if ( labelList[i].Equals(label) )
                {
                    str = valueList[i];
                    break;
                }
            }
            return str;
        }
    }
}

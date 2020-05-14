using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace _ExcelLib
{
    public static class ExcelHandler
    {
        public static void CreateNewExcelFile(string filename)
        {
            //Creates an Excel Application instance
            Excel.Application excelApplication = new Excel.Application();
            excelApplication.Visible = true;

            //Creates an Excel Workbook with a default number of sheets
            Excel.Workbook excelWorkbook = excelApplication.Workbooks.Add();
            excelWorkbook.SaveAs(filename, AccessMode: Excel.XlSaveAsAccessMode.xlNoChange);

            //"eliminates" the instances
            excelWorkbook.Close();
            excelApplication.Quit();

            ReleaseCOMObjects(excelWorkbook);
        }

        public static void ReleaseCOMObjects(object excelWorkbook)
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorkbook);
            excelWorkbook = null;

            GC.Collect();
        }

        public static void WriteToExcelFile(string filename)
        {
            //Creates an Excel Application instance
            Excel.Application excelApplication = new Excel.Application();
            excelApplication.Visible = true;

            //Opens the excel file
            Excel.Workbook excelWorkbook = excelApplication.Workbooks.Open(filename);
            Excel.Worksheet excelWorksheet = excelWorkbook.ActiveSheet;

            //excelWorksheet.Cells[1, 1].Value = "Hello";
            //excelWorksheet.Cells[1, 2].Value = "world!";

            //ou
            string word = "Hello world";
            Excel.Range range = excelWorksheet.Range["A1", "B1"];
            range.Value = word;

            Excel.Worksheet excelWorksheet2 = excelApplication.Worksheets.Add();
            excelWorksheet2.Cells[1, 1].Value = "Goodbye";
            excelWorksheet2.Cells[1, 2].Value = "world!";

            //save the file
            excelWorkbook.Save();

            //"eliminates" the instances
            excelWorkbook.Close();
            excelApplication.Quit();

            ReleaseCOMObjects(excelWorkbook);
        }

        public static string ReadFromExcelFile(string filename)
        {
            //Creates an Excel Application instance
            var excelApplication = new Excel.Application();
            excelApplication.Visible = false;

            //Opens the excel file
            var excelWorkbook = excelApplication.Workbooks.Open(filename);
            var excelWorksheet = (Excel.Worksheet)excelWorkbook.ActiveSheet;

            string content = excelWorksheet.Cells[1, 1].Value;
            content += (excelWorksheet.Cells[1, 2] as Excel.Range).Text;
            

            //"eliminates" the instances
            excelWorkbook.Close();
            excelApplication.Quit();

            ReleaseCOMObjects(excelWorkbook);

            return content;
        }

        public static string Exercise_5_1(string filename, string n, string m)
        {
            //Creates an Excel Application instance
            var excelApplication = new Excel.Application();
            excelApplication.Visible = false;

            //Opens the excel file
            var excelWorkbook = excelApplication.Workbooks.Open(filename);
            var excelWorksheet = (Excel.Worksheet)excelWorkbook.ActiveSheet;

            string content = "";
            Excel.Range myRange = excelWorksheet.Range[n, m];
            foreach (Excel.Range range in myRange)
            {
                content = content + " " + range.Value;
            }

            //"eliminates" the instances
            excelWorkbook.Close();
            excelApplication.Quit();

            ReleaseCOMObjects(excelWorkbook);

            return content;
        }

        public static string Exercise_5_2(string filename, string worksheet)
        {
            //Creates an Excel Application instance
            var excelApplication = new Excel.Application();
            excelApplication.Visible = false;

            //Convert string worksheet to int
            int num = int.Parse(worksheet);

            //Opens the excel file
            var excelWorkbook = excelApplication.Workbooks.Open(filename);
            var excelWorksheet = (Excel.Worksheet) excelApplication.Worksheets.Item[num];

            string content = "";

            Excel.Range totalRange = excelWorksheet.UsedRange;
            int rows = totalRange.Rows.Count;
            int columns = totalRange.Columns.Count;

            foreach (Excel.Range range in totalRange)
            {
                content = content + " " + range.Value;
            }

            //"eliminates" the instances
            excelWorkbook.Close();
            excelApplication.Quit();

            ReleaseCOMObjects(excelWorkbook);

            return content;
        }

        public static void Exercise_5_3(string filename, string textToAdd, string cell)
        {
            //Creates an Excel Application instance
            var excelApplication = new Excel.Application();
            excelApplication.Visible = false;

            //Convert string worksheet to int

            //Opens the excel file
            var excelWorkbook = excelApplication.Workbooks.Open(filename);
            var excelWorksheet = (Excel.Worksheet)excelWorkbook.ActiveSheet;

            string[] lines = textToAdd.Split('\n');

            int colNumb = 0;
            int linNumb = lines.Length;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Split(' ').Length > colNumb)
                {
                    colNumb = lines[i].Split(' ').Length;
                }
            }

            string[,] content = new string[linNumb, colNumb];
            string[] wordsOnThatLine;
            for (int i = 0; i < linNumb; i++)
            {
                wordsOnThatLine = lines[i].Split(' ');
                for (int j = 0; j < wordsOnThatLine.Length; j++)
                {
                    content[i,j] = wordsOnThatLine[j];
                }
            }


            Excel.Range range = excelWorksheet.Range[cell];

           // string a = content.GetLength(0).ToString() + ' ' + content.GetLength(1).ToString();
            //string b = lines.Count().ToString() + colNumb.Count().ToString();
           // return a;
            range = range.Resize[content.GetLength(0), content.GetLength(1)];
            range.Value = content;

            //"eliminates" the instances
            excelWorkbook.Close();
            excelApplication.Quit();

            ReleaseCOMObjects(excelWorkbook);
        }

        public static string Exercise_5_4(string filename, string worksheet)
        {
            //Creates an Excel Application instance
            var excelApplication = new Excel.Application();
            excelApplication.Visible = false;

            //Convert string worksheet to int

            int num = int.Parse(worksheet);
            Console.WriteLine(num);

            //Opens the excel file
            var excelWorkbook = excelApplication.Workbooks.Open(filename);
            var excelWorksheet = (Excel.Worksheet)excelApplication.Worksheets.Item[num];
            
            Excel.Range totalRange = excelWorksheet.UsedRange;
            int rows = totalRange.Rows.Count;
            //int columns = totalRange.Columns.Count;

         
            //"eliminates" the instances
            excelWorkbook.Close();
            excelApplication.Quit();

            ReleaseCOMObjects(excelWorkbook);

            return rows.ToString();
        }

        public static string Exercise_5_5(string filename, string worksheet, string stringToSearch)
        {
            //Creates an Excel Application instance
            var excelApplication = new Excel.Application();
            excelApplication.Visible = false;

            //Convert string worksheet to int

            int num = int.Parse(worksheet);
            Console.WriteLine(num);

            //Opens the excel file
            var excelWorkbook = excelApplication.Workbooks.Open(filename);
            var excelWorksheet = (Excel.Worksheet)excelApplication.Worksheets.Item[num];

            Excel.Range totalRange = excelWorksheet.UsedRange;
            Excel.Range searchedRange = totalRange.Find(stringToSearch);

            bool exist = false;
            if (searchedRange != null)
            {
                exist = true;
            }

            string cell = "";

            if (exist)
            {
                cell = "[" + searchedRange.Row.ToString() + "," + searchedRange.Column.ToString() + "]";
            }
            else
            {
                cell = "not exist";
            }
            
            //"eliminates" the instances
            excelWorkbook.Close();
            excelApplication.Quit();

            ReleaseCOMObjects(excelWorkbook);

            return cell;
            
        }
        

        public static void CreateChart(string filename)
        {
            //Creates an Excel Application instance
            Excel.Application excelApplication = new Excel.Application();
            excelApplication.Visible = true;

            //Opens the excel file
            Excel.Workbook excelWorkbook = excelApplication.Workbooks.Open(filename);
            Excel.Worksheet excelWorksheet = excelWorkbook.ActiveSheet;

            //Add a chart object
            Excel.Chart myChart = null;
            Excel.ChartObjects charts = excelWorksheet.ChartObjects();
            Excel.ChartObject chartObject = charts.Add(50,50,300,300); //Left; top; Width; Height
            myChart = chartObject.Chart;

            //set chart range -- cell values to be used in the graph
            Excel.Range myRange = excelWorksheet.Range["B1", "B4"];
            myChart.SetSourceData(myRange);

            //chart properties using the named properties and default parameters functionality in the .Net Framwork
            myChart.ChartType = Excel.XlChartType.xlLine;
            myChart.ChartWizard(Source: myRange, 
                Title: "Graph Title",
                CategoryTitle: "Title of X axis...",
                CategoryLabels: "Title of Y axis...");

            excelWorkbook.SaveAs(filename);
            excelWorkbook.Close();
            excelApplication.Quit();

            ReleaseCOMObjects(excelWorkbook);
        }
    }
}

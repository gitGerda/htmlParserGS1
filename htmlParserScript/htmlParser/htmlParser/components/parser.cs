using System;
using System.Collections.Generic;
using System.Text;
using AngleSharp;
using System.IO;
using htmlParser.models;

namespace htmlParser.components
{
    public static class parser
    {
        static List<parsedTable> parsedTablesList = new List<parsedTable>();
        static StringBuilder logsLocal = new StringBuilder();
        public static async void parsing(string fileName)
        { 
            try
            {
                FileInfo file = new FileInfo(fileName);

                logsLocal.AppendLine();
                logsLocal.AppendLine("   # File name : "+file.Name);

                StreamReader stRead = file.OpenText();
                string fi = stRead.ReadToEnd();
                
                IBrowsingContext context = BrowsingContext.New();
                AngleSharp.Dom.IDocument doc = await context.OpenAsync(req => req.Content(fi));

                AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement> docTables = doc.QuerySelectorAll("table table");

                foreach (AngleSharp.Dom.IElement docTable in docTables)
                {
                    AngleSharp.Dom.IElement resultElem = null;
                    resultElem = docTable.QuerySelector("tr .gtinapp_err");

                    if (resultElem == null)
                    {
                        AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement> tableRows = docTable.QuerySelectorAll("tr");
                        parsedTable parsedTableNew = new parsedTable();

                        foreach (AngleSharp.Dom.IElement tableRow in tableRows)
                        {
                            AngleSharp.Dom.IHtmlCollection<AngleSharp.Dom.IElement> tableTds = tableRow.QuerySelectorAll("td");

                            if (tableTds.Length == 2)
                            {
                                switch (tableTds[0].TextContent)
                                {
                                    case "Наименование товара на этикетке:":
                                        {
                                            parsedTableNew.nameOfProduct = tableTds[1].TextContent.ToString();
                                            nameOfProdParse(ref parsedTableNew);
                                            break;
                                        }
                                    case "Количество/Мера нетто:":
                                        {
                                            parsedTableNew.countOfProduct = tableTds[1].TextContent.ToString();
                                            break;
                                        }
                                    case "Результат:":
                                        {
                                            parsedTableNew.barcodeOfProduct = tableTds[1].TextContent.ToString();
                                            resOfProdParse(ref parsedTableNew);
                                            break;
                                        }
                                }
                            }
                        }

                        logsLocal.AppendLine("       " + DateTime.Now.ToString() + "-->"+parsedTableNew.artOfProduct+"("+parsedTableNew.barcodeOfProduct+")"+" [parsedOK]");
                        parsedTablesList.Add(parsedTableNew);
                    }
                }

                stRead.Close();
                File.Delete(fileName);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void nameOfProdParse(ref parsedTable parsedTableNew)
        {
            string name = parsedTableNew.nameOfProduct;

            int ind = name.IndexOf("артикул");

            if(ind>0)
            {
                try
                {
                    int startInd = ind + 8;
                    int endInd = name.Length - startInd;
                    parsedTableNew.artOfProduct = name.Substring(startInd, endInd);
                    name = name.Remove(ind);
                    parsedTableNew.nameOfProduct = name;
                }
                catch(Exception ex)
                {
                    logsLocal.AppendLine("   # " + DateTime.Now.ToString() + "-->"+ "Error: Не удалось извлечь артикул из значения наименования");
                    //Console.WriteLine("Error: Не удалось извлечь артикул из значения наименования");
                }
            }
        }

        static void resOfProdParse(ref parsedTable parsedTableNew)
        {
            int ind = parsedTableNew.barcodeOfProduct.IndexOf("EAN-13:");
            if(ind>0)
            {
                try
                {
                    int startInd = ind + 8;
                    int endInd = parsedTableNew.barcodeOfProduct.Length - startInd;
                    parsedTableNew.barcodeOfProduct = parsedTableNew.barcodeOfProduct.Substring(startInd, endInd);
                }
                catch(Exception ex)
                {
                    logsLocal.AppendLine("   # " + DateTime.Now.ToString() + "-->" + "Error: Не удалось извлечь штрихкод из результата");
                    //Console.WriteLine("Error: Не удалось извлечь штрихкод из результата");
                }
            }
        }

        public static List<parsedTable> startParser(string FilesDir, ref StringBuilder logs)
        { 
            if (Directory.Exists(FilesDir))
            {
                logsLocal = logs;
                DirectoryInfo mainDir = new DirectoryInfo(FilesDir);
                FileInfo[] filesOfDir = mainDir.GetFiles("*.html");

                if(filesOfDir.Length!=0)
                {
                    logs.AppendLine("-------------------------------------------------------------------------------");
                    logs.AppendLine("##" + DateTime.Now.ToString() + " - Start ##");
                }

                foreach(FileInfo fileOfDir in filesOfDir)
                {
                    parsing(fileOfDir.FullName);
                }                
            }
            else
            {
                //Console.WriteLine("Error: No access or incorrect path to[PathToDocumentsDirectory]");
                logs.AppendLine("   # "+ DateTime.Now.ToString() + "-->"+ "Error: No access or incorrect path to[PathToDocumentsDirectory]");
            }

            if(logsLocal!=null)
            {
                logs = logsLocal;
            }

            return parsedTablesList;
        }
    }
}

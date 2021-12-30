using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using htmlParserGS1_service.parser.models;
using htmlParserGS1_service.parser.components;
using System.IO;
using AngleSharp;

namespace htmlParserGS1_service.parser.components
{
    public class parser
    {
        List<parsedTable> parsedTablesList = new List<parsedTable>();
        StringBuilder logsLocal = new StringBuilder();

        public async void parsing(string fileName)
        {
            try
            {
                FileInfo file = new FileInfo(fileName);

                string fi ="";

                using (FileStream fs = File.OpenRead(fileName))
                {
                    byte[] array = new byte[fs.Length];
                    fs.Read(array, 0, array.Length);
                    fi = System.Text.Encoding.UTF8.GetString(array);
                }

                logsLocal.AppendLine();
                logsLocal.AppendLine("   # File name : " + file.Name);


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

                        logsLocal.AppendLine("       " + DateTime.Now.ToString() + "-->" + parsedTableNew.artOfProduct + "(" + parsedTableNew.barcodeOfProduct + ")" + " [parsedOK]");
                        parsedTablesList.Add(parsedTableNew);
                    }
                }
                doc.Dispose();
                doc.Close();

                /*stRead.DiscardBufferedData();
                stRead.Dispose();
                stRead.Close();*/

                File.Delete(fileName);

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                logsLocal.AppendLine("   # " + DateTime.Now.ToString() + "--> ERROR: "+ex.Message+" ["+ex.StackTrace+"]");
            }
        }

        void nameOfProdParse(ref parsedTable parsedTableNew)
        {
            string name = parsedTableNew.nameOfProduct;

            int ind = name.IndexOf("артикул");

            if (ind > 0)
            {
                try
                {
                    int startInd = ind + 8;
                    int endInd = name.Length - startInd;
                    parsedTableNew.artOfProduct = name.Substring(startInd, endInd);
                    name = name.Remove(ind);
                    parsedTableNew.nameOfProduct = name;
                }
                catch (Exception ex)
                {
                    logsLocal.AppendLine("   # " + DateTime.Now.ToString() + "-->" + "Error: Не удалось извлечь артикул из значения наименования");
                    //Console.WriteLine("Error: Не удалось извлечь артикул из значения наименования");
                }
            }
        }

        void resOfProdParse(ref parsedTable parsedTableNew)
        {
            int ind = parsedTableNew.barcodeOfProduct.IndexOf("EAN-13:");
            if (ind > 0)
            {
                try
                {
                    int startInd = ind + 8;
                    int endInd = parsedTableNew.barcodeOfProduct.Length - startInd;
                    parsedTableNew.barcodeOfProduct = parsedTableNew.barcodeOfProduct.Substring(startInd, endInd);
                }
                catch (Exception ex)
                {
                    logsLocal.AppendLine("   # " + DateTime.Now.ToString() + "-->" + "Error: Не удалось извлечь штрихкод из результата");
                    //Console.WriteLine("Error: Не удалось извлечь штрихкод из результата");
                }
            }
        }

        public List<parsedTable> goParser(string FilePath, ref StringBuilder logs)
        {
            logs.AppendLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            logs.AppendLine("##" + DateTime.Now.ToString() + " - Start Parsing ##");

            logsLocal = logs;

            parsing(FilePath);

            logs = logsLocal;

            return parsedTablesList;
        }
    
    }
}

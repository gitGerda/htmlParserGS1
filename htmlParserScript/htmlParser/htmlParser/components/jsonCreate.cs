using System;
using System.Collections.Generic;
using System.Text;
using htmlParser.models;

namespace htmlParser.components
{
    public static class jsonCreate
    {
        public static string getJson(List<parsedTable> listParsedTables)
        {
            int index = listParsedTables.Count;
            string jsonDoc = "[";

            foreach(parsedTable table in listParsedTables)
            {
                jsonDoc += "{";
                jsonDoc += @"""Артикул"":""" + table.artOfProduct.Replace(@"""", "'") + @""",";
                jsonDoc += @"""Наименование"":""" + table.nameOfProduct.Replace(@"""","'") + @""",";
                jsonDoc += @"""Количество"":""" + table.countOfProduct.Replace(@"""", "'") + @""",";
                jsonDoc += @"""Штрихкод"":""" + table.barcodeOfProduct.Replace(@"""", "'") + @"""}";

                index = index - 1;
                if(index!=0)
                {
                    jsonDoc += ",";
                }
                else
                {
                    jsonDoc += "]";
                }
            }

            return jsonDoc;
        }
    }
}

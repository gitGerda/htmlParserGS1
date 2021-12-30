using System;
using System.Collections.Generic;
using System.Text;
using htmlParser.models;

namespace htmlParser.components
{
    public static class argsCheck
    {
        public static bool argumentsCheckFunc(string[] args, ref param paramObj)
        {
            try
            {
                int index = 0;

                if (args.Length < 6)
                {
                    throw new Exception("Error: Не верно заданы параметры!");
                }

                foreach(string param in args)
                {
                    try
                    {
                        switch (param)
                        {
                            case "--host":
                                {
                                    paramObj.host = args[index + 1];
                                    break;
                                }
                            case "--pwd":
                                {
                                    paramObj.pwd = args[index + 1];
                                    break;
                                }
                            case "--dir":
                                {
                                    paramObj.dir = args[index + 1];
                                    break;
                                }
                               
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Error: Не верно заданы параметры!");
                        return false;
                    }

                    index = index + 1;
                }
                
                if(paramObj.dir=="" || paramObj.host=="" || paramObj.pwd=="")
                {
                    Console.WriteLine("Error: Не верно заданы параметры!");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }

        }
    }
}

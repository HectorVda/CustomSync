using Aplication;
using System;
using System.IO;

namespace CustomSyncCLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Welcome to CustomSync");

            string configFilePath = "";
            do
            {
                Console.WriteLine("Please insert a valid config file path:");
                configFilePath = Console.ReadLine();
            } while (string.IsNullOrEmpty(configFilePath) || !File.Exists(configFilePath));

            try
            {
                ConfigApplicationService configApplicationService = new ConfigApplicationService();
                Trasversal.Result<Model.Config> configResponse = configApplicationService.ReadConfig(configFilePath);
                if (configResponse.ActionResult && configResponse.ResultObject != null)
                {
                    Console.WriteLine("Config is Ok - Starting process... ");

                    Model.Config config = configResponse.ResultObject;
                    FileApplicationService fileService = new FileApplicationService();
                    Console.WriteLine("Coping Files, please be patient....");
                    Trasversal.Result resultado = fileService.CopyFilesReorganizing(config);

                    if (resultado.ActionResult)
                    {
                        Console.WriteLine("SUCCESS - No errors found :)");
                    }
                    else
                    {
                        Console.WriteLine("Warning - Errors found :(");
                        Console.WriteLine("*********************************");
                        Console.WriteLine("************ ERRORS *************");
                        Console.WriteLine("*********************************");
                        foreach (string error in resultado.Errors)
                        {
                            Console.WriteLine(error);

                        }

                    }


                }


            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }


        }
    }
}

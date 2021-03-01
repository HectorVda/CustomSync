using Model;
using Newtonsoft.Json;
using System.IO;
using Trasversal;

namespace Aplication
{
  public  class ConfigApplicationService
    {
        public Result<Config> ReadConfig(string configFilePath)
        {
            Result<Config> result = new Result<Config>();
           
            try
            {
                if (!File.Exists(configFilePath)) {
                    result.Errors.Add("Invalid File Path");
                    result.ActionResult = false;
                    return result;
                }
                if(string.IsNullOrEmpty(Path.GetExtension(configFilePath)) || Path.GetExtension(configFilePath).ToLower() != ".json")
                {
                    result.Errors.Add("Config file must be a valid .json");
                    result.ActionResult = false;
                    return result;
                }

                using (StreamReader file = File.OpenText(configFilePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var configuration = (Config)serializer.Deserialize(file, typeof(Config));
                    result.ResultObject = configuration;

                }


            }
            catch (System.Exception ex)
            {
                result.ActionResult = false;
                result.Errors.Add(ex.Message);
                
            }
            return result;
        }

    }
}

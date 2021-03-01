using Model;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Trasversal;

namespace Aplication
{
    public class FileApplicationService
    {
        public Result CopyFilesReorganizing(Config config)
        {
            Result result = new Result();



            IEnumerable<Task> taskList = new List<Task>();

            if (config.SourcePaths != null && config.SourcePaths.Any())
            {
                taskList = config.SourcePaths.Select(sp => CopyDirectoryAsync(sp, config));
            }

            Task.WaitAll(taskList.ToArray());

            return result;
        }

        public async Task<Result> CopyDirectoryAsync(string origin, Config config)
        {
            Result result = new Result();
            List<Task> taskList = new List<Task>();
            try
            {
                if (config.BackupOriginalFolder)
                {
                    ZipFile.CreateFromDirectory(origin, $"{config.DestinationPath[0]}{Path.DirectorySeparatorChar}{System.DateTime.Now.Year}-{System.DateTime.Now.Month}-{System.DateTime.Now.Day}-BackUp.zip");
                }
                string[] filePaths = ReadDirectoryFilesRecursive(origin);
                Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
                OrderFilesByCreationDateAndExtension(filePaths, dictionary);

                foreach (KeyValuePair<string, List<string>> dictionaryEntry in dictionary)
                {
                    foreach (string destinationDirectory in config.DestinationPath)
                    {

                        Task<IEnumerable<Task>> innerTaskList = ManageDirectoryAsync(destinationDirectory, dictionaryEntry, config);
                        taskList.Add(innerTaskList);
                    }
                }

                Task.WaitAll(taskList.ToArray());
            }
            catch (System.Exception ex)
            {
                result.ActionResult = false;
                result.Errors.Add(ex.Message);

            }
            return result;
        }

        private static async Task<IEnumerable<Task>> ManageDirectoryAsync(string destinationDirectory, KeyValuePair<string, List<string>> item, Config config)
        {
            IEnumerable<Task> taskList;
            string directoryStructure = GetDirectoryStructureFromDictionaryKey(item.Key);
            string destinationFolder = GetDestinationFolder(destinationDirectory, directoryStructure);
            CreateDestinationFolder(destinationFolder);

            taskList = item.Value.Select(f => CopyFileAsync(destinationFolder, f, config));
            Task.WaitAll(taskList.ToArray());

            return taskList;
        }

        private static async Task CopyFileAsync(string destinationFolder, string filePath, Config config)
        {

            string destName = destinationFolder + Path.DirectorySeparatorChar + GetFileName(filePath);
            if (!File.Exists(destName))
            {
                if (config.MoveFiles)
                {

                    File.Move(filePath, destName);
                }
                else
                {

                    File.Copy(filePath, destName);
                }
            }
        }

        private static string GetDestinationFolder(string destinationDirectory, string directoryStructure)
        {
            return destinationDirectory + Path.DirectorySeparatorChar + directoryStructure;
        }

        private static void CreateDestinationFolder(string destinationFolder)
        {
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);


            }
        }

        private static void OrderFilesByCreationDateAndExtension(string[] filePaths, Dictionary<string, List<string>> dictionary)
        {
            foreach (string filePath in filePaths)
            {
                string name = GetFileName(filePath);
                string key = ComponeDictionaryKey(filePath, name);

                if (dictionary.ContainsKey(key))
                {
                    dictionary[key].Add(filePath);

                }
                else
                {
                    dictionary[key] = new List<string>() { name };

                }
            }
        }

        private static string ComponeDictionaryKey(string filePath, string name)
        {

            System.DateTime creationTime = File.GetCreationTime(filePath);
            System.DateTime modifiedTime = File.GetLastWriteTime(filePath);
            if (modifiedTime < creationTime)
            {
                creationTime = modifiedTime;
            }
            string year = creationTime.Year.ToString("00");
            string month = creationTime.Month.ToString("00");
            string extension = Path.GetExtension(filePath);

            string key = $"{year}#{month}#{extension}";
            return key;
        }
        private static string GetDirectoryStructureFromDictionaryKey(string key)
        {


            string[] keyItems = key.Split('#');
            string year = keyItems[0];
            string month = keyItems[1];
            string extension = keyItems[2];

            return $"{year}{Path.DirectorySeparatorChar}{month}{Path.DirectorySeparatorChar}{extension}";

        }
        private static string GetFileName(string filePath)
        {
            return filePath.Split(Path.DirectorySeparatorChar).Last();
        }

        private static string[] ReadDirectoryFilesRecursive(string origin)
        {
            return Directory.GetFiles(origin, "*", SearchOption.AllDirectories);
        }
    }
}

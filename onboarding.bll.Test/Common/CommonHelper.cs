using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace onboarding.bll.Test.Common
{
    public class CommonHelper
    {
        public static T LoadDataFromFile<T>(string folderFilePath)
        {
            Console.WriteLine(folderFilePath);
            string path = Path.Combine(Environment.CurrentDirectory, folderFilePath);
            T result = default;
            using (var reader = new StreamReader(path))
            {
                var data = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<T>(data);
            }
            return result;
        }
    }
}

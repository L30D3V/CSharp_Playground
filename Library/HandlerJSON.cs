using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Library {
    public class HandlerJSON {
        public void ExtractJsonData(string filepath) {
            if (File.Exists(filepath)) {
                using (StreamReader jsonFile = new StreamReader(filepath)) {
                    string json = jsonFile.ReadToEnd();
                    dynamic jsonArray = JsonConvert.DeserializeObject<dynamic>(json);

                    Console.WriteLine(jsonArray);
                    Console.WriteLine(jsonArray.testUrl);
                    Console.WriteLine(jsonArray.header);
                    Console.WriteLine(jsonArray.header[0]);
                }
            }
        }
    }
}
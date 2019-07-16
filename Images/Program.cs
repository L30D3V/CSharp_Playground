using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Images
{
    class Program
    {
        static readonly string rootFolder = @"C:\Users\leonardot.MOSTQI\Documents\Projetos\Ref\CSharp_Playground\Images";
        static readonly string imageFolderPath = @"\images\";
        static readonly string base64FolderPath = @"\base64\";
        static void Main(string[] args)
        {
            List<string> files = Directory.GetFiles(rootFolder + imageFolderPath).ToList();
            
            foreach(string file in files) {
                string imagePath = file;
                string base64 = Base64String(imagePath);

                string base64file = Path.GetFileNameWithoutExtension(file);
                
                using(StreamWriter writer = new StreamWriter(rootFolder + base64FolderPath + base64file + ".txt"))
                {
                    writer.Write(base64);
                }
            }
        }

        static string Base64String(string imagePath) 
        {
            if (File.Exists(imagePath)){
                byte[] imageByte = File.ReadAllBytes(imagePath);
                return Convert.ToBase64String(imageByte);
            } else {
                return null;
            }
        }
    }
}

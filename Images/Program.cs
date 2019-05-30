using System;
using System.IO;
using System.Text;

namespace Images
{
    class Program
    {
        static readonly string rootFolder = @"C:\Users\Leonardot\Documents\Projetos\Ref\CSharp_Playground\Images";
        static readonly string imageFolderPath = @"\images";
        static readonly string base64FolderPath = @"\base64";
        static void Main(string[] args)
        {
            string imagePath = @"\COMPROVANTE_DE_RENDIMENTO_MARIDO.jpg";
            string base64 = Base64String(imagePath);

            using(StreamWriter writer = new StreamWriter(rootFolder + base64FolderPath + @"\base64.txt"))
            {
                writer.WriteLine(base64);
            }
        }

        static string Base64String(string imagePath) 
        {
            if (File.Exists(rootFolder + imageFolderPath + imagePath)){
                byte[] imageByte = File.ReadAllBytes(rootFolder + imageFolderPath + imagePath);
                return Convert.ToBase64String(imageByte);
            } else {
                return null;
            }
        }
    }
}

using System;
using System.IO;

class FindData
{
    static void Main(string[] args)
    {
        string directory_in_str = "D:\\ExportProject\\All Goods\\Scripts";
        DirectoryInfo directory = new DirectoryInfo(directory_in_str);
        FileInfo[] files = directory.GetFiles("*.cs", SearchOption.AllDirectories);

        foreach (FileInfo file in files)
        {
            string path_in_str = file.FullName;
            StreamReader reader = new StreamReader(path_in_str);
            string contents = reader.ReadToEnd();
            if (contents.Contains("ScriptableObject"))
            {
                Console.WriteLine(path_in_str);
            }
            reader.Close();
        }
        
        Console.ReadKey();
    }
}
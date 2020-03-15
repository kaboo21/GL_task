using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
//using Newtonsoft.Json;


namespace GL_task
{
    class Program
    {
        

        class Catalog
        {
            public static JsonSerializerOptions serializeOptions = new JsonSerializerOptions();


            //return Folder with files and nested folders
            public static Folder GetFolder(string path)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);

                if (dirInfo.Exists)
                {
                    //create result Folder
                    var resultFolder = new Folder();
                    resultFolder.Files = new List<File>();
                    resultFolder.Children = new List<Folder>();

                    //Name
                    resultFolder.Name = dirInfo.Name;

                    //DateCreated
                    resultFolder.DateCreated = dirInfo.CreationTime;

                    //Get a reference to each file in that directory.
                    FileInfo[] fiArr = dirInfo.GetFiles();

                    if (fiArr.Length > 0)
                    {
                        //ADD FILES
                        foreach (FileInfo f in fiArr)
                        {
                            var file = new File();

                            //Name
                            file.Name = f.Name;

                            //Size
                            file.Size = String.Format($"{f.Length} B");

                            //Path  f.TOstring or f.DirectoryName
                            file.Path = f.FullName;

                            //Add file to Files in Folder class 
                            resultFolder.Files.Add(file);
                        }
                    }
                    else
                        resultFolder.Files = null;


                    //WORKING WITH CHILDREN/SUB FOLDERS     //dirs - directories

                    string[] dirs = Directory.GetDirectories(path);

                    if (dirs.Length > 0)
                    {
                        foreach (var item in dirs)
                        {
                            Folder f = new Folder();
                            f = GetFolder(item);
                            resultFolder.Children.Add(f);
                        }
                    }
                    else
                        resultFolder.Children = null;

                    //return Folder
                    return resultFolder;
                }
                else
                {
                    Console.WriteLine("There is no folder with this name/path");
                    //no folder result
                    return null;
                }
                
            }

            //Serialize Folder to Json file "Result.json"
            public static void SerializedFolderToJsonFile(Folder folder)
            {
                serializeOptions.Converters.Add(new DateTimeConverter());
                serializeOptions.WriteIndented = true;
                serializeOptions.IgnoreNullValues = true;
                serializeOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic);

                // Saving to json file
                using (FileStream fs = new FileStream("Result.json", FileMode.OpenOrCreate))
                {
                    JsonSerializer.SerializeAsync<Folder>(fs, folder, serializeOptions);
                    Console.WriteLine("Data has been saved to file");
                }

                
            }

            //Showing serialized Folder in Console
            public static void ShowSerializedFolder(Folder folder)
            {
                
                string json = JsonSerializer.Serialize<Folder>(folder, serializeOptions);
                Console.WriteLine(json);
            }
        }


        class Folder
        {
            public string Name { get; set; }

            public DateTime DateCreated { get; set; }

            public List <File> Files{ get; set; }
            
            //nested Folders
            public List <Folder> Children { get; set; }

        }

        class File
        {
            public string Name { get; set; }
            public string Size { get; set; }
            public string Path { get; set; }

        }



    static void Main(string[] args)
        {

            //INPUT PATH
            string path = @"C:\Програми";

           
            Folder folder = Catalog.GetFolder(path);

            //Serialize Folder to Json file "Result.json"
            Catalog.SerializedFolderToJsonFile(folder);

            //Show result in Console
            Catalog.ShowSerializedFolder(folder);


            

            

           
        }

    }
}

using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
//using Newtonsoft.Json;


namespace GL_task
{
    class Program
    {
        

        class CatalogInfo
        {
            public static Folder GetFolder(string path)
            {
                //create result Folder
                var resultFolder = new Folder();
                resultFolder.Files = new List<File>();
                resultFolder.Children = new List<Folder>();



                DirectoryInfo dirInfo = new DirectoryInfo(path);

                //Name
                resultFolder.Name = dirInfo.Name;

                //DateCreated
                resultFolder.DateCreated = dirInfo.CreationTime;

                //Get a reference to each file in that directory.
                FileInfo[] fiArr = dirInfo.GetFiles();

                if (fiArr.Length >0)
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


                //WORKING WITH CHILDREN/SUB FOLDERS

                string[] dirs = Directory.GetDirectories(path);

                if (dirs.Length >0)
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
        }
        class Folder
        {
            public string Name { get; set; }

            public DateTime DateCreated { get; set; }

            public List <File> Files{ get; set; }
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
            string path = @"C:\TestGL";

           

            Folder folder = CatalogInfo.GetFolder(path);


            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new DateTimeConverter());
            serializeOptions.WriteIndented = true;
            serializeOptions.IgnoreNullValues = true;
            string json = JsonSerializer.Serialize<Folder>(folder,serializeOptions);
            Console.WriteLine(json);


            //// сохранение данных
            //using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
            //{
            //    Folder folder = new Folder() { Name = "Project", DateCreated = DateTime.Now };
            //    JsonSerializer.SerializeAsync<Folder>(fs, folder);
            //    Console.WriteLine("Data has been saved to file");
            //}



            //DirectoryInfo dirInfo = new DirectoryInfo(path);
            //Console.WriteLine($"Название каталога: {dirInfo.Name}");
            //Console.WriteLine($"Время создания каталога: {dirInfo.CreationTime}");



            //// Make a reference to a directory.
            //DirectoryInfo di = new DirectoryInfo("c:\\");
            //// Get a reference to each file in that directory.
            //FileInfo[] fiArr = di.GetFiles();
            //// Display the names and sizes of the files.
            //Console.WriteLine("The directory {0} contains the following files:", di.Name);
            //foreach (FileInfo f in fiArr)
            //    Console.WriteLine("The size of {0} is {1} bytes.", f.Name, f.Length);

            //if (Directory.Exists(path))
            //{
            //    Console.WriteLine("Подкаталоги:");
            //    string[] dirs = Directory.GetDirectories(path);
            //    foreach (string s in dirs)
            //    {
            //        Console.WriteLine(s);
            //    }
            //    Console.WriteLine();
            //    Console.WriteLine("Файлы:");
            //    string[] files = Directory.GetFiles(path);
            //    foreach (string s in files)
            //    {
            //        Console.WriteLine(s);
            //        Console.WriteLine(s.Length);
            //    }
        }
        
    }
}

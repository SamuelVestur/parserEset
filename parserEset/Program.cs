using System;
using System.Collections.Generic;
using System.IO;

class InfectedFile
{
    public string Name { get; set; }
    public string Archiver { get; set; }
    public string Packer { get; set; }
    public string Threat { get; set; }
   
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Paste the path to the source file: (for example - C:\\My Stuff\\Programovanie\\Zadanie\\ESET_Parser attachment.log)");
        string filePath = Console.ReadLine();

        List<InfectedFile> infectedFiles = ParseInfectedFiles(filePath);

        PrintInfectedFiles(infectedFiles);
        PrintSummary(infectedFiles);

        Console.ReadKey();
    }

    static List<InfectedFile> ParseInfectedFiles(string filePath)
    {
        List<InfectedFile> infectedFiles = new List<InfectedFile>();

        // Načítanie obsahu vstupného súboru
        using (StreamReader reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                if (line.StartsWith("name="))
                {
                    // Parsovanie riadku a extrahovanie informácií
                    string name = GetValueFromLine(line, "name=");
                    string threat = GetValueFromLine(line, "threat=");
                    string archiver = GetArchiver(name);
                    string packer = GetPacker(name);

                    // Vytvorenie objektu InfectedFile a pridanie do zoznamu
                    InfectedFile infectedFile = new InfectedFile
                    {
                        Name = name,
                        Threat = threat,
                        Archiver = archiver,
                        Packer = packer
                    };

                    infectedFiles.Add(infectedFile);
                }
            }
        }

        return infectedFiles;
    }

    static string GetValueFromLine(string line, string key)
    {
        int startIndex = line.IndexOf(key) + key.Length;
        int endIndex = line.IndexOf(",", startIndex);

        if (endIndex == -1)
        {
            endIndex = line.Length;
        }

        return line.Substring(startIndex, endIndex - startIndex).Trim('\"');
    }

    static string GetArchiver(string fileName)
    {
        string[] parts = fileName.Split(" » ");
        if (parts.Length > 1)
        {
            return parts[1].Replace(" »", "").Trim();
        }
        else
        {
            return "";
        }
    }

    static string GetPacker(string fileName)
    {
        string[] parts = fileName.Split(" » ");
        if (parts.Length > 2)
        {
            return parts[2].Trim();
        }
        else
        {
            return "";
        }
    }

    static void PrintSummary(List<InfectedFile> infectedFiles)
    {
        int totalCount = infectedFiles.Count;
        int okCount = infectedFiles.Count(file => file.Threat == "is OK");
        int infectedCount = totalCount - okCount;

        Console.WriteLine($"\nTotal Files: {totalCount}");
        Console.WriteLine($"Infected Files: {infectedCount}");
        Console.WriteLine($"Non-infected Files: {okCount}");
    }

    static void PrintInfectedFiles(List<InfectedFile> infectedFiles)
    {
        Console.WriteLine("\nInfected Files:");
        int count = 1;
        foreach (InfectedFile file in infectedFiles)
        {
            if (file.Threat != "is OK")
            {
                string fileName = RemoveArchiverFromFileName(file.Name);
                string archive = RemoveArchiverFromThreat(file.Archiver);
                string packer = RemovePackerFromFileName(file.Name);

                Console.WriteLine($"[{count}] File: {fileName} | Threat: {file.Threat}" +
                                  (!string.IsNullOrEmpty(archive) ? $" | Archive: {archive}" : "") +
                                  (!string.IsNullOrEmpty(packer) ? $" | Packer: {packer}" : ""));
                count++;
            }
        }
    }

    static string RemoveArchiverFromFileName(string fileName)
    {
        string[] parts = fileName.Split(" » ");
        if (parts.Length > 1)
        {
            return parts[0].Trim();
        }
        else
        {
            return fileName;
        }
    }

    static string RemoveArchiverFromThreat(string threat)
    {
        if (threat.StartsWith("NSIS"))
        {
            return threat.Replace("» NSIS »", "").Trim();
        }
        else
        {
            return threat;
        }
    }

    static string RemovePackerFromFileName(string fileName)
    {
        string[] parts = fileName.Split(" » ");
        if (parts.Length > 2)
        {
            return parts[2].Trim();
        }
        else
        {
            return "";
        }
    }
}

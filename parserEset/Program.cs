using parserEset;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("A parser that identifies infected files.");
        Console.WriteLine();
        Console.WriteLine("Insert the path to the source file with the extension .log or .txt. \n(For example = C:\\My Stuff\\Programovanie\\Zadanie\\ESET_Parser attachment.log)");
        string filePath = Console.ReadLine();

        /*
            vytvorenie premennej infectedFiles typu List<InfectedFile>, metóda ParseInfectedFiles je volaná argumentom filePath 
            čo je cesta k vstupnému súboru
        */
        List<InfectedFile> infectedFiles = ParseInfectedFiles(filePath);

        //volanie metod PrintInfectedFiles() a PrintSummary()
        PrintInfectedFiles(infectedFiles);
        PrintSummary(infectedFiles);

        Console.ReadKey();
    }


    /*
        Táto metóda "ParseInfectedFiles" vykoná spracovanie vstupného súboru s cestou filePath 
        a vytvára zoznam objektov typu "InfectedFile". Metóda otvára vstupný súbor pomocou 
        "StreamReader" a postupne spracovava každý riadok v súbore. 
     */
    static List<InfectedFile> ParseInfectedFiles(string filePath)
    {
        //vytvorenie objektu typu List<InfectedFile>
        List<InfectedFile> infectedFiles = new List<InfectedFile>();

        // Načítanie obsahu vstupného súboru
        using (StreamReader reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                // overenie pomocou metódy "StartsWith()" či daný retazec začína danou hodnotou "name="
                if (line.StartsWith("name="))
                {
                    // Parsovanie riadku a extrahovanie informácií
                    string name = GetValueFromLine(line, "name=");
                    string threat = GetValueFromLine(line, "threat=");
                    string archiver = GetArchiver(name);
                    string packer = GetPacker(name);
                    string info = GetValueFromLine(line, "info=");

                    // Vytvorenie objektu InfectedFile a pridanie do zoznamu
                    InfectedFile infectedFile = new InfectedFile
                    {
                        Name = name,
                        Threat = threat,
                        Archiver = archiver,
                        Packer = packer,
                        Info = info
                    };

                    infectedFiles.Add(infectedFile);
                }
            }
        }

        return infectedFiles;
    }

    /*
        Metóda PrintInfectedFiles slúži na výpis informácií o infikovaných súborov 
        na konzolu. Metóda prechádza každý objekt InfectedFile v zozname infectedFiles pomocou cyklu foreach.
        Pre každý infikovaný súbor, ktorý nie je označený ako "is OK", sa vykoná mnou navrhnuté zobrazenie 
        pre infikované súbory. 
     */

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
                string info = file.Info;

                Console.Write($"[{count}] File path: {fileName} | Threat: {file.Threat}" +
                                  (!string.IsNullOrEmpty(archive) ? $" | Archive: {archive}" : "") +
                                  (!string.IsNullOrEmpty(packer) ? $" | Packer: {packer}" : ""));

                if (!string.IsNullOrEmpty(info))
                {
                    Console.Write($" | Info: {info}");
                }

                Console.WriteLine();
                count++;
            }
        }
    }

    /*
        Metóda GetValueFromLine() slúži na získanie hodnoty zo zadaného riadku na základe kľúča.
     */
    static string GetValueFromLine(string line, string key)
    {
        int startIndex = line.IndexOf(key) + key.Length;
        int endIndex = line.IndexOf(",", startIndex);

        // -1 znamená, že žiadna čiarka sa nenaška, nastaví sa index konca na koniec reťazca 
        if (endIndex == -1)
        {
            endIndex = line.Length;
        }
        /*
            operácia Szbstring, ktorá vytvorí podretazec v riadku.
            Tento podretazec je od indexu zaciatku po koniec a tým 
            dostaneme samotnu hodnotu. Pomocou metódy Trim odstranime 
            uvodzovky.
         */
        return line.Substring(startIndex, endIndex - startIndex).Trim('\"');
    }

    /*
       Metóda slúži na získanie informácií o archíve. Parametrom je fileName čo je reťazec obsahujúci názov súboru.
     
       Rozdelí vstupný súbor na viacero častí pomocou metódy Split, kde oddeľovacím reťazcom
       je tento symbol " » ". Ak existuje aspoň jedna časť tak pomocou metódy Replace()
       sa nahradí za prazdny string. 

        Ak sa v názve súboru nevyskytuje sekcia archivátora, vráti sa prázdny reťazec
     */
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

    // Metóda slúžiaca na vypísanie informácií o počte daných súborov. 
    static void PrintSummary(List<InfectedFile> infectedFiles)
    {
        int totalCount = infectedFiles.Count;
        int okCount = infectedFiles.Count(file => file.Threat == "is OK");
        int infectedCount = totalCount - okCount;

        Console.WriteLine($"\nTotal Files: {totalCount}");
        Console.WriteLine($"Infected Files: {infectedCount}");
        Console.WriteLine($"Non-infected Files: {okCount}");
    }


    // Metódy slúžiace na odstránenie.

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

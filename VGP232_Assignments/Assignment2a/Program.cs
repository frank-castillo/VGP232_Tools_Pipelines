using System;
using System.Collections.Generic;
using System.IO;

// TODO: Fill in your name and student number.
// Assignment 1
// NAME: Juan Francisco Castillo Regalado
// STUDENT NUMBER: 2042805

namespace Assignment2a
{
    internal class MainClass
    {
        public static void Main(string[] args)
        {
            // Variables and flags

            // The path to the input file to load.
            string inputFile = string.Empty;

            // The path of the output file to save.
            string outputFile = string.Empty;

            // The flag to determine if we overwrite the output file or append to it.
            bool appendToFile = false;

            // The flag to determine if we need to display the number of entries
            bool displayCount = false;

            // The flag to determine if we need to sort the results via name.
            bool sortEnabled = false;

            // The column name to be used to determine which sort comparison function to use.
            string sortColumnName = string.Empty;

            // The results to be output to a file or to the console
            List<Weapon> results = new List<Weapon>();

            //args = new string[7];
            //args[0] = "-i";
            //args[1] = "data.csv";
            //args[2] = "-o";
            //args[3] = "output.csv";
            //args[4] = "-c";
            //args[5] = "-s";
            //args[6] = "Rarity";

            Console.WriteLine(args[6]);

            for (int i = 0; i < args.Length; i++)
            {
                // h or --help for help to output the instructions on how to use it
                if (args[i].ToLower() == "-h" || args[i].ToLower() == "--help")
                {
                    Console.WriteLine("-i <path> or --input <path> : loads the input file path specified (required)");
                    Console.WriteLine("-o <path> or --output <path> : saves result in the output file path specified (optional)");

                    // TODO: include help info for count
                    Console.WriteLine("-c or --count : displays the number of entries in the input file (optional).");

                    // TODO: include help info for append
                    Console.WriteLine("-a or --append : enables append mode when writing to an existing output file (optional)");

                    // TODO: include help info for sort
                    Console.WriteLine("-s or --sort <column name> : outputs the results sorted by column name");
                    Console.WriteLine("Available Columns: Name, Type, Rarity, BaseAttack");

                    break;
                }
                else if (args[i].ToLower() == "-i" || args[i].ToLower() == "--input")
                {
                    // Check to make sure there's a second argument for the file name.
                    if (args.Length > i + 1)
                    {
                        // stores the file name in the next argument to inputFile
                        ++i;
                        inputFile = args[i];
                        bool wasErrorSolved = false;

                        do
                        {
                            if (string.IsNullOrEmpty(inputFile))
                            {
                                // TODO: print no input file specified.
                                Console.WriteLine("No input file specified. Please select a file to open!");
                                Console.WriteLine("Enter input file: ");
                                inputFile = Console.ReadLine();
                            }
                            else if (!File.Exists(inputFile))
                            {
                                // TODO: print the file specified does not exist.
                                Console.WriteLine("The input file does not exist. Please verify the file name!");
                                Console.WriteLine("Enter input file: ");
                                inputFile = Console.ReadLine();
                            }
                            else
                            {
                                // This function returns a List<Weapon> once the data is parsed.
                                wasErrorSolved = true;
                                results = Parse(inputFile);
                            }
                        } while (!wasErrorSolved);
                    }
                }
                else if (args[i].ToLower() == "-s" || args[i].ToLower() == "--sort")
                {
                    // TODO: set the sortEnabled flag and see if the next argument is set for the column name
                    // TODO: set the sortColumnName string used for determining if there's another sort function.
                    sortEnabled = true;

                    if (args.Length > i + 1)
                    {
                        // Store column's name to sort by in it's variable
                        ++i;
                        sortColumnName = args[i];

                        if (string.IsNullOrEmpty(sortColumnName))
                        {
                            Console.WriteLine("No column specified to sort by. Sorting will be done by Name.");
                            sortColumnName = "name";
                        }
                        else if ((sortColumnName.ToLower() != "type") &&
                                (sortColumnName.ToLower() != "rarity") &&
                                (sortColumnName.ToLower() != "name") &&
                                (sortColumnName.ToLower() != "baseattack"))
                        {
                            Console.WriteLine("Not a valid column name. Sorting will be done by Name.");
                            sortColumnName = "name";
                        }
                    }
                }
                else if (args[i].ToLower() == "-c" || args[i].ToLower() == "--count")
                {
                    displayCount = true;
                }
                else if (args[i].ToLower() == "-a" || args[i].ToLower() == "--append")
                {
                    // TODO: set the appendToFile flag
                    appendToFile = true;
                }
                else if (args[i].ToLower() == "-o" || args[i].ToLower() == "--output")
                {
                    // validation to make sure we do have an argument after the flag
                    if (args.Length > i + 1)
                    {
                        // increment the index.
                        ++i;
                        string filePath = args[i];

                        if (string.IsNullOrEmpty(filePath))
                        {
                            // TODO: print No output file specified.
                            Console.WriteLine("No output file specified.");
                        }
                        else
                        {
                            // TODO: set the output file to the outputFile
                            outputFile = filePath;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"The argument Arg[{i}] = [{args[i]}] is invalid");
                }
            }

            if (sortEnabled)
            {
                Console.WriteLine($"Sorting by <{sortColumnName}>");

                // TODO: add implementation to determine the column name to trigger a different sort. (Hint: column names are the 4 properties of the weapon class)
                if (sortColumnName.ToLower() == "type")
                {
                    // Sorts the list based off of the Weapon type.
                    results.Sort(Weapon.CompareByType);
                }
                else if (sortColumnName.ToLower() == "rarity")
                {
                    // Sorts the list based off of the Weapon rarity.
                    results.Sort(Weapon.CompareByRarity);
                }
                else if (sortColumnName.ToLower() == "baseattack")
                {
                    // Sorts the list based off of the Weapon BaseAttack.
                    results.Sort(Weapon.CompareByBaseAttack);
                }
                else
                {
                    // Sorts the list based off of the Weapon name.
                    results.Sort(Weapon.CompareByName);
                }
            }

            if (displayCount)
            {
                Console.WriteLine($"There are {results.Count} entries");
            }

            if (results.Count > 0)
            {
                if (!string.IsNullOrEmpty(outputFile))
                {
                    FileStream fs;

                    // Check if the append flag is set, and if so, then open the file in append mode; otherwise, create the file to write.
                    if (appendToFile && File.Exists((outputFile)))
                    {
                        fs = File.Open(outputFile, FileMode.Append);
                    }
                    else
                    {
                        fs = File.Open(outputFile, FileMode.Create);
                    }

                    // opens a stream writer with the file handle to write to the output file.
                    // Hint: use writer.WriteLine
                    // TODO: write the header of the output "Name,Type,Rarity,BaseAttack"
                    // TODO: use the writer to output the results.
                    // TODO: print out the file has been saved.

                    try
                    {
                        using (StreamWriter writer = new StreamWriter(fs))
                        {
                            writer.WriteLine("Name,Type,Rarity,BaseAttack");

                            foreach (var weapon in results)
                            {
                                writer.WriteLine(weapon.ToString());
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine("Exception: " + ex.Message);
                    }
                    finally
                    {
                        Console.WriteLine("Output file has been saved.");
                    }
                }
                else
                {
                    // prints out each entry in the weapon list results.
                    for (int i = 0; i < results.Count; i++)
                    {
                        Console.WriteLine(results[i]);
                    }
                }
            }
            Console.WriteLine("Done!");
        }

        /// <summary>
        /// Reads the file and line by line parses the data into a List of Weapons
        /// </summary>
        /// <param name="fileName">The path to the file</param>
        /// <returns>The list of Weapons</returns>
        public static List<Weapon> Parse(string fileName)
        {
            // TODO: implement the streamreader that reads the file and appends each line to the list
            // note that the result that you get from using read is a string, and needs to be parsed 
            // to an int for certain fields i.e. HP, Attack, etc.
            // i.e. int.Parse() and if the results cannot be parsed it will throw an exception
            // or can use int.TryParse() 

            // streamreader https://msdn.microsoft.com/en-us/library/system.io.streamreader(v=vs.110).aspx
            // Use string split https://msdn.microsoft.com/en-us/library/system.string.split(v=vs.110).aspx

            List<Weapon> output = new List<Weapon>();

            try
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    // Skip the first line because header does not need to be parsed.
                    // Name,Type,Rarity,BaseAttack

                    string header = reader.ReadLine();

                    // The rest of the lines looks like the following:
                    // Skyward Blade,Sword,5,46
                    while (reader.Peek() > 0)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(',');
                        int tempRarity = 0;
                        int tempBaseAttack = 0;

                        Weapon weapon = new Weapon();
                        // TODO: validate that the string array the size expected.
                        // TODO: use int.Parse or TryParse for stats/number values.
                        // Populate the properties of the Weapon
                        // TODO: Add the Weapon to the list

                        if (values.Length == 4)
                        {
                            weapon.Name = values[0];
                            weapon.Type = values[1];

                            if (int.TryParse(values[2], out tempRarity))
                            {
                                weapon.Rarity = tempRarity;
                            }
                            else
                            {
                                Console.WriteLine("Rarity parameter failed parsing.");
                            }

                            if (int.TryParse(values[3], out tempBaseAttack))
                            {
                                weapon.BaseAttack = tempBaseAttack;
                            }
                            else
                            {
                                Console.WriteLine("Rarity parameter failed parsing.");
                            }

                            output.Add(weapon);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            return output;
        }
    }
}
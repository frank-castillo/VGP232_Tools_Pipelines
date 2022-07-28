using System;
using System.Collections.Generic;
using System.IO;

// TODO: Fill in your name and student number.
// Assignment 2
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

            // The flag to determine if we need to save our file
            bool saveFile = false;

            // The column name to be used to determine which sort comparison function to use.
            string sortColumnName = string.Empty;

            // The results to be output to a file or to the console
            WeaponCollection results = new WeaponCollection();

            args = new string[7];
            args[0] = "-i";
            args[1] = "output.xml";
            args[2] = "-o";
            args[3] = "output.xml";
            args[4] = "-c";
            args[5] = "-s";
            args[6] = "Rarity";

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
                    Console.WriteLine("Available Columns: Name, Type, Rarity, BaseAttack, Image, SecondaryStat, Passive");

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
                                results.Load(inputFile);
                            }
                        } while (!wasErrorSolved);
                    }
                }
                else if (args[i].ToLower() == "-s" || args[i].ToLower() == "--sort")
                {
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
                                (sortColumnName.ToLower() != "baseattack") &&
                                (sortColumnName.ToLower() != "image") &&
                                (sortColumnName.ToLower() != "secondarystat") &&
                                (sortColumnName.ToLower() != "passive")
                                )
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
                            Console.WriteLine("No output file specified.");
                        }
                        else
                        {
                            outputFile = filePath;
                            saveFile = true;
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
                else if (sortColumnName.ToLower() == "image")
                {
                    // Sorts the list based off of the Weapon BaseAttack.
                    results.Sort(Weapon.CompareByImage);
                }
                else if (sortColumnName.ToLower() == "secondarystat")
                {
                    // Sorts the list based off of the Weapon BaseAttack.
                    results.Sort(Weapon.CompareBySecondaryStat);
                }
                else if (sortColumnName.ToLower() == "passive")
                {
                    // Sorts the list based off of the Weapon BaseAttack.
                    results.Sort(Weapon.CompareByPassive);
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

            if (results.Count > 0 && saveFile)
            {
                results.Save(outputFile, appendToFile);
            }
            else if (results.Count <= 0 && saveFile)
            {
                Console.WriteLine($"Collection is empty. Nothing to save or append.");
            }

            Console.WriteLine("Done!");
        }
    }
}
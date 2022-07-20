using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace VGP232_Notes.Week_01
{
    class FileExercise
    {
        static void Main(string[] args)
        {
            // check if first argument is -i
            // check if next argument is path
            // if path exists open and display
            Console.WriteLine("OPENING FILE");
            if (args.Length > 1)  
            {
                if (args[0] == "-i")
                {
                    string path = args[1];
                    if (File.Exists(path))
                    {
                        StreamReader streamReader = null;
                        try
                        {
                            streamReader = new StreamReader(path);
                            while (streamReader.Peek() > 0)
                            {
                                Console.WriteLine(streamReader.ReadLine());
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"FAILED TO READ FILE [{ex.Message}]");
                        }
                        finally
                        {
                            if (streamReader != null)
                            {
                                streamReader.Close();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{path} DOES NOT EXIST");
                    }
                }
                else if (args[0] == "-c")
                {
                    string path = args[1];
                    if (File.Exists(path))
                    {
                        StreamReader streamReader = null;
                        StreamWriter streamWriter = null;
                        try
                        {
                            streamReader = new StreamReader(path);
                            string newPath = path.Substring(0, path.Length - 4);
                            newPath += "2.txt";
                            streamWriter = new StreamWriter(newPath);
                            while (streamReader.Peek() > 0)
                            {
                                string readLine = streamReader.ReadLine();
                                Console.WriteLine(readLine);
                                streamWriter.WriteLine(readLine);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"FAILED TO WRITE FILE [{ex.Message}]");
                        }
                        finally
                        {
                            if (streamReader != null)
                            {
                                streamReader.Close();
                            }
                            if (streamWriter != null)
                            {
                                streamWriter.Close();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{path} DOES NOT EXIST");
                    }
                }
                else
                {
                    Console.WriteLine($"INVALID ARGUMENT {args[0]}");
                }
            }
            else
            {
                Console.WriteLine($"NO ARGUMENTS");
            }
        }
    }
}

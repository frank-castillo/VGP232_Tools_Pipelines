using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace WeaponLib
{
    [XmlRoot("WeaponCollection")]
    public class WeaponCollection : List<Weapon>, ICsvSerializable, IJsonSerializable, IXmlSerializable
    {
        [XmlIgnore] private bool _append { get; set; }
        public int GetHighestBaseAttack()
        {
            int highestAttack = 0;

            foreach (var weapon in this)
            {
                if (weapon.BaseAttack > highestAttack)
                    highestAttack = weapon.BaseAttack;
            }

            return highestAttack;
        }

        public int GetLowestBaseAttack()
        {
            int lowestAttack = int.MaxValue;

            foreach (var weapon in this)
            {
                if (weapon.BaseAttack < lowestAttack)
                    lowestAttack = weapon.BaseAttack;
            }

            return lowestAttack;
        }

        public List<Weapon> GetAllWeaponsOfType(WeaponType type)
        {
            if(type  == WeaponType.All)
            {
                return this;
            }

            List<Weapon> weaponsByType = new List<Weapon>();

            foreach (var weapon in this)
            {
                if (weapon.Type == type)
                    weaponsByType.Add(weapon);
            }

            return weaponsByType;
        }

        public List<Weapon> GetAllWeaponsOfRarity(int stars)
        {
            List<Weapon> weaponsByRarity = new List<Weapon>();

            foreach (var weapon in this)
            {
                if (weapon.Rarity == stars)
                    weaponsByRarity.Add(weapon);
            }

            return weaponsByRarity;
        }

        public void SortBy(string columnName)
        {
            if (columnName.ToLower() == "type")
            {
                // Sorts the list based off of the Weapon type.
                this.Sort(Weapon.CompareByType);
            }
            else if (columnName.ToLower() == "rarity")
            {
                // Sorts the list based off of the Weapon rarity.
                this.Sort(Weapon.CompareByRarity);
            }
            else if (columnName.ToLower() == "baseattack")
            {
                // Sorts the list based off of the Weapon BaseAttack.
                this.Sort(Weapon.CompareByBaseAttack);
            }
            else if (columnName.ToLower() == "image")
            {
                this.Sort(Weapon.CompareByImage);
            }
            else if (columnName.ToLower() == "secondarystat")
            {
                this.Sort(Weapon.CompareBySecondaryStat);
            }
            else if (columnName.ToLower() == "passive")
            {
                this.Sort(Weapon.CompareByPassive);
            }
            else
            {
                // Sorts the list based off of the Weapon name.
                this.Sort(Weapon.CompareByName);
            }
        }

        public bool Load(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string extension = Path.GetExtension(fileName);

                if (extension.ToLower() == ".csv")
                {
                    if (!LoadCSV(fileName))
                    {
                        Console.WriteLine($"Failed to load file with {extension.ToLower()} extension. See message above for more details.");
                        return false;
                    }
                    else
                    {
                        Console.WriteLine($"{extension} file loaded correctly");
                    }
                }
                else if (extension.ToLower() == ".json")
                {
                    if (!LoadJSON(fileName))
                    {
                        Console.WriteLine($"Failed to load file with {extension.ToLower()} extension. See message above for more details.");
                        return false;
                    }
                    else
                    {
                        Console.WriteLine($"{extension} file loaded correctly");
                    }
                }
                else if (extension.ToLower() == ".xml")
                {
                    if (!LoadXML(fileName))
                    {
                        Console.WriteLine($"Failed to load file with {extension.ToLower()} extension. See message above for more details.");
                        return false;
                    }
                    else
                    {
                        Console.WriteLine($"{extension} file loaded correctly");
                    }
                }
                else
                {
                    Console.WriteLine($"{extension} is not a valid file extension! Please make sure you are using the correct file.");
                    Console.WriteLine($"Use only .csv, .json, or .XML files");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Not a valid path to save the file. Path is non-existent");
                return false;
            }

            return true;
        }

        public bool Save(string filename, bool append = false)
        {
            _append = append;

            if (!string.IsNullOrEmpty(filename))
            {
                string extension = Path.GetExtension(filename);

                if (extension == null)
                {
                    Console.WriteLine("Failed to obtain extension! Please make sure the path is correct and there are no typos in the file extension.");
                    return false;
                }
                else if (extension.ToLower() == ".csv")
                {
                    if (!SaveAsCSV(filename))
                    {
                        Console.WriteLine($"Failed to save file with {extension.ToLower()} extension. See message above for more details.");
                        return false;
                    }
                }
                else if (extension.ToLower() == ".json")
                {
                    if (!SaveAsJSON(filename))
                    {
                        Console.WriteLine($"Failed to save file with {extension.ToLower()} extension. See message above for more details.");
                        return false;
                    }
                }
                else if (extension.ToLower() == ".xml")
                {
                    if (!SaveAsXML(filename))
                    {
                        Console.WriteLine($"Failed to save file with {extension.ToLower()} extension. See message above for more details.");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine($"{extension} is not a valid file extension! Please make sure you are using the correct one.");
                    Console.WriteLine($"Use only .csv, .json, or .XML files");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Not a valid path to save the file. Path is non-existent");
                return false;
            }

            return true;
        }

        public bool LoadCSV(string filename)
        {
            if (File.Exists(filename))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(filename))
                    {
                        // Skip the first line because header does not need to be parsed.
                        // Name,Type,Rarity,BaseAttack, and other parameters

                        if (reader.Peek() > 0)
                        {
                            this.Clear();

                            string header = reader.ReadLine();
                            string[] headerColumns = header.Split(',');

                            // The rest of the lines looks like the following:
                            // Skyward Blade,Sword,5,46
                            while (reader.Peek() > 0)
                            {
                                string line = reader.ReadLine();
                                string[] values = line.Split(',');

                                if (values.Length == headerColumns.Length)
                                {
                                    if (Weapon.TryParse(values, out Weapon weapon))
                                    {
                                        this.Add(weapon);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failed to parse data into weapon. Please revise original data");
                                        return false;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Incorrect number of columns relating to weapon properties, please revise the data.");
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("File is empty! No data to read");
                            return false;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                    return false;
                }
                finally
                {
                    Console.WriteLine(".csv file loaded correctly.");
                }
            }
            else
            {
                Console.WriteLine("CSV file does not exist. Please check the filename and make sure it is correct");
            }

            return true;
        }

        public bool SaveAsCSV(string filename)
        {
            FileStream fs;

            // Check if the append flag is set, and if so, then open the file in append mode; otherwise, create the file to write.
            if (File.Exists((filename)) && _append)
            {
                fs = File.Open(filename, FileMode.Append, FileAccess.ReadWrite);
            }
            else if (File.Exists((filename)) && !_append)
            {
                fs = File.Open(filename, FileMode.Open, FileAccess.ReadWrite);
            }
            else
            {
                fs = File.Open(filename, FileMode.Create, FileAccess.ReadWrite);
            }

            try
            {
                // opens a stream writer with the file handle to write to the output file.
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine("Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive");

                    foreach (var weapon in this)
                    {
                        writer.WriteLine(weapon.ToString());
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
            finally
            {
                Console.WriteLine(".csv file has been saved correctly.");
                fs.Close();
            }

            return true;
        }

        public bool LoadJSON(string filename)
        {
            if (File.Exists(filename))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(filename))
                    {
                        // Skip the first line because header does not need to be parsed.
                        // Name,Type,Rarity,BaseAttack, and other parameters
                        WeaponCollection tempCollection = new WeaponCollection();

                        if (reader.Peek() > 0)
                        {
                            string jsonString = reader.ReadToEnd();
                            tempCollection = JsonSerializer.Deserialize<WeaponCollection>(jsonString);

                            this.Clear();

                            foreach (var weapon in tempCollection)
                            {
                                this.Add(weapon);
                            }
                        }
                        else
                        {
                            Console.WriteLine("File is empty! No data to read");
                            return false;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                    return false;
                }
                finally
                {
                    Console.WriteLine(".csv file loaded correctly.");
                }
            }
            else
            {
                Console.WriteLine(".json file does not exist. Please check the filename and make sure it is correct");
            }

            return true;
        }

        public bool SaveAsJSON(string filename)
        {
            FileStream fs;

            // Check if the append flag is set, and if so, then open the file in append mode; otherwise, create the file to write.
            if (File.Exists((filename)) && _append)
            {
                fs = File.Open(filename, FileMode.Append, FileAccess.ReadWrite);
            }
            else if (File.Exists((filename)) && !_append)
            {
                fs = File.Open(filename, FileMode.Open, FileAccess.ReadWrite);
            }
            else
            {
                fs = File.Open(filename, FileMode.Create, FileAccess.ReadWrite);
            }

            try
            {
                // opens a stream writer with the file handle to write to the output file.
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize<WeaponCollection>(this, options);
                    writer.WriteLine(jsonString);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
            finally
            {
                Console.WriteLine(".json file has been saved correctly.");
                fs.Close();
            }

            return true;
        }

        public bool LoadXML(string filename)
        {
            if (File.Exists(filename))
            {
                try
                {
                    WeaponCollection tempCollection = new WeaponCollection();

                    using (FileStream fileStream = new FileStream(filename, FileMode.Open))
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(WeaponCollection));
                        tempCollection = (WeaponCollection)xs.Deserialize(fileStream);

                        this.Clear();

                        foreach (var weapon in tempCollection)
                        {
                            this.Add(weapon);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("Loading file as XML file failed. See message below for more details.");
                    Console.WriteLine("Exception: " + ex.Message);
                    return false;
                }
                finally
                {
                    Console.WriteLine(".xml file loaded correctly.");
                }
            }
            else
            {
                Console.WriteLine("XML file does not exist. Please check the filename and make sure it is correct");
            }

            return true;
        }

        public bool SaveAsXML(string filename)
        {
            try
            {
                FileMode mode = FileMode.Open;

                if (File.Exists((filename)) && _append)
                {
                    mode = FileMode.Append;
                }
                else if (File.Exists((filename)) && !_append)
                {
                    mode = FileMode.Open;
                }
                else
                {
                    mode = FileMode.Create;
                }

                using (FileStream fs = new FileStream(filename, mode, FileAccess.ReadWrite))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(WeaponCollection));
                    xs.Serialize(fs, this);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Saving file as XML file failed. See message below for more details.");
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
            finally
            {
                Console.WriteLine("XML file saved correctly.");
            }

            return true;
        }

    }
}

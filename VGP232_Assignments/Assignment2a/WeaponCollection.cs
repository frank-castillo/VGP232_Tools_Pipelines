using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Assignment2a
{
    public class WeaponCollection : List<Weapon>, IPeristence
    {
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
            try
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    // Skip the first line because header does not need to be parsed.
                    // Name,Type,Rarity,BaseAttack, and other parameters

                    string header = reader.ReadLine();
                    string[] headerColumns = header.Split(',');

                    // The rest of the lines looks like the following:
                    // Skyward Blade,Sword,5,46
                    while (reader.Peek() > 0)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(',');

                        if (values.Length != headerColumns.Length)
                        {
                            if (Weapon.TryParse(values, out Weapon weapon))
                            {
                                this.Add(weapon);
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            Console.WriteLine("Incorrect number of columns relating to weapon properties, please revise the data.");
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            return true;
        }

        public bool Save(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                FileStream fs;

                // Check if the append flag is set, and if so, then open the file in append mode; otherwise, create the file to write.
                if (File.Exists((filename)))
                {
                    fs = File.Open(filename, FileMode.Append);
                }
                else
                {
                    fs = File.Open(filename, FileMode.Create);
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
                    Console.WriteLine("Output file has been saved.");
                }
            }
            else
            {
                Console.WriteLine("Not a valid path file to save this collection.");
                return false;
            }

            return true;
        }
    }
}

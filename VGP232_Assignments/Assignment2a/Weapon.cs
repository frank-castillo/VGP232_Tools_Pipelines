using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2a
{
    public enum WeaponType
    {
        Sword,
        Polearm,
        Claymore,
        Catalyst,
        Bow,
        None
    }

    public class Weapon
    {
        // Name,Type,Rarity,BaseAttack
        public string Name { get; set; }
        public WeaponType Type { get; set; }
        public int Rarity { get; set; }
        public int BaseAttack { get; set; }
        public string Image { get; set; }
        public string SecondaryStat { get; set; }
        public string Passive { get; set; }

        /// <summary>
        /// The Comparator function to check for name
        /// </summary>
        /// <param name="left">Left side Weapon</param>
        /// <param name="right">Right side Weapon</param>
        /// <returns> -1 (or any other negative value) for "less than", 0 for "equals", or 1 (or any other positive value) for "greater than"</returns>
        public static int CompareByName(Weapon left, Weapon right)
        {
            return left.Name.CompareTo(right.Name);
        }

        // TODO: add sort for each property:
        // CompareByType
        // CompareByRarity
        // CompareByBaseAttack
        public static int CompareByType(Weapon left, Weapon right)
        {
            return left.Type.CompareTo(right.Type);
        }

        public static int CompareByRarity(Weapon left, Weapon right)
        {
            return left.Rarity.CompareTo(right.Rarity);
        }

        public static int CompareByBaseAttack(Weapon left, Weapon right)
        {
            return left.BaseAttack.CompareTo(right.BaseAttack);
        }

        public static int CompareByImage(Weapon left, Weapon right)
        {
            return left.Image.CompareTo(right.Image);
        }

        public static int CompareBySecondaryStat(Weapon left, Weapon right)
        {
            return left.SecondaryStat.CompareTo(right.SecondaryStat);
        }

        public static int CompareByPassive(Weapon left, Weapon right)
        {
            return left.Passive.CompareTo(right.Passive);
        }

        /// <summary>
        /// The Weapon string with all the properties
        /// </summary>
        /// <returns>The Weapon formated string</returns>
        public override string ToString()
        {
            // TODO: construct a comma separated value string
            // Name,Type,Rarity,BaseAttack
            return $"{Name},{Type},{Image},{Rarity},{BaseAttack},{SecondaryStat},{Passive}";
        }

        public static bool TryParse(string[] RawData, out Weapon weapon)
        {
            weapon = new Weapon();
            WeaponType tempType = WeaponType.None;
            int tempRarity = 0;
            int tempBaseAttack = 0;

            if (RawData[0].Length < 0)
            {
                Console.WriteLine("Missing value for name. Please revise data.");
                weapon = null;
                return false;
            }
            else
            {
                weapon.Name = RawData[0];
            }

            if (Enum.TryParse<WeaponType>(RawData[2],out tempType))
            {
                weapon.Type = tempType;
            }
            else
            {
                Console.WriteLine("Type of weapon could not be parsed.");
                weapon = null;
                return false;
            }

            if (RawData[2].Length < 0)
            {
                Console.WriteLine("Missing value for image. Please revise data.");
                weapon = null;
                return false;
            }
            else
            {
                weapon.Image = RawData[2];
            }

            if (int.TryParse(RawData[3], out tempRarity))
            {
                weapon.Rarity = tempRarity;
            }
            else
            {
                Console.WriteLine("Rarity parameter failed parsing.");
                weapon = null;
                return false;
            }

            if (int.TryParse(RawData[4], out tempBaseAttack))
            {
                weapon.BaseAttack = tempBaseAttack;
            }
            else
            {
                Console.WriteLine("Rarity parameter failed parsing.");
                weapon = null;
                return false;
            }

            if (RawData[5].Length < 0)
            {
                Console.WriteLine("Missing value for secondary stat. Please revise data.");
                weapon = null;
                return false;
            }
            else
            {
                weapon.Name = RawData[5];
            }

            if (RawData[6].Length < 0)
            {
                Console.WriteLine("Missing value for passive. Please revise data.");
                weapon = null;
                return false;
            }
            else
            {
                weapon.Name = RawData[6];
            }

            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2a
{
    public class Weapon
    {
        // Name,Type,Rarity,BaseAttack
        public string Name { get; set; }
        public string Type { get; set; }
        public int Rarity { get; set; }
        public int BaseAttack { get; set; }

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

        /// <summary>
        /// The Weapon string with all the properties
        /// </summary>
        /// <returns>The Weapon formated string</returns>
        public override string ToString()
        {
            // TODO: construct a comma separated value string
            // Name,Type,Rarity,BaseAttack
            return $"{Name},{Type},{Rarity},{BaseAttack}";
        }
    }
}

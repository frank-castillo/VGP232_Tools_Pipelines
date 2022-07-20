using System;
using System.Collections.Generic;

namespace Week02_Notes
{
    class Program
    {
        public class SuperHero
        {
            public string Name;
            public override string ToString()
            {
                return Name;
            }
        }

        public class SuperVillain
        {
            public string Name;
            public override string ToString()
            {
                return Name;
            }
        }

        static void Main(string[] args)
        {
            Stack<SuperHero> superHeroes = new Stack<SuperHero>();
            superHeroes.Push(new SuperHero() { Name = "BATMAN" });
            superHeroes.Push(new SuperHero() { Name = "SPIDERMAN" });
            superHeroes.Push(new SuperHero() { Name = "SUPERMAN" });
            superHeroes.Push(new SuperHero() { Name = "WONDERWOMAN" });

            Queue<SuperVillain> superVillains = new Queue<SuperVillain>();
            superVillains.Enqueue(new SuperVillain() { Name = "JOKER" });
            superVillains.Enqueue(new SuperVillain() { Name = "VENOM" });
            superVillains.Enqueue(new SuperVillain() { Name = "LEX" });
            superVillains.Enqueue(new SuperVillain() { Name = "ARES" });

            while (superHeroes.Count > 0)
            {
                Console.WriteLine(superHeroes.Pop());
            }

            Console.WriteLine("");

            while (superVillains.Count > 0)
            {
                Console.WriteLine(superVillains.Dequeue());
            }
        }
    }
}

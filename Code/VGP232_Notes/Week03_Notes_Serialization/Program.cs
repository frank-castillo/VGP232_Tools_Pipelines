using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Week03_Notes_Serialization
{
    [XmlRoot("CartItem")]
    public class ShoppingCartItem
    {
        [XmlAttribute] public Int32 productID;
        public decimal price;
        public Int32 quantity;
        [XmlIgnore] public decimal total;

        public ShoppingCartItem()
        {

        }

        public override string ToString()
        {
            return ($"{productID}, {price}, {quantity}");
        }
    }

    class Program
    {
        [Serializable]
        public class PersonData
        {
            public string Name = "";
            public int Age = 0;
            public string Job = "";

            public override string ToString()
            {
                return ($"{Name}, {Age}, {Job}");
            }
        }

        [Serializable]
        public class ClassData
        {
            public List<PersonData> Students = new List<PersonData>();
        }

        static void Main(string[] args)
        {
            string outputFile = @"D:\zJuanC\VGP232\Code\VGP232_Notes\Week03_Notes_Serialization\output.txt";
            string outputFileXML = @"D:\zJuanC\VGP232\Code\VGP232_Notes\Week03_Notes_Serialization\output.XML";
            string readLine = "";
            ClassData students = new ClassData();

            while (readLine.ToLower() != "done")
            {
                PersonData personData = new PersonData();
                ShoppingCartItem item = new ShoppingCartItem();
                item.productID = 123;
                item.price = 123;
                item.quantity = 123;

                Console.Clear();
                Console.WriteLine("Serialize or Deserialize");
                readLine = Console.ReadLine();

                if (readLine == "Serialize")
                {
                    Console.Clear();
                    Console.WriteLine("Serialize");
                    Console.WriteLine("PERSON DATA");
                    Console.WriteLine("PERSON NAME");
                    personData.Name = Console.ReadLine();

                    int age = 0;
                    while (age <= 0)
                    {
                        Console.WriteLine("AGE: ");
                        readLine = Console.ReadLine();
                        if (int.TryParse(readLine, out age))
                        {
                            personData.Age = age;
                        }
                    }

                    Console.WriteLine("PERSON JOB");
                    personData.Job = Console.ReadLine();
                    students.Students.Add(personData);

                    using (FileStream fs = new FileStream(outputFile, FileMode.OpenOrCreate))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(fs, students);
                    }

                    Console.WriteLine("DONE WITH PEOPLE DATA!");
                    Console.ReadKey();

                    using (FileStream fs = new FileStream(outputFileXML, FileMode.OpenOrCreate))
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(ShoppingCartItem));
                        xs.Serialize(fs, item);
                    }

                    Console.WriteLine("DONE WITH XML DATA!");
                    Console.ReadKey();
                }
                else if (readLine == "Deserialize")
                {
                    Console.Clear();
                    Console.WriteLine("Deserialize");
                    Console.WriteLine("");

                    if (File.Exists(outputFile))
                    {
                        using (FileStream fs = new FileStream(outputFile, FileMode.Open))
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            students = (ClassData)bf.Deserialize(fs);

                            foreach (var student in students.Students)
                            {
                                Console.WriteLine(student.ToString());
                            }

                            Console.WriteLine("DONE WITH PEOPLE");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.WriteLine("That files does not exist");
                    }

                    if (File.Exists(outputFileXML))
                    {
                        using (FileStream fs = new FileStream(outputFileXML, FileMode.Open))
                        {
                            XmlSerializer xs = new XmlSerializer(typeof(ShoppingCartItem));
                            item = (ShoppingCartItem)xs.Deserialize(fs);

                            Console.WriteLine(item.ToString());
                            Console.WriteLine("DONE WITH XML");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.WriteLine("That XML file does not exist");

                    }
                }
            }
        }
    }
}

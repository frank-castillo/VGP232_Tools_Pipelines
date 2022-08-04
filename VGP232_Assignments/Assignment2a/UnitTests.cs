using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Assignment2a
{
    [TestFixture]
    public class UnitTests
    {
        private WeaponCollection weaponCollection;
        private string inputPath;
        private string inputPathXML;
        private string inputPathJSON;
        private string outputPath;
        private string outputPathXML;
        private string outputPathJSON;
        private string emptyOutputPath;
        private string emptyOutputPathXML;
        private string emptyOutputPathJSON;

        const string INPUT_FILE = "data2.csv";
        const string XML_INPUT_FILE = "weapons.xml";
        const string JSON_INPUT_FILE = "weapons.json";

        const string OUTPUT_FILE = "weapons.csv";
        const string XML_OUTPUT_FILE = "weapons.xml";
        const string JSON_OUTPUT_FILE = "weapons.json";

        const string EMPTY_OUTPUT_FILE = "empty.csv";
        const string EMPTY_XML_OUTPUT_FILE = "empty.xml";
        const string EMPTY_JSON_OUTPUT_FILE = "empty.json";

        // A helper function to get the directory of where the actual path is.
        private string CombineToAppPath(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        }

        private void LoadCollection()
        {
            SetUp();
            weaponCollection.Load(inputPath);
        }

        private void LoadCollectionXML()
        {
            SetUp();
            weaponCollection.Load(inputPathXML);
        }

        private void LoadCollectionJSON()
        {
            SetUp();
            weaponCollection.Load(inputPathJSON);
        }

        [SetUp]
        public void SetUp()
        {
            inputPath           = CombineToAppPath(INPUT_FILE);
            inputPathXML        = CombineToAppPath(XML_INPUT_FILE);
            inputPathJSON       = CombineToAppPath(JSON_INPUT_FILE);
            outputPath          = CombineToAppPath(OUTPUT_FILE);
            outputPathXML       = CombineToAppPath(XML_OUTPUT_FILE);
            outputPathJSON      = CombineToAppPath(JSON_OUTPUT_FILE);
            emptyOutputPath     = CombineToAppPath(EMPTY_OUTPUT_FILE);
            emptyOutputPathXML  = CombineToAppPath(EMPTY_XML_OUTPUT_FILE);
            emptyOutputPathJSON = CombineToAppPath(EMPTY_JSON_OUTPUT_FILE);
            weaponCollection = new WeaponCollection();
        }

        [TearDown]
        public void CleanUp()
        {
            // We remove the output file after we are done.
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            if (File.Exists(outputPathXML))
            {
                File.Delete(outputPathXML);
            }

            if (File.Exists(outputPathJSON))
            {
                File.Delete(outputPathJSON);
            }

            if (File.Exists(emptyOutputPath))
            {
                File.Delete(emptyOutputPath);
            }

            if (File.Exists(emptyOutputPathJSON))
            {
                File.Delete(emptyOutputPathJSON);
            }

            if (File.Exists(emptyOutputPathXML))
            {
                File.Delete(emptyOutputPathXML);
            }
        }

        // WeaponCollection Unit Tests
        [Test]
        public void WeaponCollection_GetHighestBaseAttack_HighestValue()
        {
            // Expected Value: 48
            // TODO: call WeaponCollection.GetHighestBaseAttack() and confirm that it matches the expected value using asserts.
            LoadCollection();
            var value = weaponCollection.GetHighestBaseAttack();
            Assert.IsTrue(value == 48);
        }

        [Test]
        public void WeaponCollection_GetLowestBaseAttack_LowestValue()
        {
            // Expected Value: 23
            // TODO: call WeaponCollection.GetLowestBaseAttack() and confirm that it matches the expected value using asserts.
            LoadCollection();
            var value = weaponCollection.GetLowestBaseAttack();
            Assert.IsTrue(value == 23);
        }

        [TestCase(WeaponType.Sword, 21)]
        public void WeaponCollection_GetAllWeaponsOfType_ListOfWeapons(WeaponType type, int expectedValue)
        {
            // TODO: call WeaponCollection.GetAllWeaponsOfType(type) and confirm that the weapons list returns Count matches the expected value using asserts.
            LoadCollection();
            Assert.IsTrue(weaponCollection.GetAllWeaponsOfType(type).Count == expectedValue);
        }

        [TestCase(5, 10)]
        public void WeaponCollection_GetAllWeaponsOfRarity_ListOfWeapons(int stars, int expectedValue)
        {
            // TODO: call WeaponCollection.GetAllWeaponsOfRarity(stars) and confirm that the weapons list returns Count matches the expected value using asserts.
            LoadCollection();
            Assert.IsTrue(weaponCollection.GetAllWeaponsOfRarity(stars).Count == expectedValue);
        }

        [Test]
        public void WeaponCollection_LoadThatExistAndValid_True()
        {
            // TODO: load returns true, expect WeaponCollection with count of 95 .
            SetUp();
            Assert.IsTrue(weaponCollection.Load(inputPath));
            Assert.IsTrue(weaponCollection.Count == 95);
            CleanUp();
        }

        [Test]
        public void WeaponCollection_LoadThatDoesNotExist_FalseAndEmpty()
        {
            // TODO: load returns false, expect an empty WeaponCollection
            SetUp();
            Assert.IsFalse(weaponCollection.Load(string.Empty));
            Assert.AreEqual(0, weaponCollection.Count);
            CleanUp();
        }

        [Test]
        public void WeaponCollection_SaveWithValuesCanLoad_TrueAndNotEmpty()
        {
            // TODO: save returns true, load returns true, and WeaponCollection is not empty.
            SetUp();
            Assert.IsTrue(weaponCollection.Save(outputPath));
            Assert.IsTrue(weaponCollection.Load(outputPath));
            Assert.IsTrue(weaponCollection != null);
            CleanUp();
        }

        [Test]
        public void WeaponCollection_SaveEmpty_TrueAndEmpty()
        {
            // After saving an empty WeaponCollection, load the file and expect WeaponCollection to be empty.
            SetUp();
            weaponCollection.Clear();
            Assert.IsTrue(weaponCollection.Save(outputPath));
            Assert.IsTrue(weaponCollection.Load(outputPath));
            Assert.IsTrue(weaponCollection.Count == 0);
            CleanUp();
        }

        // Weapon Unit Tests
        [Test]
        public void Weapon_TryParseValidLine_TruePropertiesSet()
        {
            // TODO: create a Weapon with the stats above set properly
            Weapon expected = null;
            //TODO: uncomment this once you added the Type1 and Type2
            expected = new Weapon()
            {
                Name = "Skyward Blade",
                Type = WeaponType.Sword,
                Image = "https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png",
                Rarity = 5,
                BaseAttack = 46,
                SecondaryStat = "Energy Recharge",
                Passive = "Sky-Piercing Fang"
            };

            string line = "Skyward Blade,Sword,https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png,5,46,Energy Recharge,Sky-Piercing Fang";
            string[] parameters = line.Split(',');
            Weapon actual = null;

            // TODO: uncomment this once you have TryParse implemented.
            Assert.IsTrue(Weapon.TryParse(parameters, out actual));
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Type, actual.Type);
            Assert.AreEqual(expected.Image, actual.Image);
            Assert.AreEqual(expected.Rarity, actual.Rarity);
            Assert.AreEqual(expected.BaseAttack, actual.BaseAttack);
            Assert.AreEqual(expected.SecondaryStat, actual.SecondaryStat);
            Assert.AreEqual(expected.Passive, actual.Passive);
            // TODO: check for the rest of the properties, Image,Rarity,SecondaryStat,Passive
        }

        [Test]
        public void Weapon_TryParseInvalidLine_FalseNull()
        {
            // TODO: use "1,Bulbasaur,A,B,C,65,65", Weapon.TryParse returns false, and Weapon is null.
            Weapon expected = null;
            expected = new Weapon()
            {
                Name = "Skyward Blade",
                Type = WeaponType.Sword,
                Image = "https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png",
                Rarity = 5,
                BaseAttack = 46,
                SecondaryStat = "Energy Recharge",
                Passive = "Sky-Piercing Fang"
            };

            string line = "1,Bulbasaur,A,B,C,65,65";
            string[] parameters = line.Split(',');
            Weapon actual = null;

            // TODO: uncomment this once you have TryParse implemented.
            Assert.IsFalse(Weapon.TryParse(parameters, out actual));
            Assert.AreEqual(null, actual);
        }

        // Serialization tests
        [Test]
        public void WeaponCollection_Load_Save_Load_ValidJson()
        {
            //Load the data2.csv and Save() it to weapons.json and call Load() output and validate that there’s 95 entries
            LoadCollection();
            Assert.IsTrue(weaponCollection.Save(outputPathJSON));
            Assert.IsTrue(weaponCollection.Load(inputPathJSON));
            Assert.IsTrue(weaponCollection.Count == 95);
            CleanUp();
        }

        [Test]
        public void WeaponCollection_Load_SaveAsJSON_Load_ValidJson()
        {
            //Load the data2.csv and SaveAsJSON() it to weapons.json and call Load() output and validate that there’s 95 entries
            LoadCollection();
            Assert.IsTrue(weaponCollection.SaveAsJSON(outputPathJSON));
            Assert.IsTrue(weaponCollection.Load(inputPathJSON));
            Assert.IsTrue(weaponCollection.Count == 95);
            CleanUp();
        }

        [Test]
        public void WeaponCollection_Load_SaveAsJSON_LoadJSON_ValidJson()
        {
            //Load the data2.csv and SaveAsJSON() it to weapons.json and call LoadJSON() on output and validate that there’s 95 entries
            LoadCollection();
            Assert.IsTrue(weaponCollection.SaveAsJSON(outputPathJSON));
            Assert.IsTrue(weaponCollection.Load(inputPathJSON));
            Assert.IsTrue(weaponCollection.Count == 95);
            CleanUp();
        }

        [Test]
        public void WeaponCollection_Load_Save_LoadJSON_ValidJson()
        {
            //Load the data2.csv and Save() it to weapons.json and call LoadJSON() on output and validate that there’s 95 entries
            LoadCollection();
            Assert.IsTrue(weaponCollection.SaveAsJSON(outputPathJSON));
            Assert.IsTrue(weaponCollection.LoadJSON(inputPathJSON));
            Assert.IsTrue(weaponCollection.Count == 95);
            CleanUp();
        }

        [Test]
        public void WeaponCollection_Load_Save_Load_ValidCsv()
        {
            //Load the data2.csv and Save() it to weapons.csv and Load() output and validate that there’s 95 entries
            LoadCollection();
            Assert.IsTrue(weaponCollection.Save(outputPath));
            weaponCollection = new WeaponCollection();
            Assert.IsTrue(weaponCollection.Load(outputPath));
            Assert.IsTrue(weaponCollection.Count == 95);
            CleanUp();
        }

        [Test]
        public void WeaponCollection_Load_SaveAsCSV_LoadCSV_ValidCsv()
        {
            // Load the data2.csv and SaveAsCSV() it to weapons.csv and LoadCsv() output and validate that there’s 95 entries
            LoadCollection();
            Assert.IsTrue(weaponCollection.SaveAsCSV(outputPath));
            weaponCollection = new WeaponCollection();
            Assert.IsTrue(weaponCollection.LoadCSV(outputPath));
            Assert.IsTrue(weaponCollection.Count == 95);
            CleanUp();
        }

        [Test]
        public void WeaponCollection_Load_Save_Load_ValidXml()
        {
            // Load the data2.csv and Save() it to weapons.xml and Load() output and validate that there’s 95 entries
            LoadCollection();
            Assert.IsTrue(weaponCollection.Save(outputPathXML));
            Assert.IsTrue(weaponCollection.Load(inputPathXML));
            Assert.IsTrue(weaponCollection.Count == 95);
            CleanUp();
        }

        [Test]
        public void WeaponCollection_Load_SaveAsXML_LoadXML_ValidXml()
        {
            // Load the data2.csv and SaveAsXML() it to weapons.xml and LoadXML() output and validate that there’s 95 entries
            LoadCollection();
            Assert.IsTrue(weaponCollection.SaveAsXML(outputPathXML));
            Assert.IsTrue(weaponCollection.LoadXML(inputPathXML));
            Assert.IsTrue(weaponCollection.Count == 95);
            CleanUp();
        }

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidJson()
        {
            // Create an empty WeaponCollection, call SaveAsJSON() to empty.json, and Load() the output and verify the WeaponCollection has a Count of 0
            SetUp();
            weaponCollection = new WeaponCollection();
            Assert.IsTrue(weaponCollection.SaveAsJSON(emptyOutputPathJSON));
            Assert.IsTrue(weaponCollection.Load(emptyOutputPathJSON));
            Assert.IsTrue(weaponCollection.Count == 0);
            CleanUp();
        }

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidCsv()
        {
            // Create an empty WeaponCollection, call SaveAsCSV() to empty.csv, and Load() the output and verify the WeaponCollection has a Count of 0
            SetUp();
            weaponCollection = new WeaponCollection();
            Assert.IsTrue(weaponCollection.SaveAsCSV(emptyOutputPath));
            Assert.IsTrue(weaponCollection.Load(emptyOutputPath));
            Assert.IsTrue(weaponCollection.Count == 0);
            CleanUp();
        }

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidXml()
        {
            // Create an empty WeaponCollection, call SaveAsXML() to empty.xml, and Load and verify the WeaponCollection has a Count of 0
            weaponCollection = new WeaponCollection();
            SetUp();
            Assert.IsTrue(weaponCollection.SaveAsCSV(emptyOutputPath));
            Assert.IsTrue(weaponCollection.Load(emptyOutputPath));
            Assert.IsTrue(weaponCollection.Count == 0);
            CleanUp();
        }

        [Test]
        public void WeaponCollection_Load_SaveJSON_LoadXML_InvalidXml()
        {
            // Load the data2.csv and SaveAsJSON() it to weapons.json and call LoadXML() output and validate that it returns false, and there’s 0 entries
            LoadCollection();
            Assert.IsTrue(weaponCollection.SaveAsJSON(outputPathJSON));
            weaponCollection = new WeaponCollection();
            Assert.IsFalse(weaponCollection.LoadXML(outputPathJSON));
            Assert.IsTrue(weaponCollection.Count == 0);
            CleanUp();
        }

        [Test]
        public void WeaponCollection_Load_SaveXML_LoadJSON_InvalidJson()
        {
            // Load the data2.csv and SaveAsXML() it to weapons.xml and call LoadJSON() output and validate that it returns false, and there’s 0 entries
            LoadCollection();
            Assert.IsTrue(weaponCollection.SaveAsXML(outputPathXML));
            weaponCollection = new WeaponCollection();
            Assert.IsFalse(weaponCollection.LoadJSON(outputPathXML));
            Assert.IsTrue(weaponCollection.Count == 0);
            CleanUp();
        }

        [Test]
        public void WeaponCollection_ValidCsv_LoadXML_InvalidXml()
        {
            // On the data2.csv and validate that returns false, and there’s 0 entries
            LoadCollection();
            Assert.IsTrue(weaponCollection.SaveAsCSV(outputPath));
            weaponCollection = new WeaponCollection();
            Assert.IsFalse(weaponCollection.LoadXML(outputPath));
            Assert.IsTrue(weaponCollection.Count == 0);
            CleanUp();
        }

        [Test]
        public void WeaponCollection_ValidCsv_LoadJSON_InvalidJson()
        {
            // LoadJSON() on the data2.csv and validate that Load returns false, and there’s 0 entries
            LoadCollection();
            Assert.IsTrue(weaponCollection.SaveAsCSV(outputPath));
            weaponCollection = new WeaponCollection();
            Assert.IsFalse(weaponCollection.LoadJSON(outputPath));
            Assert.IsTrue(weaponCollection.Count == 0);
            CleanUp();
        }
    }
}

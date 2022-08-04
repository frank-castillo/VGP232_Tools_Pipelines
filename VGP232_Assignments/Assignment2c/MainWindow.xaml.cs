using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeaponLib;

namespace Assignment2c
{
    public partial class MainWindow : Window
    {
        WeaponCollection myWeapons = new WeaponCollection();
        List<Weapon> helperListWeapons = new List<Weapon>();
        string currentSortOption = string.Empty;
        WeaponType currentTypeSelection = WeaponType.None;
        Weapon selectedWeapon = null;
        EditWeaponWindow editWeaponWindow = new EditWeaponWindow();

        public MainWindow()
        {
            InitializeComponent();

            editWeaponWindow.Closing += (object sender, System.ComponentModel.CancelEventArgs e) =>
            {
                e.Cancel = true;
                editWeaponWindow.Hide();
            };

            var optionNames = Enum.GetNames(typeof(WeaponType));
            foreach (var option in optionNames)
            {
                ListBoxItem lbi = new ListBoxItem();
                lbi.Content = option;
                WeaponTypeBox.Items.Add(lbi);
            }
        }

        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            if (myWeapons.Count == 0)
                return;

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "NewCollection"; // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "CSV (.csv)|.csv;|JSON (.json)|.json|XML (.xml)|.xml";

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                myWeapons.Save(filename);

                if(File.Exists(filename))
                {
                    MessageBox.Show("File saved correctly!");
                }
            }
        }

        private void LoadClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files(*.csv)|*.csv|JSON Files(*.json)|*.json|XML Files(*.xml)|*.xml";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == true)
            {
                myWeapons.Load(openFileDialog.FileName);
                FillWeaponList();
            }
        }

        void FillWeaponList()
        {
            if (myWeapons.Count == 0)
            {
                MessageBox.Show("Your collection is empty! Please revise your file.");
                return;
            }

            if (currentSortOption != string.Empty)
            {
                myWeapons.SortBy(currentSortOption);
            }

            if (WeaponTypeBox.SelectedIndex == -1)
            {
                WeaponTypeBox.SelectedIndex = 0;
            }

            helperListWeapons = null;
            helperListWeapons = myWeapons.GetAllWeaponsOfType(currentTypeSelection);

            if(!String.IsNullOrEmpty(FilterInput.Text))
            {
                List<Weapon> temp = new List<Weapon>();
                foreach (var item in helperListWeapons)
                {
                    if (item.ToString().IndexOf(FilterInput.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        temp.Add(item);
                    }
                }

                helperListWeapons = temp;
            }

            WeaponsViewList.ItemsSource = null;
            WeaponsViewList.Items.Clear();
            WeaponsViewList.ItemsSource = helperListWeapons;
            WeaponsViewList.SelectedIndex = -1;

            LabelCount.Content = $"Total/Displayed - {myWeapons.Count}/{helperListWeapons.Count}";
        }

        public void UpdateWeaponList()
        {
            FillWeaponList();
        }

        public void RegisterNewWeapon(Weapon weapon)
        {
            myWeapons.Add(weapon);
            FillWeaponList();
        }

        private void SortRadioSelected(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null)
                return;
            string name = radioButton.Content.ToString();
            currentSortOption = name;
            FillWeaponList();
        }

        //TODO: REFACTOR
        private void OpenWeaponEditWindow(EditWindowMode mode)
        {
            editWeaponWindow.SetWindowMode(mode, this);
            editWeaponWindow.Show();
        }

        private void AddClicked(object sender, RoutedEventArgs e)
        {
            OpenWeaponEditWindow(EditWindowMode.Add);
        }

        private void EditClicked(object sender, RoutedEventArgs e)
        {
            OpenWeaponEditWindow(EditWindowMode.Edit);
            editWeaponWindow.ShowSelectedWeaponData(selectedWeapon);
        }

        private void RemoveClicked(object sender, RoutedEventArgs e)
        {
            if (helperListWeapons.Count == 0)
            {
                MessageBox.Show("Your collection is empty. Nothing to remove.");
                return;
            }
            else if (WeaponsViewList.SelectedIndex == -1)
            {
                MessageBox.Show("No weapon is currently selected. Please make a selection before removing");
                return;
            }

            selectedWeapon = helperListWeapons[WeaponsViewList.SelectedIndex];
            helperListWeapons.Remove(selectedWeapon);
            myWeapons.Remove(selectedWeapon);
            selectedWeapon = null;
            FillWeaponList();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FillWeaponList();
        }

        private void WeaponType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (myWeapons.Count == 0)
            {
                MessageBox.Show("Your collection is empty. Nothing to Sort by! Load a file.");
                WeaponTypeBox.SelectedIndex = -1;
                return;
            }

            currentTypeSelection = (WeaponType)Enum.Parse(typeof(WeaponType), WeaponTypeBox.SelectedIndex.ToString());

            FillWeaponList();
        }

        private void WeaponsViewList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(helperListWeapons.Count == 0 || WeaponsViewList.SelectedIndex == -1)
            {
                return;
            }

            selectedWeapon = helperListWeapons[WeaponsViewList.SelectedIndex];
        }
    }
}

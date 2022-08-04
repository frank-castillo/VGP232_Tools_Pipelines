using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WeaponLib;

namespace Assignment2c
{
    public enum EditWindowMode { None, Add, Edit };

    public partial class EditWeaponWindow : Window
    {
        EditWindowMode modeSelected = EditWindowMode.None;
        const int MAX_RARITY = 5;
        private Weapon currentSelectedWeapon = null;
        private Weapon newWeapon = null;
        private MainWindow _owner = null;

        public EditWeaponWindow()
        {
            InitializeComponent();
            Owner = null;

            newWeapon = new Weapon();

            for (int i = 0; i < MAX_RARITY; i++)
            {
                RarityBox.Items.Add(i + 1);
            }

            string[] weaponTypes = Enum.GetNames(typeof(WeaponType));
            foreach (string weaponType in weaponTypes)
            {
                if (weaponType == "All")
                    continue;

                TypeBox.Items.Add(weaponType);
            }
        }

        public void SetWindowMode(EditWindowMode mode, MainWindow owner)
        {
            modeSelected = mode;

            switch (modeSelected)
            {
                case EditWindowMode.None:
                    break;
                case EditWindowMode.Add:
                    this.Title = "Add Weapon";
                    SaveButton.Content = "Add";
                    break;
                case EditWindowMode.Edit:
                    this.Title = "Edit Weapon";
                    SaveButton.Content = "Save";
                    break;
                default:
                    break;
            }

            this.Owner = owner;
            _owner = owner;

            ResetWindow();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateWeapons();

            switch (modeSelected)
            {
                case EditWindowMode.None:
                    break;
                case EditWindowMode.Add:
                    _owner.RegisterNewWeapon(newWeapon);
                    break;
                case EditWindowMode.Edit:
                    _owner.UpdateWeaponList();
                    break;
                default:
                    break;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            Random randomGenerator = new Random();

            TypeBox.SelectedIndex = randomGenerator.Next(1, TypeBox.Items.Count);
            RarityBox.SelectedIndex = randomGenerator.Next(0, RarityBox.Items.Count);
            BaseAttackBox.Text = randomGenerator.Next(15, 80).ToString();
        }

        private void ResetWindow()
        {
            ImageBox.Source = null;
            currentSelectedWeapon = null;
            NameTextBox.Text = String.Empty;
            URLTextBox.Text = String.Empty;
            BaseAttackBox.Text = String.Empty;
            SecondaryStatBox.Text = String.Empty;
            PassiveBox.Text = String.Empty;
            RarityBox.SelectedIndex = -1;
            TypeBox.SelectedIndex = -1;
        }

        public void ShowSelectedWeaponData(Weapon weapon)
        {
            currentSelectedWeapon = weapon;
            NameTextBox.Text = weapon.Name;
            URLTextBox.Text = weapon.Image;
            BaseAttackBox.Text = weapon.BaseAttack.ToString();
            SecondaryStatBox.Text = weapon.SecondaryStat;
            PassiveBox.Text = weapon.Passive;
            RarityBox.SelectedIndex = weapon.Rarity - 1;
            TypeBox.SelectedIndex = (int)weapon.Type - 1;

            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri($@"{weapon.Image}", UriKind.Absolute);
                bitmap.EndInit();
                ImageBox.Source = bitmap;
            }
            catch (Exception e)
            {
                MessageBox.Show("Loading Image failed:" + e.Message);
            }
        }

        private void UpdateWeapons()
        {
            int baseAttack = 0;

            switch (modeSelected)
            {
                case EditWindowMode.None:
                    break;
                case EditWindowMode.Add:
                    newWeapon.Passive = PassiveBox.Text;
                    newWeapon.SecondaryStat = SecondaryStatBox.Text;

                    if (int.TryParse(BaseAttackBox.Text, out baseAttack))
                        newWeapon.BaseAttack = baseAttack;

                    newWeapon.Image = URLTextBox.Text;

                    if (TypeBox.SelectedIndex == -1 || TypeBox.SelectedIndex == 0)
                    {
                        newWeapon.Type = WeaponType.None;
                    }
                    else
                    {
                        var type = (WeaponType)TypeBox.SelectedIndex + 1;
                        newWeapon.Type = type;
                    }

                    newWeapon.Name = NameTextBox.Text;
                    newWeapon.Rarity = RarityBox.SelectedIndex + 1;
                    break;
                case EditWindowMode.Edit:
                    currentSelectedWeapon.Passive = PassiveBox.Text;
                    currentSelectedWeapon.SecondaryStat = SecondaryStatBox.Text;

                    int.TryParse(BaseAttackBox.Text, out baseAttack);
                    currentSelectedWeapon.BaseAttack = baseAttack;

                    currentSelectedWeapon.Image = URLTextBox.Text;

                    if (TypeBox.SelectedIndex == -1 || TypeBox.SelectedIndex == 0)
                    {
                        currentSelectedWeapon.Type = WeaponType.None;
                    }
                    else
                    {
                        var type = (WeaponType)TypeBox.SelectedIndex + 1;
                        currentSelectedWeapon.Type = type;
                    }

                    currentSelectedWeapon.Name = NameTextBox.Text;
                    currentSelectedWeapon.Rarity = RarityBox.SelectedIndex + 1;
                    break;
                default:
                    break;
            }
        }
    }
}

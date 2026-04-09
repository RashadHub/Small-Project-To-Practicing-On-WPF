using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Media.Imaging;
using Busniess_Layer;
using Microsoft.Win32;

namespace Presentation_Layer__WPF_
{
    public partial class AddEditForm : Window
    {

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private string _imagePath = "";

        int _ContactID;
        clsContacts _Contact = new clsContacts();
        public AddEditForm(int ContactID)
        {
            InitializeComponent();
            _ContactID = ContactID;

            if (_ContactID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;
            _LoadData();
        }



        private void _LoadCountries()
        {
            DataTable dtCountries = clsCountries.GetAllCountries();

            foreach (DataRow row in dtCountries.Rows)
            {

                cboCountry.Items.Add(row["CountryName"]);

            }
            //cboCountry.DisplayMemberPath = "CountryName";
            //cboCountry.SelectedValuePath = "CountryID";
        }

        private void _LoadData()
        {

            _LoadCountries();
            cboCountry.SelectedIndex = 0;

            if (_Mode == enMode.AddNew)
            {
                txtblcMode.Text = "Add New Contact";
                _Contact = new clsContacts();
                return;
            }

            _Contact = clsContacts.Find(_ContactID);

            if (_Contact == null)
            {
                MessageBox.Show("This form will be closed because No Contact with ID = " + _ContactID);
                this.Close();

                return;
            }

            txtblcMode.Text = "Edit Contact ID = " + _ContactID;
            txtblcContactID.Text = _ContactID.ToString();
            txtFirstName.Text = _Contact.FirstName;
            txtLastName.Text = _Contact.LastName;
            txtEmail.Text = _Contact.Email;
            txtPhone.Text = _Contact.Phone;
            txtAddress.Text = _Contact.Address;
            dpBirthDate.SelectedDate = _Contact.DateOfBirth;

            if (!string.IsNullOrEmpty(_Contact.ImagePath))
            {
                imgPreview.Source = new BitmapImage(new Uri(_Contact.ImagePath));
            }

            bool hasImage = imgPreview.Source != null;

            RemoveImageButton.Visibility = Visibility.Collapsed;
            //this will select the country in the combobox.
            cboCountry.SelectedValue = _Contact.CountryID;
        }



        // Image handling - Set Image button
        private void SetImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                Title = "Select an image",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All files|*.*"
            };

            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openDialog.FileName);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    imgPreview.Source = bitmap;
                    _imagePath = openDialog.FileName;

                    tbNoImage.Visibility = Visibility.Collapsed;
                    RemoveImageButton.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Image handling - Remove Image button
        private void RemoveImageButton_Click(object sender, RoutedEventArgs e)
        {
            _imagePath = "";
            imgPreview.Source = null;

            tbNoImage.Visibility = Visibility.Visible;

            RemoveImageButton.Visibility = Visibility.Collapsed;
        }


        // Save button
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("First Name is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFirstName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Last Name is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtLastName.Focus();
                return;
            }
            int CountryID = clsCountries.FindCountryByName(cboCountry.Text).CountryID;
            // Gather data
            _Contact.FirstName = txtFirstName.Text;
            _Contact.LastName = txtLastName.Text;
            _Contact.Email = txtEmail.Text;
            _Contact.Phone = txtPhone.Text;
            _Contact.DateOfBirth = dpBirthDate.SelectedDate.Value;
            _Contact.CountryID = CountryID;
            _Contact.Address = txtAddress.Text;
            if (imgPreview.Source is BitmapImage bitmap && bitmap.UriSource != null)
            {
                _Contact.ImagePath = _imagePath; ;
            }
            else
            {
                _Contact.ImagePath = "";
            }
            bool hasImage = imgPreview.Source != null;


            string message = $"Contact saved successfully!\n\n" +
                             $"Name: {_Contact.FirstName} {_Contact.LastName}\n" +
                             $"Email: {(string.IsNullOrEmpty(_Contact.Email) ? "—" : _Contact.Email)}\n" +
                             $"Phone: {(string.IsNullOrEmpty(_Contact.Phone) ? "—" : _Contact.Phone)}\n" +
                             $"DOB: {(_Contact.DateOfBirth != null ? _Contact.DateOfBirth.ToShortDateString() : "—")}\n" +
                             $"Country: {_Contact.CountryID}\n" +
                             $"Address: {(string.IsNullOrEmpty(_Contact.Address) ? "—" : _Contact.Address)}\n" +
                             $"Image Path: {_Contact.ImagePath}";

            if (_Contact.Save())
                MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.");

            _Mode = enMode.Update;
            txtblcMode.Text = "Edit Contact ID = " + _Contact.ContactID;
            txtblcContactID.Text = _Contact.ContactID.ToString();

            Close();
        }

        // Close button
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to close? Any unsaved changes will be lost.",
                "Confirm Close", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Close();
            }
        }
    }
}
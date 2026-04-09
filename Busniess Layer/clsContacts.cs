using Data_Layer;
using System.Data;


namespace Busniess_Layer
{

    public class clsContacts
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int ContactID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CountryID { get; set; }
        public string ImagePath { get; set; }

        public clsContacts()
        {
            this.ContactID = -1;
            this.FirstName = "";
            this.LastName = "";
            this.Email = "";
            this.Phone = "";
            this.Address = "";
            this.DateOfBirth = DateTime.Now;
            this.CountryID = -1;
            this.ImagePath = "";
            Mode = enMode.AddNew;

        }

        private clsContacts(int ContactID, string FirstName, string LastName,
           string Email, string Phone, string Address, DateTime DateOfBirth, int CountryID, string ImagePath)

        {
            this.ContactID = ContactID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Phone = Phone;
            this.Address = Address;
            this.DateOfBirth = DateOfBirth;
            this.CountryID = CountryID;
            this.ImagePath = ImagePath;

            Mode = enMode.Update;
        }

        static public clsContacts Find(int ContactID)
        {
            string FirstName = "", LastName = "", Email = "", Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int CountryID = -1;

            if (DataLayerContacts.FindContactByID(ContactID, ref FirstName, ref LastName,
                        ref Email, ref Phone, ref Address, ref DateOfBirth, ref CountryID, ref ImagePath))
            {
                return new clsContacts(ContactID, FirstName, LastName,
                         Email, Phone, Address, DateOfBirth, CountryID, ImagePath);
            }
            else
            {
                return null;
            }



        }

        private bool _AddNewContact()
        {

            this.ContactID = DataLayerContacts.AddNewContact(this.FirstName, this.LastName, this.Email, this.Phone,
                this.Address, this.DateOfBirth, this.CountryID, this.ImagePath);

            return (this.ContactID != -1);
        }
        private bool _UpdateContact()
        {
            return DataLayerContacts.UpdateContact(this.ContactID, this.FirstName, this.LastName, this.Email, this.Phone,
                 this.Address, this.DateOfBirth, this.CountryID, this.ImagePath);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewContact())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateContact();

            }
            return false;
        }

        public static DataTable GetAllContacts()
        {
            return DataLayerContacts.GetAllContacts();

        }
        public static bool DeleteContact(int ID)
        {
            return DataLayerContacts.DeleteContact(ID);
        }

        public static bool isContactExist(int ID)
        {
            return DataLayerContacts.IsContactExist(ID);
        }



    }



}

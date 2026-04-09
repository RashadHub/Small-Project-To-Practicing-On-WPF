using Data_Layer;
using System.Data;
namespace Busniess_Layer
{
    public class clsCountries
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string Code { get; set; }
        public string PhoneCode { get; set; }


        public clsCountries()
        {
            CountryID = -1;
            CountryName = string.Empty;
            Code = string.Empty;
            PhoneCode = string.Empty;
            Mode = enMode.AddNew;
        }

        private clsCountries(int countryId, string CountryName, string Code, string PhoneCode)
        {
            this.CountryID = countryId;
            this.CountryName = CountryName;
            this.Code = Code;
            this.PhoneCode = PhoneCode;
            Mode = enMode.Update;
        }


        static public clsCountries FindCountryByID(int countryID)
        {

            string countryName = string.Empty, Code = string.Empty, PhoneCode = string.Empty;

            if (DataLayerCountries.FindCountryByID(countryID, ref countryName, ref Code, ref PhoneCode))
                return new clsCountries(countryID, countryName, Code, PhoneCode);
            else
                return null;
        }
        
        static public clsCountries FindCountryByName(string CountryName)
        {
            int countryID = -1;
            string  Code = string.Empty, PhoneCode = string.Empty;

            if (DataLayerCountries.FindCountryByName(CountryName ,ref countryID, ref Code, ref PhoneCode))
                return new clsCountries(countryID, CountryName, Code, PhoneCode);
            else
                return null;
        }

        private bool _AddNewCountry()
        {

            this.CountryID = DataLayerCountries.AddNewCountry(this.CountryName, this.Code, this.PhoneCode);

            return (this.CountryID != -1);
        }

        private bool _UpdateCountry()
        {

            return DataLayerCountries.UpdateCountry(this.CountryID, this.CountryName, this.Code, this.PhoneCode);

        }

        public bool Save()
        {


            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCountry())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateCountry();

            }




            return false;
        }

        public static DataTable GetAllCountries()
        {
            return DataLayerCountries.GetAllCountries();

        }

        public static bool DeleteCountry(int ID)
        {
            return DataLayerCountries.DeleteCountry(ID);
        }

        public static bool isCountryExist(int ID)
        {
            return DataLayerCountries.IsCountryExist(ID);
        }

        public static bool isCountryExist(string CountryName)
        {
            return DataLayerCountries.IsCountryExist(CountryName);
        }


    }
}

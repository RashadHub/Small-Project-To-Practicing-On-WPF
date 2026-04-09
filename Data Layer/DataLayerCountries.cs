using Microsoft.Data.SqlClient;
using System.Data;

namespace Data_Layer
{
    public class DataLayerCountries
    {

        static public bool FindCountryByID(int ID, ref string CountryName, ref string Code, ref string PhoneCode)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataLayerSettings.ConnectionString);

            string Query = "SELECT * FROM [Countries] WHERE [CountryID] = @CountryID;";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.Add("@CountryID", SqlDbType.Int).Value = ID;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    CountryName = reader["CountryName"] != DBNull.Value ? (string)reader["CountryName"] : "";
                    Code = reader["Code"] != DBNull.Value ? (string)reader["Code"] : "";
                    PhoneCode = reader["PhoneCode"] != DBNull.Value ? (string)reader["PhoneCode"] : "";

                }
                reader.Close();
            }
            catch (Exception)
            {
                isFound = false;
                throw;
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }


        static public bool FindCountryByName(string CountryName, ref int CountryID, ref string Code, ref string PhoneCode)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataLayerSettings.ConnectionString);

            string Query = "SELECT * FROM [Countries] WHERE [CountryName] = @CountryName;";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = CountryName;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;


                    CountryID = reader["CountryID"] != DBNull.Value ? (int)reader["CountryID"] : -1;
                    Code = reader["Code"] != DBNull.Value ? (string)reader["Code"] : "";
                    PhoneCode = reader["PhoneCode"] != DBNull.Value ? (string)reader["PhoneCode"] : "";

                }
                reader.Close();
            }
            catch (Exception)
            {
                isFound = false;
                throw;
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public int AddNewCountry(string CountryName, string Code, string PhoneCode)
        {

            int CountryID = -1;
            SqlConnection connection = new SqlConnection(clsDataLayerSettings.ConnectionString);

            string Query = @"INSERT INTO Countries (CountryName,Code,PhoneCode)
                             VALUES (@CountryName,@Code,@PhoneCode);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = CountryName;

            if (Code != "")
                command.Parameters.Add("@Code", SqlDbType.NVarChar).Value = Code;
            else
                command.Parameters.Add("@Code", SqlDbType.NVarChar).Value = System.DBNull.Value;

            if (PhoneCode != "")
                command.Parameters.Add("@PhoneCode", SqlDbType.NVarChar).Value = PhoneCode;
            else
                command.Parameters.Add("@PhoneCode", SqlDbType.NVarChar).Value = System.DBNull.Value;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    CountryID = insertedID;
                }

            }

            catch (Exception)
            {

                throw;
            }
            finally
            {
                connection.Close();
            }

            return CountryID;
        }

        public static bool UpdateCountry(int ID, string CountryName, string Code, string PhoneCode)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataLayerSettings.ConnectionString);

            string Query = @"Update  Countries  
                            set CountryName=@CountryName,
                                Code=@Code,
                                PhoneCode=@PhoneCode
                                where CountryID = @CountryID";

            SqlCommand command = new SqlCommand(Query, connection);


            command.Parameters.Add("@CountryID", SqlDbType.Int).Value = ID;
            command.Parameters.Add("@Code", SqlDbType.NVarChar).Value = Code;
            command.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = CountryName;
            command.Parameters.Add("@PhoneCode", SqlDbType.NVarChar).Value = PhoneCode;

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception)
            {

                return false;
                throw;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }


        public static DataTable GetAllCountries()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataLayerSettings.ConnectionString);

            string query = "SELECT * FROM Countries order by CountryName";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }

            return dt;

        }

        public static bool DeleteCountry(int CountryID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataLayerSettings.ConnectionString);

            string query = @"Delete Countries 
                                where CountryID = @CountryID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@CountryID", CountryID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);

        }

        public static bool IsCountryExist(int ID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataLayerSettings.ConnectionString);

            string query = "SELECT Found=1 FROM Countries WHERE CountryID = @CountryID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@CountryID", ID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception )
            {
                isFound = false;
                throw;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }


        public static bool IsCountryExist(string CountryName)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataLayerSettings.ConnectionString);

            string query = "SELECT Found=1 FROM Countries WHERE CountryName = @CountryName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception)
            {
                isFound = false;
                throw;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

    }
}

using System;
using System.Data;
using System.Net;
using Microsoft.Data.SqlClient;
using System.Xml.Linq;
using System.Security.Policy;
using System.Numerics;
using System.Security.Cryptography.Pkcs;
using System.Collections.Generic;

namespace Data_Layer
{


    public class DataLayerContacts
    {
        static public bool FindContactByID(int ContactID, ref string FirstName, ref string LastName,
            ref string Email, ref string Phone, ref string Address,
            ref DateTime DateOfBirth, ref int CountryID, ref string ImagePath)
        {

            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataLayerSettings.ConnectionString);

            string Query = "SELECT * FROM Contacts WHERE ContactID = @ContactID";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.Add("@ContactID", SqlDbType.Int).Value = ContactID;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    FirstName = reader["FirstName"] != DBNull.Value ? (string)reader["FirstName"] : "";
                    LastName = reader["LastName"] != DBNull.Value ? (string)reader["LastName"] : "";
                    Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : "";
                    Phone = reader["Phone"] != DBNull.Value ? (string)reader["Phone"] : "";
                    Address = reader["Address"] != DBNull.Value ? (string)reader["Address"] : "";
                    DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime)reader["DateOfBirth"] : DateTime.Now;
                    CountryID = reader["CountryID"] != DBNull.Value ? (int)reader["CountryID"] : -1;
                    ImagePath = reader["ImagePath"] != DBNull.Value ? (string)reader["ImagePath"] : "";
                }
                else
                {
                    isFound = false;
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                isFound = false;

            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }



        static public int AddNewContact(string FirstName, string LastName,
             string Email, string Phone, string Address,
             DateTime DateOfBirth, int CountryID, string ImagePath)
        {
            SqlConnection connection = new SqlConnection(clsDataLayerSettings.ConnectionString);

            int ContactID = -1;
            string Query = "INSERT INTO [Contacts]([FirstName],[LastName],[Email],[Phone],[Address]," +
                "[DateOfBirth],[CountryID],[ImagePath])" +
                "VALUES(" +
                "@FirstName,@LastName,@Email,@Phone,@Address,@DateOfBirth,@CountryID,@ImagePath)" +
                "SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.Add("@FirstName", SqlDbType.NChar).Value = FirstName;
            command.Parameters.Add("@LastName", SqlDbType.NChar).Value = LastName;
            command.Parameters.Add("@Email", SqlDbType.NChar).Value = Email;
            command.Parameters.Add("@Phone", SqlDbType.NChar).Value = Phone;
            command.Parameters.Add("@Address", SqlDbType.NChar).Value = Address;
            command.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = DateOfBirth;
            command.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;
            if (ImagePath != "" && ImagePath != null)
                command.Parameters.Add("@ImagePath", SqlDbType.NChar).Value = ImagePath;
            else
                command.Parameters.Add("@ImagePath", SqlDbType.NChar).Value = System.DBNull.Value;

            try
            {
                connection.Open();
                object result = command.ExecuteNonQuery();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    ContactID = insertedID;
                }
                else
                    ContactID = -1;


            }
            catch (Exception ex)
            {
                ContactID = -1;

            }
            finally
            {
                connection.Close();
            }

            return ContactID;
        }

        public static bool UpdateContact(int ContactID, string FirstName, string LastName,
            string Email, string Phone, string Address, DateTime DateOfBirth, int CountryID, string ImagePath)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataLayerSettings.ConnectionString);

            string query = @"Update  Contacts  
                            set FirstName = @FirstName, 
                                LastName = @LastName, 
                                Email = @Email, 
                                Phone = @Phone, 
                                Address = @Address, 
                                DateOfBirth = @DateOfBirth,
                                CountryID = @CountryID,
                                ImagePath =@ImagePath
                                where ContactID = @ContactID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.Add("@ContactID", SqlDbType.Int).Value = ContactID;
            command.Parameters.Add("@FirstName", SqlDbType.NChar).Value = FirstName;
            command.Parameters.Add("@LastName", SqlDbType.NChar).Value = LastName;
            command.Parameters.Add("@Email", SqlDbType.NChar).Value = Email;
            command.Parameters.Add("@Phone", SqlDbType.NChar).Value = Phone;
            command.Parameters.Add("@Address", SqlDbType.NChar).Value = Address;
            command.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = DateOfBirth;
            command.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;
            if (ImagePath != "" && ImagePath != null)
                command.Parameters.Add("@ImagePath", SqlDbType.NChar).Value = ImagePath;
            else
                command.Parameters.Add("@ImagePath", SqlDbType.NChar).Value = System.DBNull.Value;

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        static public DataTable GetAllContacts()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataLayerSettings.ConnectionString);

            string Query = "SELECT * FROM Contacts";

            SqlCommand command = new SqlCommand(Query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static bool DeleteContact(int ContactID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataLayerSettings.ConnectionString);

            string query = @"Delete Contacts 
                                where ContactID = @ContactID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@ContactID", SqlDbType.Int).Value = ContactID;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);

        }

        public static bool IsContactExist(int ID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataLayerSettings.ConnectionString);

            string query = "SELECT Found=1 FROM Contacts WHERE ContactID = @ContactID";

            SqlCommand command = new SqlCommand(query, connection);

            
            command.Parameters.Add("@ContactID", SqlDbType.Int).Value =ID;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
               
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }



    }
}

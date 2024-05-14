using FinalProject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace FinalProject.DataAccess
{
    public class EnquiryLoginDataAccess : DBConnection
    {

        public void CreateEnquirer(string email, string password)
        {
            string sql = "sp_createLoginEnquiry";
            try
            {
                ExecuteNonQuery(
                sqltext: sql,
                    commandType: CommandType.StoredProcedure,
                new SqlParameter("@email", email),
                new SqlParameter("@password", password));

            }
            catch (SqlException sqle)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }



        public CreateEnquiry GetEnquirer(string email, string password)
        {

            string sql4 = "SELECT * FROM EnquiryLogin WHERE email=@email and password=@password";

            using (var reader4 = ExecuteReader(
                sqltext: sql4,
                commandType: CommandType.Text,
                new SqlParameter("@email", email),
                new SqlParameter("@password", password)))
            {
                bool isActive = false;

                while (reader4.Read())
                {
                    isActive = reader4.GetBoolean(3);
                }

                if (!reader4.IsClosed)
                {
                    reader4.Close();
                }

                if (!isActive)
                {
                    return null;
                }
            }



            try
            {
                string sql2 = "SELECT * FROM Enquiries WHERE EmailAddress=@email";
                CreateEnquiry model = null;

                using (var reader9 = ExecuteReader(
                    sqltext: sql2,
                    commandType: CommandType.Text,
                    new SqlParameter("@email", email)))
                {
                    int enqid = 0;
                    while (reader9.Read())
                    {
                        enqid = (reader9["Enquiry_ID"] is not DBNull) ? (int)reader9["Enquiry_ID"] : 0;
                    }

                    if (!reader9.IsClosed)
                    {
                        reader9.Close();
                    }

                    // Fetch documents using enqid
                    string sql5 = "Select * from Documents where enqId=@enqid and docType=1";
                    string sql6 = "Select * from Documents where enqId=@enqid and docType=2";
                    string sql7 = "Select * from Documents where enqId=@enqid and docType=3";

                    byte[] bytes1 = null;
                    byte[] bytes2 = null;
                    byte[] bytes3 = null;

                    using (var reader5 = ExecuteReader(
                        sqltext: sql5,
                        commandType: CommandType.Text,
                        new SqlParameter("@enqid", enqid)))
                    {
                        while (reader5.Read())
                        {
                            if (reader5["document"] != DBNull.Value)
                            {
                                bytes1 = (byte[])reader5["document"];
                            }
                        }
                    }

                    using (var reader6 = ExecuteReader(
                        sqltext: sql6,
                        commandType: CommandType.Text,
                        new SqlParameter("@enqid", enqid)))
                    {
                        while (reader6.Read())
                        {
                            if (reader6["document"] != DBNull.Value)
                            {
                                bytes2 = (byte[])reader6["document"];
                            }
                        }
                    }

                    using (var reader7 = ExecuteReader(
                        sqltext: sql7,
                        commandType: CommandType.Text,
                        new SqlParameter("@enqid", enqid)))
                    {
                        while (reader7.Read())
                        {
                            if (reader7["document"] != DBNull.Value)
                            {
                                bytes3 = (byte[])reader7["document"];
                            }
                        }
                    }




                    IFormFile f1 = CreateFormFileFromBytes(bytes1, "photo.png");
                    IFormFile f2 = CreateFormFileFromBytes(bytes2, "aadhar.png");
                    IFormFile f3 = CreateFormFileFromBytes(bytes3, "pancard.png");

                    string base64Image1 = bytes1!=null?Convert.ToBase64String(bytes1) : "111";
                    string base64Image2 = bytes2 != null ? Convert.ToBase64String(bytes2) : "111";
                    string base64Image3 = bytes3 != null ? Convert.ToBase64String(bytes3) : "111";




                    string sqlDetails = "SELECT * FROM Enquiries WHERE EmailAddress=@email";

                    using (var reader2 = ExecuteReader(
                 sqltext: sqlDetails,
                 commandType: CommandType.Text,
                 new SqlParameter("@email", email)))
                    {
                        while (reader2.Read())
                        {
                            model = new CreateEnquiry
                            {

                                FirstName = (reader2["FirstName"] is not DBNull) ? reader2["FirstName"].ToString() : null,
                                LastName = (reader2["LastName"] is not DBNull) ? reader2["LastName"].ToString() : null,
                                Address1 = (reader2["AddressLine1"] is not DBNull) ? reader2["AddressLine1"].ToString() : null,
                                Address2 = (reader2["AddressLine2"] is not DBNull) ? reader2["AddressLine2"].ToString() : null,
                                Address3 = (reader2["AddressLine3"] is not DBNull) ? reader2["AddressLine3"].ToString() : null,
                                PhoneNumber = (reader2["PhoneNumber"] is not DBNull) ? reader2["PhoneNumber"].ToString() : null,
                                Email = (reader2["EmailAddress"] is not DBNull) ? reader2["EmailAddress"].ToString() : null,
                                DOB = (reader2["DateOfBirth"] is not DBNull) ? Convert.ToDateTime(reader2["DateOfBirth"]) : default(DateTime),
                                City = (reader2["City"] is not DBNull) ? reader2["City"].ToString() : null,
                                Country = (reader2["Country"] is not DBNull) ? reader2["Country"].ToString() : null,
                                Status = (reader2["Status"] is not DBNull) ? (int)reader2["Status"] : default(int),
                                Pincode = (reader2["Pincode"] is not DBNull) ? (int)reader2["Pincode"] : default(int),
                                WantsCheque = (reader2["WantsCheque"] is not DBNull) ? Convert.ToBoolean(reader2["WantsCheque"]) : default(bool),
                                Feedback = (reader2["Feedback"] is not DBNull) ? reader2["Feedback"].ToString() : null,
                                IsActive = (reader2["IsActive"] is not DBNull) ? Convert.ToBoolean(reader2["IsActive"]) : default(bool),
                                AccountType = (reader2["AccountType"] is not DBNull) ? reader2["AccountType"].ToString() : null,
                                Balance = (reader2["Balance"] is not DBNull) ? (decimal)reader2["Balance"] : default(decimal),
                                Photo = f1,
                                Aadhar = f2,
                                PanCard = f3,
                                basePhoto = base64Image1,
                                baseAadhar = base64Image2,
                                basePanCard = base64Image3


                            };
                        }
                        if (!reader2.IsClosed)
                        {
                            reader2.Close();
                        }





                        if (model == null)
                        {
                            string sql3 = "sp_createEmptyEnquiry";


                            var reader3 = ExecuteReader(
                                sqltext: sql3,
                                commandType: CommandType.StoredProcedure,
                                new SqlParameter("@email", email)
                            );
                            while (reader3.Read())
                            {
                                model = new CreateEnquiry
                                {
                                    FirstName = (reader3["FirstName"] is not DBNull) ? reader3["FirstName"].ToString() : null,
                                    LastName = (reader3["LastName"] is not DBNull) ? reader3["LastName"].ToString() : null,
                                    Address1 = (reader3["AddressLine1"] is not DBNull) ? reader3["AddressLine1"].ToString() : null,
                                    Address2 = (reader3["AddressLine2"] is not DBNull) ? reader3["AddressLine2"].ToString() : null,
                                    Address3 = (reader3["AddressLine3"] is not DBNull) ? reader3["AddressLine3"].ToString() : null,
                                    PhoneNumber = (reader3["PhoneNumber"] is not DBNull) ? reader3["PhoneNumber"].ToString() : null,
                                    Email = (reader3["EmailAddress"] is not DBNull) ? reader3["EmailAddress"].ToString() : null,
                                    DOB = (reader3["DateOfBirth"] is not DBNull) ? Convert.ToDateTime(reader3["DateOfBirth"]) : default(DateTime),
                                    City = (reader3["City"] is not DBNull) ? reader3["City"].ToString() : null,
                                    Country = (reader3["Country"] is not DBNull) ? reader3["Country"].ToString() : null,
                                    Status = (reader3["Status"] is not DBNull) ? (int)reader3["Status"] : default(int),
                                    Pincode = (reader3["Pincode"] is not DBNull) ? (int)reader3["Pincode"] : default(int),
                                    WantsCheque = (reader3["WantsCheque"] is not DBNull) ? Convert.ToBoolean(reader3["WantsCheque"]) : default(bool),
                                    Feedback = (reader3["Feedback"] is not DBNull) ? reader3["Feedback"].ToString() : null,
                                    IsActive = (reader3["IsActive"] is not DBNull) ? Convert.ToBoolean(reader3["IsActive"]) : default(bool),
                                    AccountType = (reader3["AccountType"] is not DBNull) ? reader3["AccountType"].ToString() : null,
                                    Balance = (reader3["Balance"] is not DBNull) ? (decimal)reader3["Balance"] : default(decimal)
                                };
                            }

                            if (!reader3.IsClosed)
                            {
                                reader3.Close();
                            }
                        }



                        return model;
                    }
                }
            }
            catch (SqlException sqle)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        private IFormFile CreateFormFileFromBytes(byte[] bytes, string fileName)
        {
            if (bytes == null)
            {
                return null;
            }
            //string basees = Convert.ToBase64String(bytes);
            string contentType = "application/octet-stream";
            MemoryStream stream = new MemoryStream(bytes);
            return new FormFile(stream, 0, bytes.Length, fileName, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }



        public void CreateEnquiry(
              string firstName,
              string lastName,
              string address1,
              string address2,
              string address3,
              string phoneNumber,
              string email,
               DateTime dob,
              string city,
              string country,
               int status,
               int pincode,
              bool wants_cheque,
              string feedback,
             bool isActive,
            string accountType,           
           decimal balance,
            byte[] photo,
           byte[] aadhar,
           byte[] pancard
           
       )
        {
            string sql = "sp_createEnquiry";
            try
            {
                
                ExecuteNonQuery(
                    sqltext: sql,
                    commandType: CommandType.StoredProcedure,
                    new SqlParameter("@fName", firstName),
                    new SqlParameter("@lName", lastName),
                    new SqlParameter("@address1", address1),
                    new SqlParameter("@address2", address2),
                    new SqlParameter("@address3", address3),
                    new SqlParameter("@phoneNumber", phoneNumber),
                    new SqlParameter("@email", email),
                    new SqlParameter("@DOB", dob),
                    new SqlParameter("@city", city),
                    new SqlParameter("@country", country),
                    new SqlParameter("@status", status),
                    new SqlParameter("@pincode", pincode),
                    new SqlParameter("@wants_cheque", wants_cheque),
                    new SqlParameter("@feedback", feedback),
                    new SqlParameter("@isActive", isActive),
                    new SqlParameter("@accountType", accountType),                
                    new SqlParameter("@balance", balance),
                    new SqlParameter("@documentPhoto", photo),
                    new SqlParameter("@documentAadhar", aadhar),
                    new SqlParameter("@documentPanCard", pancard)
                );
            }
            catch (SqlException sqle)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }


        public void SaveEnquiry(
            string? firstName,
            string? lastName,
            string? address1,
            string? address2,
            string? address3,
            string? phoneNumber,
            string? email,
             DateTime dob,
            string? city,
            string? country,
             int status,
             int pincode,
            bool wants_cheque,
            string? feedback,
           bool isActive,
          string? accountType,
         decimal balance,
           byte[] photo,
           byte[] aadhar,
           byte[] pancard
     )
        {
            string sql = "sp_saveEnquiry";
            try
            {
                firstName = firstName.IsNullOrEmpty() || firstName.Equals("@@") ? "" : firstName;
                lastName = lastName.IsNullOrEmpty() || lastName.Equals("@@") ? "" : lastName;
                address1 = address1.IsNullOrEmpty() || address1.Equals("@@") ? "" : address1;
                address2 = address2.IsNullOrEmpty() || address2.Equals("@@") ? "" : address2;
                address3 = address3.IsNullOrEmpty() || address3.Equals("@@") ? "" : address3;
                phoneNumber = phoneNumber.IsNullOrEmpty() || phoneNumber.Equals("@@") ? "" : phoneNumber;
                city = city.IsNullOrEmpty() || city.Equals("@@") ? "" : city;
                country = country.IsNullOrEmpty() || country.Equals("@@") ? "" : country;
                pincode = pincode == 0 ? 0 : pincode;
                accountType = accountType.IsNullOrEmpty() || accountType.Equals("@@") ? "":accountType;
                
                ExecuteNonQuery(
                    sqltext: sql,
                    commandType: CommandType.StoredProcedure,
                    new SqlParameter("@fName", firstName),
                    new SqlParameter("@lName", lastName),
                    new SqlParameter("@address1", address1),
                    new SqlParameter("@address2", address2),
                    new SqlParameter("@address3", address3),
                    new SqlParameter("@phoneNumber", phoneNumber),
                    new SqlParameter("@email", email),
                    new SqlParameter("@dob", dob),
                    new SqlParameter("@city", city),
                    new SqlParameter("@country", country),
                    new SqlParameter("@status", status),
                    new SqlParameter("@pincode", pincode),
                    new SqlParameter("@wants_cheque", wants_cheque),
                    new SqlParameter("@feedback", feedback),
                    new SqlParameter("@isActive", isActive),
                    new SqlParameter("@accountType", accountType),
                    new SqlParameter("@balance", balance),
                     new SqlParameter("@documentPhoto", photo),
                    new SqlParameter("@documentAadhar", aadhar),
                    new SqlParameter("@documentPanCard", pancard)
                );
            }
            catch (SqlException sqle)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

    }
}

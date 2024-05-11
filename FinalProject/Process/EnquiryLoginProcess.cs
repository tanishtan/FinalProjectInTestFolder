using FinalProject.DataAccess;
using FinalProject.Model;

namespace FinalProject.Process
{
    public class EnquiryLoginProcess
    {

        EnquiryLoginDataAccess elda = new EnquiryLoginDataAccess();

        public void CreateEnquirer(string email, string password)
        {
            try
            {
                elda.CreateEnquirer(email, password);
            }
            catch
            {
                throw ;
            }
           
        }

        public CreateEnquiry GetEnquirer(string email, string password)
        {
            try
            {
                return elda.GetEnquirer(email, password);
            }
            catch
            {
                throw;
            }

        }

        public void CreateEnquiry(string firstName, string lastName, string address1, string address2, string address3, string phoneNumber,
                         string email, DateTime dob, string city, string country, int status, int pincode, bool wantsCheque,
                         string feedback, bool isActive, string accountType , decimal balance, byte[] formPhoto, byte[] formAadhar, byte[] formPanCard)
        {
            try
            {
               

                    elda.CreateEnquiry(firstName, lastName, address1, address2, address3, phoneNumber, email, dob, city, country,
                                             status, pincode, wantsCheque, feedback, isActive, accountType, balance, formPhoto, formAadhar, formPanCard);
              

            }
            catch
            {
               
                throw;
            }
        }

        public void SaveEnquiry(string? firstName, string? lastName, string? address1, string? address2, string? address3, string? phoneNumber,
                      string? email, DateTime dob, string? city, string? country, int status, int pincode, bool wantsCheque,
                      string? feedback, bool isActive, string? accountType, decimal balance, byte[]? formPhoto, byte[]? formAadhar, byte[]? formPanCard)
        {
            try
            {


                elda.SaveEnquiry(firstName, lastName, address1, address2, address3, phoneNumber, email, dob, city, country,
                                         status, pincode, wantsCheque, feedback, isActive, accountType, balance, formPhoto, formAadhar, formPanCard);


            }
            catch
            {

                throw;
            }
        }


    }
}

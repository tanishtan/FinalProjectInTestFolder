using FinalProject.Model;
using FinalProject.Process;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Controllers
{
    [Route("api")]
    [ApiController]
    public class EnquiryLoginController : Controller
    {
        EnquiryLoginProcess process = new EnquiryLoginProcess();

        [HttpPost(template:"AddEnquirer")]
        public IActionResult CreateEnquirer(EnquiryLogin model)
        {
            try {
                process.CreateEnquirer(model.Email, model.Password);
                return Ok(200);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(template: "GetEnquirer")]
        public IActionResult GetEnquirer(EnquiryLogin model)
        {
            try
            {
                CreateEnquiry model2 = null;
                model2 = process.GetEnquirer(model.Email, model.Password);


                if (model2 is null)
                {
                    return BadRequest("User not found");
                }
                else
                {

                    return Ok(model2);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private byte[] ConvertToBytes(IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }


       

        [HttpPost(template: "CreateEnquiry")]
        public IActionResult CreateEnquiry(CreateEnquiry model)
        {
            try
            {



                byte[] photoBytes = ConvertToBytes(model.Photo);
                byte[] aadharBytes = ConvertToBytes(model.Aadhar);
                byte[] pancardBytes = ConvertToBytes(model.PanCard);


                process.CreateEnquiry(
                model.FirstName,
                model.LastName,
                model.Address1,
                model.Address2,
                model.Address3,
                model.PhoneNumber,
                model.Email,
                model.DOB,
                model.City,
                model.Country,
                model.Status,
                model.Pincode,
                model.WantsCheque,
                model.Feedback,
                model.IsActive,
                model.AccountType,
                model.Balance,
                photoBytes,
                aadharBytes,
                pancardBytes
                 );
                return Ok(200);



            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(template: "SaveEnquiry")]
        public IActionResult SaveEnquiry(CreateEnquiry? model)
        {
            try
            {

                byte[]? photoBytes = null;
                if (model.Photo != null)
                {
                    photoBytes = ConvertToBytes(model.Photo);
                }

                byte[]? aadharBytes = null;
                if (model.Aadhar != null)
                {
                    aadharBytes = ConvertToBytes(model.Aadhar);
                }

                byte[]? pancardBytes = null;
                if (model.PanCard != null)
                {
                    pancardBytes = ConvertToBytes(model.PanCard);
                }
                

                process.SaveEnquiry(
            model.FirstName,
            model.LastName,
            model.Address1,
            model.Address2,
            model.Address3,
            model.PhoneNumber,
            model.Email,
            model.DOB,
            model.City,
            model.Country,
            model.Status,
            model.Pincode,
            model.WantsCheque,
            model.Feedback,
            model.IsActive,
            model.AccountType,
            model.Balance,
             photoBytes,
              aadharBytes,
              pancardBytes
              
             );
                return Ok(200);
            

                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok("Api Connected and Up!");
        }
    }
}

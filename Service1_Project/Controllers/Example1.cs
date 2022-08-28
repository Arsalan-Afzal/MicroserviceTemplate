using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service1_Project.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class Example1 : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> MyExample1()
        {
            //ApiResponseModel<Object> res = new ApiResponseModel<Object>();
            try
            {
               // res.ResponseData = await _pharmacy.LoadPharmacyValidation(TreatmentPlanDetailID, IsCondensedView);
            }
            catch (Exception ex)
            {
                //CustomExceptionsUtility.CustomSaveLog(ex);
                //res.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                //res.ResponseMessage = ex.ToString();
                //res.ResponseData = "Exception";
                return BadRequest(ex.ToString());
            }
            return Ok("Hello");
        }
    }
}

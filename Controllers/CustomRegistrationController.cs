using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELOTEC.Access.Interfaces;
using ELOTEC.Access.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ELOTEC.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class CustomRegistrationController : Controller
    {
        private ICustomRegiatration _ICustomRegiatration;


        public CustomRegistrationController() {
            this._ICustomRegiatration = new CustomRegiatrationRepo();
        }

        //// GET: api/CustomRegistration
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/CustomRegistration/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/CustomRegistration
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/CustomRegistration/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        [Route("GetCustomItemlist")]
        [HttpGet]
        public async Task<ActionResult> GetCustomItemlist(int userId, int deviceId)
        {
            var CustomItemList = _ICustomRegiatration.GetCustomItemlist(userId, deviceId);
            return Json(CustomItemList);
        }
        [Route("UpdateCustomItem")]
        [HttpGet]
        public async Task<ActionResult> UpdateCustomItem(int userId, int deviceId,int itemId,byte RegStatus) {
            var UpdatedStatus = await _ICustomRegiatration.UpdateCustomItem(userId, deviceId, itemId, RegStatus);
            return Json(UpdatedStatus);
        }
            


    }
}

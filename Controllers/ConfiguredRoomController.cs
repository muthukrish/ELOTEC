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
    public class ConfiguredRoomController : Controller
    {

        private IConfiguredRoom _IConfiguredRoom;
        public ConfiguredRoomController()
        {
            this._IConfiguredRoom = new ConfiguredRoomRepo();
        }
        //// GET: api/ConfiguredRoom
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/ConfiguredRoom/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/ConfiguredRoom
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/ConfiguredRoom/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        [HttpGet]
        [Route("GetDeviceList")]
        public async Task<ActionResult> GetDeviceList(DateTime fromDateVal, DateTime todateVal, string deviceName, int userId)
        {
            string filterStr = "";
            filterStr = " where RegistrationId > 0 and isActive=1";
            if (fromDateVal != null && todateVal != null)
            {
                filterStr = filterStr + " and ((Updated_Date >= '" + String.Format("{0:M/d/yyyy}", fromDateVal) + "' and Updated_Date <= '" + String.Format("{0:M/d/yyyy}", todateVal) + "') or Updated_Date is null)";
            }
            else if (fromDateVal != null)
            {
                filterStr = filterStr + " and ((Updated_Date >= '" + String.Format("{0:M/d/yyyy}", fromDateVal) + "') or Updated_Date is null)";
            }
            else if (todateVal != null)
            {
                filterStr = filterStr + " and ((Updated_Date <= '" + String.Format("{0:M/d/yyyy}", todateVal) + "') or Updated_Date is null)";
            }
            if (deviceName != "")
            {
                filterStr = filterStr + " and DeviceName ='" + deviceName + "'";
            }
            if (userId != null && userId > 0)
            {
                filterStr = filterStr + " and (UserId in(" + userId + "))";
            }
            var DeviceList = await _IConfiguredRoom.GetDeviceSettingDetails(filterStr,fromDateVal, todateVal, deviceName, userId);
            return Json(DeviceList);
        }
    }
}

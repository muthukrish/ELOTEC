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
        public async Task<ActionResult> GetDeviceList(string fromDateVal, string toDateVal, string deviceName, int userId,int page,int size)
        {
            string filterStr = "";
            filterStr = " where DeviceId > 0 and isActive=1";
            if (fromDateVal != null && toDateVal != null)
            {
                filterStr = filterStr + " and ((Updated_Date >= '" + String.Format("{0:M/d/yyyy}", fromDateVal) + "' and Updated_Date <= '" + String.Format("{0:M/d/yyyy}", toDateVal) + "') or Updated_Date is null)";
            }
            else if (fromDateVal != null)
            {
                filterStr = filterStr + " and ((Updated_Date >= '" + String.Format("{0:M/d/yyyy}", fromDateVal) + "') or Updated_Date is null)";
            }
            else if (toDateVal != null)
            {
                filterStr = filterStr + " and ((Updated_Date <= '" + String.Format("{0:M/d/yyyy}", toDateVal) + "') or Updated_Date is null)";
            }
            if (deviceName != "" && deviceName != null)
            {
                filterStr = filterStr + " and DeviceName like '%" + deviceName + "%'";
            }
            if (userId != null && userId > 0)
            {
                filterStr = filterStr + " and (UserId in(" + userId + "))";
            }
            var DeviceList = await _IConfiguredRoom.GetDeviceSettingDetails(filterStr, fromDateVal, toDateVal, deviceName, userId, page, size);
            return Json(DeviceList);
        }

        [HttpGet]
        [Route("GetUserList")]
        public async Task<ActionResult> GetUserList()
        {
            var UserList = await _IConfiguredRoom.GetUserList();
            return Json(UserList);
        }
    }

}

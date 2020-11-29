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
        [Route("DList")]
        public async Task<ActionResult> GetDeviceList(string fVal, string tVal, string dName, int uId, int p, int s)
        {
            string filterStr = "";
            filterStr = " where DeviceId > 0 and isActive=true";
            if (fVal != null && tVal != null)
            {
                filterStr = filterStr + " and ((Updated_Date >= '" + String.Format("{0:M/d/yyyy}", fVal) + "' and Updated_Date <= '" + String.Format("{0:M/d/yyyy}", tVal) + "') or Updated_Date is null)";
            }
            else if (fVal != null)
            {
                filterStr = filterStr + " and ((Updated_Date >= '" + String.Format("{0:M/d/yyyy}", fVal) + "') or Updated_Date is null)";
            }
            else if (tVal != null)
            {
                filterStr = filterStr + " and ((Updated_Date <= '" + String.Format("{0:M/d/yyyy}", tVal) + "') or Updated_Date is null)";
            }
            if (dName != "" && dName != null)
            {
                filterStr = filterStr + " and DeviceName like '%" + dName + "%'";
            }
            if (uId != null && uId > 0)
            {
                filterStr = filterStr + " and (UserId in(" + uId + "))";
            }
            var DeviceList = await _IConfiguredRoom.GetDeviceSettingDetails(filterStr, fVal, tVal, dName, uId, p, s);
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

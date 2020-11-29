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
    public class DeviceController : Controller
    {
        private IDevice _IDevice;
        public DeviceController() {
            this._IDevice = new DeviceRepo();
        }


        // GET: api/Device
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Device/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Device
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/Device/5
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
        [Route("UpdateRoomNumber")]
        public async Task<ActionResult> UpdateRoomNo(int userId, int deviceId, string roomName,string pfix,int roomNo) {
            var updateStatus = await _IDevice.UpdateRoomNo(userId, deviceId, roomName, pfix, roomNo);
            return Json(updateStatus);
        }

        [HttpGet]
        [Route("RegisterRoom")]
        public async Task<ActionResult> RegisterRoom(int userId, int deviceId, int roomId)
        {
            var updateStatus = await _IDevice.RegisterRoom(userId, deviceId, roomId);
            return Json(updateStatus);
        }

        [HttpGet]
        [Route("UnRegisterRoom")]
        public async Task<ActionResult> UnRegisterRoom(int userId, int deviceId, int roomId)
        {
            var updateStatus = await _IDevice.UnRegisterRoom(userId, deviceId, roomId);
            return Json(updateStatus);
        }


        [HttpGet]
        [Route("DeviceRegRoomDetails")]
        public async Task<ActionResult> DeviceRegRoomDetails(string bleid)
        {
            var updateStatus = await _IDevice.DeviceRegRoomDetails(bleid);
            return Json(updateStatus);
        }
    }
}

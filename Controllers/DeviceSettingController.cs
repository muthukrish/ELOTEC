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
    public class DeviceSettingController : Controller
    {
        private IDeviceSetting _IDeviceSetting;
        public DeviceSettingController() {
           this. _IDeviceSetting = new DeviceSettingRepo();
        }
        //// GET: api/DeviceSetting
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/DeviceSetting/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/DeviceSetting
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/DeviceSetting/5
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
        [Route("GetDeviceSettingDetails")]
        public async Task<ActionResult> GetDeviceSettingDetails(int userId, int deviceId) {
            var DeviceSettingDetails = await _IDeviceSetting.GetDeviceSettingDetails(userId, deviceId);
            return Json(DeviceSettingDetails);
        }

        [HttpGet]
        [Route("UpdateDeviceSetting")]
        public async Task<ActionResult> UpdateDeviceSetting(int userId, int deviceId,int radorCoverageVal, byte radorCoverageOnOff, int radorSensitivityLevelval, byte radorSensitivityOnOff, byte beepOnoff,byte radorLEDOnoff) {
            var UpdatedStatus = await _IDeviceSetting.UpdateDeviceSetting(userId, deviceId, radorCoverageVal, radorCoverageOnOff, radorSensitivityLevelval, radorSensitivityOnOff, beepOnoff, radorLEDOnoff);
            return Json(UpdatedStatus);
        }

        [HttpGet]
        [Route("GetDeviceDetails")]
        public async Task<ActionResult> GetDeviceDetails(int userId, int deviceId)
        {
            var DeviceDetails = await _IDeviceSetting.GetDeviceDetails(userId, deviceId);
            return Json(DeviceDetails);
        }
    }
}

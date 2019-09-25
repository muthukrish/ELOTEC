﻿using System;
using System.Threading.Tasks;
using ELOTEC.Access.Interfaces;
using ELOTEC.Access.Repositories;
using ELOTEC.Models;
using ELOTEC.Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ELOTEC.Infrastructure.Security;

namespace ELOTEC.Controllers
{
    //[Route(Routes.Registration)]
    
    [ApiController]
    public class RegistrationController : Controller
    {
        private IRegistrationDetails _RegistrationDetails;
        public RegistrationController()
        {
            this._RegistrationDetails = new RegistrationDetailsRepo();
        }
        [HttpPost]
        [Route("UpdateRegistration")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> UpdateRegistrationDetails([FromForm]Registration lstRegistration)
        {
            var RegistrationDetailsStatus = await _RegistrationDetails.UpdateRegistrationDetails(lstRegistration.UserId, lstRegistration.DeviceId, lstRegistration.ItemId, lstRegistration.IsReg, lstRegistration.Axis);
            //return Json(new { Message = "Success" });
            return Json(RegistrationDetailsStatus);
        }

        
        [HttpPost]
        [Route("RegistrationHistory")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> RegistrationDetails([FromForm]GetRegistered GetRegistered)
        {
            var registrationDetails = await _RegistrationDetails.GetDeviceDetailByUserName(GetRegistered.UserId, GetRegistered.DeviceId);
            return Json(registrationDetails);
            //return Json(new { RegistrationDetails = registrationDetails.Registration, DeviceLastUpdated = registrationDetails.DeviceLastUpdatedDetails, Message = "Success" });
        }


        [HttpGet]
        [Route("GetUpdateRegistration")]
        public async Task<IActionResult> GetUpdateRegistration( int UserId,int DeviceId,int ItemId,bool IsReg,string Axis)
        {
            var RegistrationDetailsStatus = await _RegistrationDetails.UpdateRegistrationDetails(UserId, DeviceId, ItemId, IsReg, Axis);
            return Json(RegistrationDetailsStatus);
        }

        [HttpGet]
        [Route("GetRegistrationHistory")]
        public async Task<IActionResult> GetRegistrationHistory( int UserId,int DeviceId)
        {
            var registrationDetails = await _RegistrationDetails.GetDeviceDetailByUserName(UserId, DeviceId);
            return Json(registrationDetails);
            //return Json(new { RegistrationDetails = registrationDetails.Registration, DeviceLastUpdated = registrationDetails.DeviceLastUpdatedDetails, Message = "Success" });
        }
    }
}
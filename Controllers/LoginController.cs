namespace ELOTEC.Controllers
{
    using System;
    using System.Threading.Tasks;
    using ELOTEC.Access.Interfaces;
    using ELOTEC.Access.Repositories;
    using ELOTEC.Models;
    using ELOTEC.Infrastructure.Common;
    using ELOTEC.Infrastructure.Constants;
    using ELOTEC.Infrastructure.Errors;
    using ELOTEC.Infrastructure.Security;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    //[Route(Routes.Login)]
    [ApiController]
    public class LoginController : Controller
    {
        private ILogin _loginRepo;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public LoginController(IJwtTokenGenerator jwtTokenGenerator)
        {
            this._loginRepo = new LoginRepo();
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        //[AllowAnonymous]
        //[HttpPost]
        //[Route("Login")]
        //// [Route(Routes.Login)]
        //public async Task<IActionResult> CheckLogincredentials([FromForm]Authenticationuser login)
        //{
        //    try
        //    {
        //        var loginResponse = await _loginRepo.LoginCheck(login.UserName, login.PassWord);
        //        if (Convert.ToBoolean(loginResponse[ResultKey.Success]))
        //        {
        //            loginResponse[ResultKey.Token] = await _jwtTokenGenerator.CreateToken(login.UserName);
        //        }
        //        return Json(loginResponse);
        //    }
        //    catch (Exception ex) {
        //        throw ex;
        //    }
           
        //}

        [AllowAnonymous]
        [HttpGet]
        [Route("GetLogin")]
        // [Route(Routes.Login)]
        public async Task<IActionResult> CheckLogincredentials(string userName, string passWord)
        {
            try
            {
                var loginResponse = await _loginRepo.LoginCheck(userName, passWord);
                return Json(loginResponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
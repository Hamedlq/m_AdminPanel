using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using CoreManager.Models;
using ServiceLayer;
using ServiceLayer.Models;
using Microsoft.Owin.Security;

namespace WebApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserManager _userManager;
        private CarInfoModel _carInfoModel = null;
        private LicenseInfoModel _licenseInfoModel = null;
        private PersoanlInfoModel _persoanlInfoModel = null;

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }
        public UserController()
        {
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (returnUrl == null || returnUrl == "")
                ViewBag.ReturnUrl = Url.Action("Index", "Home");
            else
                ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult SubmitContactUs(ContactUsModel model)
        {
            var res = _userManager.SubmitContactUs(model);
            return Json(res , JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LoginUser(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var res = _userManager.CallLogin(loginModel);
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    LoginWeb(res.userName, res.access_token);
                }
                if (res.error != null)
                {
                    return Json(new {Status= "Error", res.error }, JsonRequestBehavior.AllowGet);
                }
                if (!res.isMobileConfirmed)
                {
                    return Json(new { Status = "Confirmation" }, JsonRequestBehavior.AllowGet);
                }
                return Json( new {Status="OK"} , JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "WrongInfo" }, JsonRequestBehavior.AllowGet);
        }
        private void LoginWeb(string userName, string accessToken)
        {
            var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.UserData, accessToken),
                new Claim(ClaimTypes.Role,"WebUser"),
            }, "ApplicationCookie");

            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignIn(new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc= DateTimeOffset.Now.AddDays(30)
            }, identity);
        }
        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("Exituser", Name = "Exituser")]
        public ActionResult Exituser()
        {
            if (ModelState.IsValid)
            {
                var token = GetUserToken();
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
                }
                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignOut(new AuthenticationProperties());
                return Json(new {status = true});
            }
            return Json(new { status = "WrongInfo" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RegisterUser(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var res = _userManager.RegisterNewUser(model);
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    var loginRes = _userManager.CallLogin(new LoginModel() {Loginmobile = model.Mobile,Pass = model.Password});
                    if (loginRes.StatusCode == HttpStatusCode.OK)
                    {
                        LoginWeb(loginRes.userName, loginRes.access_token);
                    }
                    if (!loginRes.isMobileConfirmed)
                    {
                        res.Status = "Confirmation";
                        return Json(res, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json( res, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "WrongInfo" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ConfirmMobile(string mobileNo)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            bool res = _userManager.ConfirmMobile(token, mobileNo);
            return Json(res);
        }
        [HttpPost]
        [Authorize]
        [Route("SubmitPersonalPic", Name = "SubmitPersonalPic")]
        public ActionResult SubmitPersonalPic(HttpPostedFileBase userPic)
        {
            if (ModelState.IsValid)
            {
                var token = GetUserToken();
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
                }
                var res = _userManager.SubmitPersonalPic(userPic, token);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "WrongInfo" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        [Route("SubmitPersonalInfo", Name = "SubmitPersonalInfo")]
        public ActionResult SubmitPersonalInfo(PersoanlInfoModel model)
        {
            if (ModelState.IsValid)
            {
                var token = GetUserToken();
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
                }
                var res = _userManager.SubmitPersonalInfo(model, token);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "WrongInfo" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        [Route("GetPersonalInfo", Name = "GetPersonalInfo")]
        public ActionResult GetPersonalInfo()
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _persoanlInfoModel ?? _userManager.GetPersonalInfo(token);
            //_persoanlInfoModel = res;
            return Json(new {res.Name, res.Family, res.Gender, res.NationalCode, res.Mobile,res.Email,res.UserImageId, IsUserPicUploaded=(res.Base64UserPic!=null) });
        }
        [HttpGet]
        [Authorize]
        [Route("GetPersonalUserImage", Name = "GetPersonalUserImage")]
        public ActionResult GetPersonalUserImage()
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _persoanlInfoModel ?? _userManager.GetPersonalInfo(token);
            //_persoanlInfoModel = res;
            if (res.Base64UserPic != null)
            {
                var bytes = Convert.FromBase64String(res.Base64UserPic);
                return new FileContentResult(bytes, "image/jpeg");
            }
            return null;
        }
        [HttpPost]
        [Authorize]
        [Route("SubmitLicenseInfo", Name = "SubmitLicenseInfo")]
        public ActionResult SubmitLicenseInfo(LicenseInfoModel model)
        {
            if (ModelState.IsValid)
            {
                var token = GetUserToken();
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
                }
                var res = _userManager.SubmitLicenseInfo(model, token);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            return Json(new { uploaded = "BadRequest", status = "WrongInfo" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        [Route("SubmitLicensePic", Name = "SubmitLicensePic")]
        public ActionResult SubmitLicensePic(HttpPostedFileBase licensePic)
        {
            if (ModelState.IsValid)
            {
                var token = GetUserToken();
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
                }
                var res = _userManager.SubmitLicensePic(licensePic, token);
                return Json(new { res }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { uploaded = "BadRequest", status = "WrongInfo" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        [Route("GetLicenseInfo", Name = "GetLicenseInfo")]
        public ActionResult GetLicenseInfo()
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            LicenseInfoModel res = _licenseInfoModel ?? _userManager.GetLicenseInfo(token);
            //_licenseInfoModel = res;
            return Json(new { res.LicenseNo, IsLicensePicUploaded = (res.Base64LicensePic != null) });
        }
        [HttpGet]
        [Authorize]
        [Route("GetLicenseImage", Name = "GetLicenseImage")]
        public ActionResult GetLicenseImage()
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            LicenseInfoModel res = _licenseInfoModel ?? _userManager.GetLicenseInfo(token);
            //_licenseInfoModel = res;
            if (res.Base64LicensePic != null)
            {
                var bytes = Convert.FromBase64String(res.Base64LicensePic);
                return new FileContentResult(bytes, "image/jpeg");
            }
            return null;
        }
        [HttpGet]
        [Authorize]
        [Route("GetImageById", Name = "GetImageById")]
        public ActionResult GetImageById(Guid? id)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res =  _userManager.GetImageById(token, id);
            if (res.Base64ImageFile != null)
            {
                var bytes = Convert.FromBase64String(res.Base64ImageFile);
                return new FileContentResult(bytes, "image/jpeg");
            }
            return null;
        }
        [HttpPost]
        [Authorize]
        [Route("SubmitCarInfo", Name = "SubmitCarInfo")]
        public ActionResult SubmitCarInfo(CarInfoModel model)
        {
            if (ModelState.IsValid)
            {
                var token = GetUserToken();
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
                }
                var res = _userManager.SubmitCarInfo(model, token);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            return Json(new { uploaded = "ERROR", status = "WrongInfo" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        [Route("SubmitCarPic", Name = "SubmitCarPic")]
        public ActionResult SubmitCarPic(HttpPostedFileBase carCardPic)
        {
            if (ModelState.IsValid)
            {
                var token = GetUserToken();
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
                }
                var res = _userManager.SubmitCarPic(carCardPic, token);
                return Json(new { res }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { uploaded = "ERROR", status = "WrongInfo" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        [Route("SubmitBankPic", Name = "SubmitBankPic")]
        public ActionResult SubmitBankPic(HttpPostedFileBase bankCardPic)
        {
            if (ModelState.IsValid)
            {
                var token = GetUserToken();
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
                }
                var res = _userManager.SubmitBankPic(bankCardPic, token);
                return Json(new { res }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { uploaded = "ERROR", status = "WrongInfo" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        [Route("SubmitBankInfo", Name = "SubmitBankInfo")]
        public ActionResult SubmitBankInfo(BankInfoModel model)
        {
            if (ModelState.IsValid)
            {
                var token = GetUserToken();
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
                }
                var res = _userManager.SubmitBankInfo(model, token);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            return Json(new { uploaded = "ERROR", status = "WrongInfo" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        [Route("GetbankInfo", Name = "GetbankInfo")]
        public ActionResult GetbankInfo()
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            BankInfoModel res = _userManager.GetBankInfo(token);
            return Json(new { res.BankName,res.BankAccountNo,res.BankCardNo,res.BankCardImageId, IsBankCardPicUploaded = (res.Base64BankCardPic!= null) });
        }
        [HttpGet]
        [Authorize]
        [Route("GetBankCardImage", Name = "GetBankCardImage")]
        public ActionResult GetBankCardImage()
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            BankInfoModel res = _userManager.GetBankInfo(token);
            if (res.Base64BankCardPic != null)
            {
                var bytes = Convert.FromBase64String(res.Base64BankCardPic);
                return new FileContentResult(bytes, "image/jpeg");
            }
            return null;
        }

        [HttpPost]
        [Authorize]
        [Route("GetCarInfo", Name = "GetCarInfo")]
        public ActionResult GetCarInfo()
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            CarInfoModel res = _carInfoModel ?? _userManager.GetCarInfo(token);
            //_carInfoModel = res;
            return Json(new { res.CarType, res.CarColor, res.CarPlateNo ,res.CarBackImageId, res.CarFrontImageId, IsCarCardPicUploaded = (res.Base64CarCardPic != null) });
        }
        [HttpPost]
        [Authorize]
        [Route("GetUserCarInfo", Name = "GetUserCarInfo")]
        public ActionResult GetUserCarInfo(RegisterModel model)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            CarInfoModel res = _carInfoModel ?? _userManager.GetUserCarInfo(token,model.Mobile);
            //_carInfoModel = res;
            return Json(new { res.CarType, res.CarColor, res.CarPlateNo , res.CarBackImageId, res.CarFrontImageId, IsCarCardPicUploaded = (res.Base64CarCardPic != null) });
        }
        
        [HttpGet]
        [Authorize]
        [Route("GetCarCardImage", Name = "GetCarCardImage")]
        public ActionResult GetCarCardImage()
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            CarInfoModel res = _carInfoModel ?? _userManager.GetCarInfo(token);
            //_carInfoModel = res;
            if (res.Base64CarCardPic != null)
            {
                var bytes = Convert.FromBase64String(res.Base64CarCardPic);
                return new FileContentResult(bytes, "image/jpeg");
            }
            return null;
        }
        [HttpGet]
        [Authorize]
        [Route("GetCarCardBckImage", Name = "GetCarCardBckImage")]
        public ActionResult GetCarCardBckImage()
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            CarInfoModel res = _carInfoModel ?? _userManager.GetCarInfo(token);
            //_carInfoModel = res;
            if (res.Base64CarCardBckPic != null)
            {
                var bytes = Convert.FromBase64String(res.Base64CarCardBckPic);
                return new FileContentResult(bytes, "image/jpeg");
            }
            return null;
        }
        [HttpGet]
        [Authorize]
        [Route("RouteUserImage", Name = "RouteUserImage")]
        public ActionResult RouteUserImage(int id)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _userManager.GetRouteUserImage(token, id);
            //_persoanlInfoModel = res;
            if (res.Base64UserPic != null)
            {
                var bytes = Convert.FromBase64String(res.Base64UserPic);
                return new FileContentResult(bytes, "image/jpeg");
            }
            return null;
        }
        [HttpGet]
        [Authorize]
        [Route("CommentUserImage", Name = "CommentUserImage")]
        public ActionResult CommentUserImage(int id)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _userManager.GetCommentUserImage(token, id);
            //_persoanlInfoModel = res;
            if (res.Base64UserPic != null)
            {
                var bytes = Convert.FromBase64String(res.Base64UserPic);
                return new FileContentResult(bytes, "image/jpeg");
            }
            return null;
        }
        [HttpPost]
        [Authorize]
        [Route("GetAllUsers", Name = "GetAllUsers")]
        public ActionResult GetAllUsers()
        {
                var token = GetUserToken();
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
                }
                var res = _userManager.GetAllUsers(token);
                return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        [Route("GetUserPersonalInfo", Name = "GetUserPersonalInfo")]
        public ActionResult GetUserPersonalInfo(RegisterModel model)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _persoanlInfoModel ?? _userManager.GetUserPersonalInfo(token,model.Mobile);
            //_persoanlInfoModel = res;
            return Json(new { res.Name, res.Family, res.Gender, res.NationalCode, res.Mobile,res.UserImageId, res.Email, IsUserPicUploaded = (res.Base64UserPic != null) });
        }
        [HttpPost]
        [Authorize]
        [Route("GetUserByInfo", Name = "GetUserByInfo")]
        public ActionResult GetUserByInfo(UserModel model)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _userManager.GetUserByInfo(token,model);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        [Route("GetUserLicenseInfo", Name = "GetUserLicenseInfo")]
        public ActionResult GetUserLicenseInfo(RegisterModel model)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            LicenseInfoModel res = _licenseInfoModel ?? _userManager.GetUserLicenseInfo(token,model.Mobile);
            //_licenseInfoModel = res;
            return Json(new { res.LicenseNo,res.LicenseImageId, IsLicensePicUploaded = (res.Base64LicensePic != null) });
        }
        private string GetUserToken()
        {
            var identity = (ClaimsIdentity)User.Identity;
            List<Claim> claims = identity.Claims.ToList();
            if (!claims.Any(x => x.Type.Contains("userdata")))
            {
                //user is not loged in
                return string.Empty;
            }
            var token = claims.FirstOrDefault(x => x.Type.Contains("userdata")).Value;
            return token;
        }

    }
}

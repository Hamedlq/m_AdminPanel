using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using ServiceLayer;
using ServiceLayer.GroupManager;
using ServiceLayer.Models;

namespace WebApplication.Controllers
{
    public class AppController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IRouteManager _routeManager;
        private readonly IGroupManager _groupManager;

        public AppController(IUserManager userManager, IRouteManager routeManager,IGroupManager groupManager)
        {
            _userManager = userManager;
            _routeManager = routeManager;
            _groupManager = groupManager;
        }

        public AppController()
        {
        }


        [Authorize(Roles = "WebUser")]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("RegisterRoute", Name = "RegisterRoute")]
        public ActionResult RegisterRoute(RouteRequestModel routeModel)
        {
            if (ModelState.IsValid)
            {
                var token = GetUserToken();
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
                }
                var res = _routeManager.SubmitNewRoute(routeModel, token);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "WrongInfo" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("ConfirmRoute", Name = "ConfirmRoute")]
        public ActionResult ConfirmRoute(ConfirmationModel confirmModel)
        {
            if (ModelState.IsValid)
            {
                var token = GetUserToken();
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
                }
                var res = _routeManager.ConfirmRoute(confirmModel, token);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "WrongInfo" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("GetRouteSuggests", Name = "GetRouteSuggests")]
        public ActionResult GetRouteSuggests()
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _routeManager.GetRouteSuggest(token);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("GetRoutes", Name = "GetRoutes")]
        public ActionResult GetRoutes()
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _routeManager.GetUserRoutes(token);
                return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("GetAllRoutes", Name = "GetAllRoutes")]
        public ActionResult GetAllRoutes()
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _routeManager.GetAllRoutes(token);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("GetSuggestRoute", Name = "GetSuggestRoute")]
        public ActionResult GetSuggestRoute(int routeId)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _routeManager.GetSuggestRouteByRouteId(token, routeId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("GetRouteUser", Name = "GetRouteUser")]
        public ActionResult GetRouteUser(int routeId)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _routeManager.GetRouteUserByRouteId(token, routeId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        [Route("GetUserRoutes", Name = "GetUserRoutes")]
        public ActionResult GetUserRoutes(RegisterModel model)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _routeManager.GetUserRoutesByMobile(token,model.Mobile);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("AcceptSuggestRoute", Name = "AcceptSuggestRoute")]
        public ActionResult AcceptSuggestRoute(int routeId,int selRouteId)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _routeManager.AcceptSuggestRoute(token, routeId, selRouteId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("JoinGroup", Name = "JoinGroup")]
        public ActionResult JoinGroup(int routeId, int groupId)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _routeManager.JoinGroup(token, routeId, groupId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("LeaveGroup", Name = "LeaveGroup")]
        public ActionResult LeaveGroup(int routeId,int groupId)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _routeManager.LeaveGroup(token, routeId, groupId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("DeleteRoute", Name = "DeleteRoute")]
        public ActionResult DeleteRoute(int routeId)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _routeManager.DeleteRoute(token, routeId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("DeleteGroupSuggest", Name = "DeleteGroupSuggest")]
        public ActionResult DeleteGroupSuggest(int routeId, int groupId)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _routeManager.DeleteGroupSuggest(token, routeId, groupId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("ConfirmGroup", Name = "ConfirmGroup")]
        public ActionResult ConfirmGroup(AppointmentModel appointmentModel)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _groupManager.ConfirmGroup(token, appointmentModel);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("finalconfirmGroup", Name = "finalconfirmGroup")]
        public ActionResult FinalconfirmGroup(AppointmentModel appointmentModel)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _groupManager.FinalconfirmGroup(token, appointmentModel);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("DeleteRouteSuggest", Name = "DeleteRouteSuggest")]
        public ActionResult DeleteRouteSuggest(int selfRouteId,int routeId)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _routeManager.DeleteRouteSuggest(token, selfRouteId,routeId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("GetComments", Name = "GetComments")]
        public ActionResult GetComments(int groupId)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _routeManager.GetComments(token,groupId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        [Route("GetChats", Name = "GetComments")]
        public ActionResult GetChats(RegisterModel model)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _routeManager.GetChats(token, model.Mobile);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("SubmitComment", Name = "SubmitComment")]
        public ActionResult SubmitComment(int groupId,string comment)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _routeManager.SubmitComment(token, groupId,comment);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        [Route("submitChat", Name = "submitChat")]
        public ActionResult SubmitChat(string mobileNo, string chat)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _routeManager.SubmitChat(token, mobileNo, chat);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("GetPassengerConfirm", Name = "GetPassengerConfirm")]
        public ActionResult GetPassengerConfirmInfo(int groupId, int routeId)
        {
            var token = GetUserToken();
            if (string.IsNullOrEmpty(token))
            {
                return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
            }
            var res = _routeManager.GetPassengerConfirmInfo(token, groupId, routeId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "WebUser")]
        [Route("ValidateTiming", Name = "ValidateTiming")]
        public ActionResult ValidateTiming(RouteRequestModel routeModel)
        {
            if (ModelState.IsValid)
            {
                var token = GetUserToken();
                if (string.IsNullOrEmpty(token))
                {
                    return Json(new { status = "UnAuthorized" }, JsonRequestBehavior.AllowGet);
                }
                var res = _routeManager.ValidateTiming(routeModel, token);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "WrongInfo" }, JsonRequestBehavior.AllowGet);
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
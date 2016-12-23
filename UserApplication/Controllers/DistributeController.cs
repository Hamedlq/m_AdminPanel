using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceLayer;
using ServiceLayer.Models;

namespace WebApplication.Controllers
{
    [AllowAnonymous]
    public class DistributeController : Controller
    {
        private readonly IRouteManager _routeManager;

        public DistributeController(IRouteManager routeManager)
        {
            _routeManager = routeManager;
        }

        public DistributeController()
        {
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Insert", Name = "Insert")]
        public ActionResult Insert(List<DistributeModel> distributeModelList)
        {
            try
            {
                var res = _routeManager.DistributeRequest(distributeModelList);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Errors = "WrongInfo" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
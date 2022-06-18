using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SYMVC.Controllers
{
    public class ControllerBase : Controller
    {
        public ViewResult ErrorView(string ErrMsg)
        {
            ViewBag.ErrMsg = ErrMsg;
            return View("Error");
        }
    }
}
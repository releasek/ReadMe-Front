﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadMe_Front.Controllers
{

    public class CartController : Controller
    {
        [Authorize]
        // GET: Cart
        public ActionResult Cart()
        {
            return View();
        }
        //[Authorize]
        //public ActionResult 
        //{
        //}
    }
}
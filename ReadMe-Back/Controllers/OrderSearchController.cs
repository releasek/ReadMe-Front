﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadMe_Back.Models.EFModels;

namespace ReadMe_Back.Controllers
{
    [Authorize]

    public class OrderSearchController : Controller
    {
        private readonly AppDbContext _context;

        public OrderSearchController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OrderMain()
        {
            return View();
        }


    }
}

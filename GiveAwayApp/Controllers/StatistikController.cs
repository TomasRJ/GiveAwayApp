using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiveAwayApp.Data;
using GiveAwayApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiveAwayApp.Controllers
{
    public class StatistikController : Controller
    {
        private readonly GiveAwayAppContext _context;
        public StatistikController(GiveAwayAppContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var spilStatistikQuery = from spil in _context.Spil orderby spil.ValgtAntal descending select spil;

            StatiskViewModel statiskVM = new StatiskViewModel
            {
                PopulæreSpilList = await spilStatistikQuery.ToListAsync()
            };

            return View(statiskVM);
        }
    }
}

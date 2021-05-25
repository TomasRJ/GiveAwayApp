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
            var spilQuery = from spil in _context.Spil orderby spil.ValgtAntal descending select spil;
            var antalBrugere = from bruger in _context.Brugere select bruger;
            var statistikQuery = from statistik in _context.Statistik where statistik.AntalBesøgereForDato >= DateTime.UtcNow.AddDays(-7) select statistik;

            StatiskViewModel statiskVM = new StatiskViewModel
            {
                PopulæreSpilList = await spilQuery.ToListAsync(),
                PopulæreGenreList = await GenreListSetup(spilQuery),
                AntalBrugere = await antalBrugere.CountAsync(),
                SyvDageStatistik = await statistikQuery.ToListAsync()
            };

            return View(statiskVM);
        }
        private async Task<List<Spil>> GenreListSetup(IQueryable<Spil> query)
        {
            List<Spil> newGenreList = new();
            foreach (Spil spil in await query.ToListAsync())
            {
                if (newGenreList.Any(x => x.Genre == spil.Genre))
                {
                    newGenreList.Find(x => x.Genre == spil.Genre).ValgtAntal += spil.ValgtAntal;
                }
                else
                {
                    newGenreList.Add(spil);
                }
            }
            return newGenreList.OrderByDescending(x => x.ValgtAntal).ToList();
        }
    }
}

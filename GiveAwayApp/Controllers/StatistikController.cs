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

            StatistikViewModel statiskVM = new StatistikViewModel
            {
                PopulæreSpilList = await spilQuery.ToListAsync(),
                PopulæreGenreList = GenreListSetup(await spilQuery.ToListAsync()),
                AntalBrugere = await antalBrugere.CountAsync(),
                SyvDageStatistik = await statistikQuery.ToListAsync()
            };

            return View(statiskVM);
        }
        private List<Spil> GenreListSetup(List<Spil> spilList)
        {
            List<Spil> newGenreList = new();
            foreach (Spil spil in spilList)
            {
                if (newGenreList.Any(x => x.Genre == spil.Genre))
                {
                    newGenreList.Find(x => x.Genre == spil.Genre).ValgtAntal += spil.ValgtAntal;
                }
                else
                {
                    newGenreList.Add(new Spil() 
                    {
                        Genre = spil.Genre,
                        ValgtAntal = spil.ValgtAntal
                    });
                }
            }
            return newGenreList.OrderByDescending(x => x.ValgtAntal).ToList();
        }
    }
}

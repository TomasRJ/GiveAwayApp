using GiveAwayApp.Areas.Identity.Data;
using GiveAwayApp.Data;
using GiveAwayApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveAwayApp.Controllers
{
    public class LodtrækningController : Controller
    {
        private readonly GiveAwayAppContext _context;
        private readonly UserManager<GiveAwayAppUser> _userManager;
        public LodtrækningController(GiveAwayAppContext context, UserManager<GiveAwayAppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> Index()
        {
            var valgteSpilQuery = from spil in _context.Spil
                                  join bs in _context.BrugereSpil on spil.SpilId equals bs.SpilId
                                  select spil;

            var filtreretValgteSpilQuery = from spil in _context.Spil
                                           where (from lodtrækning in _context.Lodtrækning
                                                   where lodtrækning.ValgteSpilId == spil.SpilId
                                                   select lodtrækning.ValgteSpilId).Contains(spil.SpilId)
                                           select spil;

            var valgtTilLodtrækningQuery = from lodtrækning in _context.Lodtrækning
                                           where lodtrækning.ValgtTilLodtrækning == true
                                           join ls in _context.Spil on lodtrækning.ValgteSpilId equals ls.SpilId                                           
                                           select ls;
            var erTrukketSpilQuery = from lodtrækning in _context.Lodtrækning
                                     where lodtrækning.ErTrukket == true
                                     join ls in _context.Spil on lodtrækning.ValgteSpilId equals ls.SpilId
                                     select ls;

            LodtrækningViewModel lodtrækningVM = new LodtrækningViewModel
            {
                ValgteSpilList = await filtreretValgteSpilQuery.AnyAsync() ?
                    await valgteSpilQuery.Except(filtreretValgteSpilQuery).Distinct().ToListAsync() :
                    await valgteSpilQuery.Distinct().ToListAsync(),
                TrukketSpilList = await erTrukketSpilQuery.ToListAsync(),
                SpilTilLodtrækning = await valgtTilLodtrækningQuery.AnyAsync() ? await valgtTilLodtrækningQuery.SingleAsync() : null,
                BrugerInfo = await _userManager.GetUserAsync(HttpContext.User)
            };

            return View(lodtrækningVM);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> TilføjSpilTilLodtrækning(int[] valgteSpilIds)
        {
            if (valgteSpilIds.Length != 0)
            {
                List<Lodtrækning> nyeLodtrækninger = new();
                foreach (int valgteSpilId in valgteSpilIds)
                {
                    nyeLodtrækninger.Add(new Lodtrækning()
                    {
                        ValgteSpilId = valgteSpilId,
                        ValgtTilLodtrækning = true,
                        ErTrukket = false
                    });
                }

                await _context.Lodtrækning.AddRangeAsync(nyeLodtrækninger);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> SletSpilFraLodtrækning(int[] valgteSpilIds)
        {
            if (valgteSpilIds.Length != 0)
            {
                var lodtrækningQuery = from lodtrækning in _context.Lodtrækning select lodtrækning;
                List<Lodtrækning> valgteLodtrækning = await lodtrækningQuery.ToListAsync();

                List<Lodtrækning> fjernLodList = new();
                foreach (int valgteSpilId in valgteSpilIds)
                {
                    fjernLodList.Add(valgteLodtrækning.Find(x => x.ValgteSpilId == valgteSpilId));
                }

                _context.Lodtrækning.RemoveRange(fjernLodList);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> SletSpilFraTrukket(int[] valgteSpilIds)
        {
            if (valgteSpilIds.Length != 0)
            {
                var lodtrækningQuery = from lodtrækning in _context.Lodtrækning select lodtrækning;
                List<Lodtrækning> lodtrækningList = await lodtrækningQuery.ToListAsync();

                List<Lodtrækning> lodtrækningSletList = new();
                foreach (int lod in valgteSpilIds)
                {
                    lodtrækningSletList.Add(lodtrækningList.Find(x => x.ValgteSpilId == lod));
                }
                _context.Lodtrækning.RemoveRange(lodtrækningSletList);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> PåbegyndLodtrækning(int[] valgteSpilIds)
        {
            if (valgteSpilIds.Length != 0)
            {
                var brugereSpilQuery = from brugere in _context.Brugere
                                   join bs in _context.BrugereSpil on brugere.Id equals bs.BrugerId
                                   join spil in _context.Spil on bs.SpilId equals spil.SpilId
                                   select new { spil, brugere };
                var lodtrækningQuery = from lodtrækning in _context.Lodtrækning
                                       select lodtrækning;
                
                var brugereQueryList = await brugereSpilQuery.ToListAsync();
                List<Lodtrækning> lodtrækningList = await lodtrækningQuery.ToListAsync();
                List<Lodtrækning> opdaterLodde = new();
                Random tilfældig = new Random();

                foreach (var valgteSpilId in valgteSpilIds)
                {
                    Lodtrækning lod = lodtrækningList.Find(l => l.ValgteSpilId == valgteSpilId);
                    List<GiveAwayAppUser> brugerListFraSpilId = brugereQueryList.Where(s => s.spil.SpilId == valgteSpilId).Select(b => b.brugere).ToList();

                    lod.ValgtTilLodtrækning = false;
                    lod.ErTrukket = true;
                    lod.VinderBrugerId = brugerListFraSpilId[tilfældig.Next(brugerListFraSpilId.Count)].Id;

                    opdaterLodde.Add(lod);
                }

                _context.Lodtrækning.UpdateRange(opdaterLodde);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
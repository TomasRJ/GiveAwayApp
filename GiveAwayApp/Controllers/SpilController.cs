using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiveAwayApp.Data;
using GiveAwayApp.Models;
using Microsoft.AspNetCore.Identity;
using GiveAwayApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace GiveAwayApp.Controllers
{
    public class SpilController : Controller
    {
        private readonly GiveAwayAppContext _context;
        private readonly UserManager<GiveAwayAppUser> _userManager;
        private readonly SteamWebApiController _steamWebApi;

        public SpilController(GiveAwayAppContext context, UserManager<GiveAwayAppUser> userManager, SteamWebApiController steamWebApi)
        {
            _context = context;
            _userManager = userManager;
            _steamWebApi = steamWebApi;
        }
        private static GiveAwayAppUser BrugerInfo { get; set; }

        // GET: Spil
        public async Task<IActionResult> Index(string spilGenre, string titelFilter)
        {
            BrugerInfo = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.BrugerId = BrugerInfo == null ? "" : BrugerInfo.Id;
            IQueryable<string> spiludvalgGenreQuery;
            IQueryable<Spil> spiludvalgSpilQuery;

            if (BrugerInfo != null)
            {
                spiludvalgGenreQuery = from spil in _context.Spil
                                       where !(from bs in _context.BrugereSpil where bs.BrugerId == BrugerInfo.Id select bs.SpilId).Contains(spil.SpilId)
                                       orderby spil.Genre
                                       select spil.Genre;
                spiludvalgSpilQuery = from spil in _context.Spil
                                      where !(from bs in _context.BrugereSpil where bs.BrugerId == BrugerInfo.Id select bs.SpilId).Contains(spil.SpilId)
                                      select spil;
            }
            else
            {
                spiludvalgGenreQuery = from s in _context.Spil
                                       orderby s.Genre
                                       select s.Genre;
                spiludvalgSpilQuery = from s in _context.Spil
                                      select s;
            }

            if (!string.IsNullOrEmpty(titelFilter))
            {
                spiludvalgSpilQuery = spiludvalgSpilQuery.Where(s => s.Titel.Contains(titelFilter));
            }

            if (!string.IsNullOrEmpty(spilGenre))
            {
                spiludvalgSpilQuery = spiludvalgSpilQuery.Where(g => g.Genre == spilGenre);
            }

            SpilGenreViewModel spiludvalgVM = new SpilGenreViewModel
            {
                Genre = new SelectList(await spiludvalgGenreQuery.Distinct().ToListAsync()),
                SpilList = await spiludvalgSpilQuery.ToListAsync()
            };
            return View(spiludvalgVM);
        }
        // GET: Ønskeliste
        public async Task<IActionResult> Ønskeliste(string spilGenre, string titelFilter)
        {
            BrugerInfo = await _userManager.GetUserAsync(HttpContext.User);

            if (BrugerInfo != null)
            {

                IQueryable<string> valgteSpilGenreQuery = from spil in _context.Spil
                                                          join bs in _context.BrugereSpil on spil.SpilId equals bs.SpilId
                                                          where bs.BrugerId == BrugerInfo.Id
                                                          orderby spil.Genre
                                                          select spil.Genre; 
                IQueryable<Spil> valgteSpilQuery = from spil in _context.Spil
                                                   join bs in _context.BrugereSpil on spil.SpilId equals bs.SpilId
                                                   where bs.BrugerId == BrugerInfo.Id
                                                   select spil;

                if (!string.IsNullOrEmpty(titelFilter))
                {
                    valgteSpilQuery = valgteSpilQuery.Where(s => s.Titel.Contains(titelFilter));
                }

                if (!string.IsNullOrEmpty(spilGenre))
                {
                    valgteSpilQuery = valgteSpilQuery.Where(g => g.Genre == spilGenre);
                }
                SpilGenreViewModel valgteSpilVM = new SpilGenreViewModel
                {
                    Genre = new SelectList(await valgteSpilGenreQuery.Distinct().ToListAsync()),
                    SpilList = await valgteSpilQuery.ToListAsync()

                };
                return View(valgteSpilVM);
            }
            else
            {
                SpilGenreViewModel valgteSpilVM = new SpilGenreViewModel
                {
                    Genre = new SelectList(Enumerable.Empty<SelectListItem>()),
                    SpilList = new List<Spil>(Enumerable.Empty<Spil>())
                };
                return View(valgteSpilVM);
            }
        }
        // POST: Gem valgte spil
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> IndsendValgteSpilAsync(int[] valgteSpilIds)
        {
            if (valgteSpilIds.Length != 0)
            {
                List<BrugereSpil> brugereSpilData = new();
                foreach (int valgteSpilId in valgteSpilIds)
                {
                    brugereSpilData.Add(new BrugereSpil()
                    {
                        BrugerId = BrugerInfo.Id,
                        SpilId = valgteSpilId,
                        OprettelsesDato = DateTime.UtcNow 
                    });
                }
                await OpdaterAntalValgt(brugereSpilData, false);
                _context.BrugereSpil.AddRange(brugereSpilData);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        // POST: Slet valgte spil
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SletValgteSpilAsync(int[] valgteSpilIds)
        {
            if (valgteSpilIds.Length != 0)
            {
                List<BrugereSpil> brugereSpilData = new();
                foreach (int valgteSpilId in valgteSpilIds)
                {
                    brugereSpilData.Add(new BrugereSpil()
                    {
                        BrugerId = BrugerInfo.Id,
                        SpilId = valgteSpilId
                    });                    
                }
                await OpdaterAntalValgt(brugereSpilData, true);
                _context.BrugereSpil.RemoveRange(brugereSpilData);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Ønskeliste));
        }
        // OPDATERER Spil.AntalValgt
        private async Task OpdaterAntalValgt(List<BrugereSpil> brugereSpilData, bool slet)
        {
            foreach (int gamleSpilId in brugereSpilData.Select(s => s.SpilId))
            {
                Spil opdateretSpil = await _context.Spil.SingleAsync(spil => spil.SpilId == gamleSpilId);

                opdateretSpil.ValgtAntal = slet ? opdateretSpil.ValgtAntal -= 1UL : opdateretSpil.ValgtAntal += 1UL;

                _context.Spil.Update(opdateretSpil);
            }
        }

        // GET: Spil/Details/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spil = await _context.Spil
                .FirstOrDefaultAsync(m => m.SpilId == id);
            if (spil == null)
            {
                return NotFound();
            }

            return View(spil);
        }

        // GET: Spil/Create
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(string SteamID)
        {
            if (SteamID == null)
            {
                return View();
            }
            else
            {
                Spil spil = await _steamWebApi.GetSteamInfo(int.Parse(SteamID));
                return View(spil);
            }            
        }

        // POST: Spil/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("SpilId,SteamId,Titel,SpilCoverUrl,Udgivelsesdato,ValgtAntal,Genre,Pris")] Spil spil)
        {
            if (ModelState.IsValid)
            {
                _context.Add(spil);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(spil);
        }

        // GET: Spil/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spil = await _context.Spil.FindAsync(id);
            if (spil == null)
            {
                return NotFound();
            }
            return View(spil);
        }

        // POST: Spil/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("SpilId,SteamId,Titel,SpilCoverUrl,Udgivelsesdato,ValgtAntal,Genre,Pris")] Spil spil)
        {
            if (id != spil.SpilId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spil);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpilExists(spil.SpilId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(spil);
        }

        // GET: Spil/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spil = await _context.Spil
                .FirstOrDefaultAsync(m => m.SpilId == id);
            if (spil == null)
            {
                return NotFound();
            }

            return View(spil);
        }

        // POST: Spil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var spil = await _context.Spil.FindAsync(id);
            _context.Spil.Remove(spil);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpilExists(int id)
        {
            return _context.Spil.Any(e => e.SpilId == id);
        }
    }
}

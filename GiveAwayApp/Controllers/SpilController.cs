using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiveAwayApp.Data;
using GiveAwayApp.Models;

namespace GiveAwayApp.Controllers
{
    public class SpilController : Controller
    {
        private readonly GiveAwayAppContext _context;

        public SpilController(GiveAwayAppContext context)
        {
            _context = context;
        }

        // GET: Spil
        public async Task<IActionResult> Index(string spilGenre, string titelFilter)
        {
            var genreQuery = from s in _context.Spil orderby s.Genre select s.Genre;
            var spil = from s in _context.Spil select s;

            if (!string.IsNullOrEmpty(titelFilter))
            {
                spil = spil.Where(s => s.Titel.Contains(titelFilter));
            }

            if (!string.IsNullOrEmpty(spilGenre))
            {
                spil = spil.Where(g => g.Genre == spilGenre);
            }

            var spilGenreVM = new SpilGenreViewModel
            {
                Genre = new SelectList(await genreQuery.Distinct().ToListAsync()),
                SpilList = await spil.ToListAsync()
            };

            return View(spilGenreVM);
        }

        // GET: Spil/Details/5
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Spil/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

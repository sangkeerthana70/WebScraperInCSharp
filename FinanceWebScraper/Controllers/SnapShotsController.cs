using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinanceWebScraper.Data;
using FinanceWebScraper.Models;
using Microsoft.AspNetCore.Authorization;
using FinanceWebScraper.Services;

namespace FinanceWebScraper.Controllers
{
    public class SnapShotsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SnapShotsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SnapShots
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.SnapShot.ToListAsync());
        }

        // GET: SnapShots/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var snapShot = await _context.SnapShot
                .FirstOrDefaultAsync(m => m.ID == id);
            if (snapShot == null)
            {
                return NotFound();
            }

            return View(snapShot);
        }

        // GET: SnapShots/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: SnapShots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Userid,SnapshotTime")] SnapShot snapShot)
        {
            if (ModelState.IsValid)
            {
                SnapShot sn = new SnapShot("asangeethu@yahoo.com",DateTime.Now);
                Scraper s = new Scraper("asangeethu@yahoo.com", "@nuk1978");
                sn.Stocks = s.Scrape();
                _context.Add(sn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(snapShot);
        }

        // GET: SnapShots/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var snapShot = await _context.SnapShot.FindAsync(id);
            if (snapShot == null)
            {
                return NotFound();
            }
            return View(snapShot);
        }

        // POST: SnapShots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Userid,SnapshotTime")] SnapShot snapShot)
        {
            if (id != snapShot.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {


                try
                {
                    _context.Update(snapShot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SnapShotExists(snapShot.ID))
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
            return View(snapShot);
        }

        // GET: SnapShots/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var snapShot = await _context.SnapShot
                .FirstOrDefaultAsync(m => m.ID == id);
            if (snapShot == null)
            {
                return NotFound();
            }

            return View(snapShot);
        }

        // POST: SnapShots/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var snapShot = await _context.SnapShot.FindAsync(id);
            _context.SnapShot.Remove(snapShot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SnapShotExists(int id)
        {
            return _context.SnapShot.Any(e => e.ID == id);
        }
    }
}

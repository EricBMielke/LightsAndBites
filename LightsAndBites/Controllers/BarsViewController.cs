using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LightsAndBites.Data;
using LightsAndBites.Models;

namespace LightsAndBites.Controllers
{
    public class BarsViewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BarsViewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BarsView
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Bars.Include(b => b.Category).Include(b => b.City).Include(b => b.Comment);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BarsView/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bar = await _context.Bars
                .Include(b => b.Category)
                .Include(b => b.City)
                .Include(b => b.Comment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bar == null)
            {
                return NotFound();
            }

            return View(bar);
        }

        // GET: BarsView/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id");
            ViewData["CommentId"] = new SelectList(_context.Comments, "Id", "Id");
            return View();
        }

        // POST: BarsView/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CategoryId,Longitude,Latitude,Likes,Dislikes,CommentId,CityId,Website,CardPhoto")] Bar bar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", bar.CategoryId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id", bar.CityId);
            ViewData["CommentId"] = new SelectList(_context.Comments, "Id", "Id", bar.CommentId);
            return View(bar);
        }

        // GET: BarsView/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bar = await _context.Bars.FindAsync(id);
            if (bar == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", bar.CategoryId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id", bar.CityId);
            ViewData["CommentId"] = new SelectList(_context.Comments, "Id", "Id", bar.CommentId);
            return View(bar);
        }

        // POST: BarsView/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CategoryId,Longitude,Latitude,Likes,Dislikes,CommentId,CityId,Website,CardPhoto")] Bar bar)
        {
            if (id != bar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BarExists(bar.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", bar.CategoryId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id", bar.CityId);
            ViewData["CommentId"] = new SelectList(_context.Comments, "Id", "Id", bar.CommentId);
            return View(bar);
        }

        // GET: BarsView/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bar = await _context.Bars
                .Include(b => b.Category)
                .Include(b => b.City)
                .Include(b => b.Comment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bar == null)
            {
                return NotFound();
            }

            return View(bar);
        }

        // POST: BarsView/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bar = await _context.Bars.FindAsync(id);
            _context.Bars.Remove(bar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BarExists(int id)
        {
            return _context.Bars.Any(e => e.Id == id);
        }
    }
}

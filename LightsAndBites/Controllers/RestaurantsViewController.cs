using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightsAndBites.Data;
using LightsAndBites.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LightsAndBites.Controllers
{
    public class RestaurantsViewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantsViewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RestaurantsView
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Restaurants.Include(b => b.Category).Include(b => b.City);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RestaurantsView/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants
                .Include(b => b.Category)
                .Include(b => b.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            var likes = await _context.Rating.Where(r => r.RestaurantId == restaurant.Id).ToListAsync();
            var userLike = await _context.Rating.Where(r => r.RestaurantId == restaurant.Id).FirstOrDefaultAsync(r => r.UserEmail == User.Identity.Name);
            var comments = await _context.Comments.Where(c => c.RestaurantId == restaurant.Id).ToListAsync();
            if (restaurant == null)
            {
                return NotFound();
            }
            ViewData.Add("Comments", comments);
            ViewData.Add("Ratings", likes);
            ViewData.Add("UserRatings", userLike);

            return View(restaurant);
        }

        // GET: RestaurantsView/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id");
            ViewData["CommentId"] = new SelectList(_context.Comments, "Id", "Id");
            return View();
        }

        // POST: RestaurantsView/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CategoryId,Longitude,Latitude,Likes,Dislikes,CommentId,CityId,Website,CardPhoto")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", restaurant.CategoryId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id", restaurant.CityId);
            return View(restaurant);
        }

        // GET: RestaurantsView/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", restaurant.CategoryId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id", restaurant.CityId);
            return View(restaurant);
        }

        // POST: RestaurantsView/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CategoryId,Longitude,Latitude,Likes,Dislikes,CommentId,CityId,Website,CardPhoto")] Restaurant restaurant)
        {
            if (id != restaurant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", restaurant.CategoryId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id", restaurant.CityId);
            return View(restaurant);
        }

        // GET: RestaurantsView/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants
                .Include(b => b.Category)
                .Include(b => b.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: RestaurantsView/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(int id)
        {
            return _context.Restaurants.Any(e => e.Id == id);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LightsAndBites.Data;
using LightsAndBites.Models;
using LightsAndBites.ViewModels;
using System.Collections;
using Microsoft.AspNetCore.Authorization;

namespace LightsAndBites.Controllers
{
    public class UserProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public bool userDoesntExist = true;

        public UserProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }


        [Authorize(Roles = "Super Admin")]
        // GET: UserProfiles
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Index), "Recommendations");
            }
            return View(await _context.UserProfile.ToListAsync());
        }

        // GET: UserProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var thisUserName = User.Identity.Name;
            var thisUser = _context.UserProfile.FirstOrDefault(u => u.Email == thisUserName);
            id = thisUser.Id;
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfile
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // GET: UserProfiles/Create
        public IActionResult Create()
        {
            var currentUser = User.Identity.Name;
            foreach (UserProfile u in _context.UserProfile)
            {
                if (u.Email == currentUser)
                {
                    return RedirectToAction(nameof(Index), "Recommendations");
                }
            }
            UserProfileCreateViewModel userProfileCreateViewModel = new UserProfileCreateViewModel(_context);
            return View(userProfileCreateViewModel);
        }

        // POST: UserProfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,FirstName,LastName,Hometown,BarCategoryIdOne,BarCategoryIdTwo,RestaurantCategoryIdOne,RestaurantCategoryIdTwo,RestaurantCategoryIdThree,EventCategoryIdOne,EventCategoryIdTwo,EventCategoryIdThree")] UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),"Recommendations");
            }
            return View(userProfile);
        }

        // GET: UserProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfile.FindAsync(id);
            if (userProfile == null)
            {
                return NotFound();
            }
            return View(userProfile);
        }

        // POST: UserProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,FirstName,LastName,Hometown,BarCategoryIdOne,BarCategoryIdTwo,RestaurantCategoryIdOne,RestaurantCategoryIdTwo,RestaurantCategoryIdThree,EventCategoryIdOne,EventCategoryIdTwo,EventCategoryIdThree")] UserProfile userProfile)
        {
            if (id != userProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProfileExists(userProfile.Id))
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
            return View(userProfile);
        }

        // GET: UserProfiles/Delete/5
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfile
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // POST: UserProfiles/Delete/5
        [Authorize(Roles = "Super Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userProfile = await _context.UserProfile.FindAsync(id);
            _context.UserProfile.Remove(userProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserProfileExists(int id)
        {
            return _context.UserProfile.Any(e => e.Id == id);
        }
    }
}

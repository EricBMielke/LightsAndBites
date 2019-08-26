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
    public class EventsViewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsViewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventsView
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Events.Include(b => b.Category).Include(b => b.City);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EventsView/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventt = await _context.Events
                .Include(b => b.Category)
                .Include(b => b.City)
                .FirstOrDefaultAsync(e => e.EventId == id);
            if (eventt == null)
            {
                return NotFound();
            }

            return View(eventt);
        }

        // GET: EventsView/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id");
            ViewData["CommentId"] = new SelectList(_context.Comments, "Id", "Id");
            return View();
        }


        // GET: EventsView/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventt = await _context.Events.FindAsync(id);
            if (eventt == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", eventt.CategoryId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id", eventt.CityId);
            return View(eventt);
        }
    }
}
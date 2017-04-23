using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FdsWeb.Data;
using FdsWeb.Models;
using FdsWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FdsWeb.Controllers{
    public class EventsController : Controller{
        private readonly ApplicationDbContext _context;
        readonly UserManager<ApplicationUser> _userManager;

        private ApplicationUser GetUser() {
            var uid = _userManager.GetUserId( User );
            if( uid == null )
                return null;
            return _context.Users.First( u => u.Id == uid );
        }

        public EventsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager){
            _context = context;
            _userManager = userManager;
        }

        // GET: Events
        [AllowAnonymous]
        public async Task<IActionResult> Index() {
            ViewData[ "User" ] = GetUser();

            return View(await _context.Event.Include( e => e.ApplicationUser ).ToListAsync());
        }

        // GET: Events/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id){
            if (id == null){
                return NotFound();
            }

            var @event = await _context.Event
                .SingleOrDefaultAsync(m => m.Id == id);
            if (@event == null){
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create() {
            var usr = GetUser();
            if( usr == null || usr.Role == Role.User )
                return BadRequest();

            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( CreateEvent createEvent) {
            if (ModelState.IsValid) {
                var ev = new Event() {
                    ApplicationUser = GetUser(),
                    Name = createEvent.Name,
                    Latitude = double.Parse(createEvent.Latitude, CultureInfo.InvariantCulture),
                    Longitude = double.Parse(createEvent.Latitude, CultureInfo.InvariantCulture)
                };

                _context.Event.Add(ev);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(createEvent);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id){
            if (id == null){
                return NotFound();
            }

            var @event = await _context.Event.SingleOrDefaultAsync(m => m.Id == id);
            if (@event == null){
                return NotFound();
            }

            var usr = GetUser();
            if( usr == null || @event.ApplicationUserId != usr.Id )
                return BadRequest();

            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ApplicationUserId,Latitude,Longitude")] Event @event){
            if (id != @event.Id){
                return NotFound();
            }

            if (ModelState.IsValid){
                try{
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }catch (DbUpdateConcurrencyException){
                    if (!EventExists(@event.Id)){
                        return NotFound();
                    }else{
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id){
            if (id == null){
                return NotFound();
            }

            var @event = await _context.Event
                .SingleOrDefaultAsync(m => m.Id == id);
            if (@event == null){
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id){
            var @event = await _context.Event.SingleOrDefaultAsync(m => m.Id == id);
            _context.Event.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool EventExists(int id){
            return _context.Event.Any(e => e.Id == id);
        }
    }
}

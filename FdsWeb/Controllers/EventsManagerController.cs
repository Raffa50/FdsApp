using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fds.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FdsWeb.Data;
using FdsWeb.Models;
using FdsWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Identity;

namespace FdsWeb.Controllers
{
    public class EventsManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        readonly UserManager<ApplicationUser> _userManager;

        public EventsManagerController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }

        private ApplicationUser GetUser()
        {
            var uid = _userManager.GetUserId(User);
            if (uid == null)
                return null;
            return _context.Users.First(u => u.Id == uid);
        }

        // GET: EventsManager
        public async Task<IActionResult> Index()
        {
            var userEvents = _context.Events.Include(e => e.ApplicationUser).Include(e => e.EventType).Where( e => e.ApplicationUserId == GetUser().Id );
            return View(await userEvents.ToListAsync());
        }

        // GET: EventsManager/Create
        // GET: Events/Create
        public IActionResult Create(){
            var usr = GetUser();
            if (usr == null || usr.Role == Role.User)
                return BadRequest();

            ViewData["EventTypes"] = _context.EventTypes.ToList();

            return View();
        }

        // POST: EventsManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEvent model)
        {
            if (ModelState.IsValid)
            {
                var ev = new Event() {
                    ApplicationUser = GetUser(),
                    Name = model.Name,
                    Description = model.Description,
                    AgeMin = model.AgeMin,
                    AgeMax = model.AgeMax,
                    EventTypeId = model.EventTypeId,
                    Latitude = double.Parse(model.Latitude, CultureInfo.InvariantCulture),
                    Longitude = double.Parse(model.Longitude, CultureInfo.InvariantCulture)
                };

                _context.Events.Add(ev);

                foreach (var date in model.Schedule)
                {
                    try
                    {
                        _context.Schedules.Add(
                            new Schedule() { DateTime = DateTime.Parse(date, new CultureInfo("it-IT")), Event = ev });
                    } catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewData["EventTypes"] = _context.EventTypes.ToList();
            return View(model);
        }

        // GET: EventsManager/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.Include(e => e.Schedule).SingleOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            var usr = GetUser();
            if (usr == null || @event.ApplicationUserId != usr.Id)
                return BadRequest();

            var eventModel = new CreateEvent() {
                Id = @event.Id,
                Name = @event.Name,
                Description = @event.Description,
                Latitude = @event.Latitude.ToString(CultureInfo.InvariantCulture),
                Longitude = @event.Longitude.ToString(CultureInfo.InvariantCulture),
                Schedule = @event.Schedule.Select(e => e.DateTime.ToString(new CultureInfo("it-IT"))).ToList()
            };

            return View(eventModel);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateEvent @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var e = _context.Events.First(o => o.Id == id);
                    e.Name = @event.Name;
                    e.EventTypeId = @event.EventTypeId;
                    e.Schedule.Clear();
                    foreach (var dt in @event.Schedule)
                    {
                        e.Schedule.Add(new Schedule() { DateTime = DateTime.Parse(dt, new CultureInfo("it-IT")) });
                    }

                    _context.Update(e);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(id))
                    {
                        return NotFound();
                    } else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: EventsManager/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.ApplicationUser)
                .Include(e => e.EventType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: EventsManager/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.SingleOrDefaultAsync(m => m.Id == id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}

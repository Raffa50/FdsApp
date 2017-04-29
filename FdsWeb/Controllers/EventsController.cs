using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
using Newtonsoft.Json;

namespace FdsWeb.Controllers {
    public class EventsController : Controller {
        private readonly ApplicationDbContext _context;
        readonly UserManager< ApplicationUser > _userManager;
        private readonly Api.EventsController _eventApi;

        private ApplicationUser GetUser() {
            var uid = _userManager.GetUserId( User );
            if( uid == null )
                return null;
            return _context.Users.First( u => u.Id == uid );
        }

        public EventsController( ApplicationDbContext context, UserManager< ApplicationUser > userManager ) {
            _context = context;
            _userManager = userManager;

            _eventApi = new Api.EventsController(_context);
        }

        // GET: Events
        [AllowAnonymous]
        public async Task< IActionResult > Index( Search search ) {
            ViewData[ "User" ] = GetUser();

            return View( await _eventApi.GetSearchEvents( search ).ToListAsync() );
        }

        // GET: Events/Details/5
        [AllowAnonymous]
        public async Task< IActionResult > Details( int? id ) {
            if( id == null ) {
                return RedirectToAction( "Index" );
            }

            var @event = await _context.Events.Include( e => e.ApplicationUser ).Include( e => e.EventType )
                .Include( e => e.Schedule ).ThenInclude( uj => uj.UserJoined ).Include( e => e.UserJoined )
                .SingleOrDefaultAsync( m => m.Id == id );
            if( @event == null ) {
                return RedirectToAction( "Index" );
            }

            var user = GetUser();
            ViewData[ "user" ] = user;

            if( user == null )
                ViewData[ "reviewed" ] = false;
            else
                ViewData[ "reviewed" ] =
                    _context.UserJoinEvents.Any( o => o.ApplicationUserId == user.Id && o.EventId == id );

            return View( @event );
        }

        // GET: Events/Create
        public IActionResult Create() {
            var usr = GetUser();
            if( usr == null || usr.Role == Role.User )
                return BadRequest();

            ViewData[ "EventTypes" ] = _context.EventTypes.ToList();

            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task< IActionResult > Create( CreateEvent model ) {
            if( ModelState.IsValid ) {
                var ev = new Event<ApplicationUser>() {
                    ApplicationUser = GetUser(),
                    Name = model.Name,
                    Description = model.Description,
                    EventTypeId = model.EventTypeId,
                    Latitude = double.Parse( model.Latitude, CultureInfo.InvariantCulture ),
                    Longitude = double.Parse( model.Longitude, CultureInfo.InvariantCulture )
                };

                _context.Events.Add( ev );

                foreach( var date in model.Schedule ) {
                    _context.Schedules.Add( new Schedule<ApplicationUser>() {DateTime = date, Event = ev} );
                }

                await _context.SaveChangesAsync();
                return RedirectToAction( "Index" );
            }

            ViewData[ "EventTypes" ] = _context.EventTypes.ToList();
            return View( model );
        }

        // GET: Events/Edit/5
        public async Task< IActionResult > Edit( int? id ) {
            if( id == null ) {
                return NotFound();
            }

            var @event = await _context.Events.Include( e => e.Schedule ).SingleOrDefaultAsync( m => m.Id == id );
            if( @event == null ) {
                return NotFound();
            }

            var usr = GetUser();
            if( usr == null || @event.ApplicationUserId != usr.Id )
                return BadRequest();

            var eventModel = new CreateEvent() {
                Id = @event.Id,
                Name = @event.Name,
                Description = @event.Description,
                Latitude = @event.Latitude.ToString( CultureInfo.InvariantCulture ),
                Longitude = @event.Longitude.ToString( CultureInfo.InvariantCulture ),
                Schedule = @event.Schedule.Select( e => e.DateTime ).ToList()
            };

            return View( eventModel );
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task< IActionResult > Edit( int id, CreateEvent @event ) {
            if( id != @event.Id ) {
                return NotFound();
            }

            if( ModelState.IsValid ) {
                try {
                    var e = _context.Events.First( o => o.Id == id );
                    e.Name = @event.Name;
                    e.EventTypeId = @event.EventTypeId;
                    e.Schedule.Clear();
                    foreach( var dt in @event.Schedule ) {
                        e.Schedule.Add( new Schedule<ApplicationUser>() {DateTime = dt} );
                    }

                    _context.Update( e );
                    await _context.SaveChangesAsync();
                } catch( DbUpdateConcurrencyException ) {
                    if( !EventExists( id ) ) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction( "Index" );
            }
            return View( @event );
        }

        // GET: Events/Delete/5
        public async Task< IActionResult > Delete( int? id ) {
            if( id == null ) {
                return NotFound();
            }

            var @event = await _context.Events.SingleOrDefaultAsync( m => m.Id == id );
            if( @event == null ) {
                return NotFound();
            }

            return View( @event );
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName( "Delete" ), ValidateAntiForgeryToken]
        public async Task< IActionResult > DeleteConfirmed( int id ) {
            var @event = await _context.Events.SingleOrDefaultAsync( m => m.Id == id );
            _context.Events.Remove( @event );
            await _context.SaveChangesAsync();
            return RedirectToAction( "Index" );
        }

        private bool EventExists( int id ) { return _context.Events.Any( e => e.Id == id ); }

        [HttpPost]
        public IActionResult UserCreteReview( UserReview ur ) {
            if( ModelState.IsValid &&
                !_context.UserJoinEvents.Any( q => q.ApplicationUserId == GetUser().Id && q.EventId == ur.EventId ) ) {
                _context.UserJoinEvents.Add( new UserJoinEvent<ApplicationUser>() {
                    EventId = ur.EventId,
                    ScheduleId = ur.SheduleId,
                    ApplicationUserId = GetUser().Id,
                    Vote = ur.Vote,
                    Review = ur.Review
                } );
                _context.SaveChanges();
            }

            return RedirectToAction( "Details", new {id = ur.EventId} );
        }

        [AllowAnonymous]
        public IActionResult Search() {
            ViewData[ "EventTypes" ] = _context.EventTypes.ToList();

            return View();
        }
    }
}
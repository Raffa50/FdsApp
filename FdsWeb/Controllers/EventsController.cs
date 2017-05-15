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
            return View( await _eventApi.GetSearchEvents( search ).ToListAsync() );
        }

        // GET: Events/Details/5
        [AllowAnonymous]
        public async Task< IActionResult > Details( int? id ) {
            if( id == null ) {
                return RedirectToAction( "Index" );
            }

            var @event = await _context.Events.Include( e => e.ApplicationUser ).Include( e => e.EventType )
                .Include( e => e.Schedule ).ThenInclude( uj => uj.UserJoined ).Include( e => e.UserJoined ).ThenInclude( e => e.ApplicationUser )
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

        [HttpPost]
        public IActionResult UserCreteReview( UserReview ur ) {
            if( ModelState.IsValid &&
                !_context.UserJoinEvents.Any( q => q.ApplicationUserId == GetUser().Id && q.EventId == ur.EventId ) ) {
                _context.UserJoinEvents.Add( new UserJoinEvent() {
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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FdsWeb.Data;
using FdsWeb.Models;
using FdsWeb.ViewModels;

namespace FdsWeb.Controllers.Api {
    [Produces( "application/json" ), Route( "api/Events" )]
    public class EventsController : Controller {
        private readonly ApplicationDbContext _context;

        public EventsController( ApplicationDbContext context ) { _context = context; }

        public IQueryable<Event> GetSearchEvents(Search search) {
            var qq = _context.Events.Include( e => e.Schedule )
                .Include( e => e.EventType ).Include( e => e.ApplicationUser );

            var qr = qq.AsQueryable();

            if (search != null){
                if (search.Longitude != null && search.Radius != null){
                    double lat = double.Parse(search.Latitude, CultureInfo.InvariantCulture),
                        lon = double.Parse(search.Longitude, CultureInfo.InvariantCulture);

                    qr = qr.Where(o => GeoCoordinate.Distance(o.Latitude, o.Longitude, lat, lon) <= search.Radius);
                }

                if (search.EventTypeId != null && search.EventTypeId != -1){
                    qr = qr.Where(o => o.EventTypeId == search.EventTypeId);
                }

                if (search.Date != null){
                    qr = qr.Where(o => o.Schedule.Any(sd => sd.DateTime.Date == search.Date.Value.Date)); //uhm ok
                } else{
                    if (search.DateBegins != null && search.DateFinish == null)
                        qr = qr.Where(o => o.Schedule.Any(s => s.DateTime >= search.DateBegins));
                    else if (search.DateBegins == null && search.DateFinish != null)
                        qr = qr.Where(o => o.Schedule.Any(s => s.DateTime <= search.DateFinish));
                    else if (search.DateBegins != null && search.DateFinish != null)
                        qr = qr.Where(o => o.Schedule.Any(
                            s => s.DateTime >= search.DateBegins && s.DateTime <= search.DateFinish));
                }
            }
            return qr;
        }

        // GET: api/Events
        [HttpGet]
        public IEnumerable< Event > GetEvents() {
            return GetSearchEvents(null);
        }

        [HttpPost]
        public IEnumerable< Event > GetEvents( Search search ) {
            return GetSearchEvents( search );
        }

        // GET: api/Events/5
        [HttpGet( "{id}" )]
        public async Task< IActionResult > GetEvent( [FromRoute] int id ) {
            if( !ModelState.IsValid ) {
                return BadRequest( ModelState );
            }

            var @event = await _context.Events.SingleOrDefaultAsync( m => m.Id == id );

            if( @event == null ) {
                return NotFound();
            }

            return Ok( @event );
        }

        // POST: api/Events
        [HttpPost]
        public async Task< IActionResult > PostEvent( [FromBody] Event @event ) {
            if( !ModelState.IsValid ) {
                return BadRequest( ModelState );
            }

            _context.Events.Add( @event );
            await _context.SaveChangesAsync();

            return CreatedAtAction( "GetEvent", new {id = @event.Id}, @event );
        }

        private bool EventExists( int id ) { return _context.Events.Any( e => e.Id == id ); }
    }
}
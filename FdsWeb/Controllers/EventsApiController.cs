using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FdsWeb.Data;
using FdsWeb.Models;

namespace FdsWeb.Controllers{
    [Produces("application/json"), Route("api/EventsApi")]
    public class EventsApiController : Controller{
        private readonly ApplicationDbContext _context;

        public EventsApiController(ApplicationDbContext context){
            _context = context;
        }

        // GET: api/EventsApi
        [HttpGet]
        public IEnumerable<Event> GetEvents(){
            return _context.Events;
        }

        // GET: api/EventsApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent([FromRoute] int id){
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var @event = await _context.Events.SingleOrDefaultAsync(m => m.Id == id);

            if (@event == null){
                return NotFound();
            }

            return Ok(@event);
        }

        // PUT: api/EventsApi/5
        [HttpPut("{id}")]public async Task<IActionResult> PutEvent([FromRoute] int id, [FromBody] Event @event)
        {
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            if (id != @event.Id){
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;

            try{
                await _context.SaveChangesAsync();
            }catch (DbUpdateConcurrencyException){
                if (!EventExists(id)){
                    return NotFound();
                }else{
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EventsApi
        [HttpPost]
        public async Task<IActionResult> PostEvent([FromBody] Event @event){
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = @event.Id }, @event);
        }

        // DELETE: api/EventsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id){
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var @event = await _context.Events.SingleOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return Ok(@event);
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
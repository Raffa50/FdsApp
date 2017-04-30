using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fds.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FdsWeb.Data;
using FdsWeb.Models;
using Microsoft.AspNetCore.Identity;

namespace FdsWeb.Controllers.Api{
    [Produces("application/json"), Route("api/Login")]
    public class LoginController : Controller{
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager){
            _context = context;
            _signInManager = signInManager;
        }

        // GET: api/Login
        /*[HttpGet]
        public IEnumerable<ApplicationUser> GetApplicationUser()
        {
            return _context.ApplicationUser;
        }*/

        // POST: api/Login
        [HttpPost]
        public async Task<IActionResult> PostApplicationUser([FromBody] UserLogin model){
            if (!ModelState.IsValid){
                return BadRequest();
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user == null){
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest();
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, true, lockoutOnFailure: false);
            if (result.Succeeded){
                return Ok(user);
            }

            return BadRequest();
        }

        private bool ApplicationUserExists(string id){
            return _context.ApplicationUser.Any(e => e.Id == id);
        }
    }
}
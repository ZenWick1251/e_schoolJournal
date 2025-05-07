using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_journal.DTOs;
using e_journal.Interfaces;
using e_journal.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_journal.Controllers;

    [ApiController]
    [Route("api/[controller]")] 
    public class UsersContoller : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersContoller(IUserService userService)
        {
            _userService = userService;
        }

        private async Task<User?> GetRequesterAsync()
        {
            var userIdHeader = Request.Headers["UserId"].FirstOrDefault();
            if (!int.TryParse(userIdHeader, out var userId))
                return null;

            return await _userService.GetByIdAsync(userId);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var requester = await GetRequesterAsync();
            if (requester == null)
                return Unauthorized("Missing or invalid UserId");

            try
            {
                var result = await _userService.CreateUserAsync(dto, requester);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var requester = await GetRequesterAsync();
            if (requester == null)
                return Unauthorized("Missing or invalid UserId");

            try
            {
                var result = await _userService.GetAllUsersAsync(requester);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        [HttpPut("{id}/role")]
        public async Task<IActionResult> ChangeUserRole(int id, [FromBody] string newRole)
        {
            var requester = await GetRequesterAsync();
            if (requester == null)
                return Unauthorized("Missing or invalid UserId");

            try
            {
                var success = await _userService.ChangeUserRoleAsync(id, newRole, requester);
                if (!success)
                    return NotFound("User not found");

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

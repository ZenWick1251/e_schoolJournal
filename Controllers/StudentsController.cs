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
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly IUserService _userService;

    public StudentsController(IStudentService studentService, IUserService userService)
    {
        _studentService = studentService;
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
    public async Task<IActionResult> CreateStudent([FromBody] StudentDto dto)
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var result = await _studentService.CreateStudentAsync(dto, requester);
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
    public async Task<IActionResult> GetAllStudents()
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var result = await _studentService.GetAllStudentsAsync(requester);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentById(int id)
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var result = await _studentService.GetStudentByIdAsync(id, requester);
            if (result == null) return NotFound();
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var success = await _studentService.DeleteStudentAsync(id, requester);
            if (!success) return NotFound();

            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }
}

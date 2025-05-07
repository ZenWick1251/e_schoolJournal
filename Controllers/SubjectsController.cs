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
public class SubjectsController : ControllerBase
{
    private readonly ISubjectService _subjectService;
    private readonly IUserService _userService;

    public SubjectsController(ISubjectService subjectService, IUserService userService)
    {
        _subjectService = subjectService;
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
    public async Task<IActionResult> CreateSubject([FromBody] SubjectDto dto)
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var result = await _subjectService.CreateSubjectAsync(dto, requester);
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
    public async Task<IActionResult> GetAllSubjects()
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var result = await _subjectService.GetAllSubjectsAsync(requester);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSubjectById(int id)
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var result = await _subjectService.GetSubjectByIdAsync(id, requester);
            if (result == null) return NotFound();

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject(int id)
    {
        var requester = await GetRequesterAsync();
        if (requester == null) return Unauthorized();

        try
        {
            var success = await _subjectService.DeleteSubjectAsync(id, requester);
            if (!success) return NotFound();

            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FarmSystem.Core.Models;
using FarmSystem.Core.Interfaces;
using FarmSystem.Infrastructure.Repositories;
using FarmSystem.API.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class CowController : Controller
    {
        // public IActionResult Index()
        // {
        //     return View();
        // }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }

private readonly ILogger<CowController> _logger;
private readonly IEmailService _emailService;
         private readonly ICowRepository _cowRepo;

    public CowController(ICowRepository cowRepo, IEmailService emailService, ILogger<CowController> logger)
    {
        _cowRepo = cowRepo;
        _emailService = emailService;
        _logger = logger;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendTestEmail(string email, string subject, string body)
    {
        await _emailService.SendEmailAsync(email, subject, body);
        return Ok("Email sent successfully.");
    }

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _cowRepo.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var cow = await _cowRepo.GetByIdAsync(id);
        return cow == null ? NotFound() : Ok(cow);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Cow cow)
    {
        await _cowRepo.AddAsync(cow);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Cow cow)
    {
        cow.Id = id;
        await _cowRepo.UpdateAsync(cow);
        return Ok();
    }
    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch (int id, [FromBody] Cow cow){
        cow.Id = id;
        await _cowRepo.UpdateAsync(cow);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _cowRepo.DeleteAsync(id);
        return Ok();
    }
    }
}
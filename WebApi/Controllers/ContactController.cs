using Infrastructure.Entities.Contact;
using Infrastructure.Models.Dtos.Contact;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApi.Filters;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[UseApiKey]
public class ContactController : ControllerBase
{
    private readonly ContactService _contactService;
    private readonly ContactRepository _contactRepository;

    public ContactController(ContactService contactService, ContactRepository contactRepository)
    {
        _contactService = contactService;
        _contactRepository = contactRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateContactRequest(ContactDto contactDto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var result = await _contactService.CreateContactUsAsync(contactDto);
                if (result)
                {
                    return Created("", null);
                }
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);

        };
            return BadRequest("Something went wrong");
        //return Problem();
    }
}

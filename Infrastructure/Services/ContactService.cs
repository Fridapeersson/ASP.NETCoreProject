﻿using Infrastructure.Entities.Contact;
using Infrastructure.Models.Dtos.Contact;
using Infrastructure.Repositories;
using System.Diagnostics;

namespace Infrastructure.Services;

public class ContactService
{
    private readonly ContactRepository _contactRepository;
    private readonly HttpClient _http;

    public ContactService(ContactRepository contactRepository, HttpClient http)
    {
        _contactRepository = contactRepository;
        _http = http;
    }


    #region create ContactUs
    public async Task<bool> CreateContactUsAsync(ContactDto contactDto)
    {
        try
        {
            var newContactRequest = new ContactUsEntity
            {
                Name = contactDto.Name,
                Email = contactDto.Email,
                Message = contactDto.Message,
                Service = contactDto.SelectedService,
            };
            await _contactRepository.CreateOneAsync(newContactRequest);
            return true;

        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return false;
    }
    #endregion
}
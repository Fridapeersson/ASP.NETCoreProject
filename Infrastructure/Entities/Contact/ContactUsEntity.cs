﻿using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities.Contact;

public class ContactUsEntity
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Message { get; set; } = null!;
    public string? Service { get; set; }

}

using System;
using System.Collections.Generic;

namespace BlazingChat.Server.Models.Entities;

public partial class User
{
    public long UserId { get; set; }

    public string Source { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? SecondName { get; set; }

    public string? LastName { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public byte[]? DateOfBirth { get; set; }

    public string? AboutMe { get; set; }

    public bool Notifications { get; set; }

    public bool DarkTheme { get; set; }

    public byte[]? CreatedDate { get; set; }

    public virtual ICollection<Contact> ContactContactUsers { get; set; } = new List<Contact>();

    public virtual ICollection<Contact> ContactPrincipalUsers { get; set; } = new List<Contact>();

    public virtual ICollection<Email> Emails { get; set; } = new List<Email>();

    public virtual ICollection<Login> Logins { get; set; } = new List<Login>();

    public virtual ICollection<Message> MessageMessageFromNavigations { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageMessageToNavigations { get; set; } = new List<Message>();

    public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
}

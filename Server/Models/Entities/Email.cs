using System;
using System.Collections.Generic;

namespace BlazingChat.Server.Models.Entities;

public partial class Email
{
    public long EmailId { get; set; }

    public string EmailAddress { get; set; } = null!;

    public bool? HasPrincipal { get; set; } = null!;

    public long UserId { get; set; }

    public virtual User User { get; set; } = null!;
}

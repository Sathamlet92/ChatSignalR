using System;
using System.Collections.Generic;

namespace BlazingChat.Server.Models.Entities;

public partial class Contact
{
    public long ContactId { get; set; }

    public string ContactName { get; set; } = null!;

    public string? ContactLastName { get; set; }

    public long PrincipalUserId { get; set; }

    public long ContactUserId { get; set; }

    public byte[] HasConversation { get; set; } = null!;

    public virtual User ContactUser { get; set; } = null!;

    public virtual User PrincipalUser { get; set; } = null!;
}

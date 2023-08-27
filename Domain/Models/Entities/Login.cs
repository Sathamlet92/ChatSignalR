using System;
using System.Collections.Generic;

namespace BlazingChat.Domain.Models.Entites;

public partial class Login
{
    public long LoginId { get; set; }

    public long UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string EmailAddres { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

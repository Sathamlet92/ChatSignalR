using System;
using System.Collections.Generic;

namespace BlazingChat.Server.Models.Entities;

public partial class Phone
{
    public long PhoneId { get; set; }

    public long UserId { get; set; }

    public string AreaCode { get; set; } = null!;

    public string Phone1 { get; set; } = null!;

    public virtual AreaCode AreaCodeNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

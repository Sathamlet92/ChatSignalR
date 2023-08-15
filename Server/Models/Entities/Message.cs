using System;
using System.Collections.Generic;

namespace BlazingChat.Server.Models.Entities;

public partial class Message
{
    public long MessageId { get; set; }

    public long MessageFrom { get; set; }

    public long MessageTo { get; set; }

    public string Content { get; set; } = null!;

    public byte[] CreateAt { get; set; } = null!;

    public virtual User MessageFromNavigation { get; set; } = null!;

    public virtual User MessageToNavigation { get; set; } = null!;
}

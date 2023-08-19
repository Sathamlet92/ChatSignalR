using System;
using System.Collections.Generic;
namespace BlazingChat.Domain.Models.Entites;

public partial class AreaCode
{
    public string AreaCode1 { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public string Country { get; set; } = null!;

    public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
}

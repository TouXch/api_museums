using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace API_museum.Models;

public partial class TbTheme
{
    public int Idtheme { get; set; }

    public string? Name { get; set; }

    [JsonIgnore]
    public virtual ICollection<TbMuseum> TbMuseums { get; } = new List<TbMuseum>();
}

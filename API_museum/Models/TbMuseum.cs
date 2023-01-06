using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace API_museum.Models;

public partial class TbMuseum
{
    public int Idmuseum { get; set; }

    public string? Name { get; set; }

    public int? Idtheme { get; set; }

    public virtual TbTheme? oTheme { get; set; }

    [JsonIgnore]
    public virtual ICollection<TbArticle> TbArticles { get; } = new List<TbArticle>();
}

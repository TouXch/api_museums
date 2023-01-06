using System;
using System.Collections.Generic;

namespace API_museum.Models;

public partial class TbArticle
{
    public int Idarticle { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? Isdamaged { get; set; }

    public int? Idmuseum { get; set; }

    public virtual TbMuseum? oMuseum { get; set; }
}

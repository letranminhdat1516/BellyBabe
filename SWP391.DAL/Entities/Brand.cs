using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class Brand
{
    public int BrandId { get; set; }

    public string BrandName { get; set; } = null!;

    public string? Description { get; set; }

    public string? ImageBrand { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

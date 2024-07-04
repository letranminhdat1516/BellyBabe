using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class Blog
{
    public int BlogId { get; set; }

    public int? UserId { get; set; }

    public string? BlogContent { get; set; }

    public int? CategoryId { get; set; }

    public string? TitleName { get; set; }

    public DateTime? DateCreated { get; set; }

    public string? Image { get; set; }

    public virtual BlogCategory? Category { get; set; }

    public virtual User? User { get; set; }
}

using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class BlogCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public int? ParentCategoryId { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<BlogCategory> InverseParentCategory { get; set; } = new List<BlogCategory>();

    public virtual BlogCategory? ParentCategory { get; set; }
}

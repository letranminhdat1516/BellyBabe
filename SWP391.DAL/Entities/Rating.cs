using System;
using System.Collections.Generic;

namespace SWP391.DAL.Entities;

public partial class Rating
{
    public int RatingId { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public int? RatingValue { get; set; }

    public DateTime? RatingDate { get; set; }

    public int? RatingCategoryId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual RatingCategory? RatingCategory { get; set; }

    public virtual User? User { get; set; }
}

﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MealSync.Domain.Enums;

namespace MealSync.Domain.Entities;

[Table("customer")]
public class Customer : BaseEntity
{
    [Key]
    public long Id { get; set; }

    public CustomerStatus Status { get; set; } = CustomerStatus.Active;

    public virtual Account Account { get; set; }

    public virtual ICollection<CustomerBuilding> CustomerBuildings { get; set; } = new List<CustomerBuilding>();

    public virtual ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}

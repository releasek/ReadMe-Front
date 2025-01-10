﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ReadMe_Back.Models.EFModels;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TotalAmount { get; set; }

    public DateTime OrderDate { get; set; }

    public string OrderName { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User User { get; set; }
}
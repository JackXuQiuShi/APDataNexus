﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class Supplier
{
    public int Supplier_ID { get; set; }

    public string CompanyName { get; set; }

    public string ContactName { get; set; }

    public string ContactTitle { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string Region { get; set; }

    public string PostalCode { get; set; }

    public string Country { get; set; }

    public string Phone { get; set; }

    public string Fax { get; set; }

    public string HomePage { get; set; }

    public int? DiscountRate { get; set; }

    public string CellNumber { get; set; }

    public string GSTNumber { get; set; }

    public string BankInfo { get; set; }

    public bool? EFT_Flag { get; set; }

    public int? Payment_Supplier_ID { get; set; }

    public string Tel { get; set; }

    public string Email { get; set; }

    public string Addr { get; set; }

    public string Post_Code { get; set; }

    public string SafetyLicense { get; set; }

    public DateTime? Date { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
}
﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace APWeb.TempModels;

public partial class Store
{
    public string StoreName { get; set; }

    public string ContactName { get; set; }

    public string ContactTitle { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string Region { get; set; }

    public string PostalCode { get; set; }

    public string Country { get; set; }

    public string Phone { get; set; }

    public string Fax { get; set; }

    public int? BusinessNo { get; set; }

    public int NextPOID { get; set; }

    public string StoreFullName { get; set; }

    public int? NextBATCHID { get; set; }

    public string Active { get; set; }

    public string HostName { get; set; }

    public int? Store_Nbr { get; set; }

    public int Store_ID { get; set; }

    public int? CategoryID { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<StoreFloorLocation> StoreFloorLocations { get; set; } = new List<StoreFloorLocation>();

    public virtual ICollection<WarehouseStorageArea> WarehouseStorageAreas { get; set; } = new List<WarehouseStorageArea>();
}
using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class UserInfo
{
    public string Name { get; set; }

    public int Store { get; set; }

    public string Role { get; set; }

    public int Id { get; set; }

    public Guid Rowguid { get; set; }
}

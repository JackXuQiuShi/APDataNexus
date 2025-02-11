using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class UserAccount
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string PasswordHash { get; set; }

    public Guid Rowguid { get; set; }
}

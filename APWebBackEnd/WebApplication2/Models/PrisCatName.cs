using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class PrisCatName
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public string CategoryName { get; set; }

    public string Department { get; set; }

    public int? DeptCode { get; set; }

    public Guid Rowguid { get; set; }
}

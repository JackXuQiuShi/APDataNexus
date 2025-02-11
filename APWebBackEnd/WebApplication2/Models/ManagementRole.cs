using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class ManagementRole
{
    public int ManagementRoleId { get; set; }

    public string ManagementRoleName { get; set; }

    public int ManagementFunctionId { get; set; }

    public Guid Rowguid { get; set; }

    public virtual ICollection<DepartmentManagement> DepartmentManagements { get; set; } = new List<DepartmentManagement>();
}

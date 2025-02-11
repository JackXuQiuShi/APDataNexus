using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class DepartmentManagement
{
    public int StoreId { get; set; }

    public int DepartmentId { get; set; }

    public int EmployeeId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int ManagementRoleId { get; set; }

    public Guid Rowguid { get; set; }

    public virtual Department Department { get; set; }

    public virtual Employee Employee { get; set; }

    public virtual ManagementRole ManagementRole { get; set; }

    public virtual Store Store { get; set; }
}

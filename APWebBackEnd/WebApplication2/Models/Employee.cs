using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public byte[] SsmaTimeStamp { get; set; }

    public int? StoreId { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string FullName { get; set; }

    public int? Id { get; set; }

    public int? PayerId { get; set; }

    public Guid Rowguid { get; set; }

    public virtual ICollection<DepartmentManagement> DepartmentManagements { get; set; } = new List<DepartmentManagement>();

    public virtual ICollection<TblBonu> TblBonus { get; set; } = new List<TblBonu>();

    public virtual ICollection<TblPayroll> TblPayrolls { get; set; } = new List<TblPayroll>();
}

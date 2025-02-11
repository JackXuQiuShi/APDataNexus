using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class Permission
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool? Invoice { get; set; }

    public bool? Cheque { get; set; }

    public bool? PrintInvoiceCheque { get; set; }

    public bool? PrintPayrollCheque { get; set; }

    public bool? VoidInvoiceCheque { get; set; }

    public bool? VoidPayrollCheque { get; set; }

    public bool? Payroll { get; set; }

    public bool? Cashed { get; set; }

    public bool? Recipt { get; set; }

    public bool? PrintRecipt { get; set; }

    public bool? Disable { get; set; }

    public string Password { get; set; }

    public byte[] SsmaTimeStamp { get; set; }

    public bool? Bonus { get; set; }

    public bool? PrintBonusCheque { get; set; }

    public bool? VoidBonusCheque { get; set; }

    public bool? Supplier { get; set; }

    public bool? Eft { get; set; }

    public Guid Rowguid { get; set; }
}

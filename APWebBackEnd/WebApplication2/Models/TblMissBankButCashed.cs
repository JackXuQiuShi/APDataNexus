using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class TblMissBankButCashed
{
    public int? PayerId { get; set; }

    public int? ChequeNumber { get; set; }

    public DateTime? ChequeCashedDate { get; set; }

    public decimal? ChequeAmount { get; set; }

    public Guid Rowguid { get; set; }
}

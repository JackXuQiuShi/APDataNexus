using System;
using System.Collections.Generic;

namespace APWeb.Models;

public partial class Poscomputer
{
    public int ComputerId { get; set; }

    public string ComputerName { get; set; }

    public int StoreId { get; set; }

    public string ComputerDomainName { get; set; }

    public string InputFilePath { get; set; }

    public string Notes { get; set; }

    public Guid Rowguid { get; set; }
}

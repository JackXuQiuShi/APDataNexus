using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace APWeb.Models;

public partial class PurchaseCostChangeRequest
{
    public string ProductId { get; set; }

    public int CostType { get; set; }

    [Key]
    public int ChangeId { get; set; }

    public int StatusId { get; set; }

    public DateTime DraftDate { get; set; }

    public DateTime SubmitDate { get; set; }

    public DateTime CompleteDate { get; set; }

    public int DraftUserId { get; set; }

    public int SubmitUserId { get; set; }

    public int CompleteUserId { get; set; }

    public decimal UnitCostOld { get; set; }

    public decimal UnitCostNew { get; set; }
}

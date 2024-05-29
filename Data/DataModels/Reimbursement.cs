using System;
using System.Collections.Generic;

namespace Data.DataModels;

public partial class Reimbursement
{
    public int Id { get; set; }

    public int? Physicianid { get; set; }

    public string? Item { get; set; }

    public decimal? Amount { get; set; }

    public string? Bill { get; set; }

    public bool? Isdeleted { get; set; }

    public string? Createdby { get; set; }

    public string? Modifirdby { get; set; }

    public int? Biweektimeid { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Modifieddate { get; set; }

    public virtual Biweektime? Biweektime { get; set; }

    public virtual Physician? Physician { get; set; }
}

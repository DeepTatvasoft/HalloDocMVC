using System;
using System.Collections.Generic;

namespace Data.DataModels;

public partial class Timesheet
{
    public int Timesheetid { get; set; }

    public int? Physicianid { get; set; }

    public bool? Isweekend { get; set; }

    public decimal? Oncallhours { get; set; }

    public int? Housecall { get; set; }

    public int? Consult { get; set; }

    public string? Createdby { get; set; }

    public string? Modifiedby { get; set; }

    public int? Biweektimeid { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? Modifieddate { get; set; }

    public DateTime? Createddate { get; set; }

    public virtual Biweektime? Biweektime { get; set; }

    public virtual Physician? Physician { get; set; }
}

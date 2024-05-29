using System;
using System.Collections.Generic;

namespace Data.DataModels;

public partial class Biweektime
{
    public int Biweekid { get; set; }

    public int? Physicianid { get; set; }

    public bool? Isfinalized { get; set; }

    public DateTime? Firstday { get; set; }

    public DateTime? Lastday { get; set; }

    public bool? Isapproved { get; set; }

    public virtual ICollection<Reimbursement> Reimbursements { get; set; } = new List<Reimbursement>();

    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
}

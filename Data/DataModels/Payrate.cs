using System;
using System.Collections.Generic;

namespace Data.DataModels;

public partial class Payrate
{
    public long Payrateid { get; set; }

    public int? Physicinaid { get; set; }

    public int? Nightshift { get; set; }

    public int? Shift { get; set; }

    public int? Housecall { get; set; }

    public int? Nighthousecall { get; set; }

    public int? Consult { get; set; }

    public int? Nightconsult { get; set; }

    public int? Batchtesting { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime? Modifieddate { get; set; }

    public virtual Physician? Physicina { get; set; }
}

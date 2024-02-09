using System;
using System.Collections.Generic;

namespace HalloDoc.Models;

public partial class Physicianlocation
{
    public int Locationid { get; set; }

    public int Physicianid { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public DateTime? Createddate { get; set; }

    public string? Physicianname { get; set; }

    public string? Address { get; set; }
}

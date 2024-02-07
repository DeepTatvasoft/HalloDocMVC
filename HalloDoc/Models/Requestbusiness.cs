using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Models;

[Table("requestbusiness")]
public partial class Requestbusiness
{
    [Key]
    [Column("requestbusinessid")]
    public int Requestbusinessid { get; set; }

    [Column("requestid")]
    public int Requestid { get; set; }

    [Column("businessid")]
    public int Businessid { get; set; }

    [Column("ip")]
    [StringLength(20)]
    public string? Ip { get; set; }
}

using System;
using System.Collections.Generic;

namespace Data.DataModels;

public partial class Chathistory
{
    public int Msgid { get; set; }

    public string Msg { get; set; } = null!;

    public string Sender { get; set; } = null!;

    public string Reciever { get; set; } = null!;

    public bool Isread { get; set; }

    public DateTime Senttime { get; set; }

    public DateTime Readtime { get; set; }

    public bool Issent { get; set; }

    public DateTime Recievetime { get; set; }
}

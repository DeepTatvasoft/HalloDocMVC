using Data.DataModels;

namespace Services.ViewModels
{
    public class NewStateData
    {
        public List<Request> req { get; set; }
        public List<Requestclient> requestclients { get; set; }

        public int newcount { get; set; }
        public int pendingcount { get; set; }
        public int activecount { get; set; }
        public int concludecount { get; set; }
        public int Toclosecount { get; set; }
        public int Unpaidcount { get; set; }
    }
    public class NewStateData1
    {
        public Request req { get; set; }
        public DateTime DateOnly { get; set; }
        public string region { get; set; }
        public string address { get; set; }
        public string room { get; set; }
        public string symptoms { get; set; }
    }
}

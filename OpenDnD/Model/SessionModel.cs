namespace OpenDnD.Model
{
    public class SessionModel
    {
        public Guid SessionId { get; set; }
        public string SessionName { get; set; }
        public List<Guid> PlayersIds { get; set; }
        public byte[] Image { get; set; }
    }
}

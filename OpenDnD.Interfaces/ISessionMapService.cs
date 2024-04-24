namespace OpenDnD.Interfaces
{
    public class SessionMap
    {

    }
    public class SessionMapRequest
    {
        public string SessionName { get; set; }
    }
    public interface ISessionMapService : ICRUDService<SessionMap, SessionMapRequest> {
        

    }

}

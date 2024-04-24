namespace OpenDnD.Interfaces
{
    public class SessionMapEntity
    {

    }
    public class SessionMapEntityRequest
    {

    }
    public interface ISessionMapEntityService : ICRUDService<SessionMapEntity, SessionMapEntityRequest> {
        delegate void SessionMapEntityChanged(SessionMapEntity sessionMapEntity);
        event SessionMapEntityChanged OnSessionMapEntityChanged;
    }

}

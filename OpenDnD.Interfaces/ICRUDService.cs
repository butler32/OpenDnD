﻿namespace OpenDnD.Interfaces
{
    public interface ICRUDService<T, TRequest>
    {
        public Guid CreateSession(AuthToken authToken, TRequest request);
        public void UpdateSession(AuthToken authToken, Guid id, TRequest request);
        public void DeleteSession(AuthToken authToken, Guid id);
        public T GetSession(AuthToken authToken, Guid id);
        public List<T> GetSessionList(AuthToken authToken);
    }

}

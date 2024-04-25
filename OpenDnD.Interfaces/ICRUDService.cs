namespace OpenDnD.Interfaces
{
    public interface ICRUDService<T, TRequest>
    {
        public Guid Create(AuthToken authToken, TRequest request);
        public void Update(AuthToken authToken, Guid id, TRequest request);
        public void Delete(AuthToken authToken, Guid id);
        public T Get(AuthToken authToken, Guid id);
        public List<T> GetList(AuthToken authToken);
    }

}

namespace Starter.Net.Api.Common
{
    public interface ICrud<T>
    {
        T Create(T item);
        void Update(string id, T item);
        void Remove(string id);
        T Find(string id);
    }
}

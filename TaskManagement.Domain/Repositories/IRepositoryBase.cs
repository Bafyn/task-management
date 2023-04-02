namespace TaskManagement.Domain.Repositories;

public interface IRepositoryBase<T> where T : class
{
    void Create(T entity);

    void Update(T entity);

    void Delete(T entity);
}

using Rinha.API.Core.Models;

namespace Rinha.API.Core.Interfaces;

public interface IRepository<T> where T : class
{
    Task<string> Create(T person);
    Task<T?> Read(string id);
    Task<IReadOnlyCollection<T>> Search(string term);
    Task<int> Count();
}
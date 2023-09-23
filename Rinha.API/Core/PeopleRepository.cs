using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Rinha.API.Core.Interfaces;
using Rinha.API.Core.Models;

namespace Rinha.API.Core;

public class PeopleRepository : IRepository<Person>
{
    private readonly IMongoCollection<Person> _collection;
    
    public PeopleRepository(IMongoClient client)
    {
        var database = client.GetDatabase("rinha");
        _collection = database.GetCollection<Person>("pessoas");
        if (_collection == null) database.CreateCollection("pessoas");
    }
    
    public async Task<string> Create(Person person)
    {
        if (Exists(person)) return string.Empty;
        
        await _collection.InsertOneAsync(person);
        return person.Id;
    }

    public async Task<Person?> Read(string id)
    {
        var person = await _collection.FindAsync(p => p.Id == id);
        return person.FirstOrDefault();
    }
    
    public async Task<IReadOnlyCollection<Person>> Search(string term)
    {
        // the search must be case insensitive and made in the fields "apelido" and "nome" and "stack"
        //
        var regexFilter = new BsonRegularExpression(term, "i");
        var filter = Builders<Person>.Filter.Or(
            Builders<Person>.Filter.Regex(p => p.Nickname, regexFilter),
            Builders<Person>.Filter.Regex(p => p.Name, regexFilter),
            Builders<Person>.Filter.Regex(p => p.Stack, regexFilter)
        );
        
        var people = await _collection.FindAsync(filter);
        return people.ToList();
    }

    public Task<int> Count() => _collection.AsQueryable().CountAsync();

    private bool Exists(Person person)
    {
        var filter = Builders<Person>.Filter.Eq(p => p.Nickname, person.Nickname);
        var people = _collection.Find(filter);
        return people.Any();
    }
}
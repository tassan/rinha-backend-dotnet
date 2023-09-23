using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rinha.API.Core.Models;

public class Person
{
    private Person() => Id = ObjectId.GenerateNewId().ToString()!;

    public Person(string nickname, string name, string birth, string[] stack) : this()
    {
        Nickname = nickname;
        Name = name;
        Birth = birth;
        Stack = stack;
    }

    [BsonElement("id"), BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("apelido")]
    public string Nickname { get; set; }
    
    [BsonElement("nome")]
    public string Name { get; set; }
    
    [BsonElement("nascimento")]
    public string Birth { get; set; }
    
    [BsonElement("stack")]
    public string[] Stack { get; set; }
}
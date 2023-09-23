using MongoDB.Driver;
using Rinha.API.Core;
using Rinha.API.Core.Interfaces;
using Rinha.API.Core.Models;
using Rinha.API.Core.Translators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddSingleton<IMongoClient, MongoClient>(_ =>
    new MongoClient(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? "mongodb://localhost:27017"));
builder.Services.AddSingleton<IRepository<Person>, PeopleRepository>();
builder.Services.AddSingleton<ITranslator<PersonRequest, Person>, PersonReqTranslator>();

var app = builder.Build();

// POST /pessoas { "apelido": "[:apelido]", "nome": "[:nome]", "nascimento": "[:nascimento]", "stack": [[:stack]] }
app.MapPost("/pessoas",
    async (IRepository<Person> repository, ITranslator<PersonRequest, Person> translator,
        PersonRequest personRequest) =>
    {
        if (!personRequest.IsValid()) return Results.BadRequest();
        var person = translator.Translate(personRequest);
        var id = await repository.Create(person);

        return string.IsNullOrWhiteSpace(id) ? Results.UnprocessableEntity() : Results.Created($"/pessoas/{id}", id);
    });

// GET /pessoas/[:id]
app.MapGet("/pessoas/{id}", async (string id, IRepository<Person> repository) =>
{
    var person = await repository.Read(id);
    return person == null ? Results.NotFound() : Results.Ok(person);
});

// GET /pessoas?t=[:termo da busca]
app.MapGet("/pessoas", async (string t, IRepository<Person> repository) =>
{
    if (string.IsNullOrWhiteSpace(t)) return Results.BadRequest();
    var people = await repository.Search(t);
    return Results.Ok(people);
});

// GET /contagem-pessoas
app.MapGet("/contagem-pessoas", async (IRepository<Person> repository) => await repository.Count());

app.UseCors(o =>
{
    o.AllowAnyOrigin();
    o.AllowAnyMethod();
    o.AllowAnyHeader();
});

app.Run();
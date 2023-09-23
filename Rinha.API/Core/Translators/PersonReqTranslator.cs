using Rinha.API.Core.Interfaces;
using Rinha.API.Core.Models;

namespace Rinha.API.Core.Translators;

public class PersonReqTranslator : ITranslator<PersonRequest, Person>
{
    public Person Translate(PersonRequest input) => new(input.Nickname, input.Name, input.Birth, input.Stack);
}
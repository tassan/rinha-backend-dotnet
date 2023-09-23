using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rinha.API.Core.Models;

public class PersonRequest
{
    [JsonPropertyName("apelido"), MaxLength(32)]
    public string Nickname { get; set; }

    [JsonPropertyName("nome"), MaxLength(100)]
    public string Name { get; set; }

    [JsonPropertyName("nascimento"), MaxLength(10)]
    public string Birth { get; set; }

    [JsonPropertyName("stack")] public string[] Stack { get; set; }

    public bool IsValid()
    {
        // Validate if Nickname is not null or empty and MaxLength is 32
        if (string.IsNullOrEmpty(Nickname) || Nickname.Length > 32) return false;

        // Validate if Name is not null or empty and MaxLength is 100
        if (string.IsNullOrEmpty(Name) || Name.Length > 100) return false;

        // Validate if Birth is not null or empty and MaxLength is 10 and format is AAAA-MM-DD
        if (string.IsNullOrEmpty(Birth) || Birth.Length > 10 || !DateTime.TryParse(Birth, out _)) return false;
        
        // Validate if Stack is not null Then each element must have max length of 32
        if (Stack.Any(s => s.Length > 32)) return false;
        
        // If all validations passed, return true
        return true;
    }
}
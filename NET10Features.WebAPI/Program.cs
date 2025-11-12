using Scalar.AspNetCore;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

#region Field-Backed Properties (field keyword)
app.MapGet("field-backed", () =>
{
    FieldBackedClass fieldBackedClass = new();
    fieldBackedClass.Message = "Message";

    return Results.Ok();
});
#endregion

#region Extension Members
app.MapGet("extension-members", () =>
{
    // Kullaným
    string message = "Hello from C# 14";
    Console.WriteLine(message.WordCount);      // 4
    Console.WriteLine(message.IsEmpty);        // false
    Console.WriteLine(message.Truncate(10));   // Hello from...
    Console.WriteLine(message.Reverse());      // 41# C morf olleH
    return Results.Ok();
});
#endregion

#region Null-Conditional Member Access on Assignment
app.MapGet("null-conditional", () =>
{
    NullConditional nullConditional = new();
    nullConditional?.Num += 1;
    Console.WriteLine(nullConditional?.Num);

    return Results.Ok();
});
#endregion

#region Enhanced Form Validation
app.MapPost("/enhanced-from-validation",
    ([EmailAddress] string email, [Required] string name) =>
    {
        return TypedResults.Ok(new { email, name });
    });
#endregion

app.Run();

#region Field-Backed Properties (field keyword)
public class FieldBackedClass
{
    //C# 13 ve öncesi
    //private string _message;
    //public string Message
    //{
    //    get => _message;
    //    set => _message = value ?? throw new ArgumentNullException(nameof(value));
    //}

    //C# 14
    public string? Message
    {
        get; // Compiler tarafýndan otomatik oluþturulur
        set => field = value ?? throw new ArgumentNullException(nameof(value));
    }
}
#endregion

#region Extension Members
public static class StringExtensions
{
    extension(string source)
    {
        // Extension property
        public int WordCount
            => source.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

        // Extension property
        public bool IsEmpty => string.IsNullOrWhiteSpace(source);

        // Extension method
        public string Truncate(int maxLength)
        {
            if (source.Length <= maxLength)
                return source;
            return source.Substring(0, maxLength) + "...";
        }

        // Extension method
        public string Reverse()
        {
            var chars = source.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }
    }
}
#endregion

#region Null-Conditional
public class NullConditional
{
    public int? Num { get; set; } = null;
}
#endregion
namespace Infrastructure.Extensions;

using System.Text.Json;


public static class JsonElementExtensions
{
    public static Dictionary<string, string?> ToDictionary(this JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.Object)
            throw new InvalidOperationException("JsonElement is not an object.");
        
        var dict = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        foreach (var prop in element.EnumerateObject())
        {
            dict[prop.Name] = prop.Value.ValueKind switch
            {
                JsonValueKind.String  => prop.Value.GetString(),
                JsonValueKind.Number  => prop.Value.GetRawText(),
                JsonValueKind.True or JsonValueKind.False   => prop.Value.GetRawText(),
                JsonValueKind.Null    => null,
                _                     => prop.Value.GetRawText()
            };
        }
        return dict;
    }
}
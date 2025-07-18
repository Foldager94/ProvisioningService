using System.Collections;

namespace Domain.Models;

public class ProvisioningParameters : IEnumerable<KeyValuePair<string, string?>>
{
    private readonly Dictionary<string, string?> _parameters;
    
    public ProvisioningParameters(IEnumerable<string> keys)
    {
        _parameters = keys.ToDictionary(k => k, string? (k) => null);
    }

    public void Set(string key, string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (!_parameters.ContainsKey(key))
            throw new InvalidOperationException($"'{key}' is not allowed.");

        _parameters[key] = value;
    }
    
    public bool IsValid() => _parameters.All(kvp => kvp.Value != null);
    
    public IEnumerator<KeyValuePair<string, string?>> GetEnumerator() => _parameters.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _parameters.GetEnumerator();
}
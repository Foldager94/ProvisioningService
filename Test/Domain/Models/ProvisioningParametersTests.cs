using Domain.Models;

namespace Test.Domain.Models;

[TestFixture]
public class ProvisioningParametersTests
{
    [Test]
    public void Constructor_InitializesAllKeysWithNull()
    {
        var keys = new[] { "url", "title", "owner" };
        var parameters = new ProvisioningParameters(keys);

        foreach (var key in keys)
        {
            Assert.That(parameters.Any(kvp => kvp.Key == key && kvp.Value == null), Is.EqualTo(true));
        }
    }

    [Test]
    public void Set_SetsValue_WhenKeyIsAllowed()
    {
        var parameters = new ProvisioningParameters(new[] { "url" });
        parameters.Set("url", "https://example.com");

        Assert.That(parameters.First(kvp => kvp.Key == "url").Value, Is.EqualTo("https://example.com"));
    }

    [Test]
    public void Set_Throws_WhenKeyIsNotAllowed()
    {
        var parameters = new ProvisioningParameters(new[] { "title" });

        Assert.Throws<InvalidOperationException>(() =>
        {
            parameters.Set("url", "https://fail.com");
        });
    }

    [Test]
    public void Set_Throws_WhenValueIsNull()
    {
        var parameters = new ProvisioningParameters(new[] { "title" });

        Assert.Throws<ArgumentNullException>(() =>
        {
            parameters.Set("title", null!);
        });
    }

    [Test]
    public void IsValid_ReturnsFalse_WhenAnyValueIsNull()
    {
        var parameters = new ProvisioningParameters(new[] { "url", "title" });
        parameters.Set("url", "https://example.com");

        Assert.That(parameters.IsValid(), Is.EqualTo(false));
    }

    [Test]
    public void IsValid_ReturnsTrue_WhenAllValuesAreSet()
    {
        var parameters = new ProvisioningParameters(new[] { "url", "title" });
        parameters.Set("url", "https://example.com");
        parameters.Set("title", "My Site");

        Assert.That(parameters.IsValid(), Is.EqualTo(true));
    }
}
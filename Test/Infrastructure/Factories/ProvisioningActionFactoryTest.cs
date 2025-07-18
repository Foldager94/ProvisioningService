using System.Text.Json;
using Domain.Models.Actions;
using Infrastructure.Factories;

namespace Test.Infrastructure.Factories
{
    [TestFixture]
    public class ProvisioningActionFactoryTests
    {
        private ProvisioningActionFactory _factory = null!;

        [SetUp]
        public void SetUp()
        {
            _factory = new ProvisioningActionFactory();
        }

        [Test]
        public void CreateSpoSiteAction_IsBuiltCorrectly()
        {
            // Arrange
            var json = @"
            {
                ""action"": ""CreateSpoSite"",
                ""type"": ""Communication""
            }";
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // Act
            var action = _factory.Create(root);

            // Assert
            Assert.That(action, Is.InstanceOf<CreateSpoSiteAction>());
            var create = (CreateSpoSiteAction)action;
            Assert.That(create.Type, Is.EqualTo("Communication"));
        }

        [Test]
        public void ApplyTemplateAction_BuildsWithCorrectParameters()
        {
            // Arrange
            var templateId = Guid.NewGuid();
            var required = new[] { "A", "B", "C" };
            var optional = new[] { "X", "Y" };
            var json = $@"
            {{
                ""action"": ""ApplyTemplate"",
                ""templateId"": ""{templateId}"",
                ""required"": [ {string.Join(',', required.Select(s => $"\"{s}\""))} ],
                ""optional"": [ {string.Join(',', optional.Select(s => $"\"{s}\""))} ]
            }}";
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            
            var action = _factory.Create(root);
            
            Assert.That(action, Is.InstanceOf<ApplyTemplateAction>());
            var apply = (ApplyTemplateAction)action;
            
            Assert.That(apply.TemplateId, Is.EqualTo(templateId));
            
            var requiredKeys    = apply.Required.Select(kvp => kvp.Key).ToArray();
            var optionalKeys    = apply.Optional.Select(kvp => kvp.Key).ToArray();

            Assert.That(requiredKeys, Is.EquivalentTo(required));
            Assert.That(optionalKeys, Is.EquivalentTo(optional));
            
            Assert.That(apply.Required.All(kvp => kvp.Value is null), Is.True);
            Assert.That(apply.Optional.All(kvp => kvp.Value is null), Is.True);
        }
    }
}

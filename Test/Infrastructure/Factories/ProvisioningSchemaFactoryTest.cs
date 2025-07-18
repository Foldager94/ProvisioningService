using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Application.Factories;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.Actions;
using Infrastructure.Factories;
using NUnit.Framework;

namespace Test.Infrastructure.Factories
{
    // Dummy-action til test
    public class DummyAction : IProvisioningAction
    {
        public string ActionType { get; }
        public ProvisioningParameters Required { get; }
        public ProvisioningParameters Optional { get; }
        public Dictionary<string, object?> Parameters { get; }
        public DummyAction(string actionType)
        {
            ActionType = actionType;
            Parameters = new Dictionary<string, object?>();
        }
        public void Validate() { /* ingen validering i stub */ }
    }

    // Stub af IProvisioningActionFactory
    public class StubActionFactory : IProvisioningActionFactory
    {
        public IProvisioningAction Create(JsonElement root)
        {
            // Returnér en DummyAction med ActionType = root["action"]
            var actionType = root.GetProperty("action").GetString()!;
            return new DummyAction(actionType);
        }
    }

    [TestFixture]
    public class ProvisioningSchemaFactoryTests
    {
        private ProvisioningSchemaFactory _factory = null!;

        [SetUp]
        public void SetUp()
        {
            // Inject stubben
            _factory = new ProvisioningSchemaFactory(new StubActionFactory());
        }

        [Test]
        public void Create_SingleActionFlow_ReturnsCorrectSchema()
        {
            // Arrange: id + et enkelt flow-item
            var id = Guid.NewGuid();
            var json = $@"
            {{
                ""id"": ""{id}"",
                ""flow"": [
                  {{ ""action"": ""FooAction"" }}
                ]
            }}";
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // Act
            var schema = _factory.Create(root);

            // Assert
            Assert.That(schema.Id, Is.EqualTo(id));
            Assert.That(schema.Actions.ToList(), Has.Count.EqualTo(1));
            Assert.That(schema.Actions.First().ActionType, Is.EqualTo("FooAction"));
        }

        [Test]
        public void Create_MultipleActionsFlow_ReturnsAllActions()
        {
            // Arrange: id + tre flow-items
            var id = Guid.NewGuid();
            var actions = new[] { "A1", "A2", "A3" };
            var json = $@"
            {{
                ""id"": ""{id}"",
                ""flow"": [
                  {string.Join(",\n", actions.Select(a => $"{{ \"action\": \"{a}\" }}"))}
                ]
            }}";
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // Act
            var schema = _factory.Create(root);

            // Assert
            Assert.That(schema.Id, Is.EqualTo(id));
            var returnedTypes = schema.Actions.Select(a => a.ActionType).ToArray();
            Assert.That(returnedTypes, Is.EqualTo(actions));
        }

        [Test]
        public void Create_UnsupportedJson_ThrowsOnMissingFields()
        {
            // Arrange: mangler "id"
            var json = @"{ ""flow"": [] }";
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _factory.Create(root));
        }
    }
}

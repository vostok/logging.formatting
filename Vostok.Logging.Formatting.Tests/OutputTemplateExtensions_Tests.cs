using System;
using FluentAssertions;
using NUnit.Framework;

namespace Vostok.Logging.Formatting.Tests;

[TestFixture]
internal class OutputTemplateExtensions_Tests
{
    [Test]
    public void WithPropertyAfter_should_insert_property()
    {
        var template1 = new OutputTemplateBuilder()
            .AddTimestamp()
            .AddMessage()
            .Build();

        template1.ToString().Should().Be("{Timestamp}{Message}");
        
        var template2 = template1.WithPropertyAfter(WellKnownTokens.Timestamp, "Prop");

        template2.Tokens.Count.Should().Be(3);
        template2.ToString().Should().Be("{Timestamp}{Prop}{Message}");
    }
    
    [Test]
    public void WithPropertyAfter_should_insert_properties_chain()
    {
        var template1 = new OutputTemplateBuilder()
            .AddTimestamp()
            .AddMessage()
            .Build();

        template1.ToString().Should().Be("{Timestamp}{Message}");
        
        var template2 = template1
            .WithPropertyAfter(WellKnownTokens.Timestamp, "Prop1")
            .WithPropertyAfter("Prop1", "Prop2");

        template2.Tokens.Count.Should().Be(4);
        template2.ToString().Should().Be("{Timestamp}{Prop1}{Prop2}{Message}");
    }
    
    [Test]
    public void WithPropertyAfter_should_insert_property_with_format()
    {
        var template1 = new OutputTemplateBuilder()
            .AddTimestamp("w")
            .AddMessage()
            .Build();

        template1.ToString().Should().Be("{Timestamp:w}{Message}");
        
        var template2 = template1.WithPropertyAfter(WellKnownTokens.Timestamp, "Prop", "W");

        template2.Tokens.Count.Should().Be(3);
        template2.ToString().Should().Be("{Timestamp:w}{Prop:W}{Message}");
    }
    
    [Test]
    public void WithPropertyAfter_should_be_pure()
    {
        var template1 = new OutputTemplateBuilder()
            .AddTimestamp()
            .AddMessage()
            .Build();

        template1.ToString().Should().Be("{Timestamp}{Message}");
        
        var template2 = template1.WithPropertyAfter(WellKnownTokens.Timestamp, "Prop");

        template2.Tokens.Count.Should().Be(3);
        template2.ToString().Should().Be("{Timestamp}{Prop}{Message}");
        
        template1.ToString().Should().Be("{Timestamp}{Message}");
    }
    
    [Test]
    public void WithPropertyAfter_should_insert_property_at_the_beginning()
    {
        var template1 = new OutputTemplateBuilder()
            .AddTimestamp()
            .AddMessage()
            .Build();

        template1.ToString().Should().Be("{Timestamp}{Message}");
        
        var template2 = template1.WithPropertyAfter(null, "Prop");

        template2.Tokens.Count.Should().Be(3);
        template2.ToString().Should().Be("{Prop}{Timestamp}{Message}");
    }
    
    [Test]
    public void WithPropertyAfter_should_throw_if_no_such_token()
    {
        var template1 = new OutputTemplateBuilder()
            .AddMessage()
            .Build();

        template1.ToString().Should().Be("{Message}");
        
        // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
        Action check = () => template1.WithPropertyAfter(WellKnownTokens.Timestamp, "Prop");

        check.Should().Throw<InvalidOperationException>();
    }
}
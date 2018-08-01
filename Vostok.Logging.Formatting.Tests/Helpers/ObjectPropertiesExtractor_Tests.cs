﻿using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Formatting.Helpers;

namespace Vostok.Logging.Formatting.Tests.Helpers
{
    [TestFixture]
    internal class ObjectPropertiesExtractor_Tests
    {
        [Test]
        public void Should_extract_properties_from_an_anonymous_object()
        {
            ObjectPropertiesExtractor.ExtractProperties(new {A = 1, B = 2})
                .Should()
                .BeEquivalentTo(("A", 1), ("B", 2));
        }

        [Test]
        public void Should_extract_properties_from_a_custom_object()
        {
            ObjectPropertiesExtractor.ExtractProperties(new Container())
                .Should()
                .BeEquivalentTo(("A", 1), ("B", 2));
        }

        [Test]
        public void Should_not_extract_private_properties()
        {
            ObjectPropertiesExtractor.ExtractProperties(new PrivateProperty())
                .Should().BeEmpty();
        }

        [Test]
        public void Should_not_extract_public_fields()
        {
            ObjectPropertiesExtractor.ExtractProperties(new PublicField())
                .Should().BeEmpty();
        }

        [Test]
        public void Should_not_extract_private_fields()
        {
            ObjectPropertiesExtractor.ExtractProperties(new PrivateField())
                .Should().BeEmpty();
        }

        private class Container
        {
            public int A => 1;

            public int B => 2;
        }

        private class PrivateProperty
        {
            private int A => 1;
        }

        private class PublicField
        {
            public int A = 1;
        }

        private class PrivateField
        {
            private int A = 1;
        }
    }
}
﻿using System.IO;
using Xunit;

namespace HandlebarsDotNet.Test
{
    public class HtmlEncoderTests
    {
        private readonly HtmlEncoder _htmlEncoder;

        public HtmlEncoderTests()
        {
            _htmlEncoder = new HtmlEncoder();
        }

        [Theory]
        // Escape chars based on https://github.com/handlebars-lang/handlebars.js/blob/master/lib/handlebars/utils.js
        [InlineData("&", "&amp;")]
        [InlineData("<", "&lt;")]
        [InlineData(">", "&gt;")]
        [InlineData("\"", "&quot;")]
        [InlineData("'", "&#x27;")]
        [InlineData("`", "&#x60;")]
        [InlineData("=", "&#x3D;")]

        // Don't escape.
        [InlineData("â", "â")]
        public void EscapeCorrectCharacters(string input, string expected)
        {
            using var writer = new StringWriter();

            _htmlEncoder.Encode(input, writer);

            Assert.Equal(expected, writer.ToString());
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, "")]
        [InlineData(" ", " ")]
        [InlineData("&", "&amp;")]
        [InlineData("<", "&lt;")]
        [InlineData(">", "&gt;")]
        [InlineData("  >  ", "  &gt;  ")]
        [InlineData("\"", "&quot;")]
        [InlineData("&a&", "&amp;a&amp;")]
        [InlineData("a&a", "a&amp;a")]
        public void EncodeTest(string input, string expected)
        {
            // Arrange
            using var writer = new StringWriter();

            // Act
            _htmlEncoder.Encode(input, writer);

            // Assert
            Assert.Equal(expected, writer.ToString());
        }
    }
}

using System;
using Xunit;
using PSWordCloud;
using System.Management.Automation;
using System.Collections;
using System.Collections.Generic;

namespace Cmdlet.Tests
{
    public class WCUtilsTests
    {
        [Theory]
        [InlineData(0.0, 0.0)]
        [InlineData((float)Math.PI, 180.0)]
        [InlineData((float)Math.PI / 2.0, 90.0)]
        public void Test_ToRadians(float expected, float degrees)
        {
            Assert.Equal(expected, degrees.ToRadians());
        }

        [Theory]
        [InlineData("Red", 0xff, 0x00, 0x00)]
        [InlineData("Blue", 0x00, 0x00, 0xff)]
        public void Test_GetColorByName(string colorName, byte red, byte green, byte blue)
        {
            // act
            var color = WCUtils.GetColorByName(colorName);

            // assertions
            Assert.Equal(red, color.Red);
            Assert.Equal(green, color.Green);
            Assert.Equal(blue, color.Blue);
        }

        [Theory]
        [InlineData("notacolor")]
        [InlineData("")]
        [InlineData("red")]
        public void Test_GetColorByName_InvalidName(string colorName)
        {
            Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => WCUtils.GetColorByName(colorName));
        }

        [Fact]
        public void Test_GetColorByName_NullName()
        {
            Assert.Throws<System.ArgumentNullException>(() => WCUtils.GetColorByName(null));
        }


        [Fact]
        public void Test_GetValue_FromSimpleDictionary()
        {
            // arrange
            var dict = new Dictionary<string, int>() { { "a", 1 }, { "b", 2 } };

            // assert
            Assert.Equal(1, dict.GetValue("a"));
            Assert.Equal(2, dict.GetValue("b"));
            Assert.Null(dict.GetValue("keynotfound"));
            Assert.Null(dict.GetValue(""));
        }

        [Fact]
        public void Test_GetValue_FromSimpleDictionary_NullKey()
        {
            // arrange
            var dict = new Dictionary<string, int>() { { "a", 1 }, { "b", 2 } };

            // assert
            Assert.Throws<System.ArgumentNullException>(() => dict.GetValue(null));
        }

        [Fact]
        public void Test_GetValue_FromSimpleDictionaryWithoutStringKeys()
        {
            // arrange
            var dict = new Dictionary<int, int>() { { 2, 1 }, { 3, 2 } };

            // assert
            Assert.Throws<ArgumentException>(() => dict.GetValue("a"));
        }

        [Fact]
        public void Test_GetValue_FromDynamicDictionary()
        {
            // arrange
            var dict = new Dictionary<string, dynamic>() { { "a", new { ThisKey = 2} }, { "b", new { ThatKey = 3 } } };

            // act
            dynamic a = dict.GetValue("a");
            dynamic b = dict.GetValue("b");

            // assert
            Assert.NotNull(a);
            Assert.Equal(2, a.ThisKey);
            Assert.NotNull(b);
            Assert.Equal(3, b.ThatKey);
        }

        [Fact]
        public void Test_GetValue_FromDynamicDictionary_NullKey()
        {
            // arrange
            var dict = new Dictionary<string, dynamic>() { { "a", new { ThisKey = 2 } }, { "b", new { ThatKey = 3 } } };

            // assert
            Assert.Throws<System.ArgumentNullException>(() => dict.GetValue(null));
        }

        [Fact]
        public void Test_GetValue_NonDictionary()
        {
            // arrange
            var list = new List<string>() { "a", "b" };

            // assert
            Assert.Throws<System.ArgumentException>(() => list.GetValue("a"));
        }

        [Fact]
        public void Test_GetValue_NullObject()
        {
            // assert
            Assert.Throws<System.ArgumentException>(() => ((IEnumerable)null).GetValue("a"));
            Assert.Throws<System.ArgumentException>(() => ((IDictionary<string,object>)null).GetValue("a"));
        }

    }
}

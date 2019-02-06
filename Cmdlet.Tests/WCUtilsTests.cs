using System;
using Xunit;
using PSWordCloud;
using System.Management.Automation;

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
        public void Test_GetColorByName_InvalidName(string colorName) {
            Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => WCUtils.GetColorByName(colorName));
        }

        [Fact]
        public void Test_GetColorByName_NullName()
        {
            Assert.Throws<System.ArgumentNullException>(() => WCUtils.GetColorByName(null));
        }


        [Fact]
        public void Test_GetValue_FromPSMemberInfoCollection()
        {
            // arrange
            //var col = new PSMemberInfoCollection<PSPropertyInfo>();
        }
    }
}

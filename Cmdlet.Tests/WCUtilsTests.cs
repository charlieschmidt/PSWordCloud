using System;
using Xunit;
using PSWordCloud;
using System.Management.Automation;
using System.Collections;
using System.Collections.Generic;
using SkiaSharp;

namespace Cmdlet.Tests
{
    public class WCUtilsTests
    {

        #region float-ToRadians
        [Theory]
        [InlineData(0.0, 0.0)]
        [InlineData((float)Math.PI, 180.0)]
        [InlineData((float)Math.PI / 2.0, 90.0)]
        public void Test_ToRadians(float expected, float degrees)
        {
            Assert.Equal(expected, degrees.ToRadians());
        }
        #endregion

        #region SKPaint-NextWord

        [Fact]
        public void Test_NextWord()
        {
            // arrange
            var brush = new SKPaint();

            // act
            brush.NextWord((float)10.0, (float)15.0, WCUtils.GetColorByName("Red"));

            // assert
            Assert.Equal(10.0, brush.TextSize);
            Assert.Equal(1.5, brush.StrokeWidth);
            Assert.False(brush.IsStroke);
            Assert.False(brush.IsVerticalText);
            Assert.Equal(0xff, brush.Color.Red);
            Assert.Equal(0x00, brush.Color.Blue);
            Assert.Equal(0x00, brush.Color.Green);
        }

        [Fact]
        public void Test_NextWord_ZeroStrokWidth()
        {
            // arrange
            var brush = new SKPaint();

            // act
            brush.NextWord((float)10.0, (float)0.0, WCUtils.GetColorByName("Red"));

            // assert
            Assert.Equal(10.0, brush.TextSize);
            Assert.Equal(0.0, brush.StrokeWidth);
            Assert.False(brush.IsStroke);
            Assert.False(brush.IsVerticalText);
            Assert.Equal(0xff, brush.Color.Red);
            Assert.Equal(0x00, brush.Color.Blue);
            Assert.Equal(0x00, brush.Color.Green);
        }
        #endregion

        #region GetColorByName
        [Theory]
        [InlineData("Red", 0xff, 0x00, 0x00)]
        [InlineData("Blue", 0x00, 0x00, 0xff)]
        [InlineData("red", 0xff, 0x00, 0x00)]
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
        public void Test_GetColorByName_InvalidName(string colorName)
        {
            Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => WCUtils.GetColorByName(colorName));
        }

        [Fact]
        public void Test_GetColorByName_NullName()
        {
            Assert.Throws<System.ArgumentNullException>(() => WCUtils.GetColorByName(null));
        }
        #endregion

        #region IEnumerable-GetValue
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
            var dict = new Dictionary<string, dynamic>() { { "a", new { ThisKey = 2 } }, { "b", new { ThatKey = 3 } } };

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
            Assert.Throws<System.ArgumentException>(() => ((IDictionary<string, object>)null).GetValue("a"));
        }
        #endregion

        #region SKRegion-SetPath

        [Fact]
        public void Test_SetPath()
        {
            // arrange
            var path = new SKPath();
            path.AddPoly(new SKPoint[] {
                    new SKPoint() {X = 0, Y = 0},
                    new SKPoint() {X = 0, Y = 1},
                    new SKPoint() {X = 1, Y = 1},
                    new SKPoint() {X = 1, Y = 0}
                }
            );
            var region = new SKRegion();

            // act
            region.SetPath(path, false);

            // assert
            Assert.Equal(0, region.Bounds.Left);
            Assert.Equal(1, region.Bounds.Bottom);
            Assert.Equal(1, region.Bounds.Right);
            Assert.Equal(0, region.Bounds.Top);
        }

        [Fact]
        public void Test_SetPath_UseBounds()
        {
            // arrange
            var path = new SKPath();
            path.AddPoly(new SKPoint[] {
                    new SKPoint() {X = 0, Y = 0},
                    new SKPoint() {X = 0, Y = 2},
                    new SKPoint() {X = 2, Y = 2},
                    new SKPoint() {X = 2, Y = 0}
                }
            );
            var region = new SKRegion();
            region.SetPath(path);

            var path2 = new SKPath();
            path2.AddPoly(new SKPoint[] {
                    new SKPoint() {X = 0, Y = 0},
                    new SKPoint() {X = 0, Y = 1},
                    new SKPoint() {X = 1, Y = 1},
                    new SKPoint() {X = 1, Y = 0}
                }
            );

            // act
            region.SetPath(path2, false);

            // assert
            Assert.Equal(0, region.Bounds.Left);
            Assert.Equal(1, region.Bounds.Bottom);
            Assert.Equal(1, region.Bounds.Right);
            Assert.Equal(0, region.Bounds.Top);
        }

        [Fact]
        public void Test_SetPath_UnclosedPath()
        {
            // arrange
            var path = new SKPath();
            path.AddPoly(new SKPoint[] {
                    new SKPoint() {X = 0, Y = 0},
                    new SKPoint() {X = 0, Y = 1},
                    new SKPoint() {X = 1, Y = 1}
                }
            );
            var region = new SKRegion();

            // act
            region.SetPath(path, true);

            // assert
            Assert.Equal(0, region.Bounds.Left);
            Assert.Equal(1, region.Bounds.Bottom);
            Assert.Equal(1, region.Bounds.Right);
            Assert.Equal(0, region.Bounds.Top);
        }

        [Fact]
        public void Test_SetPath_EmptyPath()
        {
            // arrange
            var path = new SKPath();
            var region = new SKRegion();

            // act
            region.SetPath(path, true);

            // assert
            Assert.Equal(0, region.Bounds.Left);
            Assert.Equal(0, region.Bounds.Bottom);
            Assert.Equal(0, region.Bounds.Right);
            Assert.Equal(0, region.Bounds.Top);
        }
        #endregion

        #region SKRegion-Op


        [Fact]
        public void Test_SetPath_Op()
        {
            // arrange
            var path = new SKPath();
            path.AddPoly(new SKPoint[] {
                    new SKPoint() {X = 0, Y = 0},
                    new SKPoint() {X = 0, Y = 2},
                    new SKPoint() {X = 2, Y = 2},
                    new SKPoint() {X = 2, Y = 0}
                }
            );
            var region = new SKRegion();
            region.SetPath(path);

            var path2 = new SKPath();
            path2.AddPoly(new SKPoint[] {
                    new SKPoint() {X = 0, Y = 0},
                    new SKPoint() {X = 0, Y = 1},
                    new SKPoint() {X = 1, Y = 1},
                    new SKPoint() {X = 1, Y = 0}
                }
            );

            // act
            region.Op(path2, SKRegionOperation.Replace);

            // assert
            Assert.Equal(0, region.Bounds.Left);
            Assert.Equal(1, region.Bounds.Bottom);
            Assert.Equal(1, region.Bounds.Right);
            Assert.Equal(0, region.Bounds.Top);
        }
        #endregion

        #region SKRegion-IntersectsPath
        [Fact]
        public void Test_IntersectsPath()
        {
            // arrange
            var path = new SKPath();
            path.AddPoly(new SKPoint[] {
                    new SKPoint() {X = 0, Y = 0},
                    new SKPoint() {X = 0, Y = 2},
                    new SKPoint() {X = 2, Y = 2},
                    new SKPoint() {X = 2, Y = 0}
                }
            );
            var region = new SKRegion();
            region.SetPath(path);

            var path2 = new SKPath();
            path2.AddPoly(new SKPoint[] {
                    new SKPoint() {X = 1, Y = 1},
                    new SKPoint() {X = 1, Y = 3},
                    new SKPoint() {X = 3, Y = 3},
                    new SKPoint() {X = 3, Y = 1}
                }
            );

            // assert
            Assert.True(region.IntersectsPath(path));
        }

        [Fact]
        public void Test_IntersectsPath_DoesntIntersect()
        {
            // arrange
            var region = new SKRegion();

            var path = new SKPath();
            path.AddPoly(new SKPoint[] {
                    new SKPoint() {X = 0, Y = 0},
                    new SKPoint() {X = 0, Y = 3},
                    new SKPoint() {X = 3, Y = 3},
                    new SKPoint() {X = 3, Y = 0}
                }
            );

            var path2 = new SKPath();
            path2.AddPoly(new SKPoint[] {
                    new SKPoint() {X = 1, Y = 1},
                    new SKPoint() {X = 1, Y = 2},
                    new SKPoint() {X = 2, Y = 2},
                    new SKPoint() {X = 2, Y = 1}
                }
            );

            // assert
            Assert.False(region.IntersectsPath(path));
        }

        [Fact]
        public void Test_IntersectsPath_EmptyRegion()
        {
            // arrange
            var region = new SKRegion();

            var path = new SKPath();
            path.AddPoly(new SKPoint[] {
                    new SKPoint() {X = 0, Y = 0},
                    new SKPoint() {X = 0, Y = 2},
                    new SKPoint() {X = 2, Y = 2},
                    new SKPoint() {X = 2, Y = 0}
                }
            );

            // assert
            Assert.False(region.IntersectsPath(path));
        }
        #endregion
    }
}

using System;
using Xunit;
using PSWordCloud;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using SkiaSharp;
using System.Linq;

namespace Cmdlet.Tests
{
    public class AttributeTests
    {

        #region ImageSizeCompleter
        [Fact]
        public void Test_ImageSizeCompleter()
        {
            // arrange
            var c = new ImageSizeCompleter();

            // act
            var result = c.CompleteArgument("name", "ImageSize", "480", null, null);

            // assert
            Assert.NotNull(result);
            var cArg = result.First();
            Assert.NotNull(cArg);
            Assert.Equal("480x800", cArg.CompletionText);
        }


        [Fact]
        public void Test_ImageSizeCompleter_Multiple()
        {
            // arrange
            var c = new ImageSizeCompleter();

            // act
            var result = c.CompleteArgument("name", "ImageSize", "Poster", null, null);

            // assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }


        [Fact]
        public void Test_ImageSizeCompleter_NullWord()
        {
            // arrange
            var c = new ImageSizeCompleter();

            // act
            var result = c.CompleteArgument("name", "ImageSize", null, null, null);

            // assert
            Assert.NotNull(result);
            Assert.True(3 < result.Count());
        }

        #endregion


        #region FontFamilyCompleter
        [Fact]
        public void Test_FontFamilyCompleter()
        {
            // arrange
            var c = new FontFamilyCompleter();

            // act
            var result = c.CompleteArgument("name", "ImageSize", "Consolas", null, null);

            // assert
            Assert.NotNull(result);
            var cArg = result.First();
            Assert.NotNull(cArg);
            Assert.Equal("Consolas", cArg.CompletionText);
        }


        [Fact]
        public void Test_FontFamilyCompleter_Multiple()
        {
            // arrange
            var c = new FontFamilyCompleter();

            // act
            var result = c.CompleteArgument("name", "ImageSize", "C", null, null);

            // assert
            Assert.NotNull(result);
            Assert.True(2 <= result.Count());
        }


        [Fact]
        public void Test_FontFamilyCompleter_NullWord()
        {
            // arrange
            var c = new FontFamilyCompleter();

            // act
            var result = c.CompleteArgument("name", "ImageSize", null, null, null);

            // assert
            Assert.NotNull(result);
            Assert.True(3 < result.Count());
        }

        #endregion

        #region TransformToSKTypefaceAttribute

        [Fact]
        public void Test_TransformToSKTypefaceAttribute_SKTypeface()
        {
            // arrange
            SKTypeface typeface = SKFontManager.CreateDefault().MatchFamily("Consolas", SKFontStyle.Normal);
            Assert.NotNull(typeface);
            var transformer = new TransformToSKTypefaceAttribute();

            // act
            var newTypeface = transformer.Transform(null, typeface);

            // assert
            Assert.Equal(typeface, newTypeface);
        }

        /*
                [Fact]
                public void Test_TransformToSKTypefaceAttribute_string()
                {
                    // arrange
                    SKTypeface typeface = SKFontManager.CreateDefault().MatchFamily("Consolas", SKFontStyle.Normal);
                    Assert.NotNull(typeface);
                    var transformer = new TransformToSKTypefaceAttribute();

                    // act
                    var newTypeface = transformer.Transform(null, "Consolas");

                    // assert
                    Assert.Equal(typeface, newTypeface);
                }
         */
         
        [Fact]
        public void Test_TransformToSKTypefaceAttribute_Hashtable()
        {
            // arrange
            var hashtable = new Hashtable() {
                {"a",1}
            };
            var transformer = new TransformToSKTypefaceAttribute();

            // act
            var newTypeface = transformer.Transform(null, hashtable);

            // assert
            Assert.NotNull(newTypeface);
        }
        /*
                [Fact]
                public void Test_TransformToSKTypefaceAttribute_Dictionary()
                {
                    // arrange
                    var dictionary = new Dictionary<string, string>() {
                        {"FontWeight","Normal"},
                        {"FontFamily","Consolas"}
                    };
                    var transformer = new TransformToSKTypefaceAttribute();

                    // act
                    var newTypeface = transformer.Transform(null, dictionary);

                    // assert
                    Assert.NotNull(newTypeface);
                }
         */

        #endregion
    }
}
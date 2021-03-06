// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Xunit;

namespace System.Text.RegularExpressions.Tests
{
    public class RegexReductionTests
    {
        // These tests depend on using reflection to access internals of Regex in order to validate
        // if, when, and how various optimizations are being employed.  As implementation details
        // change, these tests will need to be updated as well.  Note that Compiled Regexes
        // null out the _code field being accessed here, so this mechanism won't work to validate
        // Compiled, which also means it won't work to validate optimizations only enabled
        // when using Compiled, such as auto-atomicity for the last node in a regex.

        private static int[] GetRegexCodes(Regex r)
        {
            FieldInfo codeField = typeof(Regex).GetField("_code", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(codeField);

            object code = codeField.GetValue(r);
            Assert.NotNull(code);

            FieldInfo codesField = code.GetType().GetField("Codes", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(codesField);

            int[] codes = codesField.GetValue(code) as int[];
            Assert.NotNull(codes);

            return codes;
        }

        private static int GetMinRequiredLength(Regex r)
        {
            FieldInfo codeField = typeof(Regex).GetField("_code", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(codeField);

            object code = codeField.GetValue(r);
            Assert.NotNull(code);

            FieldInfo treeField = code.GetType().GetField("Tree", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(treeField);

            object tree = treeField.GetValue(code);
            Assert.NotNull(tree);

            FieldInfo minRequiredLengthField = tree.GetType().GetField("MinRequiredLength", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(minRequiredLengthField);

            object minRequiredLength = minRequiredLengthField.GetValue(tree);
            Assert.IsType<int>(minRequiredLength);

            return (int)minRequiredLength;
        }

        [Theory]
        [SkipOnTargetFramework(TargetFrameworkMonikers.NetFramework, "Many of these optimizations don't exist in netfx.")]
        // Two greedy one loops
        [InlineData("a*a*", "a*")]
        [InlineData("(a*a*)", "(a*)")]
        [InlineData("a*(?:a*)", "a*")]
        [InlineData("a*a+", "a+")]
        [InlineData("a*a?", "a*")]
        [InlineData("a*a{1,3}", "a+")]
        [InlineData("a+a*", "a+")]
        [InlineData("a+a+", "a{2,}")]
        [InlineData("a+a?", "a+")]
        [InlineData("a+a{1,3}", "a{2,}")]
        [InlineData("a?a*", "a*")]
        [InlineData("a?a+", "a+")]
        [InlineData("a?a?", "a{0,2}")]
        [InlineData("a?a{1,3}", "a{1,4}")]
        [InlineData("a{1,3}a*", "a+")]
        [InlineData("a{1,3}a+", "a{2,}")]
        [InlineData("a{1,3}a?", "a{1,4}")]
        [InlineData("a{1,3}a{1,3}", "a{2,6}")]
        // Greedy one loop and one
        [InlineData("a*a", "a+")]
        [InlineData("a+a", "a{2,}")]
        [InlineData("a?a", "a{1,2}")]
        [InlineData("a{1,3}a", "a{2,4}")]
        [InlineData("aa*", "a+")]
        [InlineData("aa+", "a{2,}")]
        [InlineData("aa?", "a{1,2}")]
        [InlineData("aa{1,3}", "a{2,4}")]
        // Two atomic one loops
        [InlineData("(?>a*)(?>a*)", "(?>a*)")]
        [InlineData("(?>a*)(?>(?:a*))", "(?>a*)")]
        [InlineData("(?>a*)(?>a+)", "(?>a+)")]
        [InlineData("(?>a*)(?>a?)", "(?>a*)")]
        [InlineData("(?>a*)(?>a{1,3})", "(?>a+)")]
        [InlineData("(?>a+)(?>a*)", "(?>a+)")]
        [InlineData("(?>a+)(?>a+)", "(?>a{2,})")]
        [InlineData("(?>a+)(?>a?)", "(?>a+)")]
        [InlineData("(?>a+)(?>a{1,3})", "(?>a{2,})")]
        [InlineData("(?>a?)(?>a*)", "(?>a*)")]
        [InlineData("(?>a?)(?>a+)", "(?>a+)")]
        [InlineData("(?>a?)(?>a?)", "(?>a{0,2})")]
        [InlineData("(?>a?)(?>a{1,3})", "(?>a{1,4})")]
        [InlineData("(?>a{1,3})(?>a*)", "(?>a+)")]
        [InlineData("(?>a{1,3})(?>a+)", "(?>a{2,})")]
        [InlineData("(?>a{1,3})(?>a?)", "(?>a{1,4})")]
        [InlineData("(?>a{1,3})(?>a{1,3})", "(?>a{2,6})")]
        // Atomic one loop and one
        [InlineData("(?>a*)a", "(?>a+)")]
        [InlineData("(?>a+)a", "(?>a{2,})")]
        [InlineData("(?>a?)a", "(?>a{1,2})")]
        [InlineData("(?>a{1,3})a", "(?>a{2,4})")]
        [InlineData("a(?>a*)", "(?>a+)")]
        [InlineData("a(?>a+)", "(?>a{2,})")]
        [InlineData("a(?>a?)", "(?>a{1,2})")]
        [InlineData("a(?>a{1,3})", "(?>a{2,4})")]
        // Two lazy one loops
        [InlineData("a*?a*?", "a*?")]
        [InlineData("a*?a+?", "a+?")]
        [InlineData("a*?a??", "a*?")]
        [InlineData("a*?a{1,3}?", "a+?")]
        [InlineData("a+?a*?", "a+?")]
        [InlineData("a+?a+?", "a{2,}?")]
        [InlineData("a+?a??", "a+?")]
        [InlineData("a+?a{1,3}?", "a{2,}?")]
        [InlineData("a??a*?", "a*?")]
        [InlineData("a??a+?", "a+?")]
        [InlineData("a??a??", "a{0,2}?")]
        [InlineData("a??a{1,3}?", "a{1,4}?")]
        [InlineData("a{1,3}?a*?", "a+?")]
        [InlineData("a{1,3}?a+?", "a{2,}?")]
        [InlineData("a{1,3}?a??", "a{1,4}?")]
        [InlineData("a{1,3}?a{1,3}?", "a{2,6}?")]
        // Lazy one loop and one
        [InlineData("a*?a", "a+?")]
        [InlineData("a+?a", "a{2,}?")]
        [InlineData("a??a", "a{1,2}?")]
        [InlineData("a{1,3}?a", "a{2,4}?")]
        [InlineData("aa*?", "a+?")]
        [InlineData("aa+?", "a{2,}?")]
        [InlineData("aa??", "a{1,2}?")]
        [InlineData("aa{1,3}?", "a{2,4}?")]
        // Two greedy notone loops
        [InlineData("[^a]*[^a]*", "[^a]*")]
        [InlineData("[^a]*[^a]+", "[^a]+")]
        [InlineData("[^a]*[^a]?", "[^a]*")]
        [InlineData("[^a]*[^a]{1,3}", "[^a]+")]
        [InlineData("[^a]+[^a]*", "[^a]+")]
        [InlineData("[^a]+[^a]+", "[^a]{2,}")]
        [InlineData("[^a]+[^a]?", "[^a]+")]
        [InlineData("[^a]+[^a]{1,3}", "[^a]{2,}")]
        [InlineData("[^a]?[^a]*", "[^a]*")]
        [InlineData("[^a]?[^a]+", "[^a]+")]
        [InlineData("[^a]?[^a]?", "[^a]{0,2}")]
        [InlineData("[^a]?[^a]{1,3}", "[^a]{1,4}")]
        [InlineData("[^a]{1,3}[^a]*", "[^a]+")]
        [InlineData("[^a]{1,3}[^a]+", "[^a]{2,}")]
        [InlineData("[^a]{1,3}[^a]?", "[^a]{1,4}")]
        [InlineData("[^a]{1,3}[^a]{1,3}", "[^a]{2,6}")]
        // Two atomic notone loops
        [InlineData("(?>[^a]*)(?>[^a]*)", "(?>[^a]*)")]
        [InlineData("(?>[^a]*)(?>[^a]+)", "(?>[^a]+)")]
        [InlineData("(?>[^a]*)(?>[^a]?)", "(?>[^a]*)")]
        [InlineData("(?>[^a]*)(?>[^a]{1,3})", "(?>[^a]+)")]
        [InlineData("(?>[^a]+)(?>[^a]*)", "(?>[^a]+)")]
        [InlineData("(?>[^a]+)(?>[^a]+)", "(?>[^a]{2,})")]
        [InlineData("(?>[^a]+)(?>[^a]?)", "(?>[^a]+)")]
        [InlineData("(?>[^a]+)(?>[^a]{1,3})", "(?>[^a]{2,})")]
        [InlineData("(?>[^a]?)(?>[^a]*)", "(?>[^a]*)")]
        [InlineData("(?>[^a]?)(?>[^a]+)", "(?>[^a]+)")]
        [InlineData("(?>[^a]?)(?>[^a]?)", "(?>[^a]{0,2})")]
        [InlineData("(?>[^a]?)(?>[^a]{1,3})", "(?>[^a]{1,4})")]
        [InlineData("(?>[^a]{1,3})(?>[^a]*)", "(?>[^a]+)")]
        [InlineData("(?>[^a]{1,3})(?>[^a]+)", "(?>[^a]{2,})")]
        [InlineData("(?>[^a]{1,3})(?>[^a]?)", "(?>[^a]{1,4})")]
        [InlineData("(?>[^a]{1,3})(?>[^a]{1,3})", "(?>[^a]{2,6})")]
        // Greedy notone loop and one
        [InlineData("[^a]*[^a]", "[^a]+")]
        [InlineData("[^a]+[^a]", "[^a]{2,}")]
        [InlineData("[^a]?[^a]", "[^a]{1,2}")]
        [InlineData("[^a]{1,3}[^a]", "[^a]{2,4}")]
        [InlineData("[^a][^a]*", "[^a]+")]
        [InlineData("[^a][^a]+", "[^a]{2,}")]
        [InlineData("[^a][^a]?", "[^a]{1,2}")]
        [InlineData("[^a][^a]{1,3}", "[^a]{2,4}")]
        // Atomic notone loop and one
        [InlineData("(?>[^a]*)[^a]", "(?>[^a]+)")]
        [InlineData("(?>[^a]+)[^a]", "(?>[^a]{2,})")]
        [InlineData("(?>[^a]?)[^a]", "(?>[^a]{1,2})")]
        [InlineData("(?>[^a]{1,3})[^a]", "(?>[^a]{2,4})")]
        [InlineData("[^a](?>[^a]*)", "(?>[^a]+)")]
        [InlineData("[^a](?>[^a]+)", "(?>[^a]{2,})")]
        [InlineData("[^a](?>[^a]?)", "(?>[^a]{1,2})")]
        [InlineData("[^a](?>[^a]{1,3})", "(?>[^a]{2,4})")]
        // Notone and notone
        [InlineData("[^a][^a]", "[^a]{2}")]
        // Two greedy set loops
        [InlineData("[0-9]*[0-9]*", "[0-9]*")]
        [InlineData("[0-9]*[0-9]+", "[0-9]+")]
        [InlineData("[0-9]*[0-9]?", "[0-9]*")]
        [InlineData("[0-9]*[0-9]{1,3}", "[0-9]+")]
        [InlineData("[0-9]+[0-9]*", "[0-9]+")]
        [InlineData("[0-9]+[0-9]+", "[0-9]{2,}")]
        [InlineData("[0-9]+[0-9]?", "[0-9]+")]
        [InlineData("[0-9]+[0-9]{1,3}", "[0-9]{2,}")]
        [InlineData("[0-9]?[0-9]*", "[0-9]*")]
        [InlineData("[0-9]?[0-9]+", "[0-9]+")]
        [InlineData("[0-9]?[0-9]?", "[0-9]{0,2}")]
        [InlineData("[0-9]?[0-9]{1,3}", "[0-9]{1,4}")]
        [InlineData("[0-9]{1,3}[0-9]*", "[0-9]+")]
        [InlineData("[0-9]{1,3}[0-9]+", "[0-9]{2,}")]
        [InlineData("[0-9]{1,3}[0-9]?", "[0-9]{1,4}")]
        [InlineData("[0-9]{1,3}[0-9]{1,3}", "[0-9]{2,6}")]
        // Greedy set loop and set
        [InlineData("[0-9]*[0-9]", "[0-9]+")]
        [InlineData("[0-9]+[0-9]", "[0-9]{2,}")]
        [InlineData("[0-9]?[0-9]", "[0-9]{1,2}")]
        [InlineData("[0-9]{1,3}[0-9]", "[0-9]{2,4}")]
        [InlineData("[0-9][0-9]*", "[0-9]+")]
        [InlineData("[0-9][0-9]+", "[0-9]{2,}")]
        [InlineData("[0-9][0-9]?", "[0-9]{1,2}")]
        [InlineData("[0-9][0-9]{1,3}", "[0-9]{2,4}")]
        // Atomic set loop and set
        [InlineData("(?>[0-9]*)[0-9]", "(?>[0-9]+)")]
        [InlineData("(?>[0-9]+)[0-9]", "(?>[0-9]{2,})")]
        [InlineData("(?>[0-9]?)[0-9]", "(?>[0-9]{1,2})")]
        [InlineData("(?>[0-9]{1,3})[0-9]", "(?>[0-9]{2,4})")]
        [InlineData("[0-9](?>[0-9]*)", "(?>[0-9]+)")]
        [InlineData("[0-9](?>[0-9]+)", "(?>[0-9]{2,})")]
        [InlineData("[0-9](?>[0-9]?)", "(?>[0-9]{1,2})")]
        [InlineData("[0-9](?>[0-9]{1,3})", "(?>[0-9]{2,4})")]
        // Two lazy set loops
        [InlineData("[0-9]*?[0-9]*?", "[0-9]*?")]
        [InlineData("[0-9]*?[0-9]+?", "[0-9]+?")]
        [InlineData("[0-9]*?[0-9]??", "[0-9]*?")]
        [InlineData("[0-9]*?[0-9]{1,3}?", "[0-9]+?")]
        [InlineData("[0-9]+?[0-9]*?", "[0-9]+?")]
        [InlineData("[0-9]+?[0-9]+?", "[0-9]{2,}?")]
        [InlineData("[0-9]+?[0-9]??", "[0-9]+?")]
        [InlineData("[0-9]+?[0-9]{1,3}?", "[0-9]{2,}?")]
        [InlineData("[0-9]??[0-9]*?", "[0-9]*?")]
        [InlineData("[0-9]??[0-9]+?", "[0-9]+?")]
        [InlineData("[0-9]??[0-9]??", "[0-9]{0,2}?")]
        [InlineData("[0-9]??[0-9]{1,3}?", "[0-9]{1,4}?")]
        [InlineData("[0-9]{1,3}?[0-9]*?", "[0-9]+?")]
        [InlineData("[0-9]{1,3}?[0-9]+?", "[0-9]{2,}?")]
        [InlineData("[0-9]{1,3}?[0-9]??", "[0-9]{1,4}?")]
        [InlineData("[0-9]{1,3}?[0-9]{1,3}?", "[0-9]{2,6}?")]
        // Lazy set loop and set
        [InlineData("[0-9]*?[0-9]", "[0-9]+?")]
        [InlineData("[0-9]+?[0-9]", "[0-9]{2,}?")]
        [InlineData("[0-9]??[0-9]", "[0-9]{1,2}?")]
        [InlineData("[0-9]{1,3}?[0-9]", "[0-9]{2,4}?")]
        [InlineData("[0-9][0-9]*?", "[0-9]+?")]
        [InlineData("[0-9][0-9]+?", "[0-9]{2,}?")]
        [InlineData("[0-9][0-9]??", "[0-9]{1,2}?")]
        [InlineData("[0-9][0-9]{1,3}?", "[0-9]{2,4}?")]
        // Set and set
        [InlineData("[ace][ace]", "[ace]{2}")]
        // Large loop patterns
        [InlineData("a*a*a*a*a*a*a*b*b*?a+a*", "a*b*b*?a+")]
        // Group elimination
        [InlineData("(?:(?:(?:(?:(?:(?:a*))))))", "a*")]
        // Nested loops
        [InlineData("(?:a*)*", "a*")]
        [InlineData("(?:a*)+", "a*")]
        [InlineData("(?:a+){4}", "a{4,}")]
        [InlineData("(?:a{1,2}){4}", "a{4,8}")]
        // Alternation reduction
        [InlineData("a|b|c|d|e|g|h|z", "[a-eghz]")]
        [InlineData("a|b|c|def|g|h", "[a-c]|def|[gh]")]
        // Auto-atomicity
        [InlineData("a*b", "(?>a*)b")]
        [InlineData("a*b+", "(?>a*)b+")]
        [InlineData("a*b{3,4}", "(?>a*)b{3,4}")]
        [InlineData("a+b", "(?>a+)b")]
        [InlineData("[^\n]*\n", "(?>[^\n]*)\n")]
        [InlineData("[^\n]*\n+", "(?>[^\n]*)\n+")]
        [InlineData("(a+)b", "((?>a+))b")]
        [InlineData("a*(?:bcd|efg)", "(?>a*)(?:bcd|efg)")]
        [InlineData("\\w*\\b", "(?>\\w*)\\b")]
        [InlineData("\\d*\\b", "(?>\\d*)\\b")]
        [InlineData("(?:a[ce]*|b*)g", "(?:a(?>[ce]*)|(?>b*))g")]
        [InlineData("(?:a[ce]*|b*)c", "(?:a[ce]*|(?>b*))c")]
        public void PatternsReduceIdentically(string pattern1, string pattern2)
        {
            AssertExtensions.Equal(GetRegexCodes(new Regex(pattern1)), GetRegexCodes(new Regex(pattern2)));
            Assert.NotEqual<int>(GetRegexCodes(new Regex(pattern1, RegexOptions.IgnoreCase)), GetRegexCodes(new Regex(pattern2)));
            Assert.NotEqual<int>(GetRegexCodes(new Regex(pattern1, RegexOptions.RightToLeft)), GetRegexCodes(new Regex(pattern2)));
        }

        [Theory]
        [SkipOnTargetFramework(TargetFrameworkMonikers.NetFramework, "Many of these optimizations don't exist in netfx.")]
        // Not coalescing loops
        [InlineData("aa", "a{2}")]
        [InlineData("a[^a]", "a{2}")]
        [InlineData("[^a]a", "[^a]{2}")]
        [InlineData("a*b*", "a*")]
        [InlineData("a*b*", "b*")]
        [InlineData("[^a]*[^b]", "[^a]*")]
        [InlineData("[ace]*[acd]", "[ace]*")]
        [InlineData("a+b+", "a+")]
        [InlineData("a+b+", "b+")]
        [InlineData("a*(a*)", "a*")]
        [InlineData("(a*)a*", "a*")]
        [InlineData("a*(?>a*)", "a*")]
        [InlineData("a*a*?", "a*")]
        [InlineData("a*?a*", "a*")]
        [InlineData("a*[^a]*", "a*")]
        [InlineData("[^a]*a*", "a*")]
        [InlineData("a{2147483646}a", "a{2147483647}")]
        [InlineData("a{2147483647}a", "a{2147483647}")]
        [InlineData("a{0,2147483646}a", "a{0,2147483647}")]
        [InlineData("aa{2147483646}", "a{2147483647}")]
        [InlineData("aa{0,2147483646}", "a{0,2147483647}")]
        [InlineData("a{2147482647}a{1000}", "a{2147483647}")]
        [InlineData("a{0,2147482647}a{0,1000}", "a{0,2147483647}")]
        [InlineData("[^a]{2147483646}[^a]", "[^a]{2147483647}")]
        [InlineData("[^a]{2147483647}[^a]", "[^a]{2147483647}")]
        [InlineData("[^a]{0,2147483646}[^a]", "[^a]{0,2147483647}")]
        [InlineData("[^a][^a]{2147483646}", "[^a]{2147483647}")]
        [InlineData("[^a][^a]{0,2147483646}", "[^a]{0,2147483647}")]
        [InlineData("[^a]{2147482647}[^a]{1000}", "[^a]{2147483647}")]
        [InlineData("[^a]{0,2147482647}[^a]{0,1000}", "[^a]{0,2147483647}")]
        [InlineData("[ace]{2147483646}[ace]", "[ace]{2147483647}")]
        [InlineData("[ace]{2147483647}[ace]", "[ace]{2147483647}")]
        [InlineData("[ace]{0,2147483646}[ace]", "[ace]{0,2147483647}")]
        [InlineData("[ace][ace]{2147483646}", "[ace]{2147483647}")]
        [InlineData("[ace][ace]{0,2147483646}", "[ace]{0,2147483647}")]
        [InlineData("[ace]{2147482647}[ace]{1000}", "[ace]{2147483647}")]
        [InlineData("[ace]{0,2147482647}[ace]{0,1000}", "[ace]{0,2147483647}")]
        // Not applying auto-atomicity
        [InlineData("a*b*", "(?>a*)b*")]
        [InlineData("[^\n]*\n*", "(?>[^\n]*)\n")]
        public void PatternsReduceDifferently(string pattern1, string pattern2)
        {
            var r1 = new Regex(pattern1);
            var r2 = new Regex(pattern2);
            Assert.NotEqual<int>(GetRegexCodes(r1), GetRegexCodes(r2));
        }

        [Theory]
        [SkipOnTargetFramework(TargetFrameworkMonikers.NetFramework, "Not computed in netfx")]
        [InlineData(@"a", 1)]
        [InlineData(@"[^a]", 1)]
        [InlineData(@"[abcdefg]", 1)]
        [InlineData(@"abcd", 4)]
        [InlineData(@"a*", 0)]
        [InlineData(@"a*?", 0)]
        [InlineData(@"a?", 0)]
        [InlineData(@"a??", 0)]
        [InlineData(@"a+", 1)]
        [InlineData(@"a+?", 1)]
        [InlineData(@"a{2}", 2)]
        [InlineData(@"a{2}?", 2)]
        [InlineData(@"a{3,17}", 3)]
        [InlineData(@"a{3,17}?", 3)]
        [InlineData(@"(abcd){5}", 20)]
        [InlineData(@"(abcd|ef){2,6}", 4)]
        [InlineData(@"abcef|de", 2)]
        [InlineData(@"abc(def|ghij)k", 7)]
        [InlineData(@"\d{1,2}-\d{1,2}-\d{2,4}", 6)]
        [InlineData(@"1(?=9)\d", 2)]
        [InlineData(@"1(?!\d)\w", 2)]
        [InlineData(@"a*a*a*a*a*a*a*b*", 0)]
        [InlineData(@"((a{1,2}){4}){3,7}", 12)]
        [InlineData(@"\b\w{4}\b", 4)]
        public void MinRequiredLengthIsCorrect(string pattern, int expectedLength)
        {
            var r = new Regex(pattern);
            Assert.Equal(expectedLength, GetMinRequiredLength(r));
        }
    }
}

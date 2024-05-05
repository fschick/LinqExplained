using FS.LinqExplained;
using System;
using System.Collections.Generic;
using System.Linq;
using V9;

namespace FS.LinqExplained
{
    public static class LinqExplained
    {
        public static void Execute()
        {
            var lines = new[] {
                "freilebende gummibärchen gibt es nicht." ,
                "man kauft sie in packungen an der kinokasse",
                "dieser kauf ist der beginn einer fast erotischen und sehr ambivalenten beziehung gummibärchen-mensch",
                "zuerst genießt man.",
                "dieser genuss umfasst alle sinne.",
                "man wühlt in den gummibärchen, man fühlt sie.",
                "gummibärchen haben eine konsistenz wie weichgekochter radiergummi.",
                "die tastempfindung geht auch ins sexuelle."
            };

            var numbers = new[] { 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144 };

            #region V1: Framework and business logic combined into same function.
            var frameworkV1 = new V1.Framework();
            frameworkV1.Filter(lines, "StartWith", "man").Dump("V1 StartWith");
            frameworkV1.Filter(lines, "Contains", "man").Dump("V1 Contains");
            #endregion

            #region V2: Business logic moved to separate class.
            var frameworkV2 = new V2.Framework();
            frameworkV2.Filter(lines, "StartWith", "man").Dump("V2 StartWith");
            frameworkV2.Filter(lines, "Contains", "man").Dump("V2 Contains");
            #endregion

            #region V3: Delegate for filter functions created. Loops combined.
            var frameworkV3 = new V3.Framework();
            frameworkV3.Filter(lines, "StartWith", "man").Dump("V3 StartWith");
            frameworkV3.Filter(lines, "Contains", "man").Dump("V3 Contains");
            #endregion

            #region V4: Parameter 'filterFunction' replaced by delegate.
            var frameworkV4 = new V4.Framework();
            frameworkV4.Filter(lines, BusinessLogic.StartsWith, "man").Dump("V4 StartWith");
            frameworkV4.Filter(lines, BusinessLogic.Contains, "man").Dump("V4 Contains");
            #endregion

            #region V5: Parameter 'filterFunction' replaced by inline function.
            var frameworkV5 = new V5.Framework();
            frameworkV5.Filter(lines, (string text, string filter) => text.StartsWith(filter), "man").Dump("V5 StartWith");
            frameworkV5.Filter(lines, (string text, string filter) => text.Contains(filter), "man").Dump("V5 Contains");
            #endregion

            #region V6: Parameter 'filter' (re)moved to inline function.
            var frameworkV6 = new V6.Framework();
            frameworkV6.Filter(lines, (string text) => text.StartsWith("man")).Dump("V6 StartWith");
            frameworkV6.Filter(lines, (string text) => text.Contains("man")).Dump("V6 Contains");
            #endregion

            #region V7: Generics added
            // V7A: Type of list made generic. Inline function simplified.
            var frameworkV7A = new V7A.Framework();
            frameworkV7A.Filter(lines, (text) => text.StartsWith("man")).Dump("V7A StartWith");
            frameworkV7A.Filter(lines, (text) => text.Contains("man")).Dump("V7A Contains");
            frameworkV7A.Filter(numbers, (number) => number > 50).Dump("V7A Greater than");

            // V7B: Return type of delegate made generic. Inline function simplified.
            var frameworkV7B = new V7B.Framework();
            frameworkV7B.Filter(lines, text => text.StartsWith("man")).Dump("V7B StartWith");
            frameworkV7B.Filter(lines, text => text.Contains("man")).Dump("V7B Contains");

            // V7C: Type parameters marked as co-variant/contra-variant.
            // Covariance (out): Enables you to use a more derived type than originally specified.
            // Contravariance (in): Enables you to use a more generic(less derived) type than originally specified.
            var frameworkV7C = new V7C.Framework();
            frameworkV7C.Filter(lines, text => text.StartsWith("man")).Dump("V7C StartWith");
            frameworkV7C.Filter(lines, text => text.Contains("man")).Dump("V7C Contains");
            #endregion

            #region V8: Delegate replace by Func<,>.
            var frameworkV8 = new V8.Framework();
            frameworkV8.Filter(lines, text => text.StartsWith("man")).Dump("V8 StartWith");
            frameworkV8.Filter(lines, text => text.Contains("man")).Dump("V8 Contains");
            #endregion

            #region V9: Switch to extension method
            lines.Filter(text => text.StartsWith("man")).Dump("V9 StartWith");
            lines.Filter(text => text.Contains("man")).Dump("V9 Contains");
            #endregion

            #region V10: 'Filter' renamed to 'Where'
            V10.Framework.Where(lines, text => text.StartsWith("man")).Dump("V10 StartWith");
            V10.Framework.Where(lines, text => text.Contains("man")).Dump("V10 Contains");

            lines.Where(text => text.StartsWith("man")).Dump(".NET Core StartsWith");
            lines.Where(text => text.Contains("man")).Dump(".NET Core Contains");
            #endregion
        }
    }

    public class BusinessLogic
    {
        public static bool StartsWith(string text, string filter)
            => text.StartsWith(filter);

        public static bool Contains(string text, string filter)
            => text.Contains(filter);
    }
}

#region V1: Framework and business logic combined into same function.
namespace V1
{
    public class Framework
    {
        public IEnumerable<string> Filter(IEnumerable<string> lines, string startOrContains, string filter)
        {
            var result = new List<string>();

            if (startOrContains == "StartWith")
            {
                foreach (var text in lines)
                {
                    var isMatch = text.StartsWith(filter);
                    if (isMatch)
                        result.Add(text);
                }
            }
            else if (startOrContains == "Contains")
            {
                foreach (var text in lines)
                {
                    var isMatch = text.Contains(filter);
                    if (isMatch)
                        result.Add(text);
                }
            }

            return result;
        }
    }
}
#endregion

#region V2: Business logic moved to separate class.
namespace V2
{
    public class Framework
    {
        public IEnumerable<string> Filter(IEnumerable<string> lines, string startOrContains, string filter)
        {
            var result = new List<string>();

            if (startOrContains == "StartWith")
            {
                foreach (var text in lines)
                {
                    var isMatch = BusinessLogic.StartsWith(text, filter);
                    if (isMatch)
                        result.Add(text);
                }
            }
            else if (startOrContains == "Contains")
            {
                foreach (var text in lines)
                {
                    var isMatch = BusinessLogic.Contains(text, filter);
                    if (isMatch)
                        result.Add(text);
                }
            }

            return result;
        }
    }
}
#endregion

#region V3: Delegate for filter functions created. Loops combined.
namespace V3
{
    public class Framework
    {
        public IEnumerable<string> Filter(IEnumerable<string> lines, string startOrContains, string filter)
        {
            var result = new List<string>();

            var filterFunction = startOrContains == "StartWith"
                ? (FilterFunc)BusinessLogic.StartsWith
                : (FilterFunc)BusinessLogic.Contains;

            foreach (var text in lines)
            {
                var isMatch = filterFunction(text, filter);
                if (isMatch)
                    result.Add(text);
            }

            return result;
        }

        public delegate bool FilterFunc(string text, string filter);
    }
}
#endregion

#region V4: Parameter 'filterFunction' replaced by delegate.
namespace V4
{
    public class Framework
    {
        public delegate bool FilterFunc(string text, string filter);

        public IEnumerable<string> Filter(IEnumerable<string> lines, FilterFunc filterFunction, string filter)
        {
            var result = new List<string>();

            foreach (var text in lines)
            {
                var isMatch = filterFunction(text, filter);
                if (isMatch)
                    result.Add(text);
            }

            return result;
        }
    }
}
#endregion

#region V5: Parameter 'filterFunction' replaced by inline function.
namespace V5
{
    public class Framework
    {
        public delegate bool FilterFunc(string text, string filter);

        public IEnumerable<string> Filter(IEnumerable<string> lines, FilterFunc filterFunction, string filter)
        {
            var result = new List<string>();

            foreach (var text in lines)
            {
                var isMatch = filterFunction(text, filter);
                if (isMatch)
                    result.Add(text);
            }

            return result;
        }
    }
}
#endregion

#region V6: Parameter 'filter' (re)moved to inline function.
namespace V6
{
    public class Framework
    {
        public delegate bool FilterFunc(string text);

        public IEnumerable<string> Filter(IEnumerable<string> lines, FilterFunc filterFunction)
        {
            var result = new List<string>();

            foreach (var text in lines)
            {
                var isMatch = filterFunction(text);
                if (isMatch)
                    result.Add(text);
            }

            return result;
        }
    }
}
#endregion

#region V7: Generics added
namespace V7A
{
    public class Framework
    {
        public delegate bool FilterFunc<TItem>(TItem item);

        public IEnumerable<TItem> Filter<TItem>(IEnumerable<TItem> items, FilterFunc<TItem> filterFunction)
        {
            var result = new List<TItem>();

            foreach (var item in items)
            {
                var isMatch = filterFunction(item);
                if (isMatch)
                    result.Add(item);
            }

            return result;
        }
    }
}

namespace V7B
{
    public class Framework
    {
        public delegate TResult FilterFunc<TItem, TResult>(TItem item);

        public IEnumerable<TItem> Filter<TItem>(IEnumerable<TItem> items, FilterFunc<TItem, bool> filterFunction)
        {
            var result = new List<TItem>();

            foreach (var item in items)
            {
                var isMatch = filterFunction(item);
                if (isMatch)
                    result.Add(item);
            }

            return result;
        }
    }
}

namespace V7C
{
    public class Framework
    {
        public delegate TResult FilterFunc<in TItem, out TResult>(TItem item);

        public IEnumerable<TItem> Filter<TItem>(IEnumerable<TItem> items, FilterFunc<TItem, bool> filterFunction)
        {
            var result = new List<TItem>();

            foreach (var item in items)
            {
                var isMatch = filterFunction(item);
                if (isMatch)
                    result.Add(item);
            }

            return result;
        }
    }
}
#endregion

#region V8: Delegate replace by Func<,>.
namespace V8
{
    public class Framework
    {
        // public delegate TResult FilterFunc<in TItem, out TResult>(TItem item);
        // public delegate TResult Func<[Nullable(2)] in T, [Nullable(2)] out TResult>(T arg);

        public IEnumerable<TItem> Filter<TItem>(IEnumerable<TItem> items, Func<TItem, bool> filterFunction)
        {
            var result = new List<TItem>();

            foreach (var item in items)
            {
                var isMatch = filterFunction(item);
                if (isMatch)
                    result.Add(item);
            }

            return result;
        }
    }
}
#endregion

#region V9: Switch to extension method
namespace V9
{
    public static class Framework
    {
        public static IEnumerable<TItem> Filter<TItem>(this IEnumerable<TItem> items, Func<TItem, bool> filterFunction)
        {
            var result = new List<TItem>();

            foreach (var item in items)
            {
                var isMatch = filterFunction(item);
                if (isMatch)
                    result.Add(item);
            }

            return result;
        }
    }
}
#endregion

#region V10: 'Filter' renamed to 'Where'
namespace V10
{
    public static class Framework
    {
        public static IEnumerable<TItem> Where<TItem>(this IEnumerable<TItem> items, Func<TItem, bool> filterFunction)
        {
            var result = new List<TItem>();

            foreach (var item in items)
            {
                var isMatch = filterFunction(item);
                if (isMatch)
                    result.Add(item);
            }

            return result;
        }
    }
}
#endregion

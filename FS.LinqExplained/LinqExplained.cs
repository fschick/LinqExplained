using FS.LinqExplained;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FS.LinqExplained
{
    public static class LinqExplained
    {
        public static void Execute()
        {
            var dummyTextLines = new[] {
                "freilebende gummibärchen gibt es nicht." ,
                "man kauft sie in packungen an der kinokasse",
                "dieser kauf ist der beginn einer fast erotischen und sehr ambivalenten beziehung gummibärchen-mensch",
                "zuerst genießt man.",
                "dieser genuss umfasst alle sinne.",
                "man wühlt in den gummibärchen, man fühlt sie.",
                "gummibärchen haben eine konsistenz wie weichgekochter radiergummi.",
                "die tastempfindung geht auch ins sexuelle."
            };

            #region V1: Framework and business logic combined into same function.
            var frameworkV1 = new V1.Framework();
            frameworkV1.Filter(dummyTextLines, "StartWith", "man").Dump("V1 StartWith");
            frameworkV1.Filter(dummyTextLines, "Contains", "man").Dump("V1 Contains");
            #endregion

            #region V2: Business logic moved to separate class.
            var frameworkV2 = new V2.Framework();
            frameworkV2.Filter(dummyTextLines, "StartWith", "man").Dump("V2 StartWith");
            frameworkV2.Filter(dummyTextLines, "Contains", "man").Dump("V2 Contains");
            #endregion

            #region V3: Delegate for filter functions created. Loops combined.
            var frameworkV3 = new V3.Framework();
            frameworkV3.Filter(dummyTextLines, "StartWith", "man").Dump("V3 StartWith");
            frameworkV3.Filter(dummyTextLines, "Contains", "man").Dump("V3 Contains");
            #endregion

            #region V4: Parameter 'filterFunction' replaced by delegate.
            var frameworkV4 = new V4.Framework();
            frameworkV4.Filter(dummyTextLines, BusinessLogic.StartsWith, "man").Dump("V4 StartWith");
            frameworkV4.Filter(dummyTextLines, BusinessLogic.Contains, "man").Dump("V4 Contains");
            #endregion

            #region V5: Parameter 'filterFunction' replaced by inline function.
            var frameworkV5 = new V5.Framework();
            frameworkV5.Filter(dummyTextLines, (string item, string filter) => item.StartsWith(filter), "man").Dump("V5 StartWith");
            frameworkV5.Filter(dummyTextLines, (string item, string filter) => item.Contains(filter), "man").Dump("V5 Contains");
            #endregion

            #region V6: Parameter 'filter' (re)moved to inline function.
            var frameworkV6 = new V6.Framework();
            frameworkV6.Filter(dummyTextLines, (string item) => item.StartsWith("man")).Dump("V6 StartWith");
            frameworkV6.Filter(dummyTextLines, (string item) => item.Contains("man")).Dump("V6 Contains");
            #endregion

            #region V7: Generics added
            // V7A: Type of list made generic. Inline function simplified.
            var frameworkV7A = new V7A.Framework();
            frameworkV7A.Filter(dummyTextLines, (item) => item.StartsWith("man")).Dump("V7A StartWith");
            frameworkV7A.Filter(dummyTextLines, (item) => item.Contains("man")).Dump("V7A Contains");

            // V7B: Return type of delegate made generic. Inline function simplified.
            var frameworkV7B = new V7B.Framework();
            frameworkV7B.Filter(dummyTextLines, item => item.StartsWith("man")).Dump("V7B StartWith");
            frameworkV7B.Filter(dummyTextLines, item => item.Contains("man")).Dump("V7B Contains");

            // V7C: Type parameters marked as co-variant/contra-variant.
            // Covariance: Enables you to use a more derived type than originally specified.
            // Contravariance: Enables you to use a more generic(less derived) type than originally specified.
            var frameworkV7C = new V7C.Framework();
            frameworkV7C.Filter(dummyTextLines, item => item.StartsWith("man")).Dump("V7C StartWith");
            frameworkV7C.Filter(dummyTextLines, item => item.Contains("man")).Dump("V7C Contains");
            #endregion

            #region V8: Delegate replace by Func<,>.
            var frameworkV8 = new V8.Framework();
            frameworkV8.Filter(dummyTextLines, item => item.StartsWith("man")).Dump("V8 StartWith");
            frameworkV8.Filter(dummyTextLines, item => item.Contains("man")).Dump("V8 Contains");
            #endregion

            #region V9: Switch to extension method
            V9.Framework.Filter(dummyTextLines, item => item.StartsWith("man")).Dump("V9 StartWith");
            V9.Framework.Filter(dummyTextLines, item => item.Contains("man")).Dump("V9 Contains");
            //list.Filter(item => item.Contains("man")).Dump("V1 StartWith");
            #endregion

            #region V10: 'Filter' renamed to 'Where'
            V10.Framework.Where(dummyTextLines, item => item.StartsWith("man")).Dump("V10 StartWith");
            V10.Framework.Where(dummyTextLines, item => item.Contains("man")).Dump("V10 Contains");

            dummyTextLines.Where(item => item.StartsWith("man")).Dump(".NET Core StartsWith");
            dummyTextLines.Where(item => item.Contains("man")).Dump(".NET Core Contains");
            #endregion
        }
    }

    public class BusinessLogic
    {
        public static bool StartsWith(string item, string filter)
            => item.StartsWith(filter);

        public static bool Contains(string item, string filter)
            => item.Contains(filter);
    }
}

#region V1
namespace V1
{
    public class Framework
    {
        public IEnumerable<string> Filter(IEnumerable<string> list, string startOrContains, string filter)
        {
            var result = new List<string>();

            if (startOrContains == "StartWith")
            {
                foreach (var item in list)
                {
                    var isMatch = item.StartsWith(filter);
                    if (isMatch)
                        result.Add(item);
                }
            }
            else if (startOrContains == "Contains")
            {
                foreach (var item in list)
                {
                    var isMatch = item.Contains(filter);
                    if (isMatch)
                        result.Add(item);
                }
            }

            return result;
        }
    }
}
#endregion

#region V2
namespace V2
{
    public class Framework
    {
        public IEnumerable<string> Filter(IEnumerable<string> list, string startOrContains, string filter)
        {
            var result = new List<string>();

            if (startOrContains == "StartWith")
            {
                foreach (var item in list)
                {
                    var isMatch = BusinessLogic.StartsWith(item, filter);
                    if (isMatch)
                        result.Add(item);
                }
            }
            else if (startOrContains == "Contains")
            {
                foreach (var item in list)
                {
                    var isMatch = BusinessLogic.Contains(item, filter);
                    if (isMatch)
                        result.Add(item);
                }
            }

            return result;
        }
    }
}
#endregion

#region V3
namespace V3
{
    public class Framework
    {
        public IEnumerable<string> Filter(IEnumerable<string> list, string startOrContains, string filter)
        {
            var result = new List<string>();

            var filterFunction = startOrContains == "StartWith"
                ? (FilterFunc)BusinessLogic.StartsWith
                : (FilterFunc)BusinessLogic.Contains;

            foreach (var item in list)
            {
                var isMatch = filterFunction(item, filter);
                if (isMatch)
                    result.Add(item);
            }

            return result;
        }

        public delegate bool FilterFunc(string item, string filter);
    }
}
#endregion

#region V4
namespace V4
{
    public class Framework
    {
        public delegate bool FilterFunc(string item, string filter);

        public IEnumerable<string> Filter(IEnumerable<string> list, FilterFunc filterFunction, string filter)
        {
            var result = new List<string>();

            foreach (var item in list)
            {
                var isMatch = filterFunction(item, filter);
                if (isMatch)
                    result.Add(item);
            }

            return result;
        }
    }
}
#endregion

#region V5
namespace V5
{
    public class Framework
    {
        public delegate bool FilterFunc(string item, string filter);

        public IEnumerable<string> Filter(IEnumerable<string> list, FilterFunc filterFunction, string filter)
        {
            var result = new List<string>();

            foreach (var item in list)
            {
                var isMatch = filterFunction(item, filter);
                if (isMatch)
                    result.Add(item);
            }

            return result;
        }
    }
}
#endregion

#region V6
namespace V6
{
    public class Framework
    {
        public delegate bool FilterFunc(string item);

        public IEnumerable<string> Filter(IEnumerable<string> list, FilterFunc filterFunction)
        {
            var result = new List<string>();

            foreach (var item in list)
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

#region V7
namespace V7A
{
    public class Framework
    {
        public delegate bool FilterFunc<TItem>(TItem item);

        public IEnumerable<TItem> Filter<TItem>(IEnumerable<TItem> list, FilterFunc<TItem> filterFunction)
        {
            var result = new List<TItem>();

            foreach (var item in list)
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

        public IEnumerable<TItem> Filter<TItem>(IEnumerable<TItem> list, FilterFunc<TItem, bool> filterFunction)
        {
            var result = new List<TItem>();

            foreach (var item in list)
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

        public IEnumerable<TItem> Filter<TItem>(IEnumerable<TItem> list, FilterFunc<TItem, bool> filterFunction)
        {
            var result = new List<TItem>();

            foreach (var item in list)
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

#region V8
namespace V8
{
    public class Framework
    {
        // public delegate TResult FilterFunc<in TItem, out TResult>(TItem item);
        // public delegate TResult Func<[Nullable(2)] in T, [Nullable(2)] out TResult>(T arg);

        public IEnumerable<TItem> Filter<TItem>(IEnumerable<TItem> list, Func<TItem, bool> filterFunction)
        {
            var result = new List<TItem>();

            foreach (var item in list)
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

#region V9
namespace V9
{
    public static class Framework
    {
        public static IEnumerable<TItem> Filter<TItem>(this IEnumerable<TItem> list, Func<TItem, bool> filterFunction)
        {
            var result = new List<TItem>();

            foreach (var item in list)
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

#region V10
namespace V10
{
    public static class Framework
    {
        public static IEnumerable<TItem> Where<TItem>(this IEnumerable<TItem> list, Func<TItem, bool> filterFunction)
        {
            var result = new List<TItem>();

            foreach (var item in list)
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

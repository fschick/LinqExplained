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
            var list = new[] {
                "freilebende gummibärchen gibt es nicht." ,
                "man kauft sie in packungen an der kinokasse",
                "dieser kauf ist der beginn einer fast erotischen und sehr ambivalenten beziehung gummibärchen-mensch",
                "zuerst genießt man.",
                "dieser genuss umfasst alle sinne.",
                "man wühlt in den gummibärchen, man fühlt sie.",
                "gummibärchen haben eine konsistenz wie weichgekochter radiergummi.",
                "die tastempfindung geht auch ins sexuelle."
            };

            #region V1
            // V1: Framework and business logic combined into one function.
            var frameworkV1 = new V1.Framework();
            frameworkV1.Filter(list, "StartWith", "man").Dump();
            frameworkV1.Filter(list, "Contains", "man").Dump();
            #endregion

            #region V2
            // V1: Business logic moved to separate class.
            var frameworkV2 = new V2.Framework();
            frameworkV2.Filter(list, "StartWith", "man").Dump();
            frameworkV2.Filter(list, "Contains", "man").Dump();
            #endregion

            #region V3
            // V3: Delegate for filter functions created. Loops combined.
            var frameworkV3 = new V3.Framework();
            frameworkV3.Filter(list, "StartWith", "man").Dump();
            frameworkV3.Filter(list, "Contains", "man").Dump();
            #endregion

            #region V4
            // V4: Parameter 'filterFunction' replaced by delegate.
            var frameworkV4 = new V4.Framework();
            frameworkV4.Filter(list, BusinessLogic.StartsWith, "man").Dump();
            frameworkV4.Filter(list, BusinessLogic.Contains, "man").Dump();
            #endregion

            #region V5
            // V5: Parameter 'filterFunction' replaced by inline function.
            var frameworkV5 = new V5.Framework();
            frameworkV5.Filter(list, (string item, string filter) => item.StartsWith(filter), "man").Dump();
            frameworkV5.Filter(list, (string item, string filter) => item.Contains(filter), "man").Dump();
            #endregion

            #region V6
            // V6: Parameter 'filter' (re)moved to inline function.
            var frameworkV6 = new V6.Framework();
            frameworkV6.Filter(list, (string item) => item.StartsWith("man")).Dump();
            frameworkV6.Filter(list, (string item) => item.Contains("man")).Dump();
            #endregion

            #region V7
            // V7A: Type of list made generic. Inline function simplified.
            var frameworkV7A = new V7A.Framework();
            frameworkV7A.Filter(list, (item) => item.StartsWith("man")).Dump();
            frameworkV7A.Filter(list, (item) => item.Contains("man")).Dump();

            // V7B: Return type of delegate made generic. Inline function simplified.
            var frameworkV7B = new V7B.Framework();
            frameworkV7B.Filter(list, item => item.StartsWith("man")).Dump();
            frameworkV7B.Filter(list, item => item.Contains("man")).Dump();

            // V7C: Type parameters marked as co-variant/contra-variant.
            // Covariance: Enables you to use a more derived type than originally specified.
            // Contravariance: Enables you to use a more generic(less derived) type than originally specified.
            var frameworkV7C = new V7C.Framework();
            frameworkV7C.Filter(list, item => item.StartsWith("man")).Dump();
            frameworkV7C.Filter(list, item => item.Contains("man")).Dump();
            #endregion

            #region V8
            // V8: Delegate replace by Func<,>.
            var frameworkV8 = new V8.Framework();
            frameworkV8.Filter(list, item => item.StartsWith("man")).Dump();
            frameworkV8.Filter(list, item => item.Contains("man")).Dump();
            #endregion

            #region V9
            // V9: Switch to extension method
            V9.Framework.Filter(list, item => item.StartsWith("man")).Dump();
            V9.Framework.Filter(list, item => item.Contains("man")).Dump();
            //list.Filter(item => item.Contains("man")).Dump();
            #endregion

            #region V10
            // V10: 'Filter' renamed to 'Where'
            V10.Framework.Where(list, item => item.StartsWith("man")).Dump();
            V10.Framework.Where(list, item => item.Contains("man")).Dump();

            list.Where(item => item.StartsWith("man")).Dump();
            list.Where(item => item.StartsWith("man")).Dump();
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

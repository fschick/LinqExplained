using FS.LinqExplained;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
            // V1: Framework and business logic combined into same function.
            var frameworkV1 = new V1.Framework();
            frameworkV1.Filter(list, "StartWith", "man").Dump("V1 StartWith");
            frameworkV1.Filter(list, "Contains", "man").Dump("V1 Contains");
            #endregion

            #region V2
            // V1: Business logic moved to separate class.
            var frameworkV2 = new V2.Framework();
            frameworkV2.Filter(list, "StartWith", "man").Dump("V2 StartWith");
            frameworkV2.Filter(list, "Contains", "man").Dump("V2 Contains");
            #endregion

            #region V3
            // V3: Delegate for filter functions created. Loops combined.
            var frameworkV3 = new V3.Framework();
            frameworkV3.Filter(list, "StartWith", "man").Dump("V3 StartWith");
            frameworkV3.Filter(list, "Contains", "man").Dump("V3 Contains");
            #endregion

            #region V4
            // V4: Parameter 'filterFunction' replaced by delegate.
            var frameworkV4 = new V4.Framework();
            frameworkV4.Filter(list, BusinessLogic.StartsWith, "man").Dump("V4 StartWith");
            frameworkV4.Filter(list, BusinessLogic.Contains, "man").Dump("V4 Contains");
            #endregion

            #region V5
            // V5: Parameter 'filterFunction' replaced by inline function.
            var frameworkV5 = new V5.Framework();
            frameworkV5.Filter(list, (string item, string filter) => item.StartsWith(filter), "man").Dump("V5 StartWith");
            frameworkV5.Filter(list, (string item, string filter) => item.Contains(filter), "man").Dump("V5 Contains");
            #endregion

            #region V6
            // V6: Parameter 'filter' (re)moved to inline function.
            var frameworkV6 = new V6.Framework();
            frameworkV6.Filter(list, (string item) => item.StartsWith("man")).Dump("V6 StartWith");
            frameworkV6.Filter(list, (string item) => item.Contains("man")).Dump("V6 Contains");
            #endregion

            #region V7
            // V7A: Type of list made generic. Inline function simplified.
            var frameworkV7A = new V7A.Framework();
            frameworkV7A.Filter(list, (item) => item.StartsWith("man")).Dump("V7A StartWith");
            frameworkV7A.Filter(list, (item) => item.Contains("man")).Dump("V7A Contains");

            // V7B: Return type of delegate made generic. Inline function simplified.
            var frameworkV7B = new V7B.Framework();
            frameworkV7B.Filter(list, item => item.StartsWith("man")).Dump("V7B StartWith");
            frameworkV7B.Filter(list, item => item.Contains("man")).Dump("V7B Contains");

            // V7C: Type parameters marked as co-variant/contra-variant.
            // Covariance: Enables you to use a more derived type than originally specified.
            // Contravariance: Enables you to use a more generic(less derived) type than originally specified.
            var frameworkV7C = new V7C.Framework();
            frameworkV7C.Filter(list, item => item.StartsWith("man")).Dump("V7C StartWith");
            frameworkV7C.Filter(list, item => item.Contains("man")).Dump("V7C Contains");
            #endregion

            #region V8
            // V8: Delegate replace by Func<,>.
            var frameworkV8 = new V8.Framework();
            frameworkV8.Filter(list, item => item.StartsWith("man")).Dump("V8 StartWith");
            frameworkV8.Filter(list, item => item.Contains("man")).Dump("V8 Contains");
            #endregion

            #region V9
            // V9: Switch to extension method
            V9.Framework.Filter(list, item => item.StartsWith("man")).Dump("V9 StartWith");
            V9.Framework.Filter(list, item => item.Contains("man")).Dump("V9 Contains");
            //list.Filter(item => item.Contains("man")).Dump("V1 StartWith");
            #endregion

            #region V10
            // V10: 'Filter' renamed to 'Where'
            V10.Framework.Where(list, item => item.StartsWith("man")).Dump("V10 StartWith");
            V10.Framework.Where(list, item => item.Contains("man")).Dump("V10 Contains");

            list.Where(item => item.StartsWith("man")).Dump(".NET Core StartsWith");
            list.Where(item => item.Contains("man")).Dump(".NET Core Contains");
            #endregion

            #region V11
            V11.Framework.Where(list, item => item.StartsWith("man")).Dump("results in");
            V11.Framework.Where(list, item => item.StartsWith("man") || item.Contains("beziehung")).Dump("results in");
            V11.Framework.Where(list, item => item.StartsWith("man") && item.Contains("wühlt")).Dump("results in");
            #endregion

            #region V12
            var containsMethodInfoV12 = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });

            // Part 'item' of item => item.Contains("man")
            var itemParameterExpressionV12 = Expression.Parameter(typeof(string), "item");

            // Part '"man"' of item => item.Contains("man")
            var valueExpressionV12 = Expression.Constant("man", typeof(string));

            // Part 'item.Contains("man")' of item => item.Contains("man")
            var stringContainsExpressionV12 = Expression.Call(itemParameterExpressionV12, containsMethodInfoV12, valueExpressionV12);

            // Full 'item => item.Contains("man")'
            var itemContainsValueLambdaExpressionV12 = Expression.Lambda(stringContainsExpressionV12, itemParameterExpressionV12);

            // Cast to requested expression type
            var typedItemContainsValueLambdaExpressionV12 = (Expression<Func<string, bool>>)itemContainsValueLambdaExpressionV12;

            // Execute
            V11.Framework.Where(list, typedItemContainsValueLambdaExpressionV12).Dump("results in");
            #endregion

            #region V13
            var filterValue = "!man";

            var containsMethodInfoV13 = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });

            // Part 'item' of item => item.Contains("man")
            var itemParameterExpressionV13 = Expression.Parameter(typeof(string), "item");

            var negateRequested = filterValue.StartsWith('!');
            if (negateRequested)
                filterValue = filterValue.Substring(1);

            // Part '"man"' of item => item.Contains("man")
            var valueExpressionV13 = Expression.Constant(filterValue, typeof(string));

            // Part 'item.Contains("man")' of item => item.Contains("man")
            var stringContainsExpressionV13 = Expression.Call(itemParameterExpressionV13, containsMethodInfoV13, valueExpressionV13);

            var stringContainsOrNotExpressionV13 = negateRequested
                ? (Expression)Expression.Not(stringContainsExpressionV13)
                : (Expression)stringContainsExpressionV13;

            // Full 'item => item.Contains("man")'
            var itemContainsOrNotValueLambdaExpressionV13 = Expression.Lambda(stringContainsOrNotExpressionV13, itemParameterExpressionV13);

            // Cast to requested expression type
            var typedItemContainsOrNotValueLambdaExpressionV13 = (Expression<Func<string, bool>>)itemContainsOrNotValueLambdaExpressionV13;

            // Execute
            V11.Framework.Where(list, typedItemContainsOrNotValueLambdaExpressionV13).Dump("results in");
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

#region V11
namespace V11
{
    public static class Framework
    {
        public static IEnumerable<TItem> Where<TItem>(this IEnumerable<TItem> list, Expression<Func<TItem, bool>> filterExpression)
        {
            var result = new List<TItem>();

            var filterDescription = GetExpressionDescription(filterExpression);
            filterDescription.Dump("V11 Filters the the list as follow");

            var filterFunction = filterExpression.Compile();
            foreach (var item in list)
            {
                var isMatch = filterFunction(item);
                if (isMatch)
                    result.Add(item);
            }

            return result;
        }

        private static string GetExpressionDescription(Expression expression)
        {
            switch (expression)
            {
                case LambdaExpression lambdaExpression:
                    return GetLambdaExpressionDescription(lambdaExpression);
                case MethodCallExpression methodCallExpression:
                    return GetMethodCallDescription(methodCallExpression);
                case BinaryExpression binaryExpression:
                    return GetLogicalOperationDescription(binaryExpression);
                case UnaryExpression unaryExpression:
                    return GetNegatedExpressionDescription(unaryExpression);
                default:
                    throw new NotSupportedException($"Expressions of type {expression.GetType()} are currently unsupported");
            }
        }

        private static string GetLambdaExpressionDescription(LambdaExpression lambdaExpression)
        {
            var parameterDescription = lambdaExpression.Parameters[0].Name;
            var bodyDescription = GetExpressionDescription(lambdaExpression.Body);
            return $"{parameterDescription} {bodyDescription}";
        }

        private static string GetMethodCallDescription(MethodCallExpression methodCallExpression)
        {
            if (methodCallExpression.Arguments.Count > 1)
                throw new NotSupportedException("Methods calls with none or more than one argument currently unsupported");

            string argument;
            var argumentExpression = methodCallExpression.Arguments[0];
            switch (argumentExpression)
            {
                case ConstantExpression constantExpression:
                    argument = constantExpression.Value.ToString();
                    break;
                default:
                    throw new NotSupportedException($"Arguments of type {argumentExpression.GetType()} for method calls currently unsupported");
            }

            return $"{methodCallExpression.Method.Name.Humanize(LetterCasing.LowerCase)} \"{argument}\"";
        }

        private static string GetLogicalOperationDescription(BinaryExpression binaryExpression)
        {
            switch (binaryExpression.NodeType)
            {
                case ExpressionType.OrElse:
                    return $"{GetExpressionDescription(binaryExpression.Left)} or {GetExpressionDescription(binaryExpression.Right)}";
                case ExpressionType.AndAlso:
                    return $"{GetExpressionDescription(binaryExpression.Left)} and {GetExpressionDescription(binaryExpression.Right)}";
                default:
                    throw new NotSupportedException($"Node type '{binaryExpression.NodeType}' for binary expressions are currently unsupported");
            }
        }

        private static string GetNegatedExpressionDescription(UnaryExpression unaryExpression)
        {
            string prefix;
            switch (unaryExpression.NodeType)
            {
                case ExpressionType.Not:
                    prefix = "not";
                    break;
                default:
                    throw new NotSupportedException($"Node type '{unaryExpression.NodeType}' for unary expressions are currently unsupported");
            }

            return $"{prefix} {GetExpressionDescription(unaryExpression.Operand)}";
        }
    }
}
#endregion

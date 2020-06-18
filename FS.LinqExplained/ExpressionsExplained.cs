using FS.LinqExplained;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FS.LinqExplained
{
    public class ExpressionsExplained
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

            #region V11 Expression evaluation
            ExpressionEvaluation.Framework.Where(dummyTextLines, item => item.StartsWith("man")).Dump("results in");
            ExpressionEvaluation.Framework.Where(dummyTextLines, item => item.StartsWith("man") || item.Contains("beziehung")).Dump("results in");
            ExpressionEvaluation.Framework.Where(dummyTextLines, item => item.StartsWith("man") && item.Contains("wühlt")).Dump("results in");
            #endregion

            #region V12 Expression creation
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
            ExpressionEvaluation.Framework.Where(dummyTextLines, typedItemContainsValueLambdaExpressionV12).Dump("results in");
            #endregion

            #region V13 Parser creation
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
            ExpressionEvaluation.Framework.Where(dummyTextLines, typedItemContainsOrNotValueLambdaExpressionV13).Dump("results in");
            #endregion
        }
    }
}

#region ExpressionEvaluation
namespace ExpressionEvaluation
{
    public static class Framework
    {
        public static IEnumerable<TItem> Where<TItem>(this IEnumerable<TItem> list, Expression<Func<TItem, bool>> filterExpression)
        {
            var result = new List<TItem>();

            var filterDescription = GetExpressionDescription(filterExpression);
            filterDescription.Dump("Filters the the list as follow");

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
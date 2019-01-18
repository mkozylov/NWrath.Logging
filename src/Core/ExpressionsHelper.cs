using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NWrath.Logging
{
    internal static class ExpressionsHelper
    {
        public static Expression BuildStringConcat(Expression[] strExprs)
        {
            var body = default(Expression);

            if (strExprs.Length == 1)
            {
                body = strExprs[0];
            }
            else if (strExprs.Length == 2)
            {
                body = Expression.Call(
                    typeof(string).GetMethod(nameof(string.Concat),
                    new[] { typeof(string), typeof(string) }),
                    strExprs[0],
                    strExprs[1]
                    );
            }
            else if (strExprs.Length == 3)
            {
                body = Expression.Call(
                    typeof(string).GetMethod(nameof(string.Concat),
                    new[] { typeof(string), typeof(string), typeof(string) }),
                    strExprs[0],
                    strExprs[1],
                    strExprs[2]
                    );
            }
            else if (strExprs.Length == 4)
            {
                body = Expression.Call(
                    typeof(string).GetMethod(nameof(string.Concat),
                    new[] { typeof(string), typeof(string), typeof(string), typeof(string) }),
                    strExprs[0],
                    strExprs[1],
                    strExprs[2],
                    strExprs[3]
                    );
            }
            else
            {
                body = Expression.Call(
                    typeof(string).GetMethod(nameof(string.Concat),
                    new[] { typeof(string[]) }),
                    Expression.NewArrayInit(typeof(string), strExprs)
                    );
            }

            return body;
        }
    }
}
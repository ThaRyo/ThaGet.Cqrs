using System;
using System.Linq.Expressions;

namespace ThaGet.Cqrs.Filter.Extensions
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Combines two expressions with logical AND operation.
        /// </summary>
        /// <typeparam name="T">Type the expressions are being used on.</typeparam>
        /// <param name="leftExp">Expression left of the AND operation.</param>
        /// <param name="rightExp">Expression right of the AND operation.</param>
        /// <param name="parameter">Parameter to be used in left, right and combined expressions.</param>
        /// <returns>The expression combining both with AND operation.</returns>
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> leftExp, Expression<Func<T, bool>> rightExp, ParameterExpression parameter)
        {
            var left = ReplaceParameter(leftExp, parameter);
            var right = ReplaceParameter(rightExp, parameter);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
        }

        /// <summary>
        /// Combines two expressions with logical OR operation.
        /// </summary>
        /// <typeparam name="T">Type the expressions are being used on.</typeparam>
        /// <param name="leftExp">Expression left of the OR operation.</param>
        /// <param name="rightExp">Expression right of the OR operation.</param>
        /// <param name="parameter">Parameter to be used in left, right and combined expressions.</param>
        /// <returns>The expression combining both with OR operation.</returns>
        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> leftExp, Expression<Func<T, bool>> rightExp, ParameterExpression parameter)
        {
            var left = ReplaceParameter(leftExp, parameter);
            var right = ReplaceParameter(rightExp, parameter);

            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(left, right), parameter);
        }

        /// <summary>
        /// Replaces the parameter of the expression with given one.
        /// </summary>
        /// <typeparam name="T">Type the expression is being used on.</typeparam>
        /// <param name="expression">Expression to be changed.</param>
        /// <param name="parameter">Parameter for the new expression.</param>
        /// <returns>The expression with the new parameter.</returns>
        private static Expression ReplaceParameter<T>(Expression<Func<T, bool>> expression, ParameterExpression parameter)
        {
            var visitor = new ReplaceParameterVisitor(expression.Parameters[0], parameter);
            var newexpression = visitor.Visit(expression.Body);

            return newexpression;
        }

        /// <summary>
        /// Helper class for replacing parameters.
        /// </summary>
        private class ReplaceParameterVisitor : ExpressionVisitor
        {
            private ParameterExpression _oldParameter;
            private ParameterExpression _newParameter;

            public ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (ReferenceEquals(node, _oldParameter))
                    return _newParameter;

                return base.Visit(node);
            }
        }
    }
}

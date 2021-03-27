using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ThaGet.Cqrs.Domain.Entities.Abstractions;
using ThaGet.Cqrs.Filter.Extensions;

namespace ThaGet.Cqrs.Filter.Internal
{
    internal class FilterPredicateBuilder<TEntity, TId>
        where TEntity : IEntity<TId>
        where TId : struct
    {
        private readonly Dictionary<string, Expression<Func<TEntity, object>>> _variableDefinitions;

        public FilterPredicateBuilder()
        {
            _variableDefinitions = new Dictionary<string, Expression<Func<TEntity, object>>>();
        }

        public void AddVariableDefinition(string variableName, Expression<Func<TEntity, object>> expression)
        {
            _variableDefinitions.Add(variableName, expression);
        }

        public Expression<Func<TEntity, bool>> BuildFromString(string propertyName, string operatorValue, string value)
        {
            var variableExpression = _variableDefinitions.FirstOrDefault(o => o.Key.ToLower() == propertyName.ToLower()).Value;
            if (variableExpression != null)
            {
                if (value.StartsWith('[') && value.EndsWith(']'))
                    return BuildOrFromString(propertyName, operatorValue, value);

                if (value.StartsWith('(') && value.EndsWith(')'))
                    return BuildAndFromString(propertyName, operatorValue, value);

                var body = variableExpression.Body;
                if (body.NodeType == ExpressionType.Convert)
                    body = ((UnaryExpression)body).Operand;

                var type = body.Type;
                ConstantExpression valueConstant = null;

                try
                {
                    valueConstant = GetValueConstant(type, value);
                }
                catch (Exception)
                {
                    return null;
                }

                var containsMethodExp = GetMethodCallExpression(body, operatorValue, valueConstant, type);
                var predicate = Expression.Lambda<Func<TEntity, bool>>(containsMethodExp, variableExpression.Parameters);

                return predicate;
            }
            return null;
        }

        private ConstantExpression GetValueConstant(Type type, string value)
        {
            ConstantExpression valueConstant;

            if (type == typeof(int) || type == typeof(int?))
            {
                var valueInt = int.Parse(value);
                valueConstant = Expression.Constant(valueInt, type);
            }
            else if (type == typeof(double) || type == typeof(double?))
            {
                var valueDouble = double.Parse(value);
                valueConstant = Expression.Constant(valueDouble, type);
            }
            else if (type == typeof(bool))
            {
                var valueBool = bool.Parse(value);
                valueConstant = Expression.Constant(valueBool, type);
            }
            else if (type == typeof(Guid) || type == typeof(Guid?))
            {
                var valueGuid = Guid.Parse(value);
                valueConstant = Expression.Constant(valueGuid, type);
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                var valueDateTime = DateTime.Parse(value);
                valueConstant = Expression.Constant(valueDateTime, type);
            }
            else if (type.IsEnum)
            {
                var valueInt = int.Parse(value);

                if (!type.IsEnumDefined(valueInt))
                    throw new ArgumentException($"Value '{value}' doesn't exist for type '{type.FullName}", nameof(value));

                var valueEnum = Enum.ToObject(type, valueInt);
                valueConstant = Expression.Constant(valueEnum, type);
            }
            else if (type.IsGenericType)
            {
                do
                {
                    type = type.GenericTypeArguments[0];
                } while (type.IsGenericType);

                valueConstant = GetValueConstant(type, value);
            }
            else
            {
                valueConstant = Expression.Constant(value, type);
            }

            return valueConstant;
        }

        private Expression<Func<TEntity, bool>> BuildOrFromString(string propertyName, string operatorValue, string value)
        {
            Expression<Func<TEntity, bool>> combinedExpression = null;
            var valueParts = value.Trim('[', ']').Split(';');

            var combinedParameter = Expression.Parameter(typeof(TEntity), "o");

            foreach (var valuePart in valueParts)
            {
                var predicatePart = BuildFromString(propertyName, operatorValue, valuePart);

                if (combinedExpression == null)
                    combinedExpression = predicatePart;
                else
                    combinedExpression = combinedExpression.OrElse(predicatePart, combinedParameter);
            }

            return combinedExpression;
        }

        private Expression<Func<TEntity, bool>> BuildAndFromString(string propertyName, string operatorValue, string value)
        {
            Expression<Func<TEntity, bool>> combinedExpression = null;
            var valueParts = value.Trim('(', ')').Split(';');

            var combinedParameter = Expression.Parameter(typeof(TEntity), "o");

            foreach (var valuePart in valueParts)
            {
                var predicatePart = BuildFromString(propertyName, operatorValue, valuePart);

                if (combinedExpression == null)
                    combinedExpression = predicatePart;
                else
                    combinedExpression = combinedExpression.AndAlso(predicatePart, combinedParameter);
            }

            return combinedExpression;
        }

        private Expression GetMethodCallExpression(Expression body, string operatorValue, ConstantExpression value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var subType = type;
                do
                {
                    subType = subType.GenericTypeArguments[0];
                } while (subType.IsGenericType);

                var methods = typeof(Enumerable).GetMethods();
                var methodAny = methods.FirstOrDefault(m => m.Name == "Any" && m.GetParameters().Length == 2);

                var subParameter = Expression.Parameter(subType, "sub");
                var subMethod = GetMethodCallExpression(subParameter, operatorValue, value, subType);
                var subLambda = Expression.Lambda(subMethod, subParameter);

                var bodyAny = Expression.Call(typeof(Enumerable), "Any", new[] { subType }, body, subLambda);
                //return Expression.Call(body, genericMethod, subMethod);
                return bodyAny;
            }

            switch (operatorValue)
            {
                case "=":
                    if (type == typeof(string))
                    {
                        var method = type.GetMethod("Contains", new[] { type });
                        return Expression.Call(body, method, value);
                    }
                    break;

                case "==":
                    return Expression.Equal(body, value);

                case "<":
                    return Expression.LessThan(body, value);

                case "<=":
                    return Expression.LessThanOrEqual(body, value);
                case ">":
                    return Expression.GreaterThan(body, value);
                case ">=":
                    return Expression.GreaterThanOrEqual(body, value);
                case "!=":
                    return Expression.NotEqual(body, value);
            }

            throw new Exception($"Invalid filter with operator {operatorValue} and type {type}");
        }
    }
}
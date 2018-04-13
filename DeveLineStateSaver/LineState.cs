using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DeveLineStateSaver
{
    public class LineState
    {
        public string MethodName { get; set; }
        public List<Argument> Arguments { get; } = new List<Argument>();
        public string Output { get; set; }

        public LineState()
        {

        }

        public LineState(MethodCallExpression methodCallExpression)
        {
            MethodName = methodCallExpression.Method.Name;

            for (int i = 0; i < methodCallExpression.Arguments.Count; i++)
            {
                var arg = methodCallExpression.Arguments[i];
                var argAsConstant = arg as ConstantExpression;

                if (argAsConstant == null)
                {
                    var memberExpression = arg as MemberExpression;

                    if (memberExpression == null)
                    {
                        throw new InvalidOperationException($"Argument [{i}] for {MethodName} is not of type MemberExpression or ConstantExpression but of type {arg.GetType()}. Value: {arg}");
                    }

                    var retval = ExpressionHelper.GetValue(memberExpression);

                    var argument2 = new Argument(arg.Type.FullName, retval);
                    Arguments.Add(argument2);
                }
                else
                {
                    var argument = new Argument(argAsConstant.Type.FullName, argAsConstant.Value);
                    Arguments.Add(argument);
                }
            }
        }

        public bool IsEqualTo(LineState other)
        {
            if (MethodName != other.MethodName)
            {
                return false;
            }

            if (Arguments.Count != other.Arguments.Count)
            {
                return false;
            }

            for (int i = 0; i < Arguments.Count; i++)
            {
                var argMe = Arguments[i];
                var argOther = other.Arguments[i];

                if (!argMe.IsEqualTo(argOther))
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            return $"{MethodName}({string.Join(", ", Arguments.Select(t => t.ToString()))})";
        }
    }
}

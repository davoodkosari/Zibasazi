using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Radyn.CrossPlatform.Tools
{
    public class SyncAction<TModel>
    {
        private bool customMethod { get; set; }

        private object staticValue { get; set; }
        private Expression<Func<TModel, object>> method { get; set; }
        private string customPropertyName { get; set; }
        //public bool InjectionChecker { get; set; }

        public SyncAction(object staticValue, string propertyName)
        {
            customMethod = true;
            //this.InjectionChecker = injectionChecker;
            this.staticValue = staticValue;
            this.customPropertyName = propertyName;
        }

        public SyncAction(Expression<Func<TModel, object>> action, string propertyName = null)
        {
            customMethod = false;
            //this.InjectionChecker = injectionChecker;
            this.method = action;
            this.customPropertyName = propertyName;
        }

        public string GetName()
        {
            if (customPropertyName == null)
            {
                var propInfo = GetPropertyInfo(method);
                return propInfo.Name;
            }
            else
            {
                return customPropertyName;
            }
        }

        public object GetValue(TModel obj)
        {
            if (customMethod == false)
            {
                var cmp = method.Compile();
                var result = cmp(obj);
                return result;
            }
            else
            {
                return staticValue;
            }
            

            //if (InjectionChecker)
            //{
            //    if (isString(result) == true)
            //    {
            //        return removeInjection(result.ToString());
            //    }
            //}
        }

        private string removeInjection(string text)
        {
            text = text.Replace("'", "''");
            return text;
        }

        private bool isString(object obj)
        {
            if (
                    obj is decimal ||
                    obj is byte ||
                    obj is int ||
                    obj is short ||
                    obj is long ||
                    obj is float ||
                    obj is double ||
                    obj is char ||
                    obj is sbyte ||
                    obj is uint ||
                    obj is ulong ||
                    obj is ushort ||
                    obj is Guid ||
                    obj is bool
                    )
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool isNumeric(object obj)
        {
            if (
                    obj is decimal ||
                    obj is byte ||
                    obj is int ||
                    obj is short ||
                    obj is long ||
                    obj is float ||
                    obj is double ||
                    obj is char ||
                    obj is sbyte ||
                    obj is uint ||
                    obj is ulong ||
                    obj is ushort
                    )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static PropertyInfo GetPropertyInfo<T>(Expression<Func<T, object>> expression)
        {
            MemberExpression memberExpression = null;

            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (memberExpression == null)
            {
                throw new ArgumentException("Not a member access", "expression");
            }

            return memberExpression.Member as PropertyInfo;
        }
    }
}

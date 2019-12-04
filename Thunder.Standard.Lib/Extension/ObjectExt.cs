using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Thunder.Standard.Lib.Extension
{
    public static class ObjectExt
    {
        public static T Copy<T>(this T obj) where T : new()
        {
            if (obj == null)
            {
                return obj;
            }
            Object targetDeepCopyObj;
            Type targetType = obj.GetType();
            //值类型
            if (targetType.IsValueType == true)
            {
                targetDeepCopyObj = obj;
            }
            //引用类型 
            else
            {
                targetDeepCopyObj = System.Activator.CreateInstance(targetType);   //创建引用对象 
                System.Reflection.MemberInfo[] memberCollection = obj.GetType().GetMembers();

                foreach (System.Reflection.MemberInfo member in memberCollection)
                {
                    if (member.MemberType == System.Reflection.MemberTypes.Field)
                    {
                        System.Reflection.FieldInfo field = (System.Reflection.FieldInfo)member;
                        Object fieldValue = field.GetValue(obj);
                        if (fieldValue is ICloneable)
                        {
                            field.SetValue(targetDeepCopyObj, (fieldValue as ICloneable).Clone());
                        }
                        else
                        {
                            field.SetValue(targetDeepCopyObj, Copy(fieldValue));
                        }

                    }
                    else if (member.MemberType == System.Reflection.MemberTypes.Property)
                    {
                        System.Reflection.PropertyInfo myProperty = (System.Reflection.PropertyInfo)member;
                        MethodInfo info = myProperty.GetSetMethod(false);
                        if (info != null)
                        {
                            object propertyValue = myProperty.GetValue(obj, null);
                            if (propertyValue is ICloneable)
                            {
                                myProperty.SetValue(targetDeepCopyObj, (propertyValue as ICloneable).Clone(), null);
                            }
                            else
                            {
                                if (propertyValue != null)
                                    myProperty.SetValue(targetDeepCopyObj, Copy(propertyValue), null);
                            }
                        }

                    }
                }
            }
            return (T)targetDeepCopyObj;
        }

        public static List<T> Copy<T>(this List<T> obj) where T : new()
        {
            var result = new List<T>();
            foreach (var item in obj)
            {
                result.Add(item.Copy());
            }
            return result;
        }

        /// <summary>
        /// 复制对象到剪贴板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="mode">复制模式：0:表格模式(制表符分隔) , 1:文本模式（换行分隔）</param>
        /// <returns></returns>
        public static string ClipString(this object obj,int mode=0)
        {
            var items = new List<string>();
            Type targetType = obj.GetType();
            //值类型
            if (targetType.IsValueType == true)
            {
                return obj.ToString();
            }
            //引用类型 
            else
            {
                //targetDeepCopyObj = System.Activator.CreateInstance(targetType);   //创建引用对象 
                System.Reflection.MemberInfo[] memberCollection = obj.GetType().GetMembers();

                foreach (System.Reflection.MemberInfo member in memberCollection)
                {
                    if (member.MemberType == System.Reflection.MemberTypes.Field)
                    {
                        System.Reflection.FieldInfo field = (System.Reflection.FieldInfo)member;
                        Object fieldValue = field.GetValue(obj);
                        if (fieldValue is ICloneable)
                        {
                            items.Add((fieldValue as ICloneable).ToString());
                            //field.SetValue(targetDeepCopyObj, (fieldValue as ICloneable).Clone());
                        }
                        else
                        {
                            items.Add(fieldValue.ToString());
                            //field.SetValue(targetDeepCopyObj, Copy(fieldValue));
                        }

                    }
                    else if (member.MemberType == System.Reflection.MemberTypes.Property)
                    {
                        System.Reflection.PropertyInfo myProperty = (System.Reflection.PropertyInfo)member;
                        MethodInfo info = myProperty.GetSetMethod(false);
                        if (info != null)
                        {
                            object propertyValue = myProperty.GetValue(obj, null);
                            if (propertyValue is ICloneable)
                            {
                                items.Add((propertyValue as ICloneable).ToString());
                                //myProperty.SetValue(targetDeepCopyObj, (propertyValue as ICloneable).Clone(), null);
                            }
                            else
                            {
                                if (propertyValue != null)
                                {
                                    items.Add(propertyValue.ToString());
                                    //myProperty.SetValue(targetDeepCopyObj, Copy(propertyValue), null);
                                }
                                else
                                {
                                    items.Add("");
                                }
                            }
                        }

                    }
                }

                return string.Join("\t", items);
            }

        }

        /// <summary>
        /// 复制对象队列到剪贴板(可粘贴到Excel中)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listObj"></param>
        /// <returns></returns>
        public static string ClipString<T>(this IList<T> listObj)
        {
            var list = new List<string>();
            foreach (var item in listObj)
            {
                list.Add(item.ClipString());
            }
            return string.Join("\r\n", list);
        }

        /// <summary>
        /// 判断两个对象值相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ValueEqual<T>(this T obja, T objb)
        {
            if (obja == null && objb == null)
            {
                return true;
            }
            if (obja == null || objb == null)
            {
                return false;
            }

            //JSON对比
            if (obja.ToJson() == objb.ToJson())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T">转换的类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="nullOrTypeErrorThrow">空值或者类型错误抛出错误（默认：true）</param>
        /// <param name="customErrorMessage">定制异常消息</param>
        /// <returns></returns>
        public static T ToType<T>(this object obj, bool nullOrTypeErrorThrow = true, string customErrorMessage = null)
        {
            if (obj == null)
            {
                if (nullOrTypeErrorThrow)
                {
                    var err = string.IsNullOrWhiteSpace(customErrorMessage) ? "obj is null." : customErrorMessage;
                    throw new NullReferenceException(err);
                }
                return default;
            }
            if (obj is T)
            {
                return (T)obj;
            }
            else
            {
                if (nullOrTypeErrorThrow)
                {
                    var err = string.IsNullOrWhiteSpace(customErrorMessage) ? $"obj can not convert to type {typeof(T).Name}." : customErrorMessage;
                    throw new ArgumentException(err);
                }

            }
            return default;
        }

        /// <summary>
        /// 空值检查
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objValue"></param>
        public static void NullCheck<T>(this T objValue)
        {
            if (objValue == null)
            {
                throw new ArgumentNullException(nameof(objValue));
            }
        }

        /// <summary>
        /// 空值检查（链式编程）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objValue"></param>
        /// <returns>原始对象</returns>
        public static T NotNull<T>(this T objValue)
        {
            objValue.NullCheck();
            return objValue;
        }
    }

    public static class TransExp<TIn, TOut>
    {

        private static readonly Func<TIn, TOut> cache = GetFunc();
        private static Func<TIn, TOut> GetFunc()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
            List<MemberBinding> memberBindingList = new List<MemberBinding>();

            foreach (var item in typeof(TOut).GetProperties())
            {
                if (!item.CanWrite)
                    continue;

                MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
                MemberBinding memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }

            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
            Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[] { parameterExpression });

            return lambda.Compile();
        }

        public static TOut Trans(TIn tIn)
        {
            return cache(tIn);
        }

    }
}

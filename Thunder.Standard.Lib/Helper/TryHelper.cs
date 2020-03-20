// <copyright file="TryHelper.cs" author="linya">
// Create time：       2020/3/17 9:17:47
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace Thunder.Standard.Lib.Helper
{
    public class TryHelper
    {
		/// <summary>
		/// 尝试执行 Func<T> ，如果错误则返回 defaultValue
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="function"></param>
		/// <param name="defaultValue">默认值</param>
		/// <returns></returns>
		public static T TryFun<T>(Func<T> function,T defaultValue=default(T), Action<Exception> exAction = null)
        {
			T result = default(T);
			try
			{
				result = function.Invoke();
			}
			catch (Exception ex)
			{
				result = defaultValue;
				exAction?.Invoke(ex);
			}
			return result;
        }

		/// <summary>
		/// 尝试执行 Action 并忽略错误
		/// </summary>
		/// <param name="action"></param>
		public static void TryAction(Action action,Action<Exception> exAction=null)
		{
			try
			{
				action?.Invoke();
			}
			catch(Exception ex) { exAction?.Invoke(ex); }
		}
    }
}

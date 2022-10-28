using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Thunder.Standard.Lib.Extension
{
    public static class ListExt
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> list)
        {
            var l = new ObservableCollection<T>(list);
            return l;
        }
        public static bool HasValue<T>(this IList<T> list)
        {
            return (list?.Count ?? 0) > 0;
        }

        public static bool IsEmpty<T>(this IList<T> list) => !HasValue(list);

    }
}

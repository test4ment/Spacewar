﻿public interface ITree<T, U> where T : notnull where U : notnull
{
    public IDictionary<T, U> tree { get; }
    // public static abstract void AddRecord(T[] record, U value, IDictionary<T, object> tree);
}

// public class UniversalTree : ITree<object, object>
// {
//     // public IDictionary<object, object> tree { get; } = new Dictionary<object, object>();

//     // public static void AddRecord(object[] record, object value, IDictionary<object, object> tree)
//     // {
//     //     try
//     //     {
//     //         _ = tree.TryAdd(record[0], new Dictionary<object, object>());
//     //         AddRecord(record[1..], value, (IDictionary<object, object>)tree[record[0]]);
//     //     }
//     //     catch (IndexOutOfRangeException)
//     //     {
//     //         tree[record[0]] = value;
//     //     }
//     // }
// }

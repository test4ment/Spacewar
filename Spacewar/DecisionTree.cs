public class UniversalTree{
    public Dictionary<object, object> tree {get;} = new Dictionary<object, object>();

    public void AddRecord(object[] record, object value, IDictionary<object, object> tree){
        try{
            tree.TryAdd(record[0], new Dictionary<object, object>());
            AddRecord(record[1..], value, (IDictionary<object, object>)tree[record[0]]);
        }
        catch(IndexOutOfRangeException){
            tree[record[0]] = value;
        }
    }

//     public object GetDecision(object[] features){
//         throw new NotImplementedException();
//     }
}
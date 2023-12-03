public interface IDecisionTree<T, U>{
    public IReadOnlyDictionary<object, object> tree {get;}

    public void AddRecord(T[] record);

    public U GetDecision(T[] features);
}
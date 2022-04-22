namespace Replacement.Repository.Storage;
public class MyGrainState {
    public IDBContext? DBContext { get; set; }
}
public class MyGrainState<TItem> : MyGrainState {
    public TItem? Value { get; set; }
}

public class TodoGrainState : MyGrainState<Todo> { }
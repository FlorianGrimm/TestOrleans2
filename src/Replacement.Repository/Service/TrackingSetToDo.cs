using Replacement.Contracts.Entity;

namespace Replacement.Repository.Service;

public sealed class TrackingSetToDo : TrackingSet<ToDoPK, ToDo> {
    public TrackingSetToDo(DBContext context, ITrackingSetApplyChanges<ToDo> trackingApplyChanges)
        : base(
            extractKey: ToDoUtiltiy.Instance,
            comparer: ToDoUtiltiy.Instance,
            trackingContext: context,
            trackingApplyChanges: trackingApplyChanges) {
    }
}

public sealed class TrackingSetApplyChangesToDo : TrackingSetApplyChangesBase<ToDo, ToDoPK> {
    private static TrackingSetApplyChangesToDo? _Instance;
    public static TrackingSetApplyChangesToDo Instance => _Instance ??= new TrackingSetApplyChangesToDo();

    private TrackingSetApplyChangesToDo() : base() { }

    protected override ToDoPK ExtractKey(ToDo value) => value.GetPrimaryKey();

    public override Task<ToDo> Insert(ToDo value, ITrackingTransConnection trackingTransaction) {
        return this.Upsert(value, trackingTransaction);
    }

    public override Task<ToDo> Update(ToDo value, ITrackingTransConnection trackingTransaction) {
        return this.Upsert(value, trackingTransaction);
    }

    private async Task<ToDo> Upsert(ToDo value, ITrackingTransConnection trackingTransaction) {
        var sqlAccess = (ISqlAccess)trackingTransaction;
        var result = await sqlAccess.ExecuteToDoUpsertAsync(value);
        return this.ValidateUpsertDataManipulationResult(value, result);
    }

    public override async Task Delete(ToDo value, ITrackingTransConnection trackingTransaction) {
        var sqlAccess = (ISqlAccess)trackingTransaction;
        var result = await sqlAccess.ExecuteToDoDeletePKAsync(value);
        this.ValidateDelete(value, result);
    }
}

public sealed class ToDoUtiltiy
    : IEqualityComparer<ToDoPK>
    , IExtractKey<ToDo, ToDoPK> {
    private static ToDoUtiltiy? _Instance;
    public static ToDoUtiltiy Instance => (_Instance ??= new ToDoUtiltiy());
    private ToDoUtiltiy() { }

    public ToDoPK ExtractKey(ToDo that) => that.GetPrimaryKey();

    bool IEqualityComparer<ToDoPK>.Equals(ToDoPK? x, ToDoPK? y) {
        if (object.ReferenceEquals(x, y)) {
            return true;
        } else if ((x is null) || (y is null)) {
            return false;
        } else {
            return x.Equals(y);
        }
    }

    int IEqualityComparer<ToDoPK>.GetHashCode(ToDoPK obj) {
        return obj.GetHashCode();
    }
}
using Replacement.Contracts.Entity;

namespace Replacement.Repository.Service;

public sealed class TrackingSetToDo : TrackingSet<ToDoPK, ToDoEntity> {
    public TrackingSetToDo(DBContext context, ITrackingSetApplyChanges<ToDoEntity> trackingApplyChanges)
        : base(
            extractKey: ToDoUtiltiy.Instance,
            comparer: ToDoUtiltiy.Instance,
            trackingContext: context,
            trackingApplyChanges: trackingApplyChanges) {
    }
}

public sealed class TrackingSetApplyChangesToDo : TrackingSetApplyChangesBase<ToDoEntity, ToDoPK> {
    private static TrackingSetApplyChangesToDo? _Instance;
    public static TrackingSetApplyChangesToDo Instance => _Instance ??= new TrackingSetApplyChangesToDo();

    private TrackingSetApplyChangesToDo() : base() { }

    protected override ToDoPK ExtractKey(ToDoEntity value) => value.GetPrimaryKey();

    public override Task<ToDoEntity> Insert(ToDoEntity value, ITrackingTransConnection trackingTransaction) {
        return this.Upsert(value, trackingTransaction);
    }

    public override Task<ToDoEntity> Update(ToDoEntity value, ITrackingTransConnection trackingTransaction) {
        return this.Upsert(value, trackingTransaction);
    }

    private async Task<ToDoEntity> Upsert(ToDoEntity value, ITrackingTransConnection trackingTransaction) {
        var sqlAccess = (ISqlAccess)trackingTransaction;
        var result = await sqlAccess.ExecuteToDoUpsertAsync(value);
        return this.ValidateUpsertDataManipulationResult(value, result);
    }

    public override async Task Delete(ToDoEntity value, ITrackingTransConnection trackingTransaction) {
        var sqlAccess = (ISqlAccess)trackingTransaction;
        var result = await sqlAccess.ExecuteToDoDeletePKAsync(value);
        this.ValidateDelete(value, result);
    }
}

public sealed class ToDoUtiltiy
    : IEqualityComparer<ToDoPK>
    , IExtractKey<ToDoEntity, ToDoPK> {
    private static ToDoUtiltiy? _Instance;
    public static ToDoUtiltiy Instance => (_Instance ??= new ToDoUtiltiy());
    private ToDoUtiltiy() { }

    public ToDoPK ExtractKey(ToDoEntity that) => that.GetPrimaryKey();

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
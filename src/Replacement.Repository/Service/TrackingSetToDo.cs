namespace Replacement.Repository.Service;

public class TrackingSetToDo : TrackingSet<ToDoPK, ToDo> {
    public TrackingSetToDo(DBContext context, ITrackingSetApplyChanges<ToDo> trackingApplyChanges)
        : base(
            extractKey: ToDoUtiltiy.ExtractKey,
            comparer: ToDoUtiltiy.Instance,
            trackingContext: context,
            trackingApplyChanges: trackingApplyChanges) {

    }
}

public class TrackingSetApplyChangesToDo : ITrackingSetApplyChanges<ToDo> {
    private static TrackingSetApplyChangesToDo? _Instance;
    public static TrackingSetApplyChangesToDo Instance => _Instance ??= new TrackingSetApplyChangesToDo();

    public TrackingSetApplyChangesToDo() : base() {

    }

    public async Task<ToDo> Insert(ToDo value, TrackingTransConnection trackingTransaction) {
        return await this.Upsert(value, trackingTransaction);
    }

    public async Task<ToDo> Update(ToDo value, TrackingTransConnection trackingTransaction) {
        return await this.Upsert(value, trackingTransaction);
    }

    public async Task<ToDo> Upsert(ToDo value, TrackingTransConnection trackingTransaction) {
        var tc = (TrackingSqlAccessTransConnection)trackingTransaction;
        var sqlAccess = tc.GetSqlAccess();
        var result = await sqlAccess.ExecuteToDoUpsertAsync(value, tc.GetDbTransaction());
        if (result.OperationResult.ResultValue == ResultValue.Inserted) {
            return result.DataResult;
        }
        if (result.OperationResult.ResultValue == ResultValue.NoNeedToUpdate) {
            // TODO: Log??
            return result.DataResult;
        }
        if (result.OperationResult.ResultValue == ResultValue.RowVersionMismatch) {
            throw new InvalidOperationException($"RowVersionMismatch {value.SerialVersion}!={result.DataResult.SerialVersion}");
        }
        throw new InvalidOperationException($"Unknown error {result.OperationResult.ResultValue} Todo {value.Id}");
    }

    public async Task Delete(ToDo value, TrackingTransConnection trackingTransaction) {
        var tc = (TrackingSqlAccessTransConnection)trackingTransaction;
        var sqlAccess = tc.GetSqlAccess();
        var result = await sqlAccess.ExecuteToDoDeletePKAsync(value, tc.GetDbTransaction());
        if (result.Count == 1) {
            if (result[0].Id == value.Id) {
                return;
            } else {
                throw new InvalidOperationException($"Unknown error Todo {result[0].Id} != {value.Id}");
            }
        } else {
            throw new InvalidOperationException($"Cannot delete Todo {value.Id}");
        }
    }
}


public sealed class ToDoUtiltiy
    : IEqualityComparer<ToDoPK> {
    private static ToDoUtiltiy? _Instance;
    public static ToDoUtiltiy Instance => (_Instance ??= new ToDoUtiltiy());
    private ToDoUtiltiy() { }

    public static ToDoPK ExtractKey(ToDo that) => new ToDoPK(that.Id);

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
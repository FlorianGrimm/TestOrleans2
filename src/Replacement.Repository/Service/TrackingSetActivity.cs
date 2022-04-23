namespace Replacement.Repository.Service;

public class TrackingSetOperation : TrackingSet<OperationPK, Operation> {
    public TrackingSetOperation(DBContext context, ITrackingSetApplyChanges<Operation> trackingApplyChanges)
        : base(
            extractKey: OperationUtiltiy.ExtractKey,
            comparer: OperationUtiltiy.Instance,
            trackingContext: context,
            trackingApplyChanges: trackingApplyChanges) {

    }
}

public class TrackingSetApplyChangesOperation : ITrackingSetApplyChanges<Operation> {
    private static TrackingSetApplyChangesOperation? _Instance;
    public static TrackingSetApplyChangesOperation Instance => _Instance ??= new TrackingSetApplyChangesOperation();

    public TrackingSetApplyChangesOperation() : base() {

    }

    public async Task<Operation> Insert(Operation value, TrackingTransConnection trackingTransaction) {
        var tc = (TrackingSqlAccessTransConnection)trackingTransaction;
        var sqlAccess = tc.GetSqlAccess();
        var result = await sqlAccess.ExecuteOperationInsertAsync(value, tc.GetDbTransaction());

        if (result is not null) {
            return result;
        } else { 
            throw new InvalidOperationException($"Cannot insert Operation {value.Id}");
        }
    }

    public Task<Operation> Update(Operation value, TrackingTransConnection trackingTransaction) {
        throw new InvalidOperationException($"Cannot update Operation {value.Id}");
    }

    public Task Delete(Operation value, TrackingTransConnection trackingTransaction) {
        throw new InvalidOperationException($"Cannot delete Operation {value.Id}");
    }
}


public sealed class OperationUtiltiy
    : IEqualityComparer<OperationPK> {
    private static OperationUtiltiy? _Instance;
    public static OperationUtiltiy Instance => (_Instance ??= new OperationUtiltiy());
    private OperationUtiltiy() { }

    public static OperationPK ExtractKey(Operation that) => new OperationPK(that.CreatedAt, that.Id);

    bool IEqualityComparer<OperationPK>.Equals(OperationPK? x, OperationPK? y) {
        if (object.ReferenceEquals(x, y)) {
            return true;
        } else if ((x is null) || (y is null)) {
            return false;
        } else {
            return x.Equals(y);
        }
    }

    int IEqualityComparer<OperationPK>.GetHashCode(OperationPK obj) {
        return obj.GetHashCode();
    }
}
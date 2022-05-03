using Replacement.Contracts.Entity;

namespace Replacement.Repository.Service;

public sealed class TrackingSetOperation : TrackingSet<OperationPK, Operation> {
    public TrackingSetOperation(DBContext context, ITrackingSetApplyChanges<Operation> trackingApplyChanges)
        : base(
            extractKey: OperationUtiltiy.Instance,
            comparer: OperationUtiltiy.Instance,
            trackingContext: context,
            trackingApplyChanges: trackingApplyChanges) {

    }
}

public sealed class TrackingSetApplyChangesOperation : ITrackingSetApplyChanges<Operation> {
    private static TrackingSetApplyChangesOperation? _Instance;
    public static TrackingSetApplyChangesOperation Instance => (_Instance ??= new TrackingSetApplyChangesOperation());

    private TrackingSetApplyChangesOperation() : base() {

    }

    public async Task<Operation> Insert(Operation value, ITrackingTransConnection trackingTransaction) {
        var sqlAccess = (ISqlAccess)trackingTransaction;
        var result = await sqlAccess.ExecuteOperationInsertAsync(value);

        if (result is not null) {
            return result;
        } else { 
            throw new InvalidOperationException($"Cannot insert Operation {value.OperationId}");
        }
    }

    public Task<Operation> Update(Operation value, ITrackingTransConnection trackingTransaction) {
        throw new InvalidOperationException($"Cannot update Operation {value.OperationId}");
    }

    public Task Delete(Operation value, ITrackingTransConnection trackingTransaction) {
        throw new InvalidOperationException($"Cannot delete Operation {value.OperationId}");
    }
}


public sealed class OperationUtiltiy
    : IEqualityComparer<OperationPK>
    , IExtractKey<Operation, OperationPK> {
    private static OperationUtiltiy? _Instance;
    public static OperationUtiltiy Instance => (_Instance ??= new OperationUtiltiy());
    private OperationUtiltiy() { }

    public OperationPK ExtractKey(Operation that) => new OperationPK(that.CreatedAt, that.OperationId);

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
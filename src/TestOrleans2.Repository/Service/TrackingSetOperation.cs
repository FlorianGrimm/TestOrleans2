using Replacement.Contracts.Entity;

namespace Replacement.Repository.Service;

public sealed class TrackingSetOperation : TrackingSet<OperationPK, OperationEntity> {
    public TrackingSetOperation(DBContext context, ITrackingSetApplyChanges<OperationEntity> trackingApplyChanges)
        : base(
            extractKey: OperationUtiltiy.Instance,
            comparer: OperationUtiltiy.Instance,
            trackingContext: context,
            trackingApplyChanges: trackingApplyChanges) {

    }
}

public sealed class TrackingSetApplyChangesOperation : ITrackingSetApplyChanges<OperationEntity> {
    private static TrackingSetApplyChangesOperation? _Instance;
    public static TrackingSetApplyChangesOperation Instance => (_Instance ??= new TrackingSetApplyChangesOperation());

    private TrackingSetApplyChangesOperation() : base() {

    }

    public async Task<OperationEntity> Insert(OperationEntity value, ITrackingTransConnection trackingTransaction) {
        var sqlAccess = (ISqlAccess)trackingTransaction;
        var result = await sqlAccess.ExecuteOperationInsertAsync(value);

        if (result is not null) {
            return result;
        } else { 
            throw new InvalidOperationException($"Cannot insert Operation {value.OperationId}");
        }
    }

    public Task<OperationEntity> Update(OperationEntity value, ITrackingTransConnection trackingTransaction) {
        throw new InvalidOperationException($"Cannot update Operation {value.OperationId}");
    }

    public Task Delete(OperationEntity value, ITrackingTransConnection trackingTransaction) {
        throw new InvalidOperationException($"Cannot delete Operation {value.OperationId}");
    }
}


public sealed class OperationUtiltiy
    : IEqualityComparer<OperationPK>
    , IExtractKey<OperationEntity, OperationPK> {
    private static OperationUtiltiy? _Instance;
    public static OperationUtiltiy Instance => (_Instance ??= new OperationUtiltiy());
    private OperationUtiltiy() { }

    public OperationPK ExtractKey(OperationEntity that) => new OperationPK(that.CreatedAt, that.OperationId);

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
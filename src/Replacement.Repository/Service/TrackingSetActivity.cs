namespace Replacement.Repository.Service;

public class TrackingSetActivity : TrackingSet<ActivityPK, Activity> {
    public TrackingSetActivity(DBContext context, ITrackingSetApplyChanges<Activity> trackingApplyChanges)
        : base(
            extractKey: ActivityUtiltiy.ExtractKey,
            comparer: ActivityUtiltiy.Instance,
            trackingContext: context,
            trackingApplyChanges: trackingApplyChanges) {

    }
}

public class TrackingSetApplyChangesActivity : ITrackingSetApplyChanges<Activity> {
    private static TrackingSetApplyChangesActivity? _Instance;
    public static TrackingSetApplyChangesActivity Instance => _Instance ??= new TrackingSetApplyChangesActivity();

    public TrackingSetApplyChangesActivity() : base() {

    }

    public async Task<Activity> Insert(Activity value, TrackingTransConnection trackingTransaction) {
        var tc = (TrackingSqlAccessTransConnection)trackingTransaction;
        var sqlAccess = tc.GetSqlAccess();
        var result = await sqlAccess.ExecuteActivityInsertAsync(value, tc.GetDbTransaction());

        if (result is not null) {
            return result;
        } else { 
            throw new InvalidOperationException($"Cannot insert Activity {value.Id}");
        }
    }

    public Task<Activity> Update(Activity value, TrackingTransConnection trackingTransaction) {
        throw new InvalidOperationException($"Cannot update Activity {value.Id}");
    }

    public Task Delete(Activity value, TrackingTransConnection trackingTransaction) {
        throw new InvalidOperationException($"Cannot delete Activity {value.Id}");
    }
}


public sealed class ActivityUtiltiy
    : IEqualityComparer<ActivityPK> {
    private static ActivityUtiltiy? _Instance;
    public static ActivityUtiltiy Instance => (_Instance ??= new ActivityUtiltiy());
    private ActivityUtiltiy() { }

    public static ActivityPK ExtractKey(Activity that) => new ActivityPK(that.CreatedAt, that.Id);

    bool IEqualityComparer<ActivityPK>.Equals(ActivityPK? x, ActivityPK? y) {
        if (object.ReferenceEquals(x, y)) {
            return true;
        } else if ((x is null) || (y is null)) {
            return false;
        } else {
            return x.Equals(y);
        }
    }

    int IEqualityComparer<ActivityPK>.GetHashCode(ActivityPK obj) {
        return obj.GetHashCode();
    }
}
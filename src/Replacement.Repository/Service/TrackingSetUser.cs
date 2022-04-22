namespace Replacement.Repository.Service;

public class TrackingSetUser : TrackingSet<UserPK, User> {
    public TrackingSetUser(DBContext context, ITrackingSetApplyChanges<User> trackingApplyChanges)
        : base(
            extractKey: UserUtiltiy.ExtractKey,
            comparer: UserUtiltiy.Instance,
            trackingContext: context,
            trackingApplyChanges: trackingApplyChanges) {

    }
}

public class TrackingSetApplyChangesUser : ITrackingSetApplyChanges<User> {
    private static TrackingSetApplyChangesUser? _Instance;
    public static TrackingSetApplyChangesUser Instance => _Instance ??= new TrackingSetApplyChangesUser();

    public TrackingSetApplyChangesUser() : base() {

    }

    public async Task<User> Insert(User value, TrackingTransConnection trackingTransaction) {
        return await this.Upsert(value, trackingTransaction);
    }

    public async Task<User> Update(User value, TrackingTransConnection trackingTransaction) {
        return await this.Upsert(value, trackingTransaction);
    }

    public async Task<User> Upsert(User value, TrackingTransConnection trackingTransaction) {
        var tc = (TrackingSqlAccessTransConnection)trackingTransaction;
        var sqlAccess = tc.GetSqlAccess();
        var result = await sqlAccess.ExecuteUserUpsertAsync(value, tc.GetDbTransaction());
        if (result.OperationResult.ResultValue == ResultValue.Inserted) {
            return result.DataResult;
        }
        if (result.OperationResult.ResultValue == ResultValue.NoNeedToUpdate) {
            // User: Log??
            return result.DataResult;
        }
        if (result.OperationResult.ResultValue == ResultValue.RowVersionMismatch) {
            throw new InvalidOperationException($"RowVersionMismatch {value.SerialVersion}!={result.DataResult.SerialVersion}");
        }
        throw new InvalidOperationException($"Unknown error {result.OperationResult.ResultValue} User {value.Id}");
    }

    public async Task Delete(User value, TrackingTransConnection trackingTransaction) {
        var tc = (TrackingSqlAccessTransConnection)trackingTransaction;
        var sqlAccess = tc.GetSqlAccess();
        var result = await sqlAccess.ExecuteUserDeletePKAsync(value, tc.GetDbTransaction());
        if (result.Count == 1) {
            if (result[0].Id == value.Id) {
                return;
            } else {
                throw new InvalidOperationException($"Unknown error User {result[0].Id} != {value.Id}");
            }
        } else {
            throw new InvalidOperationException($"Cannot delete User {value.Id}");
        }
    }
}


public sealed class UserUtiltiy
    : IEqualityComparer<UserPK> {
    private static UserUtiltiy? _Instance;
    public static UserUtiltiy Instance => (_Instance ??= new UserUtiltiy());
    private UserUtiltiy() { }

    public static UserPK ExtractKey(User that) => new UserPK(that.Id);

    bool IEqualityComparer<UserPK>.Equals(UserPK? x, UserPK? y) {
        if (object.ReferenceEquals(x, y)) {
            return true;
        } else if ((x is null) || (y is null)) {
            return false;
        } else {
            return x.Equals(y);
        }
    }

    int IEqualityComparer<UserPK>.GetHashCode(UserPK obj) {
        return obj.GetHashCode();
    }
}
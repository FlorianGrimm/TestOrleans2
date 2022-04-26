﻿namespace Replacement.Repository.Service;

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

    public async Task<User> Insert(User value, ITrackingTransConnection trackingTransaction) {
        return await this.Upsert(value, trackingTransaction);
    }

    public async Task<User> Update(User value, ITrackingTransConnection trackingTransaction) {
        return await this.Upsert(value, trackingTransaction);
    }

    public async Task<User> Upsert(User value, ITrackingTransConnection trackingTransaction) {
        var sqlAccess = (ISqlAccess)trackingTransaction;
        var result = await sqlAccess.ExecuteUserUpsertAsync(value);
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
        throw new InvalidOperationException($"Unknown error {result.OperationResult.ResultValue} User {value.UserId}");
    }

    public async Task Delete(User value, ITrackingTransConnection trackingTransaction) {
        var sqlAccess = (ISqlAccess)trackingTransaction;
        var result = await sqlAccess.ExecuteUserDeletePKAsync(value);
        if (result.Count == 1) {
            if (result[0].UserId == value.UserId) {
                return;
            } else {
                throw new InvalidOperationException($"Unknown error User {result[0].UserId} != {value.UserId}");
            }
        } else {
            throw new InvalidOperationException($"Cannot delete User {value.UserId}");
        }
    }
}


public sealed class UserUtiltiy
    : IEqualityComparer<UserPK> {
    private static UserUtiltiy? _Instance;
    public static UserUtiltiy Instance => (_Instance ??= new UserUtiltiy());
    private UserUtiltiy() { }

    public static UserPK ExtractKey(User that) => new UserPK(that.UserId);

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
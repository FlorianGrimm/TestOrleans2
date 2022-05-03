using Replacement.Contracts.Entity;

namespace Replacement.Repository.Service;

public sealed class TrackingSetUser : TrackingSet<UserPK, UserEntity> {
    public TrackingSetUser(DBContext context, ITrackingSetApplyChanges<UserEntity> trackingApplyChanges)
        : base(
            extractKey: UserUtiltiy.Instance,
            comparer: UserUtiltiy.Instance,
            trackingContext: context,
            trackingApplyChanges: trackingApplyChanges) {

    }
}

public sealed class TrackingSetApplyChangesUser : TrackingSetApplyChangesBase<UserEntity, UserPK> {
    private static TrackingSetApplyChangesUser? _Instance;
    public static TrackingSetApplyChangesUser Instance => _Instance ??= new TrackingSetApplyChangesUser();

    private TrackingSetApplyChangesUser() : base() { }

    protected override UserPK ExtractKey(UserEntity value) => value.GetPrimaryKey();

    public override Task<UserEntity> Insert(UserEntity value, ITrackingTransConnection trackingTransaction) {
        return this.Upsert(value, trackingTransaction);
    }

    public override Task<UserEntity> Update(UserEntity value, ITrackingTransConnection trackingTransaction) {
        return this.Upsert(value, trackingTransaction);
    }

    private async Task<UserEntity> Upsert(UserEntity value, ITrackingTransConnection trackingTransaction) {
        var sqlAccess = (ISqlAccess)trackingTransaction;
        var result = await sqlAccess.ExecuteUserUpsertAsync(value);
        return this.ValidateUpsertDataManipulationResult(value, result);
    }

    public override async Task Delete(UserEntity value, ITrackingTransConnection trackingTransaction) {
        var sqlAccess = (ISqlAccess)trackingTransaction;
        var result = await sqlAccess.ExecuteUserDeletePKAsync(value);
        this.ValidateDelete(value, result);
    }
}


public sealed class UserUtiltiy
    : IEqualityComparer<UserPK>
    , IExtractKey<UserEntity, UserPK> {
    private static UserUtiltiy? _Instance;
    public static UserUtiltiy Instance => (_Instance ??= new UserUtiltiy());
    private UserUtiltiy() { }

    public UserPK ExtractKey(UserEntity that) => that.GetPrimaryKey();

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
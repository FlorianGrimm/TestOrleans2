namespace Replacement.Repository.Service;

public sealed class TrackingSetUser : TrackingSet<UserPK, User> {
    public TrackingSetUser(DBContext context, ITrackingSetApplyChanges<User> trackingApplyChanges)
        : base(
            extractKey: UserUtiltiy.Instance,
            comparer: UserUtiltiy.Instance,
            trackingContext: context,
            trackingApplyChanges: trackingApplyChanges) {

    }
}

public sealed class TrackingSetApplyChangesUser : TrackingSetApplyChangesBase<User, UserPK> {
    private static TrackingSetApplyChangesUser? _Instance;
    public static TrackingSetApplyChangesUser Instance => _Instance ??= new TrackingSetApplyChangesUser();

    private TrackingSetApplyChangesUser() : base() { }

    protected override UserPK ExtractKey(User value) => value.GetPrimaryKey();

    public override Task<User> Insert(User value, ITrackingTransConnection trackingTransaction) {
        return this.Upsert(value, trackingTransaction);
    }

    public override Task<User> Update(User value, ITrackingTransConnection trackingTransaction) {
        return this.Upsert(value, trackingTransaction);
    }

    private async Task<User> Upsert(User value, ITrackingTransConnection trackingTransaction) {
        var sqlAccess = (ISqlAccess)trackingTransaction;
        var result = await sqlAccess.ExecuteUserUpsertAsync(value);
        return this.ValidateUpsertDataManipulationResult(value, result);
    }

    public override async Task Delete(User value, ITrackingTransConnection trackingTransaction) {
        var sqlAccess = (ISqlAccess)trackingTransaction;
        var result = await sqlAccess.ExecuteUserDeletePKAsync(value);
        this.ValidateDelete(value, result);
    }
}


public sealed class UserUtiltiy
    : IEqualityComparer<UserPK>
    , IExtractKey<User, UserPK> {
    private static UserUtiltiy? _Instance;
    public static UserUtiltiy Instance => (_Instance ??= new UserUtiltiy());
    private UserUtiltiy() { }

    public UserPK ExtractKey(User that) => that.GetPrimaryKey();

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
using Replacement.Contracts.Entity;

namespace Replacement.Repository.Service;

public sealed class TrackingSetProject : TrackingSet<ProjectPK, ProjectEntity> {
    public TrackingSetProject(DBContext context, ITrackingSetApplyChanges<ProjectEntity> trackingApplyChanges)
        : base(
            extractKey: ProjectUtiltiy.Instance,
            comparer: ProjectUtiltiy.Instance,
            trackingContext: context,
            trackingApplyChanges: trackingApplyChanges) {

    }
}
public sealed class TrackingSetApplyChangesProject : TrackingSetApplyChangesBase<ProjectEntity, ProjectPK> {
    private static TrackingSetApplyChangesProject? _Instance;
    public static TrackingSetApplyChangesProject Instance => _Instance ??= new TrackingSetApplyChangesProject();

    private TrackingSetApplyChangesProject() : base() { }

    protected override ProjectPK ExtractKey(ProjectEntity value) => value.GetPrimaryKey();

    public override Task<ProjectEntity> Insert(ProjectEntity value, ITrackingTransConnection trackingTransaction) {
        return this.Upsert(value, trackingTransaction);
    }

    public override Task<ProjectEntity> Update(ProjectEntity value, ITrackingTransConnection trackingTransaction) {
        return this.Upsert(value, trackingTransaction);
    }

    private async Task<ProjectEntity> Upsert(ProjectEntity value, ITrackingTransConnection trackingTransaction) {
        var sqlAccess = (ISqlAccess)trackingTransaction;
        var result = await sqlAccess.ExecuteProjectUpsertAsync(value);
        return this.ValidateUpsertDataManipulationResult(value, result);
    }

    public override async Task Delete(ProjectEntity value, ITrackingTransConnection trackingTransaction) {
        var sqlAccess = (ISqlAccess)trackingTransaction;
        var result = await sqlAccess.ExecuteProjectDeletePKAsync(value);
        this.ValidateDelete(value, result);
    }
}


public sealed class ProjectUtiltiy
    : IEqualityComparer<ProjectPK>
    , IExtractKey<ProjectEntity, ProjectPK> {
    private static ProjectUtiltiy? _Instance;
    public static ProjectUtiltiy Instance => (_Instance ??= new ProjectUtiltiy());
    private ProjectUtiltiy() { }

    public ProjectPK ExtractKey(ProjectEntity that) => that.GetPrimaryKey();

    bool IEqualityComparer<ProjectPK>.Equals(ProjectPK? x, ProjectPK? y) {
        if (object.ReferenceEquals(x, y)) {
            return true;
        } else if ((x is null) || (y is null)) {
            return false;
        } else {
            return x.Equals(y);
        }
    }

    int IEqualityComparer<ProjectPK>.GetHashCode(ProjectPK obj) {
        return obj.GetHashCode();
    }
}
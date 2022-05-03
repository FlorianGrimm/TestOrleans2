using Replacement.Contracts.Entity;

namespace Replacement.Repository.Service;

public sealed class TrackingSetProject : TrackingSet<ProjectPK, Project> {
    public TrackingSetProject(DBContext context, ITrackingSetApplyChanges<Project> trackingApplyChanges)
        : base(
            extractKey: ProjectUtiltiy.Instance,
            comparer: ProjectUtiltiy.Instance,
            trackingContext: context,
            trackingApplyChanges: trackingApplyChanges) {

    }
}
public sealed class TrackingSetApplyChangesProject : TrackingSetApplyChangesBase<Project, ProjectPK> {
    private static TrackingSetApplyChangesProject? _Instance;
    public static TrackingSetApplyChangesProject Instance => _Instance ??= new TrackingSetApplyChangesProject();

    private TrackingSetApplyChangesProject() : base() { }

    protected override ProjectPK ExtractKey(Project value) => value.GetPrimaryKey();

    public override Task<Project> Insert(Project value, ITrackingTransConnection trackingTransaction) {
        return this.Upsert(value, trackingTransaction);
    }

    public override Task<Project> Update(Project value, ITrackingTransConnection trackingTransaction) {
        return this.Upsert(value, trackingTransaction);
    }

    private async Task<Project> Upsert(Project value, ITrackingTransConnection trackingTransaction) {
        var sqlAccess = (ISqlAccess)trackingTransaction;
        var result = await sqlAccess.ExecuteProjectUpsertAsync(value);
        return this.ValidateUpsertDataManipulationResult(value, result);
    }

    public override async Task Delete(Project value, ITrackingTransConnection trackingTransaction) {
        var sqlAccess = (ISqlAccess)trackingTransaction;
        var result = await sqlAccess.ExecuteProjectDeletePKAsync(value);
        this.ValidateDelete(value, result);
    }
}


public sealed class ProjectUtiltiy
    : IEqualityComparer<ProjectPK>
    , IExtractKey<Project, ProjectPK> {
    private static ProjectUtiltiy? _Instance;
    public static ProjectUtiltiy Instance => (_Instance ??= new ProjectUtiltiy());
    private ProjectUtiltiy() { }

    public ProjectPK ExtractKey(Project that) => that.GetPrimaryKey();

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
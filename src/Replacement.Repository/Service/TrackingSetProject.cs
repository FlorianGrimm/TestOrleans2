namespace Replacement.Repository.Service;

public class TrackingSetProject : TrackingSet<ProjectPK, Project> {
    public TrackingSetProject(DBContext context, ITrackingSetApplyChanges<Project> trackingApplyChanges)
        : base(
            extractKey: ProjectUtiltiy.ExtractKey,
            comparer: ProjectUtiltiy.Instance,
            trackingContext: context,
            trackingApplyChanges: trackingApplyChanges) {

    }
}

public class TrackingSetApplyChangesProject : ITrackingSetApplyChanges<Project> {
    private static TrackingSetApplyChangesProject? _Instance;
    public static TrackingSetApplyChangesProject Instance => _Instance ??= new TrackingSetApplyChangesProject();

    public TrackingSetApplyChangesProject() : base() {

    }

    public async Task<Project> Insert(Project value, ITrackingTransConnection trackingTransaction) {
        return await this.Upsert(value, trackingTransaction);
    }

    public async Task<Project> Update(Project value, ITrackingTransConnection trackingTransaction) {
        return await this.Upsert(value, trackingTransaction);
    }

    public async Task<Project> Upsert(Project value, ITrackingTransConnection trackingTransaction) {
        var sqlAccess = (ISqlAccess)trackingTransaction;
        var result = await sqlAccess.ExecuteProjectUpsertAsync(value);
        if (result.OperationResult.ResultValue == ResultValue.Inserted) {
            return result.DataResult;
        }
        if (result.OperationResult.ResultValue == ResultValue.NoNeedToUpdate) {
            // Project: Log??
            return result.DataResult;
        }
        if (result.OperationResult.ResultValue == ResultValue.RowVersionMismatch) {
            throw new InvalidOperationException($"RowVersionMismatch {value.SerialVersion}!={result.DataResult.SerialVersion}");
        }
        throw new InvalidOperationException($"Unknown error {result.OperationResult.ResultValue} Project {value.ProjectId}");
    }

    public async Task Delete(Project value, ITrackingTransConnection trackingTransaction) {
        var sqlAccess = (ISqlAccess)trackingTransaction;
        var result = await sqlAccess.ExecuteProjectDeletePKAsync(value);
        if (result.Count == 1) {
            if (result[0].ProjectId == value.ProjectId) {
                return;
            } else {
                throw new InvalidOperationException($"Unknown error Project {result[0].ProjectId} != {value.ProjectId}");
            }
        } else {
            throw new InvalidOperationException($"Cannot delete Project {value.ProjectId}");
        }
    }
}


public sealed class ProjectUtiltiy
    : IEqualityComparer<ProjectPK> {
    private static ProjectUtiltiy? _Instance;
    public static ProjectUtiltiy Instance => (_Instance ??= new ProjectUtiltiy());
    private ProjectUtiltiy() { }

    public static ProjectPK ExtractKey(Project that) => new ProjectPK(that.ProjectId);

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
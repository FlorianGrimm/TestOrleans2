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

    public async Task<Project> Insert(Project value, TrackingTransConnection trackingTransaction) {
        return await this.Upsert(value, trackingTransaction);
    }

    public async Task<Project> Update(Project value, TrackingTransConnection trackingTransaction) {
        return await this.Upsert(value, trackingTransaction);
    }

    public async Task<Project> Upsert(Project value, TrackingTransConnection trackingTransaction) {
        var tc = (TrackingSqlAccessTransConnection)trackingTransaction;
        var sqlAccess = tc.GetSqlAccess();
        var result = await sqlAccess.ExecuteProjectUpsertAsync(value, tc.GetDbTransaction());
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
        throw new InvalidOperationException($"Unknown error {result.OperationResult.ResultValue} Project {value.Id}");
    }

    public async Task Delete(Project value, TrackingTransConnection trackingTransaction) {
        var tc = (TrackingSqlAccessTransConnection)trackingTransaction;
        var sqlAccess = tc.GetSqlAccess();
        var result = await sqlAccess.ExecuteProjectDeletePKAsync(value, tc.GetDbTransaction());
        if (result.Count == 1) {
            if (result[0].Id == value.Id) {
                return;
            } else {
                throw new InvalidOperationException($"Unknown error Project {result[0].Id} != {value.Id}");
            }
        } else {
            throw new InvalidOperationException($"Cannot delete Project {value.Id}");
        }
    }
}


public sealed class ProjectUtiltiy
    : IEqualityComparer<ProjectPK> {
    private static ProjectUtiltiy? _Instance;
    public static ProjectUtiltiy Instance => (_Instance ??= new ProjectUtiltiy());
    private ProjectUtiltiy() { }

    public static ProjectPK ExtractKey(Project that) => new ProjectPK(that.Id);

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
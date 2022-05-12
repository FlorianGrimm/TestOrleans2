namespace Replacement.Repository.Grains;

public class GrainCollectionBase : Grain {
    protected IDBContext _DBContext;
    protected bool _IsDirty;

    protected GrainCollectionBase(
        IDBContext dbContext
        ) {
        this._DBContext = dbContext;
    }

    public virtual async Task ApplyChangesAsync(ISqlAccess? sqlAccess = default, CancellationToken cancellationToken = default(CancellationToken)) {
        try {
            await this._DBContext.ApplyChangesAsync(sqlAccess, cancellationToken);
        } catch {
            this._IsDirty = true;
            this._DBContext.Clear();
            throw;
        }
    }
}

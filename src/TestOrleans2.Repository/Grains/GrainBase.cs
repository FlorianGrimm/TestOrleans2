namespace Replacement.Repository.Grains;

//

public class GrainBase<TValue> : Grain
    where TValue : class {
    protected IDBContext _DBContext;
    protected bool _IsDirty;
    protected TValue? _State;

    protected GrainBase(
        IDBContext dBContext
        ) {
        this._DBContext = dBContext;
        this._IsDirty = true;
    }

    //public override async Task OnActivateAsync() {
    //    await this.Load();
    //    // no need to await Task.CompletedTask;
    //    // return base.OnActivateAsync();
    //}

    protected async Task<TValue?> GetState() {
        if (this._IsDirty) {
            var state = await this.Load();
            this._State = state;
            this._IsDirty = false;
            return state;
        } else { 
            return this._State;
        }
    }

    protected virtual Task<TValue?> Load() {
        return Task.FromResult<TValue?>(null);
    }


    public virtual async Task ApplyChangesAsync(ISqlAccess? sqlAccess = default, CancellationToken cancellationToken = default(CancellationToken)) {
        try {
            await this._DBContext.ApplyChangesAsync(sqlAccess, cancellationToken);
        } catch {
            this.SetStateDirty();
            throw;
        }
    }

    protected virtual void SetStateDirty() {
        this._IsDirty = true;
        this._DBContext.Clear();
    }
}

//
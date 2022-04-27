namespace Replacement.Repository.Grains;

public class GrainCollectionBase : Grain {
    protected IDBContext _DBContext;
    protected bool _IsDirty;

    protected GrainCollectionBase(
        IDBContext dbContext
        ) {
        this._DBContext = dbContext;
    }
}

public record struct LazyValue<T>(
    bool IsDirty=true,
    T Value=default!
    ) 
    where T : class 
    {

    public bool TryGetValue([MaybeNullWhen(false)]out T result) {
        if (this.IsDirty) {
            result = this.Value;
            return true;
        } else {
            result = default;
            return false;
        }
    }
    public LazyValue<T> SetStatus(T? value) {
        if (value is null) {
            return new LazyValue<T>(true, default!);
        } else { 
            return new LazyValue<T>(false, value);
        }
    }
}
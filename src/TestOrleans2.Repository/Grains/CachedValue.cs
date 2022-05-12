namespace TestOrleans2.Repository.Grains;

public record struct CachedValue<T>(
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
    public CachedValue<T> SetStatus(T? value) {
        if (value is null) {
            return new CachedValue<T>(true, default!);
        } else { 
            return new CachedValue<T>(false, value);
        }
    }
}
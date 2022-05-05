namespace Replacement.Repository.Grains;

public class ObserverManager<TObserver> : ObserverManager<IAddressable, TObserver>
    where TObserver : notnull {
    public ObserverManager(TimeSpan expiration, ILogger log, string logPrefix) : base(expiration, log, logPrefix) {
    }
}

/// <summary>
/// Maintains a collection of observers.
/// </summary>
/// <typeparam name="TAddress">
/// The address type.
/// </typeparam>
/// <typeparam name="TObserver">
/// The observer type.
/// </typeparam>
public class ObserverManager<TAddress, TObserver>
    : IEnumerable<TObserver>
    where TAddress : notnull, IAddressable
    where TObserver : notnull {
    /// <summary>
    /// The log prefix.
    /// </summary>
    private readonly string logPrefix;

    /// <summary>
    /// The observers.
    /// </summary>
    private readonly ConcurrentDictionary<TAddress, ObserverEntry> observers = new ConcurrentDictionary<TAddress, ObserverEntry>();

    /// <summary>
    /// The log.
    /// </summary>
    private readonly ILogger _Logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ObserverManager{TAddress,TObserver}"/> class. 
    /// </summary>
    /// <param name="expiration">
    /// The expiration.
    /// </param>
    /// <param name="log">The log.</param>
    /// <param name="logPrefix">The prefix to use when logging.</param>
    public ObserverManager(TimeSpan expiration, ILogger log, string logPrefix) {
        this.ExpirationDuration = expiration;
        this._Logger = log;
        this.logPrefix = logPrefix;
        this.GetDateTime = () => DateTime.UtcNow;
    }

    /// <summary>
    /// Gets or sets the delegate used to get the date and time, for expiry.
    /// </summary>
    public Func<DateTime> GetDateTime { get; set; }

    /// <summary>
    /// Gets or sets the expiration time span, after which observers are lazily removed.
    /// </summary>
    public TimeSpan ExpirationDuration { get; set; }

    /// <summary>
    /// Gets the number of observers.
    /// </summary>
    public int Count => this.observers.Count;

    /// <summary>
    /// Gets a copy of the observers.
    /// </summary>
    public IDictionary<TAddress, TObserver> Observers {
        get {
            return this.observers.ToDictionary(_ => _.Key, _ => _.Value.Observer);
        }
    }

    /// <summary>
    /// Removes all observers.
    /// </summary>
    public void Clear() {
        this.observers.Clear();
    }

    /// <summary>
    /// Ensures that the provided <paramref name="observer"/> is subscribed, renewing its subscription.
    /// </summary>
    /// <param name="address">
    /// The subscriber's address
    /// </param>
    /// <param name="observer">
    /// The observer.
    /// </param>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    public void Subscribe(TAddress address, TObserver observer) {
        // Add or update the subscription.
        var now = this.GetDateTime();
        if (this.observers.TryGetValue(address, out var entry)) {
            entry.LastSeen = now;
            entry.Observer = observer;
            if (this._Logger.IsEnabled(LogLevel.Debug)) {
                this._Logger.LogDebug(this.logPrefix + ": Updating entry for {0}/{1}. {2} total subscribers.", address, observer, this.observers.Count);
            }
        } else {
            this.observers[address] = new ObserverEntry(observer: observer, lastSeen: now);
            if (this._Logger.IsEnabled(LogLevel.Debug)) {
                this._Logger.LogDebug(this.logPrefix + ": Adding entry for {0}/{1}. {2} total subscribers after add.", address, observer, this.observers.Count);
            }
        }
    }

    /// <summary>
    /// Ensures that the provided <paramref name="subscriber"/> is unsubscribed.
    /// </summary>
    /// <param name="subscriber">
    /// The observer.
    /// </param>
    public void Unsubscribe(TAddress subscriber) {
        this._Logger.LogDebug(this.logPrefix + ": Removed entry for {0}. {1} total subscribers after remove.", subscriber, this.observers.Count);
        this.observers.TryRemove(subscriber, out var removed);
    }

    /// <summary>
    /// Notifies all observers.
    /// </summary>
    /// <param name="notification">
    /// The notification delegate to call on each observer.
    /// </param>
    /// <param name="predicate">
    /// The predicate used to select observers to notify.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the work performed.
    /// </returns>
    public async Task Notify(Func<TObserver, Task> notification, Func<TObserver, bool>? predicate = null) {
        var now = this.GetDateTime();
        var defunct = default(List<TAddress>);
        foreach (var observer in this.observers) {
            if (TimeSpan.Zero < ExpirationDuration) {
                if (observer.Value.LastSeen + this.ExpirationDuration < now) {
                    // Expired observers will be removed.
                    defunct = defunct ?? new List<TAddress>();
                    defunct.Add(observer.Key);
                    continue;
                }
            }

            // Skip observers which don't match the provided predicate.
            if (predicate != null && !predicate(observer.Value.Observer)) {
                continue;
            }

            try {
                await notification(observer.Value.Observer);
            } catch (Exception) {
                // Failing observers are considered defunct and will be removed..
                defunct = defunct ?? new List<TAddress>();
                defunct.Add(observer.Key);
            }
        }

        // Remove defunct observers.
        if (defunct != default(List<TAddress>)) {
            foreach (var observer in defunct) {
                this.observers.TryRemove(observer, out var removed);
                if (this._Logger.IsEnabled(LogLevel.Debug)) {
                    this._Logger.LogDebug(this.logPrefix + ": Removing defunct entry for {0}. {1} total subscribers after remove.", observer, this.observers.Count);
                }
            }
        }
    }

    /// <summary>
    /// Notifies all observers which match the provided <paramref name="predicate"/>.
    /// </summary>
    /// <param name="notification">
    /// The notification delegate to call on each observer.
    /// </param>
    /// <param name="predicate">
    /// The predicate used to select observers to notify.
    /// </param>
    public void Notify(Action<TObserver> notification, Func<TObserver, bool>? predicate = null) {
        var now = this.GetDateTime();
        var defunct = default(List<TAddress>);
        foreach (var observer in this.observers) {
            if (TimeSpan.Zero < ExpirationDuration) {
                if (observer.Value.LastSeen + this.ExpirationDuration < now) {
                    // Expired observers will be removed.
                    defunct = defunct ?? new List<TAddress>();
                    defunct.Add(observer.Key);
                    continue;
                }
            }

            // Skip observers which don't match the provided predicate.
            if (predicate != null && !predicate(observer.Value.Observer)) {
                continue;
            }

            try {
                notification(observer.Value.Observer);
            } catch (Exception) {
                // Failing observers are considered defunct and will be removed..
                defunct = defunct ?? new List<TAddress>();
                defunct.Add(observer.Key);
            }
        }

        // Remove defunct observers.
        if (defunct != default(List<TAddress>)) {
            foreach (var observer in defunct) {
                this.observers.TryRemove(observer, out var removed);
                if (this._Logger.IsEnabled(LogLevel.Debug)) {
                    this._Logger.LogDebug(this.logPrefix + ": Removing defunct entry for {0}. {1} total subscribers after remove.", observer, this.observers.Count);
                }
            }
        }
    }

    /// <summary>
    /// Removed all expired observers.
    /// </summary>
    public void ClearExpired() {
        if (TimeSpan.Zero < ExpirationDuration) {
            var now = this.GetDateTime();
            var defunct = default(List<TAddress>);
            foreach (var observer in this.observers) {
                if (observer.Value.LastSeen + this.ExpirationDuration < now) {
                    // Expired observers will be removed.
                    defunct = defunct ?? new List<TAddress>();
                    defunct.Add(observer.Key);
                }
            }

            // Remove defunct observers.
            if (defunct != default(List<TAddress>) && defunct.Count > 0) {
                this._Logger.Info(this.logPrefix + ": Removing {0} defunct observers entries.", defunct.Count);
                foreach (var observer in defunct) {
                    this.observers.TryRemove(observer, out var removed);
                }
            }
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator<TObserver> GetEnumerator() {
        return this.observers.Select(observer => observer.Value.Observer).GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() {
        return this.GetEnumerator();
    }

    /// <summary>
    /// An observer entry.
    /// </summary>
    private class ObserverEntry {
        public ObserverEntry(
            TObserver observer,
            DateTime lastSeen
            ) {
            this.Observer = observer;
            this.LastSeen = lastSeen;
        }
        /// <summary>
        /// Gets or sets the observer.
        /// </summary>
        public TObserver Observer;

        /// <summary>
        /// Gets or sets the UTC last seen time.
        /// </summary>
        public DateTime LastSeen;
    }
}

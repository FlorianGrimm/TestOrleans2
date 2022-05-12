namespace TestOrleans2.WebApp.Services {
    [Brimborium.Registrator.Singleton]
    public class AppPolicies {
        //private static AppPolicies? _Instance;
        //public static AppPolicies GetInstance() => _Instance ??= new AppPolicies();

        public AsyncRetryPolicy ControllerToGrainPolicy { get; }


        public AppPolicies(
            ILogger<AppPolicies> logger
            ) {

            this.ControllerToGrainPolicy = Polly.Policy.Handle<Microsoft.Data.SqlClient.SqlException>((exc) => {
                // Error Number:1205,State:18,Class:13
                return exc.Number == 1205;
            }).WaitAndRetryAsync(
                5, 
                (retryAttempt) => TimeSpan.FromMilliseconds(retryAttempt * 100)
                );
        }
    }
}

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

using System.IO;

namespace TestOrleans2.TestExtensions;

[ExcludeFromCodeCoverage]
public class TestDefaultConfiguration {
    private static TestDefaultConfiguration? _Instance;

    public static TestDefaultConfiguration GetInstance() {
        if (_Instance is not null) {
            return _Instance;
        } else {
            lock (typeof(TestDefaultConfiguration)) {
                return _Instance ??= Create();
            }
        }
    }

    public static TestDefaultConfiguration Create() {
        var result = new TestDefaultConfiguration();
        return result;
    }

    private IConfiguration _DefaultConfiguration;

    public TestDefaultConfiguration() {
        this._DefaultConfiguration = this.BuildDefaultConfiguration();
    }

    public bool UseAadAuthentication {
        get {
            bool.TryParse(_DefaultConfiguration[nameof(UseAadAuthentication)], out var value);
            return value;
        }
    }

    //public Uri TableEndpoint => new Uri(defaultConfiguration[nameof(TableEndpoint)]);
    //public Uri DataBlobUri => new Uri(defaultConfiguration[nameof(DataBlobUri)]);
    //public Uri DataQueueUri => new Uri(defaultConfiguration[nameof(DataQueueUri)]);
    //public string DataConnectionString => defaultConfiguration[nameof(DataConnectionString)];
    //public string EventHubConnectionString => defaultConfiguration[nameof(EventHubConnectionString)];
    //public string EventHubFullyQualifiedNamespace => defaultConfiguration[nameof(EventHubFullyQualifiedNamespace)];
    //public string ZooKeeperConnectionString => defaultConfiguration[nameof(ZooKeeperConnectionString)];
    //public string RedisConnectionString => defaultConfiguration[nameof(RedisConnectionString)];
    public string DataConnectionString => this._DefaultConfiguration[nameof(DataConnectionString)];

    public bool GetValue(string key, [MaybeNullWhen(false)] out string value) {
        value = _DefaultConfiguration.GetValue(key, default(string));
        return value != null;
    }

    private IConfiguration BuildDefaultConfiguration() {
        var builder = new ConfigurationBuilder();
        ConfigureHostConfiguration(builder);

        var config = builder.Build();
        return config;
    }


    /// <summary>
    /// Hack, allowing PhysicalFileProvider to be serialized using json
    /// </summary>
    private class SerializablePhysicalFileProvider : IFileProvider {
        [NonSerialized]
        private PhysicalFileProvider? fileProvider;

        public string? Root { get; set; }

        public IDirectoryContents GetDirectoryContents(string subpath) {
            return this.FileProvider().GetDirectoryContents(subpath);
        }

        public IFileInfo GetFileInfo(string subpath) {
            return this.FileProvider().GetFileInfo(subpath);
        }

        public IChangeToken Watch(string filter) {
            return this.FileProvider().Watch(filter);
        }

        private PhysicalFileProvider FileProvider() {
            return this.fileProvider ??= new PhysicalFileProvider(this.Root);
        }
    }

    /// <summary>Try to find a file with specified name up the folder hierarchy, as some of our CI environments are configured this way.</summary>
    private static void AddJsonFileInAncestorFolder(IConfigurationBuilder builder, string fileName) {
        // There might be some other out-of-the-box way of doing this though.
        var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (currentDir != null && currentDir.Exists) {
            string filePath = Path.Combine(currentDir.FullName, fileName);
            if (File.Exists(filePath)) {
                builder.AddJsonFile(new SerializablePhysicalFileProvider { Root = currentDir.FullName }, fileName, false, false);
                return;
            }

            currentDir = currentDir.Parent;
        }
    }


    public static void ConfigureHostConfiguration(IConfigurationBuilder builder) {
        AddJsonFileInAncestorFolder(builder, "ReplacementTestSecrets.json");
        builder.AddUserSecrets<TestDefaultConfiguration>();
        builder.AddEnvironmentVariables("Orleans");
    }

    public static void ConfigureTestCluster(TestClusterBuilder builder) {
        builder.ConfigureHostConfiguration(ConfigureHostConfiguration);
    }
}

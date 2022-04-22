﻿

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

using System.IO;

namespace Replacement.TestExtensions;

public class TestDefaultConfiguration {
    private static readonly object LockObject = new object();
    private static IConfiguration defaultConfiguration;

    static TestDefaultConfiguration() {
        defaultConfiguration = null!;
        InitializeDefaults();
    }

    public static void InitializeDefaults() {
        lock (LockObject) {
            defaultConfiguration = BuildDefaultConfiguration();
        }
    }

    public static bool UseAadAuthentication {
        get {
            bool.TryParse(defaultConfiguration[nameof(UseAadAuthentication)], out var value);
            return value;
        }
    }

    //public static Uri TableEndpoint => new Uri(defaultConfiguration[nameof(TableEndpoint)]);
    //public static Uri DataBlobUri => new Uri(defaultConfiguration[nameof(DataBlobUri)]);
    //public static Uri DataQueueUri => new Uri(defaultConfiguration[nameof(DataQueueUri)]);
    //public static string DataConnectionString => defaultConfiguration[nameof(DataConnectionString)];
    //public static string EventHubConnectionString => defaultConfiguration[nameof(EventHubConnectionString)];
    //public static string EventHubFullyQualifiedNamespace => defaultConfiguration[nameof(EventHubFullyQualifiedNamespace)];
    //public static string ZooKeeperConnectionString => defaultConfiguration[nameof(ZooKeeperConnectionString)];
    //public static string RedisConnectionString => defaultConfiguration[nameof(RedisConnectionString)];
    public static string DataConnectionString => defaultConfiguration[nameof(DataConnectionString)];

    public static bool GetValue(string key, [MaybeNullWhen(false)] out string value) {
        value = defaultConfiguration.GetValue(key, default(string));

        return value != null;
    }

    private static IConfiguration BuildDefaultConfiguration() {
        var builder = new ConfigurationBuilder();
        ConfigureHostConfiguration(builder);

        var config = builder.Build();
        return config;
    }

    public static void ConfigureHostConfiguration(IConfigurationBuilder builder) {
        //builder.AddInMemoryCollection(new Dictionary<string, string>
        //{
        //        { nameof(ZooKeeperConnectionString), "127.0.0.1:2181" }
        //    });

        AddJsonFileInAncestorFolder(builder, "ReplacementTestSecrets.json");
        builder.AddUserSecrets<TestDefaultConfiguration>();
        builder.AddEnvironmentVariables("Orleans");
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

    public static void ConfigureTestCluster(TestClusterBuilder builder) {
        builder.ConfigureHostConfiguration(ConfigureHostConfiguration);
    }
}
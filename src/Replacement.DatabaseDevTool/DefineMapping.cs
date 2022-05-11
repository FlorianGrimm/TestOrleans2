using System.Reflection;

namespace Replacement.DatabaseDevTool;

public class DefineMapping {
    public TypeMapping[] TypeMappings { get; }
    public DefineMapping() {
        this.TypeMappings = new TypeMapping[0];
        this.TypeMappings = this.Initialize();
    }

    private TypeMapping[] Initialize() {
        var result = new List<TypeMapping>();
        add<Operation, OperationEntity>();
        add<User, UserEntity>();
        add<UserHistory, UserHistoryEntity>();
        add<Project, ProjectEntity>();
        add<ProjectHistory, ProjectHistoryEntity>();
        add<ToDo, ToDoEntity>();
        add<ToDoHistory, ToDoHistoryEntity>();
        add<RequestLog, RequestLogEntity>();
        return result.ToArray();

        void add<TypeApi, TypeEntity>() {
            result.Add(this.CreateTypeMapping(typeAPI: typeof(TypeApi), typeEntity: typeof(TypeEntity), mapEntityToAPI: mapEntityVersion, modifyPropertyMapping: modifyPropertyMapping));
        }
    }

    private string? mapEntityVersion(PropertyInfo pi) {
        var name = pi.Name;
        if (string.Equals(name, nameof(IDataEntity.EntityVersion), StringComparison.Ordinal)) {
            return nameof(IDataAPIWithVersion.DataVersion);
        }
        return name;
    }

    private PropertyMapping? modifyPropertyMapping(PropertyMapping pm) {
        if (string.Equals(pm.PropertyEntity.Name, nameof(IDataEntity.EntityVersion), StringComparison.Ordinal)) {
            return pm with {
                ConvertToAPI = "DataVersion: Brimborium.RowVersion.Extensions.DataVersionExtensions.ToDataVersion(that.EntityVersion)",
                ConvertToEntity = "EntityVersion: Brimborium.RowVersion.Extensions.DataVersionExtensions.ToEntityVersion(that.DataVersion)"
            };
        }
        return pm;
    }

    public TypeMapping CreateTypeMapping(
        Type typeAPI,
        Type typeEntity,
        Func<PropertyInfo, string?> mapEntityToAPI,
        Func<PropertyMapping, PropertyMapping?> modifyPropertyMapping
        ) {
        var propertyMappings = new List<PropertyMapping>();
        var dctPropertyAPI = typeAPI.GetProperties().ToDictionary(i => i.Name, i => i);
        var dctPropertyEntity = typeEntity.GetProperties().ToDictionary(i => i.Name, i => i);
        foreach (var (propertyNameEntity, propertyEntity) in dctPropertyEntity.ToList()) {
            var propertyNameAPI = mapEntityToAPI(propertyEntity);
            if (string.IsNullOrEmpty(propertyNameAPI)) {
                continue;
            }
            if (dctPropertyAPI.TryGetValue(propertyNameAPI, out var propertyAPI)) {
                var pm = new PropertyMapping(propertyAPI, propertyEntity);
                pm = modifyPropertyMapping(pm);
                if (pm is not null) {
                    propertyMappings.Add(pm);
                }
            }

        }

        return new TypeMapping(typeAPI, typeEntity, propertyMappings.ToArray());
    }
}

public record class TypeMapping(
    Type TypeAPI,
    Type TypeEntity,
    PropertyMapping[] PropertyMappings
    );

public record class PropertyMapping(
    System.Reflection.PropertyInfo PropertyAPI,
    System.Reflection.PropertyInfo PropertyEntity,
    string? ConvertToAPI = default,
    string? ConvertToEntity = default
    );

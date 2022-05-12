namespace TestOrleans2.Repository.Grains;

[Collection("DefaultClusterFixture")]
public class ProjectGrainTest
    : HostedTestClusterEnsureDefaultStarted
    , IClassFixture<DefaultClusterFixture> {
    public ProjectGrainTest(ITestOutputHelper output, DefaultClusterFixture fixture) : base(output, fixture) {
    }

    [Fact]
    public async Task Test_InitializeOperation() {
        var requestOperation = this.CreateRequestOperation("", "", "UnitTestUser");
        var (operation, user) = await this.InitializeOperation(requestOperation);
        Assert.NotNull(operation);
        Assert.NotNull(user);
    }

    [Fact]
    public async Task Test_Project() {
        var projectId = new Guid("39f124d2-021f-4b28-ab27-b7d3362ad5cf");

        var project1 = new Project(
            ProjectId: projectId,
            Title: "Test1",
            OperationId: Guid.Empty,
            CreatedAt: DateTimeOffset.MinValue,
            CreatedBy: null,
            ModifiedAt: DateTimeOffset.MinValue,
            ModifiedBy: null,
            DataVersion: string.Empty
        );
        ProjectEntity? projectEntityAct1;
        Project project2;

        var projectGrain = this.Client.GetProjectGrain(projectId);


        var requestOperation1 = this.CreateRequestOperation(project1.ProjectId.ToString(), project1, "UnitTestUser");
        var (operation1, user) = await this.InitializeOperation(requestOperation1);
        if (user is null) { throw new Xunit.Sdk.XunitException("user is null"); }
        if (project1 is null) { throw new Xunit.Sdk.XunitException("project1 is null"); }

        var projectEntity1 = project1.ToProjectEntity();


        projectEntityAct1 = await projectGrain.UpsertProject(projectEntity1, user, operation1);
        if (projectEntityAct1 is null) { throw new Xunit.Sdk.XunitException("projectEntityAct is null"); }
        Assert.Equal("Test1", projectEntityAct1.Title);

        project2 = projectEntityAct1.ToProject();

        var operation2 = operation1.Renew();
        project2 = project2 with { Title = $"Test1{operation2.CreatedAt.ToString("s")}" };

        if (project2 is null) { throw new Xunit.Sdk.XunitException("project2 is null"); }
        var projectEntity = project2!.ToProjectEntity();

        var projectEntityAct2 = await projectGrain.UpsertProject(projectEntity, user, operation2);
        if (projectEntityAct2 is null) { throw new Xunit.Sdk.XunitException("projectEntityAct is null"); }
        Assert.NotEqual("Test1", projectEntityAct2.Title);

        Assert.Equal(projectEntityAct1.ProjectId, projectEntityAct2.ProjectId);
        Assert.NotEqual(projectEntityAct1.OperationId, projectEntityAct2.OperationId);
        Assert.Equal(projectEntityAct1.CreatedAt, projectEntityAct2.CreatedAt);
        Assert.Equal(projectEntityAct1.CreatedBy, projectEntityAct2.CreatedBy);
        Assert.True(projectEntityAct1.ModifiedAt < projectEntityAct2.ModifiedAt);
        Assert.True(projectEntityAct1.EntityVersion < projectEntityAct2.EntityVersion);
    }
}
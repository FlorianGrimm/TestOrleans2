﻿namespace Replacement.WebApp.Controllers;

[Collection("DefaultClusterFixture")]
public class ToDoControllerTests : HostedTestClusterEnsureDefaultStarted {

    public ToDoControllerTests(
        ITestOutputHelper output,
        DefaultClusterFixture fixture
        ) : base(output, fixture) {

    }
}

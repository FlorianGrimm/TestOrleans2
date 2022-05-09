#pragma warning disable xUnit2004 // Do not use equality check to test for boolean conditions

namespace Replacement.Repository.Extensions;
public class EntityVersionExtensionsTests {
    [Fact()]
    public void EntityVersionDoesMatch_Test() {
        Assert.Equal(true, EntityVersionExtensions.EntityVersionDoesMatch(currentEntityVersion: 1, newEntityVersion: 1));
        Assert.Equal(true, EntityVersionExtensions.EntityVersionDoesMatch(currentEntityVersion: 1, newEntityVersion: 0));
        Assert.Equal(false, EntityVersionExtensions.EntityVersionDoesMatch(currentEntityVersion: 1, newEntityVersion: 2));
    }
}

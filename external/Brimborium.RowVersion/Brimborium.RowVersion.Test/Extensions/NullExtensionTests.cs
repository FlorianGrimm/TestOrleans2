namespace Brimborium.RowVersion.Extensions;

public class NullExtensionTests {
    [Fact()]
    public void TryGetNotNull_Test() {
        {
            TestEntityWithVersion? sut = null;
            Assert.False(sut.TryGetNotNull(out var result));
        }
        {
            TestEntityWithVersion? sut = new TestEntityWithVersion(1);
            Assert.True(sut.TryGetNotNull(out var result));
        }
    }

    [Fact()]
    public void GetValueOrDefault_Test() {
        {
            TestEntityWithVersion? sut = null;
            Assert.Equal(-1, sut.GetValueNotNullOrDefault(new TestEntityWithVersion(-1)).EntityVersion);
        }
        {
            TestEntityWithVersion? sut = new TestEntityWithVersion(1);
            Assert.Equal(1, sut.GetValueNotNullOrDefault(new TestEntityWithVersion(-1)).EntityVersion);
        }
    }

    [Fact()]
    public void GetValueOrDefault_Test1() {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact()]
    public void GetValueNotNullOrDefault_Test() {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact()]
    public void GetValueNotNullOrDefault_Test1() {
        Assert.True(false, "This test needs an implementation");
    }
}

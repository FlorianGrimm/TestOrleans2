namespace Brimborium.RowVersion.Extensions;

public class NullExtensionTests {
    [Fact()]
    public void GetValueOrDefault_Test() {
        {
            string? sut = null;
            Assert.Equal(false, sut.TryGetNotNull(out var v));
            Assert.Null(v);
        }
        {
            string? sut = "a";
            Assert.Equal(true, sut.TryGetNotNull(out var v));
            Assert.NotNull(v);
        }
    }

    [Fact()]
    public void GetValueOrDefault_Func_Test() {
        {
            string? sut = null;
            Assert.Equal(false, sut.TryGetNotNull(out var v));
            Assert.Null(v);
        }
        {
            string? sut = "a";
            Assert.Equal(false, sut.TryGetNotNull(_ => false, out var v));
            Assert.Null(v);
        }
        {
            string? sut = "a";
            Assert.Equal(true, sut.TryGetNotNull(_ => true, out var v));
            Assert.NotNull(v);
        }
    }

    [Fact()]
    public void GetValueNotNullOrDefault_Test() {
        {
            string? sut = null;
            Assert.Equal("default", sut.GetValueNotNullOrDefault("default"));
        }
        {
            string? sut = "a";
            Assert.Equal("a", sut.GetValueNotNullOrDefault("default"));
        }
    }

    [Fact()]
    public void GetValueNotNullOrDefault_Test1() {
        {
            string? sut = null;
            Assert.Equal("default", sut.GetValueNotNullOrDefault(()=>"default"));
        }
        {
            string? sut = "a";
            Assert.Equal("a", sut.GetValueNotNullOrDefault(()=>"default"));
        }
    }
}

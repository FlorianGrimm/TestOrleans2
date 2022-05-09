namespace Brimborium.RowVersion.Entity;

public class EntryVersionTests {
    [Fact]
    public void EntryVersion_001_Cast() {
        DataEntitiyVersion sut = new DataEntitiyVersion(42);
        Assert.Equal(42, (long)sut);
        Assert.Equal("000000000000002a", (string)sut);
    }

    [Fact]
    public void EntryVersion_002_Get() {
        DataEntitiyVersion sut = new DataEntitiyVersion(42);
        Assert.Equal(42, sut.GetEntityVersion(ref sut));
        Assert.Equal("000000000000002a", sut.GetDataVersion(ref sut));
    }
}
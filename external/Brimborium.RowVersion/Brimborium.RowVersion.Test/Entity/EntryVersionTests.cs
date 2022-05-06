namespace Brimborium.RowVersion.Entity;

public class EntryVersionTests {
    [Fact]
    public void EntryVersion_001_Test() {
        EntryVersion sut = new EntryVersion(42);
        Assert.Equal(42, (long)sut);
        Assert.Equal("42", (string)sut);
    }

    [Fact]
    public void EntryVersion_002_Test() {
        EntryVersion sut = new EntryVersion(42);
        Assert.Equal(42, sut.GetSerialVersion(ref sut));
        Assert.Equal("42", sut.GetRowVersion(ref sut));
    }
}
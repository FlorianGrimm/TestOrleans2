namespace Brimborium.RowVersion.Entity;
public class DataAPIWithVersionClassTests {
    [Fact()]
    public void DataAPIWithVersionClass_Test() {
        var sut = new DataAPIWithVersionClass();

        sut.EntityVersion = 1;
        Assert.Equal("0000000000000001", sut.DataVersion);

        sut.DataVersion = "0000000000000002";
        Assert.Equal(2, sut.EntityVersion);

        sut.DataVersion = "3";
        Assert.Equal(3, sut.EntityVersion);

        sut.DataVersion = "x";
        Assert.Equal(-1, sut.EntityVersion);
    }
}

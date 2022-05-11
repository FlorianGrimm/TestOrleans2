namespace Brimborium.RowVersion.Entity;

public class DataEntityVersionTests {
    [Fact]
    public void DataEntityVersion_Test() {
        {
            var sut = new DataEntityVersion(0);

            Assert.Equal("0000000000000000", sut.GetDataVersion(ref sut));
            Assert.Equal(0, sut.GetEntityVersion(ref sut));

            Assert.Equal("0000000000000000", (string)sut);
            Assert.Equal(0, (long)sut);
        }

        {
            var sut = new DataEntityVersion(1);

            Assert.Equal("0000000000000001", sut.GetDataVersion(ref sut));
            Assert.Equal(1, sut.GetEntityVersion(ref sut));

            Assert.Equal("0000000000000001", (string)sut);
            Assert.Equal(1, (long)sut);
        }

        {
            var sut = new DataEntityVersion("0000000000000002");

            Assert.Equal("0000000000000002", sut.GetDataVersion(ref sut));
            Assert.Equal(2, sut.GetEntityVersion(ref sut));

            Assert.Equal("0000000000000002", (string)sut);
            Assert.Equal(2, (long)sut);
        }

        {
            var sut = new DataEntityVersion(1, "0000000000000001");

            Assert.Equal("0000000000000001", sut.GetDataVersion(ref sut));
            Assert.Equal(1, sut.GetEntityVersion(ref sut));

            Assert.Equal("0000000000000001", (string)sut);
            Assert.Equal(1, (long)sut);
        }

    }
}
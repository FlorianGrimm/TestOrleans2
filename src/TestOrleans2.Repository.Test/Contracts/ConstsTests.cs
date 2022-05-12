namespace Replacement.Contracts;
public class ConstsTests {
    [Fact]
    public void Consts_1_Test() {
        Assert.True(Consts.MinDateTimeOffset < Consts.MaxDateTimeOffset);
    }
}

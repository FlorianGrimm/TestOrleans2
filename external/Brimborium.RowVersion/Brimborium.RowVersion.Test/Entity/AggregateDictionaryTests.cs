namespace Brimborium.RowVersion.Entity;
public class AggregateDictionaryTests {
    [Fact()]
    public void ToAggregateDictionary_Test() {
        var input = new Dictionary<int, TestEntityWithVersion>();
        for (int idx = 1; idx <= 42; idx += 2) {
            input.Add(idx, new TestEntityWithVersion(idx));
        }
        var act = input.ToAggregateDictionary();
        Assert.Equal(input.Last().Value.EntityVersion, act.EntityVersion.EntityVersion);
        Assert.Equal(input.Count, act.EntityVersion.CountVersion);
    }

    [Fact()]
    public void ToAggregateDictionary_Test1() {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact()]
    public void ToAggregateDictionaryValidRange_Test() {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact()]
    public void ToAggregateDictionaryValidRangeQ_Test() {
        Assert.True(false, "This test needs an implementation");
    }
}

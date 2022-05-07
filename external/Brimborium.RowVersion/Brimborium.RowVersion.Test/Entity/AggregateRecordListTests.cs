namespace Brimborium.RowVersion.Entity;

public class AggregateRecordListTests {
    [Fact()]
    public void ToAggregateRecordList_Test() {
        var sut = new List<TestEntityWithVersion>() { new TestEntityWithVersion(4), new TestEntityWithVersion(2) };
        var act = sut.ToAggregateRecordList();
        Assert.Equal(4, act.RowVersion.RowVersion);
    }

    [Fact()]
    public void ToAggregateRecordList_Predicate_Test1() {
        var sut = new List<TestEntityWithVersion>() { new TestEntityWithVersion(4), new TestEntityWithVersion(2) };
        var act = sut.ToAggregateRecordList(i=>(i<4));
        Assert.Equal(2, act.RowVersion.RowVersion);
    }

/*
    [Fact()]
    public void ToAggregateRecordListValidRange_Test() {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact()]
    public void ToAggregateRecordListValidRangeQ_Test() {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact()]
    public void ToAggregateRecordListSorted_Test() {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact()]
    public void AggregateRecordList_Test() {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact()]
    public void Add_Test() {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact()]
    public void Add_Test1() {
        Assert.True(false, "This test needs an implementation");
    }
*/
}

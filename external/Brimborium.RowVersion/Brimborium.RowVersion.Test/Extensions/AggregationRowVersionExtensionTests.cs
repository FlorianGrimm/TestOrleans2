namespace Brimborium.RowVersion.Extensions;

public class AggregationRowVersionExtensionTests {
    [Fact()]
    public void ToAggregationRowVersion_Test() {
        {
            var sut = new List<TestEntityWithVersion>() { };
            var act = sut.ToAggregationRowVersion();
            Assert.Equal(0, act.RowVersion);
            Assert.Equal(0, act.CountVersion);
        }
        {
            var sut = new List<TestEntityWithVersion>() { new TestEntityWithVersion(1), new TestEntityWithVersion(3), new TestEntityWithVersion(2), new TestEntityWithVersion(2) };
            var act = sut.ToAggregationRowVersion();
            Assert.Equal(3, act.RowVersion);
            Assert.Equal(4, act.CountVersion);
        }
    }

    [Fact()]
    public void CalculateArguments_Test() {
        var a = AggregationRowVersionExtension.CalculateArguments("A");
        var b = AggregationRowVersionExtension.CalculateArguments("A");
        var c = AggregationRowVersionExtension.CalculateArguments("B");
        Assert.True(a == b);
        Assert.True(a != c);
    }

    [Fact()]
    public void ToCacheKey_Test() {
        {
            var sut1 = new List<TestEntityWithVersion>() { new TestEntityWithVersion(1), new TestEntityWithVersion(3), new TestEntityWithVersion(2), new TestEntityWithVersion(2) };
            var arv1 = sut1.ToAggregationRowVersion();
            var act1= arv1.ToCacheKey();
         
            var sut2 = new List<TestEntityWithVersion>() { new TestEntityWithVersion(1), new TestEntityWithVersion(3), new TestEntityWithVersion(2), new TestEntityWithVersion(2) };
            var arv2 = sut2.ToAggregationRowVersion();
            var act2 = arv2.ToCacheKey();

            var sut3 = new List<TestEntityWithVersion>() { new TestEntityWithVersion(1) };
            var arv3 = sut3.ToAggregationRowVersion();
            var act3 = arv3.ToCacheKey();

            Assert.True(act1 == act2);
            Assert.True(act1 != act3);
        }
    }


    [Fact()]
    public void MaxRowVersion_Test() {
        long rowVersion = 0;
        (1L).MaxRowVersion(ref rowVersion);
        (3L).MaxRowVersion(ref rowVersion);
        (2L).MaxRowVersion(ref rowVersion);
        Assert.Equal(3L, rowVersion);
    }

    [Fact()]
    public void MaxRowVersion_Test1() {
        AggregationRowVersion aggregationRowVersion = new AggregationRowVersion();
        (1L).MaxRowVersion(ref aggregationRowVersion);
        (3L).MaxRowVersion(ref aggregationRowVersion);
        (2L).MaxRowVersion(ref aggregationRowVersion);
        (1L).MaxRowVersion(ref aggregationRowVersion);

        Assert.Equal(3L, aggregationRowVersion.RowVersion);
        Assert.Equal(4, aggregationRowVersion.CountVersion);
    }
}

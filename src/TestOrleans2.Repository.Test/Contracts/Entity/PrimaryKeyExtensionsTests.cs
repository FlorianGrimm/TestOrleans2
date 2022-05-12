namespace TestOrleans2.Contracts.Entity;
public class PrimaryKeyExtensionsTests {
    [Fact]
    public void ToDoPK_Parse() {
        {
            ToDoPK sut = new ToDoPK(Guid.NewGuid(), Guid.NewGuid());
            Assert.True(ToDoPK.Parse(sut.ToString(), out var act));
            Assert.Equal(sut, act);
        }
        {
            ToDoPK sut = new ToDoPK(new Guid("e08ed05d-b10d-47aa-b77c-3053d6501e58"), new Guid("574bf5fb-1db6-4bde-9e2f-b220556913b2"));
            Assert.Equal("e08ed05d-b10d-47aa-b77c-3053d6501e58-574bf5fb-1db6-4bde-9e2f-b220556913b2", sut.ToString());
            Assert.True(ToDoPK.Parse(sut.ToString(), out var act));
            Assert.Equal(sut, act);
        }
        // 
    }
}

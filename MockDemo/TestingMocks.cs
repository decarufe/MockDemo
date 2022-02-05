namespace MockDemo;

public class TestingMocks
{
    [Fact]
    public void AllTests()
    {
        // Arrange

        Mock<IBar> barMoqMock = new Mock<IBar>();
        IBar barMoq = Mock.Of<IBar>();
        IBar barNSub = Substitute.For<IBar>();
        IBar barFake = A.Fake<IBar>();

        var fooMoqMock = new Foo(barMoqMock.Object);
        var fooMoq = new Foo(barMoq);
        var fooNSub = new Foo(barNSub);
        var fooFake = new Foo(barFake);


        barMoqMock.Setup(m => m.BarMethod(It.IsAny<int>())).Returns(0);
        Mock.Get(barMoq).Setup(m => m.BarMethod(It.IsAny<int>())).Returns(0);
        barNSub.BarMethod(Arg.Any<int>()).Returns(0);
        A.CallTo(() => barFake.BarMethod(A<int>.Ignored)).Returns(0);

        barMoqMock.Setup(m => m.BarMethod(3)).Returns(3);
        Mock.Get(barMoq).Setup(m => m.BarMethod(3)).Returns(3);
        barNSub.BarMethod(3).Returns(3);
        A.CallTo(() => barFake.BarMethod(3)).Returns(3);

        barMoqMock.Setup(m => m.BarMethod(-1)).Throws<InvalidOperationException>();
        Mock.Get(barMoq).Setup(m => m.BarMethod(-1)).Throws<InvalidOperationException>();
        barNSub.When(m => m.BarMethod(-1)).Throw<InvalidOperationException>();
        A.CallTo(() => barFake.BarMethod(-1)).Throws<InvalidOperationException>();

        // Act

        var rk1 = fooMoqMock.FooMethod(1);
        var rm1 = fooMoq.FooMethod(1);
        var rn1 = fooNSub.FooMethod(1);
        var rf1 = fooFake.FooMethod(1);

        var rk2 = fooMoqMock.FooMethod(3);
        var rm2 = fooMoq.FooMethod(3);
        var rn2 = fooNSub.FooMethod(3);
        var rf2 = fooFake.FooMethod(3);

        var rk3 = fooMoqMock.Invoking(o => o.FooMethod(-1));
        var rm3 = fooMoq.Invoking(o => o.FooMethod(-1));
        var rn3 = fooNSub.Invoking(o => o.FooMethod(-1));
        var rf3 = fooFake.Invoking(o => o.FooMethod(-1));

        // Assert

        rk1.Should().Be(0);
        rm1.Should().Be(0);
        rn1.Should().Be(0);
        rf1.Should().Be(0);

        rk2.Should().Be(3);
        rm2.Should().Be(3);
        rn2.Should().Be(3);
        rf2.Should().Be(3);

        rk3.Should().Throw<InvalidOperationException>();
        rm3.Should().Throw<InvalidOperationException>();
        rn3.Should().Throw<InvalidOperationException>();
        rf3.Should().Throw<InvalidOperationException>();

        barMoqMock.Verify(m => m.BarMethod(3), Moq.Times.Exactly(1));
        barMoqMock.Verify(m => m.BarMethod(It.IsAny<int>()), Moq.Times.AtLeast(2));

        Mock.Get(barMoq).Verify(m => m.BarMethod(3), Moq.Times.Exactly(1));
        Mock.Get(barMoq).Verify(m => m.BarMethod(It.IsAny<int>()), Moq.Times.AtLeast(2));
            
        barNSub.Received(Quantity.Exactly(1)).BarMethod(3);
        barNSub.Received(3).BarMethod(Arg.Any<int>());
            
        A.CallTo(() => barFake.BarMethod(3)).MustHaveHappenedOnceExactly();
        A.CallTo(() => barFake.BarMethod(A<int>.Ignored)).MustHaveHappened(3, FakeItEasy.Times.Exactly);

    }
}
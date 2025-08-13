namespace NoLoops.Tests;

public class StandaloneSideEffectTests
{
    [Test]
    public void ExecutesSideEffectUponEnumeration()
        => Assert.Throws<Exception>(() => NoLoops.Do<int>(static () => throw new Exception("nasty side effect!")).ToList());

    [Test]
    public void ExecutesSideEffectUponEveryEnumeration()
    {
        var sideEffectsCount = 0;

        var sideEffect = NoLoops.Do<string>(() => ++sideEffectsCount);

        sideEffect.ToList();
        sideEffect.ToList();
        sideEffect.ToList();

        Assert.That(sideEffectsCount, Is.EqualTo(3));
    }

    [Test]
    public void DoesNotExecuteSideEffectUponReturn()
        => Assert.DoesNotThrow(() => NoLoops.Do<int>(static () => throw new Exception("nasty side effect!")));

    [Test]
    public void DoesNotExecuteSideEffectIfNotEnumerated()
        => Assert.DoesNotThrow(static () => new[] { 1, 2, 3 }
            .Concat(NoLoops.Do<int>(static () => throw new Exception("nasty side effect!")))
            .Take(2)
            .ToList());
}
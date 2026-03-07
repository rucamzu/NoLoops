using FsCheck;
using FsCheck.Fluent;
using FsCheck.NUnit;
using NUnit.Framework;
using NoLoops;

namespace NoLoops.Tests;

public class CartesianTests
{
    [Test]
    public void YieldsAllPairsFromBothSequences()
        => Prop.ForAll(Arbitrary.FirstSequence, Arbitrary.SecondSequence, (first, second) =>
            Assert.That(
                first.Cartesian(second).Count(),
                Is.EqualTo(first.Length * second.Length))).QuickCheck();

    [Test]
    public void YieldsCorrectPairs()
        => Prop.ForAll(Arbitrary.FirstSequence, Arbitrary.SecondSequence, (first, second) =>
            Assert.That(
                first.Cartesian(second).ToList(),
                Is.EqualTo(CartesianProduct(first, second).ToList()))).QuickCheck();

    [Test]
    public void YieldsCorrectOrder()
        => Prop.ForAll(Arbitrary.FirstSequence, Arbitrary.SecondSequence, (first, second) =>
            Assert.That(
                first.Cartesian(second).Select(p => p.Item1),
                Is.EqualTo(first.SelectMany(f => second.Select(_ => f))))).QuickCheck();

    [Test]
    public void YieldsEmptyWhenFirstSequenceIsEmpty()
        => Assert.That(Enumerable.Empty<int>().Cartesian([1, 2, 3]), Is.Empty);

    [Test]
    public void YieldsEmptyWhenSecondSequenceIsEmpty()
        => Assert.That(new[] { 1, 2, 3 }.Cartesian(Enumerable.Empty<int>()), Is.Empty);

    [Test]
    public void YieldsEmptyWhenBothSequencesAreEmpty()
        => Assert.That(Enumerable.Empty<int>().Cartesian(Enumerable.Empty<int>()), Is.Empty);

    [Test]
    public void IsDeterministic()
        => Prop.ForAll(Arbitrary.FirstSequence, Arbitrary.SecondSequence, (first, second) =>
            Assert.That(
                first.Cartesian(second),
                Is.EqualTo(first.Cartesian(second)))).QuickCheck();

    private static class Arbitrary
    {
        public static Arbitrary<int[]> FirstSequence
            => Gen.Choose(1, 5).ArrayOf().ToArbitrary();

        public static Arbitrary<int[]> SecondSequence
            => Gen.Choose(1, 5).ArrayOf().ToArbitrary();
    }

    private static IEnumerable<(int, int)> CartesianProduct(int[] first, int[] second)
    {
        foreach (var f in first)
        {
            foreach (var s in second)
            {
                yield return (f, s);
            }
        }
    }
}

using FsCheck;
using FsCheck.Fluent;
using FsCheck.NUnit;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NoLoops;

namespace NoLoops.Tests;

public class DoTests
{
    [Test]
    public void YieldsTheInputSequenceUnaltered()
        => Prop.ForAll(Arbitrary.RandomSequence, sequence =>
            {
                List<int> sideEffects = [];

                Assert.That(sequence.Do(sideEffects.Add), Is.EqualTo(sequence));
            }).QuickCheck();

    [Test]
    public void ExecutesSideEffectExactlyOncePerElement()
        => Prop.ForAll(Arbitrary.RandomSequence, sequence =>
            {
                List<int> sideEffects = [];

                foreach (var element in sequence.Do(sideEffects.Add)) {}

                Assert.That(sideEffects, Is.EqualTo(sequence));
            }).QuickCheck();

    private static class Arbitrary
    {
        public static Arbitrary<int[]> RandomSequence
            => Gen.Choose(1, 10).ArrayOf().ToArbitrary();
    }
}

using FsCheck;
using FsCheck.Fluent;
using FsCheck.NUnit;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NoLoops;

namespace NoLoops.Tests;

public class GroupAdjacentsByTests
{
    [Test]
    public void IsDeterministic()
        => Prop.ForAll(Arbitrary.RandomSequence, sequence => Assert.That(
            sequence.GroupConsecutivesBy(IsEven),
            Is.EqualTo(sequence.GroupConsecutivesBy(IsEven)))).QuickCheck();

    [Test]
    public void YieldsAllElementsFromInputSequence()
        => Prop.ForAll(Arbitrary.RandomSequence, sequence => Assert.That(
            sequence.GroupConsecutivesBy(IsEven).SelectMany(grouping => grouping),
            Is.EqualTo(sequence))).QuickCheck();

    [Test]
    public void GroupsElementsWithEqualKeys()
        => Prop.ForAll(Arbitrary.RandomSequence, sequence => Assert.That(
            sequence.GroupConsecutivesBy(IsEven).Select(group => group.Select(IsEven)),
            Has.All.Matches(AreAllIdentical<bool>()))).QuickCheck();

    [Test]
    public void YieldsConsecutiveGroupsWithNonEqualKeys()
        => Prop.ForAll(Arbitrary.RandomSequence, sequence => Assert.That(
            sequence.GroupConsecutivesBy(IsEven).Zip(sequence.GroupConsecutivesBy(IsEven).Skip(1)),
            Has.All.Matches(HaveNonEqualKeys<bool, int>()))).QuickCheck();

    [Test]
    public void YieldsNoEmptyGroups()
        => Prop.ForAll(Arbitrary.RandomSequence, sequence => Assert.That(
            sequence.GroupConsecutivesBy(IsEven),
            Has.All.Not.Empty)).QuickCheck();

    [Test]
    public void YieldsNoGroupsFromEmptySequence()
        => Assert.That(Enumerable.Empty<int>().GroupConsecutivesBy(IsEven), Is.Empty);

    private static class Arbitrary
    {
        public static Arbitrary<int[]> RandomSequence
            => Gen.Choose(1, 10).ArrayOf().ToArbitrary();
    }

    private static bool IsEven(int n) => n % 2 == 0;

    private static Constraint AreAllIdentical<TSource>()
        => AreAllIdentical(EqualityComparer<TSource>.Default);

    private static Constraint AreAllIdentical<TSource>(IEqualityComparer<TSource> comparer)
        => new PredicateConstraint<IEnumerable<TSource>>(source =>
            source.All(element => comparer.Equals(element, source.First())));

    private static Constraint HaveNonEqualKeys<TKey, TSource>()
        => HaveNonEqualKeys<TKey, TSource>(EqualityComparer<TKey>.Default);

    private static Constraint HaveNonEqualKeys<TKey, TSource>(IEqualityComparer<TKey> comparer)
        => new PredicateConstraint<(IGrouping<TKey, TSource> Group1, IGrouping<TKey, TSource> Group2)>(groups => 
            !comparer.Equals(groups.Group1.Key, groups.Group2.Key));
}

// NoLoops
//
// MIT License
//
// Copyright (c) 2025 Rubén Campos Zurriaga
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Collections;
using System.Diagnostics.Contracts;

namespace NoLoops;

/// <summary>
/// Extension methods for the <see cref="IEnumerable{T}"/> and <see cref="IAsyncEnumerable{T}"/> interfaces.
/// </summary>
public static class NoLoops
{
    /// <summary>
    /// Groups consecutive elements of a sequence that map to equal keys.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the elements of the <paramref name="source"/> sequence.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// The type of the keys returned by the <paramref name="keySelector"/> function.
    /// </typeparam>
    /// <param name="source">
    /// An <see cref="IEnumerable{T}"/> sequence whose elements are to be grouped.
    /// </param>
    /// <param name="keySelector">
    /// A function that maps elements of the <paramref name="source"/> sequence to keys.
    /// </param>
    /// <returns>
    /// A sequence of groups of consecutive elements from the <paramref name="source"/> sequence.
    /// </returns>
    /// <remarks>
    /// Each <see cref="IGrouping{TKey, TSource}"/> object yielded by this method contains
    /// consecutive elements from the <paramref name="source"/> sequence in their original order,
    /// all of which map to keys that are evaluated as equal by the default equality comparer
    /// <see cref="EqualityComparer{T}.Default"/>.
    /// </remarks>
    public static IEnumerable<IGrouping<TKey, TSource>> GroupConsecutivesBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
        => source.GroupConsecutivesBy(
            keySelector: keySelector,
            elementSelector: Identity,
            resultSelector: Group<TKey, TSource>.FromKeyElements,
            keyComparer: EqualityComparer<TKey>.Default);

    /// <summary>
    /// Groups consecutive elements of a sequence that map to equal keys.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the elements of the <paramref name="source"/> sequence.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// The type of the keys returned by the <paramref name="keySelector"/> function.
    /// </typeparam>
    /// <param name="source">
    /// An <see cref="IEnumerable{T}"/> whose elements are to be grouped.
    /// </param>
    /// <param name="keySelector">
    /// A function that maps elements of the <paramref name="source"/> sequence to keys.
    /// </param>
    /// <param name="keyComparer">
    /// An <see cref="IEqualityComparer{T}"/> that compares keys returned by the <paramref name="keySelector"/> function.
    /// </param>
    /// <returns>
    /// A sequence of groups of consecutive elements from the <paramref name="source"/> sequence.
    /// </returns>
    /// <remarks>
    /// Each <see cref="IGrouping{TKey, TSource}"/> object yielded by this method contains
    /// consecutive elements from the <paramref name="source"/> sequence in their original order,
    /// all of which map to keys that are evaluated as equal by the <paramref name="keyComparer"/>.
    /// </remarks>
    public static IEnumerable<IGrouping<TKey, TSource>> GroupConsecutivesBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey> keyComparer)
        => source.GroupConsecutivesBy(
            keySelector: keySelector,
            elementSelector: Identity,
            resultSelector: Group<TKey, TSource>.FromKeyElements,
            keyComparer: keyComparer);

    /// <summary>
    /// Groups projections of consecutive elements of a sequence that map to equal keys.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the elements of the <paramref name="source"/> sequence.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// The type of the keys returned by the <paramref name="keySelector"/> function.
    /// </typeparam>
    /// <typeparam name="TElement">
    /// The type of the projected elements returned by the <paramref name="elementSelector"/> function.
    /// </typeparam>
    /// <param name="source">
    /// An <see cref="IEnumerable{T}"/> sequence whose elements are to be grouped.
    /// </param>
    /// <param name="keySelector">
    /// A function that maps elements of the <paramref name="source"/> sequence to keys.
    /// </param>
    /// <param name="elementSelector">
    /// A function that maps elements of the <paramref name="source"/> sequence into projected elements that are grouped.
    /// </param>
    /// <returns>
    /// A sequence of groups of projections of consecutive elements from the <paramref name="source"/> sequence.
    /// </returns>
    /// <remarks>
    /// Each <see cref="IGrouping{TKey, TElement}"/> object yielded by this method contains
    /// projections of consecutive elements from the <paramref name="source"/> sequence
    /// in their original order, all of which map to keys that are evaluated as equal by the
    /// default equality comparer <see cref="EqualityComparer{T}.Default"/>.
    /// </remarks>
    public static IEnumerable<IGrouping<TKey, TElement>> GroupConsecutivesBy<TSource, TKey, TElement>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource,TElement> elementSelector)
        => source.GroupConsecutivesBy(
            keySelector: keySelector,
            elementSelector: elementSelector,
            resultSelector: Group<TKey, TElement>.FromKeyElements,
            keyComparer: EqualityComparer<TKey>.Default);

    /// <summary>
    /// Groups projections of consecutive elements of a sequence that map to equal keys.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the elements of the <paramref name="source"/> sequence.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// The type of the keys returned by the <paramref name="keySelector"/> function.
    /// </typeparam>
    /// <typeparam name="TElement">
    /// The type of the projected elements returned by the <paramref name="elementSelector"/> function.
    /// </typeparam>
    /// <param name="source">
    /// An <see cref="IEnumerable{T}"/> sequence whose elements are to be grouped.
    /// </param>
    /// <param name="keySelector">
    /// A function that maps elements of the <paramref name="source"/> sequence to keys.
    /// </param>
    /// <param name="elementSelector">
    /// A function that maps elements of the <paramref name="source"/> sequence into projected elements that are grouped.
    /// </param>
    /// <param name="keyComparer">
    /// An <see cref="IEqualityComparer{T}"/> that compares keys returned by the <paramref name="keySelector"/> function.
    /// </param>
    /// <returns>
    /// A sequence of groups of projections of consecutive elements from the <paramref name="source"/> sequence.
    /// </returns>
    /// <remarks>
    /// Each <see cref="IGrouping{TKey, TElement}"/> object yielded by this method contains
    /// projections of consecutive elements from the <paramref name="source"/> sequence
    /// in their original order, all of which map to keys that are evaluated as equal by the
    /// <paramref name="keyComparer"/>.
    /// </remarks>
    public static IEnumerable<IGrouping<TKey, TElement>> GroupConsecutivesBy<TSource, TKey, TElement>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource,TElement> elementSelector,
        IEqualityComparer<TKey> keyComparer)
        => source.GroupConsecutivesBy(
            keySelector: keySelector,
            elementSelector: elementSelector,
            resultSelector: Group<TKey, TElement>.FromKeyElements,
            keyComparer: keyComparer);

    /// <summary>
    /// Projects groups of consecutive elements of a sequence that can be mapped to equal keys.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the elements of the <paramref name="source"/> sequence.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// The type of the keys returned by the <paramref name="keySelector"/> function.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the group projections yielded by this method.
    /// </typeparam>
    /// <param name="source">
    /// An <see cref="IEnumerable{T}"/> whose elements are to be grouped.
    /// </param>
    /// <param name="keySelector">
    /// A function that maps elements of the <paramref name="source"/> sequence to keys.
    /// </param>
    /// <param name="resultSelector">
    /// A function that projects a key and a group of consecutive elements of the <paramref name="source"/> sequence into a result object.
    /// </param>
    /// <returns>
    /// A sequence of <typeparamref name="TResult"/> objects.
    /// </returns>
    /// <remarks>
    /// Each <typeparamref name="TResult"/> object yielded by this method results from
    /// projecting a key and a sequence of consecutive elements from the <paramref name="source"/>,
    /// all of which map to keys that are evaluated as equal by the default equality comparer
    /// <see cref="EqualityComparer{T}.Default"/>.
    /// </remarks>
    public static IEnumerable<TResult> GroupConsecutivesBy<TSource, TKey, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
        => source.GroupConsecutivesBy(
            keySelector: keySelector,
            elementSelector: Identity,
            resultSelector: resultSelector,
            keyComparer: EqualityComparer<TKey>.Default);

    /// <summary>
    /// Projects groups of consecutive elements of a sequence that can be mapped to equal keys.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the elements of the <paramref name="source"/> sequence.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// The type of the keys returned by the <paramref name="keySelector"/> function.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the group projections yielded by this method.
    /// </typeparam>
    /// <param name="source">
    /// An <see cref="IEnumerable{T}"/> whose elements are to be grouped.
    /// </param>
    /// <param name="keySelector">
    /// A function that maps elements of the <paramref name="source"/> sequence to keys.
    /// </param>
    /// <param name="resultSelector">
    /// A function that maps a key and a group of consecutive elements of the <paramref name="source"/> sequence into a result object.
    /// </param>
    /// <param name="keyComparer">
    /// An <see cref="IEqualityComparer{T}"/> that compares keys returned by the <paramref name="keySelector"/> function.
    /// </param>
    /// <returns>
    /// A sequence of <typeparamref name="TResult"/> objects.
    /// </returns>
    /// <remarks>
    /// Each <typeparamref name="TResult"/> object yielded by this method results from
    /// projecting a key and a sequence of consecutive elements from the <paramref name="source"/>,
    /// all of which map to keys that are evaluated as equal by the <paramref name="keyComparer"/>.
    /// </remarks>
    public static IEnumerable<TResult> GroupConsecutivesBy<TSource, TKey, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TKey, IEnumerable<TSource>, TResult> resultSelector,
        IEqualityComparer<TKey> keyComparer)
        => source.GroupConsecutivesBy(
            keySelector: keySelector,
            elementSelector: Identity,
            resultSelector: resultSelector,
            keyComparer: keyComparer);

    /// <summary>
    /// Projects groups of projections of consecutive elements of a sequence that can be mapped to equal keys.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the elements of the <paramref name="source"/> sequence.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// The type of the keys returned by the <paramref name="keySelector"/> function.
    /// </typeparam>
    /// <typeparam name="TElement">
    /// The type of the projected elements returned by the <paramref name="elementSelector"/> function.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the group projections yielded by this method.
    /// </typeparam>
    /// <param name="source">
    /// An <see cref="IEnumerable{T}"/> whose elements are to be grouped.
    /// </param>
    /// <param name="keySelector">
    /// A function that maps elements of the <paramref name="source"/> sequence to keys.
    /// </param>
    /// <param name="elementSelector">
    /// A function that maps elements of the <paramref name="source"/> sequence into projected elements.
    /// </param>
    /// <param name="resultSelector">
    /// A function that maps a key and a group of projections of consecutive elements of the <paramref name="source"/> sequence into a result object.
    /// </param>
    /// <returns>
    /// A sequence of <typeparamref name="TResult"/> objects.
    /// </returns>
    /// <remarks>
    /// Each <typeparamref name="TResult"/> object yielded by this method results from
    /// projecting a key and a sequence of projections of consecutive elements from the
    /// <paramref name="source"/>, all of which map to keys that are evaluated as equal
    /// by the default equality comparer <see cref="EqualityComparer{T}.Default"/>.
    /// </remarks>
    public static IEnumerable<TResult> GroupConsecutivesBy<TSource, TKey, TElement, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource,TElement> elementSelector,
        Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        => source.GroupConsecutivesBy(
            keySelector: keySelector,
            elementSelector: elementSelector,
            resultSelector: resultSelector,
            keyComparer: EqualityComparer<TKey>.Default);

    /// <summary>
    /// Projects groups of projections of consecutive elements of a sequence that can be mapped to equal keys.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the elements of the <paramref name="source"/> sequence.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// The type of the keys returned by the <paramref name="keySelector"/> function.
    /// </typeparam>
    /// <typeparam name="TElement">
    /// The type of the projected elements returned by the <paramref name="elementSelector"/> function.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the group projections yielded by this method.
    /// </typeparam>
    /// <param name="source">
    /// An <see cref="IEnumerable{T}"/> whose elements are to be grouped.
    /// </param>
    /// <param name="keySelector">
    /// A function that maps elements of the <paramref name="source"/> sequence to keys.
    /// </param>
    /// <param name="elementSelector">
    /// A function that maps elements of the <paramref name="source"/> sequence into projected elements.
    /// </param>
    /// <param name="resultSelector">
    /// A function that maps a key and a group of projections of consecutive elements of the <paramref name="source"/> sequence into a result object.
    /// </param>
    /// <param name="keyComparer">
    /// An <see cref="IEqualityComparer{T}"/> that compares keys returned by the <paramref name="keySelector"/> function.
    /// </param>
    /// <returns>
    /// A sequence of <typeparamref name="TResult"/> objects.
    /// </returns>
    /// <remarks>
    /// Each <typeparamref name="TResult"/> object yielded by this method results from
    /// projecting a key and a sequence of projections of consecutive elements from the
    /// <paramref name="source"/>, all of which map to keys that are evaluated as equal
    /// by the <paramref name="keyComparer"/>.
    /// </remarks>
    public static IEnumerable<TResult> GroupConsecutivesBy<TSource, TKey, TElement, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource,TElement> elementSelector,
        Func<TKey, IEnumerable<TElement>, TResult> resultSelector,
        IEqualityComparer<TKey> keyComparer)
    {
        var enumerator = source.GetEnumerator();

        if (!enumerator.MoveNext()) yield break;
 
        TKey groupingKey = keySelector(enumerator.Current);
        List<TElement> groupingElements = [elementSelector(enumerator.Current)];

        while (enumerator.MoveNext())
        {
            TKey nextKey = keySelector(enumerator.Current);

            if (keyComparer.Equals(nextKey, groupingKey))
            {
                groupingElements.Add(elementSelector(enumerator.Current));
            }
            else
            {
                yield return resultSelector(groupingKey, groupingElements);

                groupingKey = nextKey;
                groupingElements = [elementSelector(enumerator.Current)];
            }
        }

        yield return resultSelector(groupingKey, groupingElements);
    }

    private class Group<TKey, TSource>(TKey key, IEnumerable<TSource> elements) : IGrouping<TKey, TSource>
    {
        public static Group<TKey, TSource> FromKeyElements(TKey key, IEnumerable<TSource> elements)
            => new(key, elements);

        public TKey Key => key;

        public IEnumerator<TSource> GetEnumerator() => elements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => elements.GetEnumerator();
    }

    [Pure]
    private static T Identity<T>(T value) => value;
}

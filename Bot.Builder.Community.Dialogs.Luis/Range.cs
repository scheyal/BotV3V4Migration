using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Builder.Community.Dialogs.Luis
{
    public static partial class Range
    {
        public static Range<T> From<T>(T start, T after)
             where T : IEquatable<T>, IComparable<T>
        {
            return new Range<T>(start, after);
        }
    }

    public struct Range<T> : IEquatable<Range<T>>, IComparable<Range<T>> where T : IEquatable<T>, IComparable<T>
    {
        public T Start { get; }
        public T After { get; }

        public Range(T start, T after)
        {
            this.Start = start;
            this.After = after;
        }

        public override bool Equals(object other)
        {
            return other is Range<T> && this.Equals((Range<T>)other);
        }

        public override int GetHashCode()
        {
            return this.Start.GetHashCode() ^ this.After.GetHashCode();
        }

        public override string ToString()
        {
            return $"[{this.Start}, {this.After})";
        }

        public bool Equals(Range<T> other)
        {
            return this.Start.Equals(other.Start) && this.After.Equals(other.After);
        }

        public int CompareTo(Range<T> other)
        {
            if (this.After.CompareTo(other.Start) < 0)
            {
                return -1;
            }
            else if (other.After.CompareTo(this.Start) > 0)
            {
                return +1;
            }
            else
            {
                return 0;
            }
        }
    }

    public static partial class Extensions
    {
        public static IEnumerable<int> Enumerate(this Range<int> range)
        {
            for (int index = range.Start; index < range.After; ++index)
            {
                yield return index;
            }
        }

        public static T Min<T>(T one, T two) where T : IComparable<T>
        {
            var compare = one.CompareTo(two);
            return compare < 0 ? one : two;
        }

        private static bool Advance<T>(IEnumerator<Range<T>> enumerator, ref T index, T after)
            where T : IEquatable<T>, IComparable<T>
        {
            index = after;
            var compare = index.CompareTo(enumerator.Current.After);
            if (compare < 0)
            {
                return true;
            }
            else if (compare == 0)
            {
                bool more = enumerator.MoveNext();
                if (more)
                {
                    index = enumerator.Current.Start;
                }
                return more;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public static IEnumerable<Range<T>> SortedMerge<T>(this IEnumerable<Range<T>> oneItems, IEnumerable<Range<T>> twoItems)
            where T : IEquatable<T>, IComparable<T>
        {
            using (var one = oneItems.GetEnumerator())
            using (var two = twoItems.GetEnumerator())
            {
                T oneIndex = default(T);
                T twoIndex = default(T);

                bool oneMore = one.MoveNext();
                bool twoMore = two.MoveNext();

                if (oneMore)
                {
                    oneIndex = one.Current.Start;
                }
                if (twoMore)
                {
                    twoIndex = two.Current.Start;
                }

                if (oneMore && twoMore)
                {
                    while (true)
                    {
                        var compare = oneIndex.CompareTo(twoIndex);
                        if (compare < 0)
                        {
                            var after = Min(one.Current.After, twoIndex);
                            oneMore = Advance(one, ref oneIndex, after);
                            if (!oneMore)
                            {
                                break;
                            }
                        }
                        else if (compare == 0)
                        {
                            var after = Min(one.Current.After, two.Current.After);
                            yield return new Range<T>(oneIndex, after);
                            oneMore = Advance(one, ref oneIndex, after);
                            twoMore = Advance(two, ref twoIndex, after);
                            if (!(oneMore && twoMore))
                            {
                                break;
                            }
                        }
                        else if (compare > 0)
                        {
                            var after = Min(two.Current.After, oneIndex);
                            twoMore = Advance(two, ref twoIndex, after);
                            if (!twoMore)
                            {
                                break;
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                }
            }
        }
    }
}

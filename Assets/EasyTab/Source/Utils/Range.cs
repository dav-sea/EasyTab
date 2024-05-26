using System.Collections;
using System.Collections.Generic;

namespace EasyTab.Internals
{
    internal struct Range : IEnumerable<int>
    {
        private int _leng;
        private int _start;
        private int _count;
        private bool _reverse;
        private int _skip;

        private Range(int leng, int start, int count, bool reverse)
        {
            _leng = leng;
            _start = start;
            _count = count;
            _reverse = reverse;
            _skip = 0;
        }

        public Range Skip(int n)
        {
            var newRange = this;
            newRange._skip += n;
            return newRange;
        }

        public static Range By(int leng, int start, bool reverse)
        {
            var count = reverse ? start + 1 : leng - start;
            return new Range(leng, start, count, reverse);
        }

        public static Range Roll(int leng, int start, bool reverse)
        {
            return new Range(leng, start, leng, reverse);
        }

        public RangeEnumerator GetEnumerator()
        {
            var enumerator = new RangeEnumerator(_leng, _start, _count, _reverse);
            for (int i = 0; i < _skip; i++)
                enumerator.MoveNext();
            return enumerator;
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
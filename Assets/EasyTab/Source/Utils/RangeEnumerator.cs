using System.Collections;
using System.Collections.Generic;

namespace EasyTab.Internals
{
    internal struct RangeEnumerator : IEnumerator<int>
    {
        public readonly bool Reverse;

        private readonly int _count;
        private readonly int _leng;
        private readonly int _start;

        private int _iterationNumber;
        private int _current;

        public RangeEnumerator(int leng, int start, int count, bool reverse)
            : this()
        {
            Reverse = reverse;

            _start = start;
            _count = count;
            _leng = leng;

            Reset();
        }

        public bool MoveNext()
        {
            var n = _iterationNumber + 1;
            if (n == _count)
                return false;

            var leng = _leng;
            var i = Reverse
                ? ((-n + _start) % leng + leng) %
                  leng // fix bad mod with negative values https://stackoverflow.com/questions/1082917/mod-of-negative-number-is-melting-my-brain 
                : (n + _start) % leng;

            _current = i;
            _iterationNumber = n;

            return true;
        }


        public void Reset()
        {
            _current = -1;
            _iterationNumber = -1;
        }

        public int Current => _current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }
    }
}
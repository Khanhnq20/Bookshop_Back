using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Bookshop.Service
{
    internal class VnPayFilter : IComparer<string>
    {
        public int Compare([AllowNull] string x, [AllowNull] string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var filter = CompareInfo.GetCompareInfo("en-US");
            return filter.Compare(x, y, CompareOptions.Ordinal);
        }

    }
}
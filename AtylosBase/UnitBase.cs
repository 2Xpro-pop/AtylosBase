using System;

namespace AtylosBase
{
    public class UnitBase
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ulong Hello()
        {
            unsafe
            {
                int* x;

                int y = 11;

                x = &y;

                return ((ulong)x);
            }
        }
    }
}

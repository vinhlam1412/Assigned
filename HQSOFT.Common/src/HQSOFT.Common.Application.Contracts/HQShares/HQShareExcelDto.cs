using System;

namespace HQSOFT.Common.HQShares
{
    public class HQShareExcelDto
    {
        public string? IDParent { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanSubmit { get; set; }
        public bool CanShare { get; set; }
    }
}
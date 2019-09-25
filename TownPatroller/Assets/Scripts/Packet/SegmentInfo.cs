using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownPatroller.Packet
{
    [Serializable]
    class SegmentInfo
    {
        public int SegmentID;
        public int MaxPage;
        public int CurrentPage;

        SegmentInfo()
        {

        }

        SegmentInfo(int segmentID, int maxPage, int currentPage)
        {
            SegmentID = segmentID;
            MaxPage = maxPage;
            CurrentPage = currentPage;
        }
    }
}

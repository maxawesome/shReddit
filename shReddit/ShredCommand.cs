using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace shReddit
{
    public class ShredCommand
    {
        public bool WriteGarbage;
        public int NumberOfPasses;        
        public int DeletePostsQty;
        public int DeleteCommentsQty;

        public ShredCommand(bool writeGarbage, int numberOfPasses, int deletePostsQty, int deleteCommentsQty)
        {
            WriteGarbage = writeGarbage;
            NumberOfPasses = numberOfPasses;
            DeletePostsQty = deletePostsQty;
            DeleteCommentsQty = deleteCommentsQty;
        }
    }
}

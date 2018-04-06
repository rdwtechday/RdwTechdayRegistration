using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Utility
{

    // this class solves this problem 
    // https://stackoverflow.com/questions/22931582/memorystream-seems-be-closed-after-npoi-workbook-write
    // sigh...
    public class NpoiMemoryStream : MemoryStream
    {
        public NpoiMemoryStream()
        {
            AllowClose = true;
        }

        public bool AllowClose { get; set; }

        public override void Close()
        {
            if (AllowClose)
                base.Close();
        }
    }
}

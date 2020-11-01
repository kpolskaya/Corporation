using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
    public sealed class Uid
    {
        static Uid()
        {

        }

        private Uid()
        {

        }

        private static readonly Uid id = new Uid();

        public static uint GetId()
        {
            return ++id.Value;
        }

        public static void InitId(uint Value)
        {
            
            id.Value = Value;
        }


        public uint Value;
    }
}

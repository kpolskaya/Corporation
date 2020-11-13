﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
    /// <summary>
    /// Генератор уникальных ID в singleton классе
    /// </summary>
    public sealed class GenerateId
    {
        static GenerateId()
        {

        }

        private GenerateId()
        {

        }

        private static readonly GenerateId id = new GenerateId();

        public static uint Next()
        {
            return ++id.lastValue;
        }

        public static void InitId(uint Value)
        {
            
            if (Value > id.lastValue)
                id.lastValue = Value;
        }


        private uint lastValue;
    }
}

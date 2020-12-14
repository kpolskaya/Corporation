using System;
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
    public sealed class GlobalId
    {
        static GlobalId()
        {

        }

        private GlobalId()
        {

        }
        private uint lastValue;
        
        private static readonly GlobalId id = new GlobalId();

        /// <summary>
        /// Выдает следующий по порядку ID
        /// </summary>
        /// <returns></returns>
        public static uint Next()
        {
            return ++id.lastValue;
        }

        /// <summary>
        /// Если последний использованный ID больше текущего, реинициализирует последовательность
        /// так, чтобы новый текущий ID был равен последнему
        /// </summary>
        /// <param name="Value">Последний использованный ID</param>
        public static void InitId(uint Value)
        {
            
            if (Value > id.lastValue)
                id.lastValue = Value;
        }


        
    }
}

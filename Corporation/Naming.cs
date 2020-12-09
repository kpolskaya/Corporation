using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
    public static class Naming
    {

        public static String[] GiveAName()
        {
            Random r = new Random();

            int sex = r.Next(0, 2); // 0 - male, 1 - female 
            if (sex == 0)
            {
                FirstName = firstNamesM[r.Next(0, 100)];

                LastName = lastNames[r.Next(0, 250)];

            }
            else
            {
                FirstName = firstNamesF[r.Next(0, 100)];

                LastName = lastNames[r.Next(0, 250)] + 'а';

            }
        }





    }
}

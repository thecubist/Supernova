using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SuperNova
{
    class RandomMath
    {
        Random random = new Random();
        public float GetRandomDouble(double minimum, double maximum)
        {
            return (float)(random.NextDouble() * (maximum - minimum) + minimum);
        }

    }
}

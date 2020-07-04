## 20200702

in Wave.WaveDriver

test the time cost of different way of increasing Time

code:

```C#
using System;
using System.Collections.Generic;
using System.Linq;

namespace _20200702Test
{
    class TestOne
    {
        private int time = 0;
        private int period;

        public TestOne(int period) 
        {
            this.period = period;
        }

        public int Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value % period;
            }
        }

        public void TimeAhead(int delta_time)
        {
            time = (time + delta_time) % period;
        }
    }

    class TestTwo
    {
        private int time = 0;
        private int period;

        public TestTwo(int period) 
        {
            this.period = period;
        }

        public int Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value < period ? value : value % period;
            }
        }

        public void TimeAhead(int delta_time)
        {
            time += delta_time;
            if (time > period) 
            {
                time = time % period;
            }
        }
    }

    class Program
    {
        static readonly int TEST_TIMES = 10000;

        static int Test(Action<int> target, int target_value, int times=1000000)
        {
            DateTime start = DateTime.Now;
            for (int i=0; i<times; i++)
            {
                target(target_value);
            }
            DateTime end = DateTime.Now;
            TimeSpan ts = end - start;
            return ts.Milliseconds;
        }

        static void Main(string[] args)
        {
            Random random = new Random();
            List<int> Time_1 = new List<int>();
            List<int> Time_2 = new List<int>();
            List<int> Time_3 = new List<int>();
            List<int> Time_4 = new List<int>();
            for (int i=0; i<TEST_TIMES; i++)
            {
                int period = random.Next() % 5000 + 5000;
                int target = random.Next() % 10 + 10;
                TestOne A = new TestOne(period);
                TestTwo B = new TestTwo(period);
                Time_1.Add(Test((int t) => {A.Time += t;}, target));
                Time_2.Add(Test((int t) => {A.TimeAhead(t);}, target));
                Time_3.Add(Test((int t) => {B.Time += t;}, target));
                Time_4.Add(Test((int t) => {B.TimeAhead(t);}, target));
                if (i % 99 == 0)
                {
                    Console.Write(".");
                }
            }
            Console.Write("\n");
            Console.WriteLine("% + Time\t" + Time_1.Average());
            Console.WriteLine("% + Ahead\t" + Time_2.Average());
            Console.WriteLine("if + Time\t" + Time_3.Average());
            Console.WriteLine("if + Ahead\t" + Time_4.Average());
        }
    }
}
```
result:

```
PS D:\Projects\C#\20200702Test> dotnet run
......................................................................................................
% + Time        12.4552
% + Ahead       9.4624
if + Time       12.7302
if + Ahead      7.818
```

conclution:

1. use TimeAhead(int delta_time) to increasing Time.
2. use `if (Time>Period) Time %= Period` instead of `Time %= Period`.




----------------------------------------
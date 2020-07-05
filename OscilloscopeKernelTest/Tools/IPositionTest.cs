using System;
using System.Collections.Generic;
using System.Text;
using OscilloscopeKernel.Tools;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OscilloscopeKernelTest.Tools
{
    [TestClass]
    public class IPositionTest
    {
        [TestMethod]
        public void TestHashSet()
        {
            Random rnd = new Random();
            int TEST_TIMES = 10000;
            int ARRAY_SIZE = 128;
            for (int i=0; i<TEST_TIMES; i++)
            {
                HashSet<IPosition> set = new HashSet<IPosition>();
                int[] xs = new int[ARRAY_SIZE];
                int[] ys = new int[ARRAY_SIZE];
                PositionStruct[] pss = new PositionStruct[ARRAY_SIZE];
                for (int j = 0; j < ARRAY_SIZE; j++)
                {
                    xs[j] = rnd.Next();
                    ys[j] = rnd.Next();
                    pss[j] = new PositionStruct(xs[j], ys[j]);
                    set.Add(new Position(xs[j], ys[j]));
                }
                for (int j = 0; j < ARRAY_SIZE; j++)
                {
                    Assert.IsTrue(set.Contains(new Position(xs[j], ys[j])), "new Position(x,y) not in set") ;
                    Assert.IsTrue(set.Contains(pss[j]), "PositionStruct-Array not in set");
                    Assert.IsTrue(set.Contains(new PositionStruct(xs[j], ys[j])), "new PositionStruct(x,y) not in set");
                }
            }
        }
    }
}

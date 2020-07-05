using System;
using System.Collections.Generic;
using System.Text;
using OscilloscopeKernel.Tools;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace OscilloscopeKernelTest.Tools
{
    [TestClass]
    public class ConcurrentCachedPositionPoolTest
    {
        private static readonly Random rnd = new Random();
        private static readonly int MIN_POSITION = -1080;
        private static readonly int MAX_POSITION = 1080;

        [TestMethod]
        public void TestAlloc()
        {
            int TEST_TIMES = 1000;
            int MIN_SIZE = 32;
            int MAX_SIZE = 512;
            ConcurrentCachedPositonPool pool = new ConcurrentCachedPositonPool(16);
            for (int i = 0; i < TEST_TIMES; i++)
            {
                int size = rnd.Next(MIN_SIZE, MAX_SIZE);
                for (int j = 0; j < size; j++)
                {
                    int x = rnd.Next(MIN_POSITION, MAX_POSITION);
                    int y = rnd.Next(MIN_POSITION, MAX_POSITION);
                    Position answer = pool.AllocPosition(x, y);

                    Position sample_p = new Position(x, y);
                    Assert.IsTrue(sample_p.GetHashCode() == answer.GetHashCode());
                    Assert.IsTrue(sample_p.Equals(answer));
                    Assert.IsTrue(answer.Equals(sample_p));
                    Assert.IsTrue(sample_p == answer);
                    Assert.IsTrue(answer == sample_p);

                    PositionStruct sample_ps = new PositionStruct(x, y);
                    Assert.IsTrue(sample_ps.GetHashCode() == answer.GetHashCode());
                    Assert.IsTrue(sample_ps.Equals(answer));
                    Assert.IsTrue(answer.Equals(sample_ps));
                }
                pool.FreeAllPosition();
            }
            pool.FreeAllPosition();
        }

        [TestMethod]
        public void TestLargeAlloc()
        {
            int TEST_SIZE = 1000000;
            int TEST_TIMES = 10;
            ConcurrentCachedPositonPool pool = new ConcurrentCachedPositonPool();
            for (int i = 0; i < TEST_TIMES; i++)
            {
                int size = TEST_SIZE + (int)((2 * rnd.NextDouble() - 1) * (0.01 * TEST_SIZE));
                for (int j = 0; j < size; j++)
                {
                    int x = rnd.Next(MIN_POSITION, MAX_POSITION);
                    int y = rnd.Next(MIN_POSITION, MAX_POSITION);
                    IPosition position = pool.AllocPosition(x, y);
                    Assert.IsTrue(position.X == x);
                    Assert.IsTrue(position.Y == y);
                }
                pool.FreeAllPosition();
            }
        }

        [TestMethod]
        public void TestConcurrent()
        {
            int TEST_TIMES = 1000;
            int MIN_SIZE = 256;
            int MAX_SIZE = 1024;
            ConcurrentCachedPositonPool pool = new ConcurrentCachedPositonPool(16);
            Position[] samples = new Position[MAX_SIZE];
            Position[] answers = new Position[MAX_SIZE];
            for (int i=0; i<TEST_TIMES; i++)
            {
                int size = rnd.Next(MIN_SIZE, MAX_SIZE);
                for (int j=0; j<size; j++)
                {
                    samples[j] = new Position(rnd.Next(MIN_POSITION, MAX_POSITION), rnd.Next(MIN_POSITION, MAX_POSITION));
                }
                Parallel.For(0, size, j =>
                {
                    answers[j] = pool.AllocPosition(samples[j].X, samples[j].Y);
                });
                for (int j=0; j<size; j++)
                {
                    Assert.IsTrue(samples[j].GetHashCode() == answers[j].GetHashCode());
                    Assert.IsTrue(answers[j].Equals(samples[j]));
                    Assert.IsTrue(samples[j].Equals(answers[j]));
                    Assert.IsTrue(answers[j] == samples[j]);
                    Assert.IsTrue(samples[j] == answers[j]);
                }
                pool.FreeAllPosition();
            }
        }
    }
}

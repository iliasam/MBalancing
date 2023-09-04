using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBalancingPC
{
    class DataAnalyseClass
    {
        FixedSizedQueue<float> FIFO;

        public DataAnalyseClass(int size)
        {
            FIFO = new FixedSizedQueue<float>();
            FIFO.Limit = size;
        }

        public void AddDataPoint(float value)
        {
            //small distances are not used in analyse
            FIFO.Enqueue(value);
        }

        public float[] GetPoints()
        {
            return FIFO.q.ToArray();
        }
    }

    public class FixedSizedQueue<T>
    {
        public ConcurrentQueue<T> q = new ConcurrentQueue<T>();
        private object lockObject = new object();

        public int Limit { get; set; }
        public void Enqueue(T obj)
        {
            q.Enqueue(obj);
            lock (lockObject)
            {
                T overflow;
                while (q.Count > Limit && q.TryDequeue(out overflow)) ;
            }
        }
    }//end of class
}

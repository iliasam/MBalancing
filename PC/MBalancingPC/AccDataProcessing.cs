using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBalancingPC
{
    public class AccDataProcessing
    {
        const int MAX_PACKETS_CNT = 10000;
        const int TURNS_TO_ANALYSE = 10;
        const float ACC_LSB_VALUE = (1.0f / 255.0f);

        public struct AccelerPoint
        {
            public float AccX;
            public float AccY;
        }

        enum ProcStateT
        {
            COLLECT = 0,
            PROCESS,
            DONE,
        }

        ProcStateT ProcState = ProcStateT.DONE;

        List<SerialDataParsingClass.AccPacket> AccPackets = new List<SerialDataParsingClass.AccPacket>();

        /// <summary>
        /// Time in HW timer ticks
        /// </summary>
        private uint PrevMeasurementTime = 0;

        private int TurnsCounter = 0;

        public Action<AccelerPoint[]> DataProcessed;

        //*******************************************************************

        /// <summary>
        /// Need to be called periiodically
        /// </summary>
        public void ProcessData()
        {
            if (ProcState == ProcStateT.PROCESS)
            {
                if (AccPackets.Count < 10)
                {
                    ProcState = ProcStateT.DONE;
                    return;
                }

                AccelerPoint[] resArray = new AccelerPoint[AccPackets.Count];
                Int16 averageX = GetAverageX();
                Int16 averageY = GetAverageY();

                for (int i = 0; i < AccPackets.Count; i++)
                {
                    resArray[i].AccX = (float)(AccPackets[i].AccX - averageX) * ACC_LSB_VALUE;
                    resArray[i].AccY = (float)(AccPackets[i].AccY - averageY) * ACC_LSB_VALUE;
                }

                DataProcessed?.Invoke(resArray);
                AccPackets.Clear();
                ProcState = ProcStateT.DONE;
            }
        }

        Int16 GetAverageX()
        {
            var xValues = AccPackets.Select(item => item.AccX).ToArray();
            Array.Sort(xValues);
            return xValues[xValues.Length / 2];
        }

        Int16 GetAverageY()
        {
            var yValues = AccPackets.Select(item => item.AccY).ToArray();
            Array.Sort(yValues);
            return yValues[yValues.Length / 2];
        }

        /// <summary>
        /// Process one accelerometer packet
        /// </summary>
        /// <param name="packet"></param>
        public void ProcessAccPacket(SerialDataParsingClass.AccPacket packet)
        {
            if (ProcState == ProcStateT.DONE)
            {
                bool zeroDetected = TurnsAnalyse(packet.Time);
                if (zeroDetected)
                    ProcState = ProcStateT.COLLECT;
            }
            else if (ProcState == ProcStateT.COLLECT)
            {
                if (AccPackets.Count < MAX_PACKETS_CNT)
                {
                    AccPackets.Add(packet);
                    bool zeroDetected = TurnsAnalyse(packet.Time);

                    if (zeroDetected)
                    {
                        TurnsCounter++;
                        if (TurnsCounter >= TURNS_TO_ANALYSE)
                        {
                            TurnsCounter = 0;
                            ProcState = ProcStateT.PROCESS;
                        }
                    }
                }
                else
                {
                    //Too much data, not OK
                    AccPackets.Clear();
                    TurnsCounter = 0;
                }
            }
        }

        /// <summary>
        /// Analyse turn time
        /// </summary>
        /// <param name="time"></param>
        private bool TurnsAnalyse(UInt16 time)
        {
            bool ZeroDetected = false;
            if (time < PrevMeasurementTime)
            {
                //zero crossing detected
                ZeroDetected = true;
            }
            PrevMeasurementTime = time;

            return ZeroDetected;
        }
    }//end of class
}

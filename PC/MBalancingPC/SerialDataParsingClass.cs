using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBalancingPC
{
    public class SerialDataParsingClass
    {
        const int PACKET_LENGTH = 9;
        int PacketCounter = 0;

        byte[] PacketBytes = new byte[PACKET_LENGTH];

        public struct AccPacket
        {
            public UInt16 Time;
            public Int16 AccX;
            public Int16 AccY;
        }

        public Action<AccPacket> AccPacketReceived;
        public Action<UInt16, UInt16> NewSpeedReceived;

        //*****************************************************

        public void ParseRxData(byte[] data)
        {
            foreach (var item in data)
            {
                ParseByte(item);
            }
            /*
            if (data.Length < PACKET_LENGTH)
            {
                RemainDataBytes = data;
                return;
            }

            if (RemainDataBytes != null)
            {
                foreach (var item in RemainDataBytes)
                {
                    ParseByte(item);
                }
            }
            RemainDataBytes = null;
            */
        }

        private void ParseByte(byte value)
        {
            if (PacketCounter == 0)
            {
                if (value == 0xAA)
                    PacketCounter++;
            }
            else if (PacketCounter == 1)
            {
                if (value == 0x55)
                    PacketCounter++;
                else
                    PacketCounter = 0;
            }
            else if (PacketCounter == 2)
            {
                if ((value == 1) || (value == 2))
                {
                    PacketBytes[PacketCounter] = value;
                    PacketCounter++;
                }
                else
                    PacketCounter = 0;
            }
            else
            {
                PacketBytes[PacketCounter] = value;
                PacketCounter++;
                if (PacketCounter >= PACKET_LENGTH)
                {
                    PacketCounter = 0;

                    if (PacketBytes[2] == 1)
                    {
                        AccPacket packet;
                        packet.Time = (UInt16)(PacketBytes[3] + PacketBytes[4] * 256);
                        packet.AccX = (Int16)(PacketBytes[5] + PacketBytes[6] * 256);
                        packet.AccY = (Int16)(PacketBytes[7] + PacketBytes[8] * 256);

                        AccPacketReceived?.Invoke(packet);
                    }
                    else if (PacketBytes[2] == 2) //
                    {
                        UInt16 speed = (UInt16)(PacketBytes[3] + PacketBytes[4] * 256);
                        UInt16 data_rate_hz = (UInt16)(PacketBytes[5] + PacketBytes[6] * 256);

                        NewSpeedReceived?.Invoke(speed, data_rate_hz);
                    }
                }
            }
        }

    }//end of class
}

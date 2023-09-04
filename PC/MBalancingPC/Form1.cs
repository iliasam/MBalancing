using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MBalancingPC
{
    public partial class Form1 : Form
    {
        static public int TIMER_FREQ_HZ = 50000;
        private const int ANGLES_FIFO_LENGTH = 20;

        SerialDataParsingClass ParsingObj = new SerialDataParsingClass();
        AccDataProcessing DataProcObj = new AccDataProcessing();
        FFT_Class FFT_obj = new FFT_Class();

        DataAnalyseClass dataAnalyseX = new DataAnalyseClass(ANGLES_FIFO_LENGTH);
        DataAnalyseClass dataAnalyseY = new DataAnalyseClass(ANGLES_FIFO_LENGTH);

        /// <summary>
        /// Period of rotation in timer ticks
        /// </summary>
        private uint CurrSpeedValue = 0;

        /// <summary>
        /// Data rate from accelerometer
        /// </summary>
        public UInt16 CurrDataRateHz = 3600;

        private DateTime SpeedTimestamp = DateTime.Now;

        //**************************************************************

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            serialCommControl1.Loaded();
            serialCommControl1.DataReceivedEvent += ParseSeraialData;
            ParsingObj.AccPacketReceived += ProcessAccPacket;
            ParsingObj.NewSpeedReceived += ProcessSpeedPacket;

            DataProcObj.DataProcessed += ProcessCapturedPoints;

            timerUpdateGUI.Enabled = true;
            timerProcData.Enabled = true;
        }

        /// <summary>
        ///  Called when data of 10 rotations is received
        /// </summary>
        /// <param name="points"></param>
        public void ProcessCapturedPoints(AccDataProcessing.AccelerPoint[] points)
        {
            if (CurrSpeedValue < 10)
                return;
            float speedHz = (float)TIMER_FREQ_HZ / (float)CurrSpeedValue;

            chartControl1.UpdateLinearPlot(points);

            var xValues = points.Select(item => item.AccX).ToArray();
            FFT_obj.ProcessFFT(xValues.Length, ref xValues, true);
            float amplitudeX;
            float phaseX;
            AnalyseSpectrum(FFT_obj.spectrumOut, FFT_obj.spectrumPhase, speedHz, out amplitudeX, out phaseX);
            amplitudeX = amplitudeX / 100.0f;

            chartControl1.UpdateSpectrumPlot(FFT_obj.spectrumOut, speedHz);

            var yValues = points.Select(item => item.AccY).ToArray();
            FFT_obj.ProcessFFT(yValues.Length, ref yValues, true);
            float amplitudeY;
            float phaseY;
            AnalyseSpectrum(FFT_obj.spectrumOut, FFT_obj.spectrumPhase, speedHz, out amplitudeY, out phaseY);
            amplitudeY = amplitudeY / 100.0f;

            lblVibroX.Text = $"VibroX: {amplitudeX:0.00} g";
            lblVibroY.Text = $"VibroY: {amplitudeY:0.00} g";

            float phaseXDeg = phaseX * 180.0f / (float)Math.PI;
            float phaseYDeg = phaseY * 180.0f / (float)Math.PI;
            float diffDeg = CalculateAngleDiff(phaseXDeg, phaseYDeg);

            lblPhaseX.Text = $"PhaseX: {phaseXDeg:0} deg";
            lblPhaseY.Text = $"PhaseY: {phaseYDeg:0} deg";
            lblPhaseDiff.Text = $"Diff: {diffDeg:0} deg";

            lblRatioVibro.Text = $"Ratio: {GetRatio(amplitudeX, amplitudeY)}";

            dataAnalyseX.AddDataPoint(phaseXDeg);
            dataAnalyseY.AddDataPoint(phaseXDeg);
            radialPlot1.DrawAnglesPlot(dataAnalyseX.GetPoints());
        }

        private void AnalyseSpectrum(
            float[] amplitudes, float[] phase, float rotationFreqHz, out float Amplitude, out float Phase)
        {
            Amplitude = 0.0f;
            Phase = 0.0f;

            if (amplitudes.Length < 15)
                return;

            float freqStepHz = CurrDataRateHz / amplitudes.Length / 2;
            int spectrumPos = (int)(rotationFreqHz / freqStepHz);
            if (spectrumPos >= amplitudes.Length)
                return;

            int startPos = spectrumPos - 2;
            if (startPos < 0)
                startPos = 0;
            int endPos = startPos + 5;
            if (endPos >= amplitudes.Length)
                endPos = amplitudes.Length - 1;

            float maxVal = 0;
            int maxPos = 0;
            for (int i = startPos; i < startPos + 5; i++)
            {
                if (amplitudes[i] > maxVal)
                {
                    maxVal = amplitudes[i];
                    maxPos = i;
                }
            }

            Amplitude = amplitudes[maxPos];
            Phase = phase[maxPos];
        }

        float CalculateAngleDiff(float angle1Deg, float angle2Deg)
        {
            //bool positive1 = (angle1Deg >= 0.0f);
            //bool positive2 = (angle2Deg >= 0.0f);
            float diffDeg = 0.0f;

            diffDeg = angle2Deg - angle1Deg;
            if (diffDeg > 180)
                diffDeg = diffDeg - 180;

            return diffDeg;
        }

        public void ParseSeraialData(byte[] rxData)
        {
            ParsingObj.ParseRxData(rxData);
        }

        public void ProcessSpeedPacket(UInt16 speed, UInt16 dataRateHz)
        {
            CurrDataRateHz = dataRateHz;
            CurrSpeedValue = speed;
            SpeedTimestamp = DateTime.Now;
            chartControl1.UpdateAccDataRate(CurrDataRateHz);
        }

        public void ProcessAccPacket(SerialDataParsingClass.AccPacket packet)
        {
            if (CurrSpeedValue > 0)
            {
                DataProcObj.ProcessAccPacket(packet);
            }
        }

        private void timerUpdateGUI_Tick(object sender, EventArgs e)
        {
            UpddateSpeedGUI();
        }

        private void UpddateSpeedGUI()
        {
            TimeSpan diff = DateTime.Now - SpeedTimestamp;
            if (diff.TotalMilliseconds > 1000)
                CurrSpeedValue = 0;

            float speedHz = 0;
            if (CurrSpeedValue > 10)
                speedHz = (float)TIMER_FREQ_HZ / (float)CurrSpeedValue;

            float speedRPM = speedHz * 60.0f;

            lblSpeedHz.Text = $"Speed: {speedHz:0.0} Hz";
            lblRPM.Text = $"RPM: {speedRPM:0}";
        }

        private void timerProcData_Tick(object sender, EventArgs e)
        {
            if (CurrSpeedValue == 0)
                return;

            DataProcObj.ProcessData();
        }

        private String GetRatio(float value1, float value2)
        {
            if ((value1 == 0.0) || (value2 == 0.0))
                return $"{value1:0.00}/{value2:0.00}";

            //smaller as 1
            if (value1 < value2)
            {
                value2 = value2 / value1;
                return $"1/{value2:0.00}";
            }
            else
            {
                value1 = value1 / value2;
                return $"{value1:0.00}/1";
            }
        }
    }//end of class
}

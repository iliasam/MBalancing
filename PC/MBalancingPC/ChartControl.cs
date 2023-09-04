using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot.WindowsForms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Annotations;

namespace MBalancingPC
{
    public partial class ChartControl : UserControl
    {

        int ACC_FREQ_HZ = 3600;
        

        PlotView LinearPlotView;
        PlotModel LinearPlotModel;
        LineSeries LinearSeriesX;

        PlotView SpectrumPlotView;
        PlotModel SpectrumPlotModel;
        LineSeries SpectrumSeries;

        public void UpdateAccDataRate(uint newRateHz)
        {
            ACC_FREQ_HZ = (int)newRateHz;
        }

        public ChartControl()
        {
            InitializeComponent();

            InitLinearPlot();
            InitSpectrumPlot();
        }

        public void UpdateLinearPlot(AccDataProcessing.AccelerPoint[] points)
        {
            LinearSeriesX.Points.Clear();
            for (int i = 0; i < points.Length; i++)
            {
                DataPoint pointVal = new DataPoint(i, points[i].AccX);
                LinearSeriesX.Points.Add(pointVal);
            }

            LinearPlotView.InvalidatePlot(true);
        }

        public void UpdateSpectrumPlot(float[] points, float turningFreqHz)
        {
            

            SpectrumSeries.Points.Clear();
            float freqStepHz = ACC_FREQ_HZ / points.Length / 2;

            int spectLengthHz = (int)(ACC_FREQ_HZ / 2);
            spectLengthHz = (spectLengthHz / 200) * 200;//rounding

            SpectrumPlotModel.Axes[0].Maximum = spectLengthHz;

            for (int i = 0; i < points.Length; i++)
            {
                float freqHz = freqStepHz * i;
                DataPoint pointVal = new DataPoint(freqHz, points[i]);
                SpectrumSeries.Points.Add(pointVal);
            }
            //float testFreqHz = (int)(turningFreqHz / freqStepHz) * freqStepHz;
            //((LineAnnotation)(SpectrumPlotModel.Annotations[0])).X = testFreqHz;

            ((LineAnnotation)(SpectrumPlotModel.Annotations[0])).X = turningFreqHz;

            SpectrumPlotView.InvalidatePlot(true);
        }


        private void InitLinearPlot()
        {
            LinearPlotView = new PlotView();
            LinearPlotModel = new PlotModel { };

            LinearSeriesX = new LineSeries
            {
                // InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline,
                Color = OxyColors.Blue,

            };
            LinearPlotModel.Series.Add(LinearSeriesX);


            LinearPlotModel.Axes.Add(new LinearAxis //X - [0]
            {
                Title = "Sample",
                AxislineStyle = LineStyle.Solid,
                Position = AxisPosition.Bottom,
                //PositionAtZeroCrossing = true,
                // Minimum = 0,

                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,

                Minimum = Double.NaN,
                Maximum = Double.NaN,
            });

            LinearPlotModel.Axes.Add(new LinearAxis  //Y - [1]
            {
                Title = "Acceleration, G",
                TitleFontSize = 10,
                AxislineStyle = LineStyle.Solid,
                Position = AxisPosition.Left,

                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,

                Minimum = Double.NaN,
                Maximum = Double.NaN,
            });

            LinearPlotModel.ResetAllAxes();

            LinearPlotView.Model = LinearPlotModel;

            panel1.Controls.Add(LinearPlotView);
            LinearPlotView.Dock = DockStyle.Fill;
            LinearPlotView.Location = new System.Drawing.Point(0, 0);
        }

        private void InitSpectrumPlot()
        {
            SpectrumPlotView = new PlotView();
            SpectrumPlotModel = new PlotModel { };

            SpectrumSeries = new LineSeries
            {
                // InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline,
                Color = OxyColors.Blue,

            };
            SpectrumPlotModel.Series.Add(SpectrumSeries);


            SpectrumPlotModel.Axes.Add(new LinearAxis //X - [0]
            {
                Title = "Frequency, Hz",
                AxislineStyle = LineStyle.Solid,
                Position = AxisPosition.Bottom,
                PositionAtZeroCrossing = true,
                // Minimum = 0,

                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,

                Minimum = Double.NaN,
                Maximum = Double.NaN,
            });

            SpectrumPlotModel.Axes.Add(new LinearAxis  //Y - [1]
            {
                Title = "Amplitude",
                TitleFontSize = 10,
                AxislineStyle = LineStyle.Solid,
                Position = AxisPosition.Left,

                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,

                Minimum = Double.NaN,
                Maximum = Double.NaN,
            });

            var _lineAnnotation = new LineAnnotation
            {
                Text = "Turns",
                Type = LineAnnotationType.Vertical,
                X = 10
            };
            SpectrumPlotModel.Annotations.Add(_lineAnnotation);

            SpectrumPlotModel.ResetAllAxes();

            SpectrumPlotView.Model = SpectrumPlotModel;

            panel2.Controls.Add(SpectrumPlotView);
            SpectrumPlotView.Dock = DockStyle.Fill;
        }

        private void btnResetZoom_Click(object sender, EventArgs e)
        {
            LinearPlotModel.Axes[0].Zoom(-Double.NaN, Double.NaN);//x
            LinearPlotModel.Axes[1].Zoom(-Double.NaN, Double.NaN);//y

            SpectrumPlotModel.Axes[0].Zoom(-Double.NaN, Double.NaN);//x
            SpectrumPlotModel.Axes[1].Zoom(-Double.NaN, Double.NaN);//y
        }

        private void chkManualYRange_CheckedChanged(object sender, EventArgs e)
        {
            if (chkManualYRange.Checked)
            {
                LinearPlotModel.Axes[1].Zoom(-Double.NaN, Double.NaN);//y
                LinearPlotModel.Axes[1].Minimum = -(double)nudManualYRange.Value;
                LinearPlotModel.Axes[1].Maximum = (double)nudManualYRange.Value;
            }
            else
            {
                LinearPlotModel.Axes[1].Zoom(-Double.NaN, Double.NaN);//y
                LinearPlotModel.Axes[1].Minimum = -Double.NaN;
                LinearPlotModel.Axes[1].Maximum = Double.NaN;
            }
        }
    }//end of class
}

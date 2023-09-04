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

namespace MBalancingPC
{
    public partial class RadialPlot : UserControl
    {
        PlotView _IndiPlotView;
        PlotModel _IndiPlotModel;
        LineSeries _IndiSeries;

        public RadialPlot()
        {
            InitializeComponent();
            InitAnglesPlot();
        }

        public void InitAnglesPlot()
        {
            _IndiPlotView = new PlotView();

            _IndiPlotModel = new PlotModel { Title = "Angles" };

            _IndiSeries = new LineSeries
            {
                Color = OxyColors.Blue,
                //InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline,
                StrokeThickness = 1,
            };


            _IndiPlotModel.Series.Add(_IndiSeries);

            _IndiPlotModel.Axes.Add(
                new AngleAxis
                {
                    Minimum = -180,
                    Maximum = 180,
                    MajorStep = 15,
                    MinorStep = 15,
                    StartAngle = -180,
                    EndAngle = 180,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid
                });
            _IndiPlotModel.Axes.Add(new MagnitudeAxis
            {
                Minimum = 0,
                //Maximum = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid
            });

            _IndiPlotModel.PlotType = PlotType.Polar;
            _IndiPlotView.Model = _IndiPlotModel;

            panel1.Controls.Add(_IndiPlotView);
            _IndiPlotView.Dock = DockStyle.Fill;
        }

        public void DrawAnglesPlot(float[] anglesArray)
        {
            _IndiSeries.Points.Clear();
            foreach (var item in anglesArray)
            {
                double y_value = 1;
                double angleDeg = item;
                DataPoint point = new DataPoint(y_value, angleDeg);
                _IndiSeries.Points.Add(new DataPoint(0, 0));
                _IndiSeries.Points.Add(point);
            }

            double maxVal = 1.0;
            //MagnitudeAxis
            Axis tmpAxis = _IndiPlotView.Model.Axes[1];
            tmpAxis.Maximum = maxVal;
            tmpAxis.Zoom(0, maxVal);
            _IndiPlotView.Refresh();
            _IndiPlotView.Update();
            _IndiPlotModel.InvalidatePlot(true);
        }

    }//end of class
}

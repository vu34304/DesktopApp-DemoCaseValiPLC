using LiveCharts.Events;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ScottPlot;
using System.Windows.Threading;
using DemoCaseGui.Core.Application.ViewModels;

namespace DemoCaseGui
{
    /// <summary>
    /// Interaction logic for CaseMicroView.xaml
    /// </summary>
    public partial class CaseMicroView : UserControl
    {

       

        public CaseMicroView()
        {
            InitializeComponent();
            DataContext = new Core.Application.ViewModels.CaseMicroViewModel();
       
        }

        

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TrafficLightsView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        //private void ChartOnDataClick(object sender, ChartPoint p)
        //{
        //    var asPixels = Chart.ConvertToPixels(p.AsPoint());

        //}

        //private void Chart_OnDataHover(object sender, ChartPoint p)
        //{
        //    Console.WriteLine("[EVENT] you hovered over " + p.X + ", " + p.Y);
        //}

        //private void ChartOnUpdaterTick(object sender)
        //{
        //    Console.WriteLine("[EVENT] chart was updated");
        //}

        //private void Axis_OnRangeChanged(RangeChangedEventArgs eventargs)
        //{
        //    Console.WriteLine("[EVENT] axis range changed");
        //}

        //private void ChartMouseMove(object sender, MouseEventArgs e)
        //{
        //    var point = Chart.ConvertToChartValues(e.GetPosition(Chart));

        //    X.Text = point.X.ToString("N");
        //    Y.Text = point.Y.ToString("N");
        //}

        private void Chart_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewmodel = (Core.Application.ViewModels.CaseMicroViewModel)DataContext;
            viewmodel.ChartUpdate += Viewmodel_ChartUpdate;
        }

        private void Viewmodel_ChartUpdate()
        {
            Dispatcher.BeginInvoke(() => chart.Update(false, true), DispatcherPriority.Render);
        }
    }
}

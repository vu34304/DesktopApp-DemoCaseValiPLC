using NPOI.SS.Formula.Functions;
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
using System.Windows.Threading;

namespace DemoCaseGui
{
    /// <summary>
    /// Interaction logic for CaseView.xaml
    /// </summary>
    public partial class CaseView : UserControl
    {
        public CaseView()
        {
            InitializeComponent();
            DataContext = new Core.Application.ViewModels.CaseViewModel();
        }
       
    

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewmodel = (Core.Application.ViewModels.CaseViewModel)DataContext;
            viewmodel.ChartUpdate += Viewmodel_ChartUpdate;
        }

        private void Viewmodel_ChartUpdate()
        {
             Dispatcher.BeginInvoke(() => chart.Update(false, true), DispatcherPriority.Render);
        }

        private void chart_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}

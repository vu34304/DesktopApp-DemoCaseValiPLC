using DemoCaseGui.Core.Application.Models;
using LiveCharts.Configurations;
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
using System.Collections.Specialized;
using DemoCaseGui.Core.Application.ViewModels;

namespace DemoCaseGui
{
    /// <summary>
    /// Interaction logic for CaseCompactLogix.xaml
    /// </summary>
    public partial class CaseCompactLogix : UserControl
    {
        
        public CaseCompactLogix()
        {
            InitializeComponent();
            DataContext = new Core.Application.ViewModels.CaseCompactLogixViewModel();          
        }
       
    }
}

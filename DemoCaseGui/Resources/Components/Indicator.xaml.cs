using MaterialDesignThemes.Wpf;
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

namespace DemoCaseGui.Resources.Components;

/// <summary>
/// Interaction logic for Indicator.xaml
/// </summary>
public partial class Indicator : UserControl
{
    private bool state = false;

    public static readonly DependencyProperty ContentTextProperty = DependencyProperty
        .Register("OnColor", typeof(Color), typeof(Indicator), new PropertyMetadata(default(Color), new PropertyChangedCallback(OnColorChange)));
    public static readonly DependencyProperty StateProperty = DependencyProperty
        .Register("State", typeof(bool), typeof(Indicator), new PropertyMetadata(default(bool), new PropertyChangedCallback(OnStateChange)));

    public Indicator()
    {
        InitializeComponent();
    }

    public Color OnColor { get; set; } = Color.FromArgb(255, 30, 230, 30);
    public bool State
    {
        get { return state; }
        set
        {
            state = value;
            if (state)
            {
                Circle.Fill = new SolidColorBrush(OnColor);
            }
            else
            {
                Circle.Fill = new SolidColorBrush(Color.FromRgb(128, 128, 128));
            }
        }
    }

    private static void OnStateChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        Indicator mb = (Indicator)d;
        mb.State = (bool)e.NewValue;
    }

    private static void OnColorChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        Indicator mb = (Indicator)d;
        mb.OnColor = (Color)e.NewValue;
        mb.State = mb.State;
    }
}

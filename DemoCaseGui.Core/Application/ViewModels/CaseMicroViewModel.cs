using CommunityToolkit.Mvvm.Input;
using DemoCaseGui.Core.Application.Communication;
using MQTTnet.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiveCharts;
using Timer = System.Timers.Timer;
using MqttClient = DemoCaseGui.Core.Application.Communication.MqttClient;
using NPOI.SS.Formula.Functions;
using static NPOI.HSSF.Util.HSSFColor;
using System.ComponentModel;
using HslCommunication.Profinet.XINJE;
using System;
using System.Windows;
using DemoCaseGui.Core.Application.Models;
using LiveCharts.Configurations;

namespace DemoCaseGui.Core.Application.ViewModels
{

    public class CaseMicroViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly M850Client _Micro850Client;
        private readonly MqttClient _mqttClient;
        private readonly Timer _timer;
        public bool IsMqttConnected => _mqttClient.IsConnected;

        public ChartValues<double> Value { get; set; }
        public event Action? ChartUpdate;
        private double _axisMax;
        private double _axisMin;
        private double _trend;

        //TrafficLight

        public bool? Led2 { get; set; }
        public bool? Led3 { get; set; }
        public bool? Led4 { get; set; }
        public bool? Led5 { get; set; }
        public bool? Led6 { get; set; }
        public bool? Led7 { get; set; }

        public bool? led2_old, led3_old, led4_old, led5_old, led6_old, led7_old;

        public bool? Start_TrafficLights { get; set; }
        public bool? Stop_TrafficLights { get; set; }
        public bool? start_trafficlights_old, stop_trafficlights_old;

        public float? Edit_RedLed { get; set; }
        public float? Edit_YellowLed { get; set; }
        public float? Edit_GreenLed { get; set; }

        public ushort? edit_redled_old, edit_greenled_old, edit_yellowled_old, setpoint_old, time_display_a_old, time_display_b_old;

        //Inverter
        public bool? Start { get; set; }
        public bool? Stop { get; set; }
        public bool? start_old, stop_old, forward_old, reverse_old;

        public float? countRB3100_old, distanceUGT524_old, speed_old;

        public float? MotorSetpointWrite { get; set; }
        public float? MotorSetpoint { get; set; }
        public float? Time_Display_A { get; set; }
        public float? Time_Display_B { get; set; }
        public float? MotorSpeed { get; set; }
        public bool? MotorForward { get; set; }
        public bool? MotorReverse { get; set; }
        public bool? MotorForward1 { get; set; }
        public bool? MotorReverse1 { get; set; }

        public float? Direction { get; set; }
        public bool? ButtonStartup { get; set; }
        public bool? ButtonStop { get; set; }
        public bool? ButtonStartup1 { get; set; }
        public bool? ButtonStop1 { get; set; }
        public double MotorSpeed1 { get; set; }

        //Micro820

        public bool? i00 { get; set; }
        public bool? i01 { get; set; }
        public bool? i02 { get; set; }
        public bool? i03 { get; set; }
        public bool? i04 { get; set; }
        public bool? i05 { get; set; }
        public bool? i06 { get; set; }
        public bool? i07 { get; set; }
        public ushort? Analog_temp {  get; set; }
        public double? Analog { get; set; }
        public bool? i00_old, i01_old, i02_old, i03_old, i04_old, i05_old, i06_old, i07_old;
        public ushort? analog_old;

        //Command
        public ICommand ConnectCommand { get; set; }
        public ICommand MotorSetpointOKCommand { get; set; }
        public ICommand StartTrafficLightsCommand { get; set; }
        public ICommand StopTrafficLightsCommand { get; set; }
        public ICommand StartInverterCommand { get; set; }
        public ICommand StopInverterCommand { get; set; }
        public ICommand ConfirmTrafficLights_Command { get; set; }
        public ICommand ConfirmInverter_Command { get; set; }
        public ICommand ForwardCommand { get; set; }
        public ICommand ReverseCommand { get; set; }

        public CaseMicroViewModel()
        {
            _Micro850Client = new M850Client();
            _timer = new Timer(500);
            _timer.Elapsed += _timer_Elapsed;
            Value = new ChartValues<double> { };

            //Button Command
            ConnectCommand = new RelayCommand(Connect);// Connect PLC
            //Traffic Light
            StartTrafficLightsCommand = new RelayCommand(Start_TrafficLight);
            StopTrafficLightsCommand = new RelayCommand(Stop_TrafficLight);
            ConfirmTrafficLights_Command = new RelayCommand(ConfirmTrafficLight);

            //Inverter
            MotorSetpointOKCommand = new RelayCommand(WriteMotorSetpoint);
            StartInverterCommand = new RelayCommand(Start_Inverter);
            StopInverterCommand = new RelayCommand(Stop_Inverter);
            ForwardCommand = new RelayCommand(Forward_Inverter);
            ReverseCommand = new RelayCommand(Reverse_Inverter);

            var mapper = Mappers.Xy<MeasureModel>()
                .X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
                .Y(model => model.Value);           //use the value property as Y
            Charting.For<MeasureModel>(mapper);

            //the values property will store our values array
            ChartValues = new ChartValues<MeasureModel>();

            //lets set how to display the X Labels
            DateTimeFormatter = value => new DateTime((long)value).ToString("hh:mm:ss");

            //AxisStep forces the distance between each separator in the X axis
            AxisStep = TimeSpan.FromSeconds(1).Ticks;
            //AxisUnit forces lets the axis know that we are plotting seconds
            //this is not always necessary, but it can prevent wrong labeling
            AxisUnit = TimeSpan.TicksPerSecond;

            SetAxisLimits(DateTime.Now);

            //The next code simulates data changes every 300 ms

           

        }
        public ChartValues<MeasureModel> ChartValues { get; set; }
        public Func<double, string> DateTimeFormatter { get; set; }
        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }
        public double AxisMax
        {
            get { return _axisMax; }
            set
            {
                _axisMax = value;
                OnPropertyChanged("AxisMax");
            }
        }
        public double AxisMin
        {
            get { return _axisMin; }
            set
            {
                _axisMin = value;
                OnPropertyChanged("AxisMin");
            }
        }

      

       
        public void SetAxisLimits(DateTime now)
        {
            AxisMax = now.Ticks + TimeSpan.FromSeconds(1).Ticks; // lets force the axis to be 1 second ahead
            AxisMin = now.Ticks - TimeSpan.FromSeconds(8).Ticks; // and 8 seconds behind
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            //TrafficLight

            if ((bool?)_Micro850Client.GetTagValue("start_trafficlight") != start_trafficlights_old)
            {
                Start_TrafficLights = (bool?)_Micro850Client.GetTagValue("start_trafficlight");
            }
            start_trafficlights_old = (bool?)_Micro850Client.GetTagValue("start_trafficlight");
            if (Start_TrafficLights is true)
            {
                ButtonStartup1 = true;
                ButtonStop1 = false;
            }

            if ((bool?)_Micro850Client.GetTagValue("stop_trafficlight") != stop_trafficlights_old)
            {
                Stop_TrafficLights = (bool?)_Micro850Client.GetTagValue("stop_trafficlight");
            }
            stop_trafficlights_old = (bool?)_Micro850Client.GetTagValue("stop_trafficlight");
            if (Stop_TrafficLights is true)
            {
                ButtonStartup1 = false;
                ButtonStop1 = true;
            }


            if ((bool?)_Micro850Client.GetTagValue("led2") != led2_old)
            {
                Led2 = (bool?)_Micro850Client.GetTagValue("led2");
            }
            led2_old = (bool?)_Micro850Client.GetTagValue("led2");

            if ((bool?)_Micro850Client.GetTagValue("led3") != led3_old)
            {
                Led3 = (bool?)_Micro850Client.GetTagValue("led3");
            }
            led3_old = (bool?)_Micro850Client.GetTagValue("led3");

            if ((bool?)_Micro850Client.GetTagValue("led4") != led4_old)
            {
                Led4 = (bool?)_Micro850Client.GetTagValue("led4");
            }
            led4_old = (bool?)_Micro850Client.GetTagValue("led4");

            if ((bool?)_Micro850Client.GetTagValue("led5") != led5_old)
            {
                Led5 = (bool?)_Micro850Client.GetTagValue("led5");
            }
            led5_old = (bool?)_Micro850Client.GetTagValue("led5");

            if ((bool?)_Micro850Client.GetTagValue("led6") != led6_old)
            {
                Led6 = (bool?)_Micro850Client.GetTagValue("led6");
            }
            led6_old = (bool?)_Micro850Client.GetTagValue("led6");

            if ((bool?)_Micro850Client.GetTagValue("led7") != led7_old)
            {
                Led7 = (bool?)_Micro850Client.GetTagValue("led7");
            }
            led7_old = (bool?)_Micro850Client.GetTagValue("led7");

            if ((ushort?)_Micro850Client.GetTagValue("edit_redled") != edit_redled_old)
            {
                Edit_RedLed = (ushort?)_Micro850Client.GetTagValue("edit_redled");
            }
            edit_redled_old = (ushort?)_Micro850Client.GetTagValue("edit_redled");

            if ((ushort?)_Micro850Client.GetTagValue("edit_greenled") != edit_greenled_old)
            {
                Edit_GreenLed = (ushort?)_Micro850Client.GetTagValue("edit_greenled");
            }
            edit_greenled_old = (ushort?)_Micro850Client.GetTagValue("edit_greenled");

            if ((ushort?)_Micro850Client.GetTagValue("edit_yellowled") != edit_yellowled_old)
            {
                Edit_YellowLed = (ushort?)_Micro850Client.GetTagValue("edit_yellowled");
            }
            edit_yellowled_old = (ushort?)_Micro850Client.GetTagValue("edit_yellowled");

            if ((ushort?)_Micro850Client.GetTagValue("time_display_a") != time_display_a_old)
            {
                Time_Display_A = (ushort?)_Micro850Client.GetTagValue("time_display_a");
            }
            time_display_a_old = (ushort?)_Micro850Client.GetTagValue("time_display_a");

            if ((ushort?)_Micro850Client.GetTagValue("time_display_b") != time_display_b_old)
            {
                Time_Display_B = (ushort?)_Micro850Client.GetTagValue("time_display_b");
            }
            time_display_b_old = (ushort?)_Micro850Client.GetTagValue("time_display_b");

            //Inverter

            //if ((bool?)_Micro850Client.GetTagValue("start_inverter") != start_old)
            //{
            //    Start = (bool?)_Micro850Client.GetTagValue("start_inverter");
            //}
            //start_old = (bool?)_Micro850Client.GetTagValue("stop_inverter");
            //if (Start is true)
            //{
            //    ButtonStartup = true;
            //    ButtonStop = false;
            //}
            ButtonStartup = (bool?)_Micro850Client.GetTagValue("inverter_active");
            ButtonStop = (bool?)_Micro850Client.GetTagValue("inverter_error");
            MotorForward = (bool?)_Micro850Client.GetTagValue("inverter_fwd_status");
            MotorReverse = (bool?)_Micro850Client.GetTagValue("inverter_rev_status");

            if ((ushort?)_Micro850Client.GetTagValue("setpoint") != setpoint_old)
            {
                MotorSetpoint = (ushort?)_Micro850Client.GetTagValue("setpoint");
            }
            setpoint_old = (ushort?)_Micro850Client.GetTagValue("setpoint");

            if ((float?)_Micro850Client.GetTagValue("speed") != speed_old)
            {
                MotorSpeed = (float?)_Micro850Client.GetTagValue("speed");
                MotorSpeed1 = Math.Round((float)_Micro850Client.GetTagValue("speed"), 2);
                if (ChartValues.Count() < 150)
                {
                    
                    var now = DateTime.Now;

                   
                    ChartValues.Add(new MeasureModel
                    {
                        DateTime = now,
                        Value = MotorSpeed1
                    });

                    ChartUpdate?.Invoke();
                    SetAxisLimits(now);
                }
                else ChartValues.RemoveAt(0);
            }
            speed_old = (float?)_Micro850Client.GetTagValue("speed");






            //Micro820

            //IO
            if ((bool?)_Micro850Client.GetTagValue2("i0.0") != i00_old)
            {
                i00 = (bool?)_Micro850Client.GetTagValue2("i0.0");
            }
            i00_old = (bool?)_Micro850Client.GetTagValue2("i0.0");
           

            if ((bool?)_Micro850Client.GetTagValue2("i0.1") != i01_old)
            {
                i01 = (bool?)_Micro850Client.GetTagValue2("i0.1");
            }
            i01_old = (bool?)_Micro850Client.GetTagValue2("i0.1");

            if ((bool?)_Micro850Client.GetTagValue2("i0.2") != i02_old)
            {
                i02 = (bool?)_Micro850Client.GetTagValue2("i0.2");
            }
            i02_old = (bool?)_Micro850Client.GetTagValue2("i0.2");

            if ((bool?)_Micro850Client.GetTagValue2("i0.3") != i03_old)
            {
                i03 = (bool?)_Micro850Client.GetTagValue2("i0.3");
            }
            i03_old = (bool?)_Micro850Client.GetTagValue2("i0.3");

            if ((bool?)_Micro850Client.GetTagValue2("i0.4") != i04_old)
            {
                i04= (bool?)_Micro850Client.GetTagValue2("i0.4");
            }
            i04_old = (bool?)_Micro850Client.GetTagValue2("i0.4");

            if ((bool?)_Micro850Client.GetTagValue2("i0.5") != i05_old)
            {
                i05 = (bool?)_Micro850Client.GetTagValue2("i0.5");
            }
            i05_old = (bool?)_Micro850Client.GetTagValue2("i0.5");

            if ((bool?)_Micro850Client.GetTagValue2("i0.6") != i06_old)
            {
                i06 = (bool?)_Micro850Client.GetTagValue2("i0.6");
            }
            i06_old = (bool?)_Micro850Client.GetTagValue2("i0.6");

            if ((bool?)_Micro850Client.GetTagValue2("i0.7") != i07_old)
            {
                i07 = (bool?)_Micro850Client.GetTagValue2("i0.7");
            }
            i07_old = (bool?)_Micro850Client.GetTagValue2("i0.7");

            //ANALOG DC
            if ((ushort?)_Micro850Client.GetTagValue2("analog") != analog_old)
            {
                Analog = (double?)((ushort?)_Micro850Client.GetTagValue2("analog"))*0.0025;
               
            }
            analog_old = (ushort?)_Micro850Client.GetTagValue2("analog");

        }






        public void Connect()
        {
            _Micro850Client.Connect();
            _timer.Enabled = true;
           
        }

        //TrafficLights
        public void Start_TrafficLight()
        {
            _Micro850Client.WritePLC(_Micro850Client.GetTagAddress("start_trafficlight"), true);
            Thread.Sleep(1000);
            _Micro850Client.WritePLC(_Micro850Client.GetTagAddress("start_trafficlight"), false);
        }

        public void Stop_TrafficLight()
        {
            _Micro850Client.WritePLC(_Micro850Client.GetTagAddress("stop_trafficlight"), true);
            Thread.Sleep(1000);
            _Micro850Client.WritePLC(_Micro850Client.GetTagAddress("stop_trafficlight"), false);
        }

        public void ConfirmTrafficLight()
        {
            _Micro850Client.WriteNumberPLC(_Micro850Client.GetTagAddress("edit_redled"), (UInt16)Edit_RedLed);
            _Micro850Client.WriteNumberPLC(_Micro850Client.GetTagAddress("edit_yellowled"), (UInt16)Edit_YellowLed);
            _Micro850Client.WriteNumberPLC(_Micro850Client.GetTagAddress("edit_greenled"), (UInt16)Edit_GreenLed);
            _Micro850Client.WritePLC(_Micro850Client.GetTagAddress("confirm_trafficlight"), true);
            Thread.Sleep(500);
            _Micro850Client.WritePLC(_Micro850Client.GetTagAddress("confirm_trafficlight"), false);

        }

        //Inverter

        public void Start_Inverter()
        {
            _Micro850Client.WritePLC(_Micro850Client.GetTagAddress("start_inverter"), true);
            Thread.Sleep(1000);
            _Micro850Client.WritePLC(_Micro850Client.GetTagAddress("start_inverter"), false);
        }

        public void Stop_Inverter()
        {
            _Micro850Client.WritePLC(_Micro850Client.GetTagAddress("stop_inverter"), true);
            Thread.Sleep(1000);
            _Micro850Client.WritePLC(_Micro850Client.GetTagAddress("stop_inverter"), false);
        }

        public void Forward_Inverter()
        {
            _Micro850Client.WritePLC(_Micro850Client.GetTagAddress("forward"), true);
            Thread.Sleep(1000);
            _Micro850Client.WritePLC(_Micro850Client.GetTagAddress("forward"), false);
        }

        public void Reverse_Inverter()
        {
            _Micro850Client.WritePLC(_Micro850Client.GetTagAddress("reverse"), true);
            Thread.Sleep(1000);
            _Micro850Client.WritePLC(_Micro850Client.GetTagAddress("reverse"), false);
        }

        public void WriteMotorSetpoint()
        {
            //_Micro850Client.WriteNumberPLC(_Micro850Client.GetTagAddress("setpoint"), (float)MotorSetpointWrite);
            _Micro850Client.WritePLC(_Micro850Client.GetTagAddress("confirm_inverter"), true);
            Thread.Sleep(1000);
            _Micro850Client.WritePLC(_Micro850Client.GetTagAddress("confirm_inverter"), false);
        }



    }
}

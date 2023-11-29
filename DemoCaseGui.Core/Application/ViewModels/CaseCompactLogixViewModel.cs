using CommunityToolkit.Mvvm.Input;
using DemoCaseGui.Core.Application.Communication;
using DemoCaseGui.Core.Application.Models;
using LiveCharts.Configurations;
using LiveCharts;
using System.Windows.Input;
using Timer = System.Timers.Timer;
using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DemoCaseGui.Core.Application.ViewModels
{
    public class CaseCompactLogixViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly CPLogixClient _CPLogixClient;
        private readonly Timer _timer;
      
        //private double _axisMax;
        //private double _axisMin;
        //private double _trend;
        //IO
        public bool? I0_0 { get; set; }
        public bool? I0_1 { get; set; }
        public bool? I0_2 { get; set; }
        public bool? I0_3 { get; set; }

        public bool? Q0_0 { get; set; }
        public bool? Q0_1 { get; set; }
        public bool? Q0_2 { get; set; }
        public bool? Q0_3 { get; set; }

        public bool? i0_0, i0_1, i0_2, i0_3, q0_0, q0_1, q0_2, q0_3;
        public bool? Start_auto, Start_manual, start_auto_old, start_manual_old, Stop_auto, Stop_manual, stop_auto_old, stop_manual_old, Start_Inverter, start_inverter_old, Stop_Inverter, stop_inverter_old;
        public bool? LED_AUTO_ON { get; set; }
        public bool? LED_MANUAL_ON { get; set; }

        public bool? LED_AUTO_OFF { get; set; }
        public bool? LED_MANUAL_OFF { get; set; }

        public bool? LED_INVERTER_ON { get; set; }
        public bool? LED_INVERTER_OFF { get; set; }
        //Traffic Lights
        public bool? DO1 { get; set; }
        public bool? DO2 { get; set; }
        public bool? XANH1 { get; set; }
        public bool? XANH2 { get; set; }
        public bool? VANG1 { get; set; }
        public bool? VANG2 { get; set; }

        public bool? do1, do2, xanh1, xanh2, vang1, vang2;

        public ushort? SET_D1 { get; set; }
        public ushort? SET_V1 { get; set; }
        public ushort? SET_X1 { get; set; }
        public ushort? set_d1_old, set_v1_old, set_x1_old;

        public ushort? Display_D1 { get; set; }
        public ushort? Display_V1 { get; set; }
        public ushort? Display_X1 { get; set; }
        public ushort? display_d1_old, display_v1_old, display_x1_old;
        public ushort? Display_D2 { get; set; }
        public ushort? Display_V2 { get; set; }
        public ushort? Display_X2 { get; set; }
        public ushort? display_d2_old, display_v2_old, display_x2_old;

        public ushort? Display_1 { get; set; }
        public ushort? Display_2 { get; set; }

        //AUTO MODE
        public ushort? TIME_DO1_AUTO { get; set; }
        public ushort? TIME_DO2_AUTO { get; set; }
        public ushort? TIME_XANH1_AUTO { get; set; }
        public ushort? TIME_XANH2_AUTO { get; set; }
        public ushort? TIME_VANG1_AUTO { get; set; }
        public ushort? TIME_VANG2_AUTO { get; set; }

        public ushort? time_do1_auto, time_do2_auto, time_xanh1_auto, time_xanh2_auto, time_vang1_auto, time_vang2_auto;

        //MANUAL MODE
        public ushort? TIME_DO1_MANUAL { get; set; }
        public ushort? TIME_DO2_MANUAL { get; set; }
        public ushort? TIME_XANH1_MANUAL { get; set; }
        public ushort? TIME_XANH2_MANUAL { get; set; }
        public ushort? TIME_VANG1_MANUAL { get; set; }
        public ushort? TIME_VANG2_MANUAL { get; set; }

        public ushort? time_do1_manual, time_do2_manual, time_xanh1_manual, time_xanh2_manual, time_vang1_manual, time_vang2_manual;

        //SENSOR
        public ushort? DEVICE_UGT_524 { get; set; }
        public ushort? DEVICE_KI6000 { get; set; }
        public ushort? DEVICE_O5D_150 { get; set; }
        public ushort? DEVICE_RPV_510 { get; set; }

        public bool? DEVICE_UGT_524_Status { get; set; }
        public bool? DEVICE_KI6000_Status { get; set; }
        public bool? DEVICE_O5D_150_Status { get; set; }
        public bool? DEVICE_RPV_510_Status { get; set; }
        public bool? DEVICE_IGS_232_Status { get; set; }
        public bool? DEVICE_OGT_500_Status { get; set; }

        public ushort? device_ugt_524, device_ki6000, device_o5d_150, device_rpv_510;
        public bool? device_ugt_524_status_old, device_ki6000_status_old, device_o5d_150_status_old, device_rpv_510_status_old, device_igs_232_status_old, device_ogt_500_status_old;

        //Lights IFM
        public bool? DEN_DO_IFM { get; set; }
        public bool? DEN_VANG_IFM { get; set; }
        public bool? DEN_XANH_IFM { get; set; }

        public bool? den_do_ifm, den_vang_ifm, den_xanh_ifm;

        //Inverter
        public ushort? Speed { get; set; }
        public float? Speed1 { get; set; }
        public ushort? speed_old, motorsetpoint_old;
        public ushort? Motorsetpoint { get; set; }
        public int MotorsetpointWrite { get; set; }
        public bool? Motor_Status { get; set; }
        public bool? motor_status_old;
        public bool? Direction_Status { get; set; }
        public bool? direction_status_old;

        public bool? Direction { get; set; }
        public bool? ButtonStartup { get; set; }
        public bool? ButtonStop { get; set; }
        public bool? MotorForward { get; set; }
        public bool? MotorReverse { get; set; }

        public ICommand ConnectCommand { get; set; }
        public ICommand Start_Auto_Command { get; set; }
        public ICommand Stop_Auto_Command { get; set; }
        public ICommand Start_Manual_Command { get; set; }
        public ICommand Stop_Manual_Command { get; set; }
        public ICommand Start_Inverter_Command { get; set; }
        public ICommand Stop_Inverter_Command { get; set; }
        public ICommand Forward_Inverter_Command { get; set; }
        public ICommand Write_Setpoint_Command { get; set; }
        public ICommand Write_TimeTrafficLights_Command { get; set; }

        public CaseCompactLogixViewModel()
        {
            _CPLogixClient = new CPLogixClient();
            _timer = new Timer(500);
            _timer.Elapsed += _timer_Elapsed;

            ConnectCommand = new RelayCommand(Connect);
            Start_Auto_Command = new RelayCommand(Auto_Start);
            Stop_Auto_Command = (new RelayCommand(Auto_Stop));
            Start_Manual_Command = (new RelayCommand(Manual_Start));
            Stop_Manual_Command = new RelayCommand(Manual_Stop);
            Start_Inverter_Command = new RelayCommand(Inverter_Start);
            Stop_Inverter_Command = new RelayCommand(Inverter_Stop);
            Forward_Inverter_Command = new RelayCommand(Inverter_Forward);
            Write_Setpoint_Command = new RelayCommand(WriteSetpoint);
            Write_TimeTrafficLights_Command = new RelayCommand(Set_TrafficLights);
            //var mapper = Mappers.Xy<MeasureModel>()
            //   .X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
            //   .Y(model => model.Value);           //use the value property as Y
            //Charting.For<MeasureModel>(mapper);

            ////the values property will store our values array
            //ChartValues = new ChartValues<MeasureModel>();

            ////lets set how to display the X Labels
            //DateTimeFormatter = value => new DateTime((long)value).ToString("hh:mm:ss");

            ////AxisStep forces the distance between each separator in the X axis
            //AxisStep = TimeSpan.FromSeconds(1).Ticks;
            ////AxisUnit forces lets the axis know that we are plotting seconds
            ////this is not always necessary, but it can prevent wrong labeling
            //AxisUnit = TimeSpan.TicksPerSecond;

            //SetAxisLimits(DateTime.Now);

        }

        private void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {            
            //TrafficLights

            if ((bool?)_CPLogixClient.GetTagValue("start_auto") != start_auto_old)
            {
                Start_auto = (bool?)_CPLogixClient.GetTagValue("start_auto");
            }
            start_auto_old = (bool?)_CPLogixClient.GetTagValue("start_auto");

            if (Start_auto is true)
            {
                LED_AUTO_ON = true;
                LED_AUTO_OFF = false;
            }

            if ((bool?)_CPLogixClient.GetTagValue("stop_auto") != stop_auto_old)
            {
                Stop_auto = (bool?)_CPLogixClient.GetTagValue("stop_auto");
            }
            stop_auto_old = (bool?)_CPLogixClient.GetTagValue("stop_auto");

            if (Stop_auto is true)
            {
                LED_AUTO_OFF = true;
                LED_AUTO_ON = false;
            }

            if ((bool?)_CPLogixClient.GetTagValue("start_manual") != start_manual_old)
            {
                Start_manual = (bool?)_CPLogixClient.GetTagValue("start_manual");
            }
            start_manual_old = (bool?)_CPLogixClient.GetTagValue("start_manual");

            if (Start_manual is true)
            {
                LED_MANUAL_ON = true;
                LED_MANUAL_OFF = false;
            }

            if ((bool?)_CPLogixClient.GetTagValue("stop_manual") != stop_manual_old)
            {
                Stop_manual = (bool?)_CPLogixClient.GetTagValue("stop_manual");
            }
            stop_manual_old = (bool?)_CPLogixClient.GetTagValue("stop_manual");

            if (Stop_manual is true)
            {
                LED_MANUAL_ON = false;
                LED_MANUAL_OFF = true;
            }

     

            if ((bool?)_CPLogixClient.GetTagValue("led_do1") != do1)
            {
                DO1 = (bool?)_CPLogixClient.GetTagValue("led_do1");
            }
            do1 = (bool?)_CPLogixClient.GetTagValue("led_do1");

            if ((bool?)_CPLogixClient.GetTagValue("led_do2") != do2)
            {
                DO2 = (bool?)_CPLogixClient.GetTagValue("led_do2");
            }
            do2 = (bool?)_CPLogixClient.GetTagValue("led_do2");

            if ((bool?)_CPLogixClient.GetTagValue("led_xanh1") != xanh1)
            {
                XANH1 = (bool?)_CPLogixClient.GetTagValue("led_xanh1");
            }
            xanh1 = (bool?)_CPLogixClient.GetTagValue("led_xanh1");

            if ((bool?)_CPLogixClient.GetTagValue("led_xanh2") != xanh2)
            {
                XANH2 = (bool?)_CPLogixClient.GetTagValue("led_xanh2");
            }
            xanh2 = (bool?)_CPLogixClient.GetTagValue("led_xanh2");

            if ((bool?)_CPLogixClient.GetTagValue("led_vang1") != vang1)
            {
                VANG1 = (bool?)_CPLogixClient.GetTagValue("led_vang1");
            }
            vang1 = (bool?)_CPLogixClient.GetTagValue("led_vang1");

            if ((bool?)_CPLogixClient.GetTagValue("led_vang2") != vang2)
            {
                VANG2 = (bool?)_CPLogixClient.GetTagValue("led_vang2");
            }
            vang2 = (bool?)_CPLogixClient.GetTagValue("led_vang2");


            if ((ushort?)_CPLogixClient.GetTagValue("set_do1") != set_d1_old)
            {
                SET_D1 = (ushort?)_CPLogixClient.GetTagValue("set_do1");
            }
            set_d1_old = (ushort?)_CPLogixClient.GetTagValue("set_do1");


            if ((ushort?)_CPLogixClient.GetTagValue("set_xanh1") != set_x1_old)
            {
                SET_X1 = (ushort?)_CPLogixClient.GetTagValue("set_xanh1");
            }
            set_x1_old = (ushort?)_CPLogixClient.GetTagValue("set_xanh1");


            if ((ushort?)_CPLogixClient.GetTagValue("set_vang1") != set_v1_old)
            {
                SET_V1 = (ushort?)_CPLogixClient.GetTagValue("set_vang1");
            }
            set_v1_old = (ushort?)_CPLogixClient.GetTagValue("set_vang1");

            if ((ushort?)_CPLogixClient.GetTagValue("time_do1_dp") != display_d1_old)
            {
                Display_D1 = (ushort?)_CPLogixClient.GetTagValue("time_do1_dp");
            }
            display_d1_old = (ushort?)_CPLogixClient.GetTagValue("time_do1_dp");

            if ((ushort?)_CPLogixClient.GetTagValue("time_do2_dp") != display_d2_old)
            {
                Display_D2 = (ushort?)_CPLogixClient.GetTagValue("time_do2_dp");
            }
            display_d2_old = (ushort?)_CPLogixClient.GetTagValue("time_do2_dp");

            if ((ushort?)_CPLogixClient.GetTagValue("time_vang1_dp") != display_v1_old)
            {
                Display_V1 = (ushort?)_CPLogixClient.GetTagValue("time_vang1_dp");
            }
            display_v1_old = (ushort?)_CPLogixClient.GetTagValue("time_vang1_dp");

            if ((ushort?)_CPLogixClient.GetTagValue("time_vang2_dp") != display_v2_old)
            {
                Display_V2 = (ushort?)_CPLogixClient.GetTagValue("time_vang2_dp");
            }
            display_v2_old = (ushort?)_CPLogixClient.GetTagValue("time_vang2_dp");


            if ((ushort?)_CPLogixClient.GetTagValue("time_xanh1_dp") != display_x1_old)
            {
                Display_X1 = (ushort?)_CPLogixClient.GetTagValue("time_xanh1_dp");
            }
            display_x1_old = (ushort?)_CPLogixClient.GetTagValue("time_xanh1_dp");

            if ((ushort?)_CPLogixClient.GetTagValue("time_xanh2_dp") != display_x2_old)
            {
                Display_X2 = (ushort?)_CPLogixClient.GetTagValue("time_xanh2_dp");
            }
            display_x2_old = (ushort?)_CPLogixClient.GetTagValue("time_xanh2_dp");

            if(Display_D1 != 0)
            {
                Display_1 = Display_D1;
            }
            else
            {
                if (Display_X1 != 0) Display_1 = Display_X1;              
                else Display_1 = Display_V1;
            }

            if (Display_D2 != 0)
            {
                Display_2 = Display_D2;
            }
            else
            {
                if (Display_X2 != 0) Display_2 = Display_X2;
                else Display_2 = Display_V2;
            }



            //SENSOR

            if ((ushort?)_CPLogixClient.GetTagValue("ugt_524") != device_ugt_524)
            {
                DEVICE_UGT_524 = (ushort?)_CPLogixClient.GetTagValue("ugt_524");
            }
            device_ugt_524 = (ushort?)_CPLogixClient.GetTagValue("ugt_524");

            if ((bool?)_CPLogixClient.GetTagValue("ugt_524_st") != device_ugt_524_status_old)
            {
                DEVICE_UGT_524_Status = (bool?)_CPLogixClient.GetTagValue("ugt_524_st");
            }
            device_ugt_524_status_old = (bool?)_CPLogixClient.GetTagValue("ugt_524_st");

            if ((ushort?)_CPLogixClient.GetTagValue("ki6000") != device_ki6000)
            {
                DEVICE_KI6000 = (ushort?)_CPLogixClient.GetTagValue("ki6000");
            }
            device_ki6000 = (ushort?)_CPLogixClient.GetTagValue("ki6000");


            if ((bool?)_CPLogixClient.GetTagValue("ki6000_st") != device_ki6000_status_old)
            {
                DEVICE_KI6000_Status = (bool?)_CPLogixClient.GetTagValue("ki6000_st");
            }
            device_ki6000_status_old = (bool?)_CPLogixClient.GetTagValue("ki6000_st");


            if ((ushort?)_CPLogixClient.GetTagValue("05d_150") != device_o5d_150)
            {
                DEVICE_O5D_150 = (ushort?)_CPLogixClient.GetTagValue("05d_150");
            }
            device_o5d_150 = (ushort?)_CPLogixClient.GetTagValue("05d_150");

            if ((bool?)_CPLogixClient.GetTagValue("05d_150_st") != device_o5d_150_status_old)
            {
                DEVICE_O5D_150_Status = (bool?)_CPLogixClient.GetTagValue("05d_150_st");
            }
            device_o5d_150_status_old = (bool?)_CPLogixClient.GetTagValue("05d_150_st");

            if ((ushort?)_CPLogixClient.GetTagValue("rpv_510") != device_rpv_510)
            {
                DEVICE_RPV_510 = (ushort?)_CPLogixClient.GetTagValue("rpv_510");
            }
            device_rpv_510 = (ushort?)_CPLogixClient.GetTagValue("rpv_510");

            if ((bool?)_CPLogixClient.GetTagValue("rpv_510_st") != device_rpv_510_status_old)
            {
                DEVICE_RPV_510_Status = (bool?)_CPLogixClient.GetTagValue("rpv_510_st");
            }
            device_rpv_510_status_old = (bool?)_CPLogixClient.GetTagValue("rpv_510_st");

            if ((bool?)_CPLogixClient.GetTagValue("igs_232") != device_igs_232_status_old)
            {
                DEVICE_IGS_232_Status = (bool?)_CPLogixClient.GetTagValue("igs_232");
            }
            device_igs_232_status_old = (bool?)_CPLogixClient.GetTagValue("igs_232");

            if ((bool?)_CPLogixClient.GetTagValue("ogt_500") != device_ogt_500_status_old)
            {
                DEVICE_OGT_500_Status = (bool?)_CPLogixClient.GetTagValue("ogt_500");
            }
            device_ogt_500_status_old = (bool?)_CPLogixClient.GetTagValue("ogt_500");

            //Lights IFM

            if ((bool?)_CPLogixClient.GetTagValue("Alarm_den_do_IFM") != den_do_ifm)
            {
                DEN_DO_IFM = (bool?)_CPLogixClient.GetTagValue("Alarm_den_do_IFM");
            }
            den_do_ifm = (bool?)_CPLogixClient.GetTagValue("Alarm_den_do_IFM");


            if ((bool?)_CPLogixClient.GetTagValue("Alarm_den_vang_IFM") != den_vang_ifm)
            {
                DEN_VANG_IFM = (bool?)_CPLogixClient.GetTagValue("Alarm_den_vang_IFM");
            }
            den_vang_ifm = (bool?)_CPLogixClient.GetTagValue("Alarm_den_vang_IFM");


            if ((bool?)_CPLogixClient.GetTagValue("Alarm_den_xanh_IFM") != den_xanh_ifm)
            {
                DEN_XANH_IFM = (bool?)_CPLogixClient.GetTagValue("Alarm_den_xanh_IFM");
            }
            den_xanh_ifm = (bool?)_CPLogixClient.GetTagValue("Alarm_den_xanh_IFM");

            //Inverter
            ButtonStartup = Motor_Status;
            ButtonStop = !Motor_Status;
            MotorForward = Direction;
            MotorReverse = !Direction;
            

            if ((ushort?)_CPLogixClient.GetTagValue("speed") != speed_old)
            {
                Speed = (ushort?)_CPLogixClient.GetTagValue("speed");
                //var now = DateTime.Now;

                //Random random = new Random();
                //ChartValues.Add(new MeasureModel
                //{
                //    DateTime = now,
                //    Value = 7,
                //}) ;

                //SetAxisLimits(now);

                //if (ChartValues.Count() > 150)
                //{
                //    ChartValues.RemoveAt(0);
                //}

            }
            speed_old = (ushort?)_CPLogixClient.GetTagValue("speed");

            




            if ((ushort?)_CPLogixClient.GetTagValue("motor_sp") != motorsetpoint_old)
            {
                Motorsetpoint = (ushort?)_CPLogixClient.GetTagValue("motor_sp");
            }
            motorsetpoint_old = (ushort?)_CPLogixClient.GetTagValue("motor_sp");

            if ((bool?)_CPLogixClient.GetTagValue("direction_status_inverter") != direction_status_old)
            {
                Direction_Status = (bool?)_CPLogixClient.GetTagValue("direction_status_inverter");
            }
            direction_status_old = (bool?)_CPLogixClient.GetTagValue("direction_status_inverter");

            

            
            Motor_Status = (bool?)_CPLogixClient.GetTagValue("status_inverter");
         
            if (Motor_Status == false) Direction = null;
            else
            {
                if ((bool?)_CPLogixClient.GetTagValue("direction_status_inverter") == true)
                    Direction = true;
                else
                    Direction = false;
            }



        }

        //public ChartValues<MeasureModel> ChartValues { get; set; }
        //public Func<double, string> DateTimeFormatter { get; set; }
        //public double AxisStep { get; set; }
        //public double AxisUnit { get; set; }
        //public double AxisMax
        //{
        //    get { return _axisMax; }
        //    set
        //    {
        //        _axisMax = value;
        //        OnPropertyChanged("AxisMax");
        //    }
        //}
        //public double AxisMin
        //{
        //    get { return _axisMin; }
        //    set
        //    {
        //        _axisMin = value;
        //        OnPropertyChanged("AxisMin");
        //    }
        //}
        //private void SetAxisLimits(DateTime now)
        //{
        //    AxisMax = now.Ticks + TimeSpan.FromSeconds(1).Ticks; // lets force the axis to be 1 second ahead
        //    AxisMin = now.Ticks - TimeSpan.FromSeconds(8).Ticks; // and 8 seconds behind
        //}
        //public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged(string propertyName = null)
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        public void Connect()
        {
            _CPLogixClient.Connect();
            _timer.Enabled = true;
        }

        public void Auto_Start()
        {
            _CPLogixClient.WritePLC(_CPLogixClient.GetTagAddress("start_auto"), true);
            Thread.Sleep(500);
            _CPLogixClient.WritePLC(_CPLogixClient.GetTagAddress("start_auto"), false);
        }

        public void Auto_Stop()
        {
            _CPLogixClient.WritePLC(_CPLogixClient.GetTagAddress("stop_auto"), true);
            Thread.Sleep(500);
            _CPLogixClient.WritePLC(_CPLogixClient.GetTagAddress("stop_auto"), false);

        }

        public void Manual_Start()
        {
            _CPLogixClient.WritePLC(_CPLogixClient.GetTagAddress("start_manual"), true);
            Thread.Sleep(500);
            _CPLogixClient.WritePLC(_CPLogixClient.GetTagAddress("start_manual"), false);
        }

        public void Manual_Stop()
        {
            _CPLogixClient.WritePLC(_CPLogixClient.GetTagAddress("stop_manual"), true);
            Thread.Sleep(500);
            _CPLogixClient.WritePLC(_CPLogixClient.GetTagAddress("stop_manual"), false);
        }


        public void Inverter_Start()
        {
            _CPLogixClient.WritePLC("START_INVERTER_WEB", true);
            Thread.Sleep(500);
            _CPLogixClient.WritePLC("START_INVERTER_WEB", false);
        }

        public void Inverter_Stop()
        {
            _CPLogixClient.WritePLC("STOP_INVERTER_WEB", true);
            Thread.Sleep(500);
            _CPLogixClient.WritePLC("STOP_INVERTER_WEB", false);
        }

        public void Inverter_Forward()
        {
            _CPLogixClient.WritePLC("DAOCHIEU_WEB", true);
            Thread.Sleep(500);
            _CPLogixClient.WritePLC("DAOCHIEU_WEB", false);
        }

        public void WriteSetpoint()
            {
            _CPLogixClient.WriteNumberPLC("SPEED_INVERTER", MotorsetpointWrite);
           
        }
        public void Set_TrafficLights()
        {
            _CPLogixClient.WriteNumberPLC("SET_D1",(int)SET_D1);           
            _CPLogixClient.WriteNumberPLC("SET_X1", (int)SET_X1);
            _CPLogixClient.WriteNumberPLC("SET_V1", (int)SET_V1);
        }

    }
}

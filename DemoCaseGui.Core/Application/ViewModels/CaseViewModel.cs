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
using Timer = System.Timers.Timer;
using MqttClient = DemoCaseGui.Core.Application.Communication.MqttClient;
using LiveCharts.Configurations;
using LiveCharts;
using DemoCaseGui.Core.Application.Models;
using System.ComponentModel;

namespace DemoCaseGui.Core.Application.ViewModels
{
    public class CaseViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly S7Client _s7Client;
        private readonly MqttClient _mqttClient;
        private readonly Timer _timer;
        public bool IsConnected => _s7Client.IsConnected;
        public bool IsMqttConnected => _mqttClient.IsConnected;
        

        public ChartValues<double> Value { get; set; }
        public event Action? ChartUpdate;
        private double _axisMax;
        private double _axisMin;
        private double _trend;

        public bool? ledGreen_old, ledRed_old, ledYellow_old, dCMotor_old, statusIF6123_old, statusKT5112_old, statusO5C500_old, statusUGT524_old;
        public float? angleRB3100_old, tempTW2000_old;
        public ushort? countRB3100_old, distanceUGT524_old, setpoint_old, speed_old;
        //light and DC motor
        public bool? LedGreen { get; set; }
        public bool? LedRed { get; set; }
        public bool? LedYellow { get; set; }
        public bool? DCMotor { get; set; }

        //sensor
        public float? RB3100Angle { get; set; }
        public ushort? RB3100Count { get; set; }
        public float? TW2000Temp { get; set; }
        public bool? IF6123Status { get; set; }
        public bool? KT5112Status { get; set; }
        public bool? O5C500Status { get; set; }
        public bool? UGT524Status { get; set; }
        public ushort? UGT524Distance { get; set; }
        public ushort Resolution { get; set; } = 0;

        //inverter
        public bool? Status { get; set; } = false;
        public bool? Direction { get; set; }
        public bool? ButtonStartup { get; set; }
        public bool? ButtonStop { get; set; }
        public bool? MotorForward { get; set; }
        public bool? MotorReverse { get; set; }
        public ushort MotorSetpointWrite { get; set; }
        public ushort? MotorSetpoint { get; set; }
        public ushort? MotorSpeed { get; set; }

        //Siemens Demo Case 
        public bool? Toggle1 { get; set; }
        public bool? Toggle2 { get; set; }
        public bool? Toggle3 { get; set; }
        public bool? Toggle4 { get; set; }
        public bool? Toggle5 { get; set; }
        public bool? Toggle6 { get; set; }
        public bool? Toggle7 { get; set; }
        public bool? Toggle8 { get; set; }
        public float SetpointSpeed { get; set; } = 0;
        public float SetpointPosition { get; set; } = 0;
        //
        public bool? Led0 { get; set; }
        public bool? Led1 { get; set; }
        public bool? Led2 { get; set; }
        public bool? Led3 { get; set; }
        public bool? Led4 { get; set; }
        public bool? Led5 { get; set; }
        public bool? Led6 { get; set; }
        public bool? Led7 { get; set; }
        public float? CurrentSpeed { get; set; }
        public float? CurrentPosition { get; set; }

        public ICommand ConnectCommand { get; set; }
        public ICommand ResolutionOKCommand { get; set; }
        public ICommand MotorSetpointOKCommand { get; set; }
        public ICommand SpeedOKCommand { get; set; }
        public ICommand PositionOKCommand { get; set; }
        public ICommand StartStepMotorCommand { get; set; }
        public ICommand SethomeStepMotorCommand { get; set; }
        public ICommand AutoModeStepMotorCommand { get; set; }

        public ICommand StartInverter { get; set; }
        public ICommand StopInverter { get; set; }

        public ICommand FWDInverter { get; set; }
        public ICommand REVInverter { get; set; }

        public ICommand FWDStepMotor { get; set; }
        public ICommand BACKFWDStepMotor { get; set; }


        public CaseViewModel()
        {
            _s7Client = new S7Client();
            _mqttClient = new MqttClient();
            _timer = new Timer(1000);
            _timer.Elapsed += TimerElapsed;
            _mqttClient.ApplicationMessageReceived += OnApplicationMessageReceived;
            ConnectCommand = new RelayCommand(Connect);
            ResolutionOKCommand = new RelayCommand(WriteResolution);
            MotorSetpointOKCommand = new RelayCommand(WriteMotorSetpoint);
            SpeedOKCommand = new RelayCommand(WriteSpeed);
            PositionOKCommand = new RelayCommand(WritePosition);
            StartStepMotorCommand = new RelayCommand(StartInverterStep);
            SethomeStepMotorCommand = new RelayCommand(SethomeInverterStep);
            AutoModeStepMotorCommand = new RelayCommand(AutoInverterStep);
            StartInverter = new RelayCommand(StratInverter);
            StopInverter = new RelayCommand(StopInverter1);
            FWDInverter = new RelayCommand(FWD);
            REVInverter = new RelayCommand(REV);
            FWDStepMotor = new RelayCommand(FWD_StepMotor);
            BACKFWDStepMotor = new RelayCommand(BACKFWD_StepMotor);
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
        private async void TimerElapsed(object? sender, EventArgs args)
        {
            //light and DC motor
            if ((bool?)_s7Client.GetTagValue("ledGreen") != ledGreen_old)
            {
                LedGreen = (bool?)_s7Client.GetTagValue("ledGreen");
                await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/ledGreen", JsonConvert.SerializeObject(_s7Client.GetTag("ledGreen")), true);
            }
            ledGreen_old = (bool?)_s7Client.GetTagValue("ledGreen");

            if ((bool?)_s7Client.GetTagValue("ledRed") != ledRed_old)
            {
                LedRed = (bool?)_s7Client.GetTagValue("ledRed");
                await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/ledRed", JsonConvert.SerializeObject(_s7Client.GetTag("ledRed")), true);
            }
            ledRed_old = (bool?)_s7Client.GetTagValue("ledRed");

            if ((bool?)_s7Client.GetTagValue("ledYellow") != ledYellow_old)
            {
                LedYellow = (bool?)_s7Client.GetTagValue("ledYellow");
                await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/ledYellow", JsonConvert.SerializeObject(_s7Client.GetTag("ledYellow")), true);
            }
            ledYellow_old = (bool?)_s7Client.GetTagValue("ledYellow");

            if ((bool?)_s7Client.GetTagValue("DCMotor") != dCMotor_old)
            {
                DCMotor = (bool?)_s7Client.GetTagValue("DCMotor");
                await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/DCMotor", JsonConvert.SerializeObject(_s7Client.GetTag("DCMotor")), true);
            }
            dCMotor_old = (bool?)_s7Client.GetTagValue("DCMotor");

            
            //sensor
            if ((bool?)_s7Client.GetTagValue("statusIF6123") != statusIF6123_old)
            {
                IF6123Status = (bool?)_s7Client.GetTagValue("statusIF6123");
                await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/statusIF6123", JsonConvert.SerializeObject(_s7Client.GetTag("statusIF6123")), true);
            }
            statusIF6123_old = (bool?)_s7Client.GetTagValue("statusIF6123");

            if ((bool?)_s7Client.GetTagValue("statusKT5112") != statusKT5112_old)
            {
                KT5112Status = (bool?)_s7Client.GetTagValue("statusKT5112");
                await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/statusKT5112", JsonConvert.SerializeObject(_s7Client.GetTag("statusKT5112")), true);
            }
            statusKT5112_old = (bool?)_s7Client.GetTagValue("statusKT5112");

            if ((bool?)_s7Client.GetTagValue("statusO5C500") != statusO5C500_old)
            {
                O5C500Status = (bool?)_s7Client.GetTagValue("statusO5C500");
                await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/statusO5C500", JsonConvert.SerializeObject(_s7Client.GetTag("statusO5C500")), true);
            }
            statusO5C500_old = (bool?)_s7Client.GetTagValue("statusO5C500");

            if ((bool?)_s7Client.GetTagValue("statusUGT524") != statusUGT524_old)
            {
                UGT524Status = (bool?)_s7Client.GetTagValue("statusUGT524");
                await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/statusUGT524", JsonConvert.SerializeObject(_s7Client.GetTag("statusUGT524")), true);
            }
            statusUGT524_old = (bool?)_s7Client.GetTagValue("statusUGT524");

            if ((float?)_s7Client.GetTagValue("angleRB3100") != angleRB3100_old)
            {
                RB3100Angle = (float?)_s7Client.GetTagValue("angleRB3100");
                await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/angleRB3100", JsonConvert.SerializeObject(_s7Client.GetTag("angleRB3100")), true);
            }
            angleRB3100_old = (float?)_s7Client.GetTagValue("angleRB3100");

            if ((ushort?)_s7Client.GetTagValue("countRB3100") != countRB3100_old)
            {
                RB3100Count = (ushort?)_s7Client.GetTagValue("countRB3100");
                await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/countRB3100", JsonConvert.SerializeObject(_s7Client.GetTag("countRB3100")), true);
            }
            countRB3100_old = (ushort?)_s7Client.GetTagValue("countRB3100");

            if ((float?)_s7Client.GetTagValue("tempTW2000") != tempTW2000_old)
            {
                TW2000Temp = (float?)_s7Client.GetTagValue("tempTW2000");
                await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/tempTW2000", JsonConvert.SerializeObject(_s7Client.GetTag("tempTW2000")), true);
            }
            tempTW2000_old = (float?)_s7Client.GetTagValue("tempTW2000");

            if ((ushort?)_s7Client.GetTagValue("distanceUGT524") != distanceUGT524_old)
            {
                if ((ushort?)_s7Client.GetTagValue("distanceUGT524") > 200) UGT524Distance = null;
                else UGT524Distance = (ushort?)_s7Client.GetTagValue("distanceUGT524");
                await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/distanceUGT524", JsonConvert.SerializeObject(_s7Client.GetTag("distanceUGT524")), true);
            }
            distanceUGT524_old = (ushort?)_s7Client.GetTagValue("distanceUGT524");

            //inverter
            ButtonStartup = Status;
            ButtonStop = !Status ;
            MotorForward = Direction;
            MotorReverse = !Direction;
            MotorSetpoint = (ushort?)_s7Client.GetTagValue("setpoint");
            if ((ushort?)_s7Client.GetTagValue("speed") != speed_old)
            {
                MotorSpeed = (ushort?)_s7Client.GetTagValue("speed");
                if (ChartValues.Count() < 150)
                {

                    var now = DateTime.Now;


                    ChartValues.Add(new MeasureModel
                    {
                        DateTime = now,
                        Value = Math.Round((float)_s7Client.GetTagValue("speed"), 2)
                    }) ;
                    ChartUpdate?.Invoke();
                    SetAxisLimits(now);
                }
                else ChartValues.RemoveAt(0);
                await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/speed", JsonConvert.SerializeObject(_s7Client.GetTag("speed")), true);
            }
            speed_old = (ushort?)_s7Client.GetTagValue("speed");

            //Siemens Demo Case
            Toggle1 = (bool?)_s7Client.GetTagValue("toggle1");
            Toggle2 = (bool?)_s7Client.GetTagValue("toggle2");
            Toggle3 = (bool?)_s7Client.GetTagValue("toggle3");
            Toggle4 = (bool?)_s7Client.GetTagValue("toggle4");
            Toggle5 = (bool?)_s7Client.GetTagValue("toggle5");
            Toggle6 = (bool?)_s7Client.GetTagValue("toggle6");
            Toggle7 = (bool?)_s7Client.GetTagValue("toggle7");
            Toggle8 = (bool?)_s7Client.GetTagValue("toggle8");

            Led0 = (bool?)_s7Client.GetTagValue("led0");
            Led1 = (bool?)_s7Client.GetTagValue("led1");
            Led2 = (bool?)_s7Client.GetTagValue("led2");
            Led3 = (bool?)_s7Client.GetTagValue("led3");
            Led4 = (bool?)_s7Client.GetTagValue("led4");
            Led5 = (bool?)_s7Client.GetTagValue("led5");
            Led6 = (bool?)_s7Client.GetTagValue("led6");
            Led7 = (bool?)_s7Client.GetTagValue("led7");
            CurrentSpeed = (float?)_s7Client.GetTagValue("current_speed_M");
            CurrentPosition = (float?)_s7Client.GetTagValue("current_position_M");

            Status = (bool?)_s7Client.GetTagValue("statusInverter");
            if (Status == false) Direction = null;
            else
            {
                if ((bool?)_s7Client.GetTagValue("directionForward") == true)
                    Direction = true;
                if ((bool?)_s7Client.GetTagValue("directionReverse") == true)
                    Direction = false;
            }

            
            //await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/angleRB3100", JsonConvert.SerializeObject(_s7Client.GetTag("angleRB3100")), false);
            //await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/countRB3100", JsonConvert.SerializeObject(_s7Client.GetTag("countRB3100")), false);
            //await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/tempTW2000", JsonConvert.SerializeObject(_s7Client.GetTag("tempTW2000")), false);
            //await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/statusIF6123", JsonConvert.SerializeObject(_s7Client.GetTag("statusIF6123")), false);
            //await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/statusKT5112", JsonConvert.SerializeObject(_s7Client.GetTag("statusKT5112")), false);
            //await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/statusO5C500", JsonConvert.SerializeObject(_s7Client.GetTag("statusO5C500")), false);
            //await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/statusUGT524", JsonConvert.SerializeObject(_s7Client.GetTag("statusUGT524")), false);
            //await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/distanceUGT524", JsonConvert.SerializeObject(_s7Client.GetTag("distanceUGT524")), false);
            ///
           
            //await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/ledGreen", JsonConvert.SerializeObject(_s7Client.GetTag("ledGreen")), false);
            //await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/ledRed", JsonConvert.SerializeObject(_s7Client.GetTag("ledRed")), false);
            //await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/ledYellow", JsonConvert.SerializeObject(_s7Client.GetTag("ledYellow")), false);
            //await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/DCMotor", JsonConvert.SerializeObject(_s7Client.GetTag("DCMotor")), false);
            
            //await _mqttClient.Publish("VTSauto/AR_project/Desktop_pub/speed", JsonConvert.SerializeObject(_s7Client.GetTag("speed")), false);
        }


        private async Task OnApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            var topic = e.ApplicationMessage.Topic;
            var payloadMessage = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            var commandMessage = JsonConvert.DeserializeObject<CommandMessage>(payloadMessage);

            switch (commandMessage.name)
            {
                case "controlRed":
                    _s7Client.WritePLC("DB2.DBX16.2", commandMessage.value);
                    break;
                case "controlGreen":
                    _s7Client.WritePLC("DB2.DBX16.0", commandMessage.value);
                    break;
                case "controlYellow":
                    _s7Client.WritePLC("DB2.DBX16.3", commandMessage.value);
                    break;
                case "controlDCMotor":
                    _s7Client.WritePLC("DB2.DBX16.1", commandMessage.value);
                    break;
                //
                case "VFD_Speed_SP":
                    _s7Client.WriteNumberPLC("DB4.DBW8", commandMessage.value);
                    break;
                case "VFD_Start":
                    Status = true;
                    _s7Client.WritePLC("DB4.DBX6.0", commandMessage.value);
                    break;
                case "VFD_Stop":
                    Status = false;
                    Direction = null;
                    _s7Client.WritePLC("DB4.DBX6.1", commandMessage.value);
                    break;
                case "VFD_Forward":
                    if (Status == true) Direction = true;
                    _s7Client.WritePLC("DB4.DBX6.2", commandMessage.value);
                    break;
                case "VFD_Reverse":
                    if (Status == true) Direction = false;
                    _s7Client.WritePLC("DB4.DBX6.3", commandMessage.value);
                    break;
                default:
                    _s7Client.WritePLC(_s7Client.GetTagAddress(commandMessage.name), commandMessage.value);
                    break;
            }
        }

        public async void Connect()
        {
            await _s7Client.Connect();
            await _mqttClient.ConnectAsync();
            await _mqttClient.Subscribe("VTSauto/AR_project/Desktop_write");
            await _mqttClient.Subscribe("VTSauto/AR_project/Desktop_write/controlRed");
            await _mqttClient.Subscribe("VTSauto/AR_project/Desktop_write/controlGreen");
            await _mqttClient.Subscribe("VTSauto/AR_project/Desktop_write/controlYellow");
            await _mqttClient.Subscribe("VTSauto/AR_project/Desktop_write/controlDCMotor");

            await _mqttClient.Subscribe("VTSauto/AR_project/Desktop_write/VFD_Start");
            await _mqttClient.Subscribe("VTSauto/AR_project/Desktop_write/VFD_Stop");
            await _mqttClient.Subscribe("VTSauto/AR_project/Desktop_write/VFD_Forward");
            await _mqttClient.Subscribe("VTSauto/AR_project/Desktop_write/VFD_Reverse");
            await _mqttClient.Subscribe("VTSauto/AR_project/Desktop_write/VFD_Speed_SP");
            _timer.Enabled = true;
        }
        public void WriteResolution()
        {
            _s7Client.WriteNumberPLC(_s7Client.GetTagAddress("resolutionRB3100"), Resolution);
        }
        public void WriteMotorSetpoint()
        {
            _s7Client.WriteNumberPLC(_s7Client.GetTagAddress("setpoint"), MotorSetpointWrite);
        }
        public void WriteSpeed()
        {
            _s7Client.WritePLC(_s7Client.GetTagAddress("setpoint_speed_M"), SetpointSpeed);
        }

        public void WritePosition()
        {
            _s7Client.WritePLC(_s7Client.GetTagAddress("setpoint_position_M"), SetpointPosition);
        }

        public void StartInverterStep()
        {
            _s7Client.WritePLCI("DB2.DBX38.2", true);
            Thread.Sleep(1000);
            _s7Client.WritePLCI("DB2.DBX38.2", false);
        }
        public void AutoInverterStep()
        {
            _s7Client.WritePLCI("DB2.DBX38.0", true);
            Thread.Sleep(1000);
            _s7Client.WritePLCI("DB2.DBX38.0", false);
        }
        public void SethomeInverterStep()
        {
            _s7Client.WritePLCI("DB2.DBX38.5", true);
            Thread.Sleep(1000);
            _s7Client.WritePLCI("DB2.DBX38.5", false);
        }
        public void StratInverter()
        {
            _s7Client.WritePLC("DB4.DBX6.0", true);
            Thread.Sleep(1000);
            _s7Client.WritePLC("DB4.DBX6.0", false);
        }
        public void StopInverter1()
        {
            _s7Client.WritePLC("DB4.DBX6.1", true);
            Thread.Sleep(1000);
            _s7Client.WritePLC("DB4.DBX6.1", false);
        }

        public void FWD()
        {
            _s7Client.WritePLC("DB4.DBX6.2", true);
            Thread.Sleep(1000);
            _s7Client.WritePLC("DB4.DBX6.2", false);
        }

        public void REV()
        {
            _s7Client.WritePLC("DB4.DBX6.3", true);
            Thread.Sleep(1000);
            _s7Client.WritePLC("DB4.DBX6.3", false);
        }

        public void FWD_StepMotor()
        {
            _s7Client.WritePLC(_s7Client.GetTagAddress("foward"), true);
            Thread.Sleep(1000);
            _s7Client.WritePLC(_s7Client.GetTagAddress("foward"), false);
        }

        public void BACKFWD_StepMotor()
        {
            _s7Client.WritePLC(_s7Client.GetTagAddress("reverse"), true);
            Thread.Sleep(1000);
            _s7Client.WritePLC(_s7Client.GetTagAddress("reverse"), false);
        }
    }
}

    using HslCommunication;
using HslCommunication.Profinet.AllenBradley;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace DemoCaseGui.Core.Application.Communication
{
    public class CPLogixClient
    {
        private readonly AllenBradleyConnectedCipNet plc;
        private readonly Timer _timer;
        public List<Tag> Tags { get; private set; }
        public List<MqttTag> MqttTags { get; private set; }

        public CPLogixClient()
        {
            plc = new AllenBradleyConnectedCipNet("192.168.1.101");
            _timer = new Timer(500);
            _timer.Elapsed += _timer_Elapsed;
            Tags = new()
        {
            //IO
     

            //TrafficLights
            new("led_do1", "AB.CL5300.Traffic_do1", null, "DO1", DateTime.Now),
            new("led_do2", "AB.CL5300.Traffic_do2", null, "DO2", DateTime.Now),
            new("led_xanh1", "AB.CL5300.Traffic_xanh1", null, "XANH1", DateTime.Now),
            new("led_xanh2", "AB.CL5300.Traffic_xanh2", null, "XANH2", DateTime.Now),
            new("led_vang1", "AB.CL5300.Traffic_vang1", null, "VANG1", DateTime.Now),
            new("led_vang2", "AB.CL5300.Traffic_vang2", null, "VANG2", DateTime.Now),

            new("set_do1", "AB.CL5300.Traffic_SET_D1", null, "SET_D1", DateTime.Now),
            new("set_xanh1", "AB.CL5300.Traffic_SET_V1", null, "SET_X1", DateTime.Now),
            new("set_vang1", "AB.CL5300.Traffic_SET_X1", null, "SET_V1", DateTime.Now),
            //Time
             new("time_do1_dp", "AB.CL5300.Traffic_time_D1_manual", null, "D1_HIEN", DateTime.Now),
            new("time_xanh1_dp", "AB.CL5300.Traffic_time_X1_manual", null, "X1_HIEN", DateTime.Now),
            new("time_vang1_dp", "AB.CL5300.Traffic_time_V1_manual", null, "V1_HIEN", DateTime.Now),

             new("time_do2_dp", "AB.CL5300.Traffic_time_D2_manual", null, "D2_HIEN", DateTime.Now),
            new("time_xanh2_dp", "AB.CL5300.Traffic_time_X2_manual", null, "X2_HIEN", DateTime.Now),
            new("time_vang2_dp", "AB.CL5300.Traffic_time_V2_manual", null, "V2_HIEN", DateTime.Now),
            //AUTO MODE
            

            
            new("start_manual", "AB.CL5300.Traffic_start_manual", null, "START_MANUAL_WEB", DateTime.Now),
           
            new("stop_manual", "AB.CL5300.Traffic_stop_manual", null, "STOP_MANUAL_WEB", DateTime.Now),

            //MANUAL MODE
            

            //SENSOR
            new("ugt_524", "AB.CL5300.Sensor_UGT524_PV", null, "UGT_524_ALARM.PV_DEVICE", DateTime.Now),
            new("ugt_524_st", "AB.CL5300.Sensor_UGT524_Status", null, "UGT_524.OUT1", DateTime.Now),

            new("ki6000", "AB.CL5300.Sensor_KI6000_PV", null, "KI6000_ALARM.PV_DEVICE", DateTime.Now),
            new("ki6000_st", "AB.CL5300.Sensor_KI6000_Status", null, "KI6000.BDC1", DateTime.Now),

            new("05d_150", "AB.CL5300.Sensor_O5D150_PV", null, "O5D_150_ALARM.PV_DEVICE", DateTime.Now),
            new("05d_150_st", "AB.CL5300.Sensor_O5D150_Status", null, "O5D_150.OUT1", DateTime.Now),

            new("rpv_510", "AB.CL5300.Sensor_RVP510_PV", null, "RPV_510_ALARM.PV_DEVICE", DateTime.Now),
            new("rpv_510_st", "AB.CL5300.Sensor_RVP510_Status", null, "RPV_510.OUT1", DateTime.Now),

            new("igs_232", "AB.CL5300.Sensor_IGS232_Status", null, "IGS232_Status", DateTime.Now),
             new("ogt_500", "AB.CL5300.Sensor_OGT500_Status", null, "OGT500_Status", DateTime.Now),

            //INVERTER
            new("speed", "AB.CL5300.Motor_Speed_PV", null, "BIEUDOHMI", DateTime.Now),
            new("motor_sp", "AB.CL5300.Motor_Speed_SP", null, "SPEED_INVERTER", DateTime.Now),
             new("start_inverter", "AB.CL5300.Motor_Start", null, "START_INVERTER_WEB", DateTime.Now),
            new("stop_inverter", "AB.CL5300.Motor_Stop", null, "STOP_INVERTER_WEB", DateTime.Now),
            new("status_inverter", "AB.CL5300.Motor_Active", null, "INVERTER:I.ACTIVE", DateTime.Now),
            new("direction_status_inverter", "AB.CL5300.Motor_Direction_Status", null, "INVERTER:I.ACTUALDIR", DateTime.Now),
            new("motor_error", "AB.CL5300.Motor_Error", null, "INVERTER:I.FAULTED", DateTime.Now),
            new("motor_ready", "AB.CL5300.Motor_Ready", null, "INVERTER:I.READY", DateTime.Now),

            //Lights IFM
            new("Alarm_den_do_IFM", "AB.CL5300.Alarm_den_do_IFM", null, "IFM_Red", DateTime.Now),
            new("Alarm_den_vang_IFM", "AB.CL5300.Alarm_den_vang_IFM", null, "IFM_Yellow", DateTime.Now),
            new("Alarm_den_xanh_IFM", "AB.CL5300.Alarm_den_xanh_IFM", null, "IFM_Green", DateTime.Now),
        };
        }

        private void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (var tag in Tags)
            {

                if (tag.name is "ugt_524" or "ki6000" or "05d_150" or "rpv_510" or "speed" or "motor_sp" or "set_do1" or "set_xanh1" or "set_vang1" or "time_do1_dp" or "time_vang1_dp"
                    or "time_xanh1_dp" or "time_do2_dp" or "time_vang2_dp" or "time_xanh2_dp" )
                {
                    OperateResult<UInt16> data = plc.ReadUInt16(tag.address);

                    if (data.IsSuccess)
                    {
                        // you get the right value
                        tag.value = data.Content;
                    }
                    else
                    {
                        // failed , but you still can know the failed detail

                    }
                }

                else
                {
                    OperateResult<bool> data = plc.ReadBool(tag.address);

                    if (data.IsSuccess)
                    {
                        // you get the right value
                        tag.value = data.Content;
                    }
                    else
                    {
                        // failed , but you still can know the failed detail

                    }
                }

                MqttTags = Tags.Select(e => new MqttTag(
                 e.name,
                 e.value,
                 e.timestamp)).ToList();
            }
        }
        public object? GetTagValue(string tagName)
        {
            return Tags.First(x => x.name == tagName).value;

        }

        public string GetTagAddress(string tagName)
        {
            return Tags.First(x => x.name == tagName).address;
        }

        public void WritePLC(string TagName, bool value)
        {

            OperateResult write1 = plc.Write(TagName, value);

        }
        public void WriteNumberPLC(string TagName, int value)
        {
            OperateResult write = plc.Write(TagName, value);
        }

        public async void Connect()
        {
            await plc.ConnectServerAsync();
            _timer.Enabled = true;

        }
    }
}

    using HslCommunication;
using HslCommunication.Profinet.AllenBradley;
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
             new("", "Channel2.Device1.do1", null, "DO1", DateTime.Now),
            new("led_do1", "Channel2.Device1.do1", null, "DO1", DateTime.Now),
            new("led_do2", "Channel2.Device1.do2", null, "DO2", DateTime.Now),
            new("led_xanh1", "Channel2.Device1.xanh1", null, "XANH1", DateTime.Now),
            new("led_xanh2", "Channel2.Device1.xanh2", null, "XANH2", DateTime.Now),
            new("led_vang1", "Channel2.Device1.vang1", null, "VANG1", DateTime.Now),
            new("led_vang2", "Channel2.Device1.vang2", null, "VANG2", DateTime.Now),

            //new("set_do1", "PLC.Vali_Micro850.led7", null, "SET_D1", DateTime.Now),
            //new("set_xanh1", "PLC.Vali_Micro850.led7", null, "SET_X1", DateTime.Now),
            //new("set_vang1", "PLC.Vali_Micro850.led7", null, "SET_V1", DateTime.Now),

            //AUTO MODE
            

            new("start_auto", "Channel2.Device1.start_auto", null, "START_AUTO_WEB", DateTime.Now),
            new("start_manual", "Channel2.Device1.start_manual", null, "START_MANUAL_WEB", DateTime.Now),
            new("stop_auto", "Channel2.Device1.stop_auto", null, "STOP_AUTO_WEB", DateTime.Now),
            new("stop_manual", "Channel2.Device1.stop_manual", null, "STOP_MANUAL_WEB", DateTime.Now),

            //MANUAL MODE
            

            //SENSOR
            new("ugt_524", "Channel2.Device1.UGT_524_PV_Device", null, "UGT_524_ALARM.PV_DEVICE", DateTime.Now),
            new("ugt_524_st", "Channel2.Device1.UGT_524_PV_Device", null, "UGT_524.OUT1", DateTime.Now),
            new("ki6000", "Channel2.Device1.Ki6000_PV_Device", null, "KI6000_ALARM.PV_DEVICE", DateTime.Now),
            new("ki6000_st", "Channel2.Device1.Ki6000_PV_Device", null, "KI6000.BDC1", DateTime.Now),
            new("05d_150", "Channel2.Device1.O5D_150_PV_Device", null, "O5D_150_ALARM.PV_DEVICE", DateTime.Now),
            new("05d_150_st", "Channel2.Device1.O5D_150_PV_Device", null, "O5D_150.OUT1", DateTime.Now),
            new("rpv_510", "Channel2.Device1.RPV_510_PV_Device", null, "RPV_510_ALARM.PV_DEVICE", DateTime.Now),
            new("rpv_510_st", "Channel2.Device1.RPV_510_PV_Device", null, "RPV_510.OUT1", DateTime.Now),
            new("igs_232", "Channel2.Device1.IGS_232_PV_Device", null, "IGS232_Status", DateTime.Now),
             new("ogt_500", "Channel2.Device1.OGT_500_PV_Device", null, "OGT500_Status", DateTime.Now),

            //INVERTER
            new("speed", "Channel2.Device1.Toc_do_hien_tai", null, "BIEUDOHMI", DateTime.Now),
            new("motor_sp", "Channel2.Device1.Toc_do_hien_tai", null, "SPEED_INVERTER", DateTime.Now),
             new("start_inverter", "Channel2.Device1.start_inverter", null, "START_INVERTER_WEB", DateTime.Now),
            new("stop_inverter", "Channel2.Device1.stop_inverter", null, "STOP_INVERTER_WEB", DateTime.Now),
            new("status_inverter", "Channel2.Device1.stop_inverter", null, "INVERTER:I.ACTIVE", DateTime.Now),
            new("direction_status_inverter", "Channel2.Device1.stop_inverter", null, "INVERTER:I.ACTUALDIR", DateTime.Now),

            //Lights IFM
            new("Alarm_den_do_IFM", "Channel2.Device1.stop_inverter", null, "IFM_Red", DateTime.Now),
            new("Alarm_den_vang_IFM", "Channel2.Device1.stop_inverter", null, "IFM_Yellow", DateTime.Now),
            new("Alarm_den_xanh_IFM", "Channel2.Device1.stop_inverter", null, "IFM_Green", DateTime.Now),
        };
        }

        private void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (var tag in Tags)
            {

                if (tag.name is "ugt_524" or "ki6000" or "05d_150" or "rpv_510" or "speed" or "motor_sp")
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

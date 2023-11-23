using HslCommunication;
using HslCommunication.Profinet.AllenBradley;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace DemoCaseGui.Core.Application.Communication
{
    public class M850Client
    {
        private readonly AllenBradleyMicroCip plc;
        private readonly AllenBradleyMicroCip plc2;
        private readonly Timer _timer;
        public List<Tag> Tags { get; private set; }
        public List<Tag> Tags2 { get; private set; }
        public List<MqttTag> MqttTags { get; private set; }

        public M850Client()
        {
            //Khoi tao PLC
            plc = new AllenBradleyMicroCip("192.168.1.50");
            plc2 = new AllenBradleyMicroCip("192.168.1.20");
            _timer = new Timer(500);
            _timer.Elapsed += _timer_Elapsed;
            Tags = new()
        {
            
            //TrafficLights
        
            new("led2", "Channel1.Device1.Traffic_RedLightA", null, "_IO_EM_DO_02", DateTime.Now),
            new("led3", "Channel1.Device1.Traffic_YellowLightA", null, "_IO_EM_DO_03", DateTime.Now),
            new("led4", "Channel1.Device1.Traffic_GreenLightA", null, "_IO_EM_DO_04", DateTime.Now),
            new("led5", "Channel1.Device1.Traffic_RedLightB", null, "_IO_EM_DO_05", DateTime.Now),
            new("led6", "Channel1.Device1.Traffic_YellowLightB", null, "_IO_EM_DO_06", DateTime.Now),
            new("led7", "Channel1.Device1.Traffic_GreenLightB", null, "_IO_EM_DO_07", DateTime.Now),

            new("edit_redled", "Channel1.Device1.Traffic_RedTime", null, "HMI_DB.Traffic_Lights.Red_Time", DateTime.Now),
            new("edit_yellowled", "Channel1.Device1.Traffic_YellowTime", null, "HMI_DB.Traffic_Lights.Yellow_Time", DateTime.Now),
            new("edit_greenled", "Channel1.Device1.Traffic_GreenTime", null, "HMI_DB.Traffic_Lights.Green_Time", DateTime.Now),

             new("time_display_a", "PLC.Vali_Micro850.greentime", null, "HMI_DB.Traffic_Lights.Time_Display_A", DateTime.Now),
              new("time_display_b", "PLC.Vali_Micro850.greentime", null, "HMI_DB.Traffic_Lights.Time_Display_B", DateTime.Now),


            new("confirm_trafficlight", "Channel1.Device1.Traffic_Confirm", null, "HMI_DB.Traffic_Lights.Confirm", DateTime.Now),
            new("start_trafficlight", "Channel1.Device1.Traffic_Start", null, "HMI_DB.Traffic_Lights.Start", DateTime.Now),
            new("stop_trafficlight", "Channel1.Device1.Traffic_Stop", null, "HMI_DB.Traffic_Lights.Stop", DateTime.Now),

            //Inverter
            new("start_inverter", "Channel1.Device1.Inverter_Start", null, "HMI_DB.Inverter.Start", DateTime.Now),
            new("stop_inverter", "Channel1.Device1.Inverter_Stop", null, "HMI_DB.Inverter.Stop", DateTime.Now),
            new("setpoint", "Channel1.Device1.Inverter_Speed_SP", null, "HMI_DB.Inverter.Speed_SP", DateTime.Now),
            new("speed", "Channel1.Device1.Inverter_Speed_PV", null, "HMI_DB.Inverter.Speed_PV", DateTime.Now),
            new("forward", "Channel1.Device1.Inverter_Fwd", null, "HMI_DB.Inverter.Fwd", DateTime.Now),
            new("reverse", "Channel1.Device1.Inverter_Rev", null, "HMI_DB.Inverter.Rev", DateTime.Now),
            new("confirm_inverter", "Channel1.Device1.Inverter_Confirm", null, "HMI_DB.Inverter.Confirm", DateTime.Now),
        };
            Tags2 = new()
        {
            new("i0.0", "Channel1.Device1.I0.0", null, "_IO_EM_DI_04", DateTime.Now),
            new("i0.1", "Channel1.Device1.Traffic_YellowLightA", null, "_IO_EM_DI_05", DateTime.Now),
            new("i0.2", "Channel1.Device1.Traffic_GreenLightA", null, "_IO_EM_DI_06", DateTime.Now),
            new("i0.3", "Channel1.Device1.Traffic_RedLightB", null, "_IO_EM_DI_07", DateTime.Now),
            new("i0.4", "Channel1.Device1.Traffic_YellowLightB", null, "_IO_EM_DI_08", DateTime.Now),
            new("i0.5", "Channel1.Device1.Traffic_GreenLightB", null, "_IO_EM_DI_09", DateTime.Now),
            new("i0.6", "Channel1.Device1.Traffic_RedLightB", null, "_IO_EM_DI_10", DateTime.Now),
            new("i0.7", "Channel1.Device1.Traffic_YellowLightB", null, "_IO_EM_DI_11", DateTime.Now),
            new("analog", "Channel1.Device1.Traffic_GreenLightB", null, "_IO_EM_AI_00", DateTime.Now),

        };
        }

        private  void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            foreach(var tag in Tags)
            {
                if(tag.name is  "speed") 
                {
                    OperateResult<float> data =  plc.ReadFloat(tag.address);

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
                else if (tag.name is "edit_redled" or "edit_yellowled" or "edit_greenled")
                {
                    OperateResult<UInt16> data =  plc.ReadUInt16(tag.address);

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
                else if (tag.name is "setpoint" or "time_display_a" or "time_display_b")
                {
                    OperateResult<Byte> data =  plc.ReadByte(tag.address);

                    if (data.IsSuccess)
                    {
                        // you get the right value
                        object value = data.Content;
                        tag.value = Convert.ToUInt16(value);
                    }
                    else
                    {
                        // failed , but you still can know the failed detail

                    }
                }
                else
                {
                    OperateResult<bool>  data =  plc.ReadBool(tag.address);

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

            foreach (var tag in Tags2)
            {
               
                if (tag.name is "analog")
                {
                    OperateResult<UInt16> data = plc2.ReadUInt16(tag.address);

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
                    OperateResult<bool> data = plc2.ReadBool(tag.address);

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

        public object? GetTagValue2(string tagName)
        {
            return Tags2.First(x => x.name == tagName).value;

        }

        public string GetTagAddress(string tagName)
        {
            return Tags.First(x => x.name == tagName).address;
        }

        public string GetTagAddress2(string tagName)
        {
            return Tags2.First(x => x.name == tagName).address;
        }

        public void WritePLC(string TagName, bool value)
        {
          
            OperateResult write1 =  plc.Write(TagName, value);
          
        }
        public void WriteNumberPLC(string TagName, UInt16 value)
        {
            OperateResult write =  plc.Write(TagName,value);      
        }

        public async void Connect()
        {
            await plc.ConnectServerAsync();
            _timer.Enabled = true;
            
        }
    }
}

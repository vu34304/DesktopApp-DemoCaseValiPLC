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
        
            new("led2", "Micro.Micro850.Traffic_RedLightA", null, "_IO_EM_DO_02", DateTime.Now),
            new("led3", "Micro.Micro850.Traffic_YellowLightA", null, "_IO_EM_DO_03", DateTime.Now),
            new("led4", "Micro.Micro850.Traffic_GreenLightA", null, "_IO_EM_DO_04", DateTime.Now),
            new("led5", "Micro.Micro850.Traffic_RedLightB", null, "_IO_EM_DO_05", DateTime.Now),
            new("led6", "Micro.Micro850.Traffic_YellowLightB", null, "_IO_EM_DO_06", DateTime.Now),
            new("led7", "Micro.Micro850.Traffic_GreenLightB", null, "_IO_EM_DO_07", DateTime.Now),

            new("edit_redled", "Micro.Micro850.Traffic_RedTime", null, "HMI_DB.Traffic_Lights.Red_Time", DateTime.Now),
            new("edit_yellowled", "Micro.Micro850.Traffic_YellowTime", null, "HMI_DB.Traffic_Lights.Yellow_Time", DateTime.Now),
            new("edit_greenled", "Micro.Micro850.Traffic_GreenTime", null, "HMI_DB.Traffic_Lights.Green_Time", DateTime.Now),

            new("time_display_a", "Micro.Micro850.Traffic_Display_A", null, "HMI_DB.Traffic_Lights.Time_Display_A", DateTime.Now),
            new("time_display_b", "Micro.Micro850.Traffic_Display_B", null, "HMI_DB.Traffic_Lights.Time_Display_B", DateTime.Now),


            new("confirm_trafficlight", "Micro.Micro850.Traffic_Confirm", null, "HMI_DB.Traffic_Lights.Confirm", DateTime.Now),
            new("start_trafficlight", "Micro.Micro850.Traffic_Start", null, "HMI_DB.Traffic_Lights.Start", DateTime.Now),
            new("stop_trafficlight", "Micro.Micro850.Traffic_Stop", null, "HMI_DB.Traffic_Lights.Stop", DateTime.Now),

            //Inverter
            new("start_inverter", "Micro.Micro850.Inverter_Start", null, "HMI_DB.Inverter.Start", DateTime.Now),
            new("stop_inverter", "Micro.Micro850.Inverter_Stop", null, "HMI_DB.Inverter.Stop", DateTime.Now),
            new("setpoint", "Micro.Micro850.Inverter_Speed_SP", null, "HMI_DB.Inverter.Speed_SP", DateTime.Now),
            new("speed", "Micro.Micro850.Inverter_Speed_PV", null, "HMI_DB.Inverter.Speed_PV", DateTime.Now),
            new("forward", "Micro.Micro850.Inverter_Fwd", null, "HMI_DB.Inverter.Fwd", DateTime.Now),
            new("reverse", "Micro.Micro850.Inverter_Rev", null, "HMI_DB.Inverter.Rev", DateTime.Now),
            new("confirm_inverter", "Micro.Micro850.Inverter_Confirm", null, "HMI_DB.Inverter.Confirm", DateTime.Now),

            new("inverter_active", "Micro.Micro850.Inverter_Active", null, "INVERTER_ACTIVE", DateTime.Now),
            new("inverter_ready", "Micro.Micro850.Inverter_Ready", null, "INVERTER_READY", DateTime.Now),
            new("inverter_error", "Micro.Micro850.Inverter_Error", null, "INVERTER_ERROR", DateTime.Now),
            new("inverter_fwd_status", "Micro.Micro850.Inverter_Fwd_Status", null, "INVERTER_FWD_STS", DateTime.Now),
            new("inverter_rev_status", "Micro.Micro850.Inverter_Rev_Status", null, "INVERTER_REV_STS", DateTime.Now),
        };
            Tags2 = new()
        {
            new("i0.0", "Micro.Micro820.Micro820_input_1", null, "_IO_EM_DI_04", DateTime.Now),
            new("i0.1", "Micro.Micro820.Micro820_input_2", null, "_IO_EM_DI_05", DateTime.Now),
            new("i0.2", "Micro.Micro820.Micro820_input_3", null, "_IO_EM_DI_06", DateTime.Now),
            new("i0.3", "Micro.Micro820.Micro820_input_4", null, "_IO_EM_DI_07", DateTime.Now),
            new("i0.4", "Micro.Micro820.Micro820_input_5", null, "_IO_EM_DI_08", DateTime.Now),
            new("i0.5", "Micro.Micro820.Micro820_input_6", null, "_IO_EM_DI_09", DateTime.Now),
            new("i0.6", "Micro.Micro820.Micro820_input_7", null, "_IO_EM_DI_10", DateTime.Now),
            new("i0.7", "Micro.Micro820.Micro820_input_8", null, "_IO_EM_DI_11", DateTime.Now),
            new("analog", "Micro.Micro820.Micro820_Analog_1", null, "_IO_EM_AI_00", DateTime.Now),

        };
        }

        private  void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            foreach(var tag in Tags)
            {
               
                if(tag.name is  "speed") 
                {
                    OperateResult<float> data = plc.ReadFloat(tag.address);

                    if (data.IsSuccess)
                    {
                        // you get the right value
                        tag.value = data.Content;
                        //tag.value = rd.NextDouble();
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

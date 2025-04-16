using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static delegate0.User;

namespace delegate0
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var user = new User("Алексей");
            var smartHome = new SmartHome();
            var light = new Light();
            var thermostat = new Thermostat();
            var alarm = new SecurityAlarm();

            var player = new MusicPlayer();
            smartHome.SubscribeToGoodMorning(light.TurnOn, thermostat.SetTemperature, player.PlaySong);

            smartHome.SubscribeToGoodMorning(light.TurnOn, thermostat.SetTemperature);
            user.SendGoodMorningCommand(smartHome);
            smartHome.UnsubscribeToGoodMorning(light.TurnOn, thermostat.SetTemperature);
            Console.WriteLine();

            smartHome.SubscribeToAwayFromHome(light.TurnOn, thermostat.SetTemperature, alarm.Trigger);
            user.SendAwayFromHomeCommand(smartHome); 
            smartHome.UnsubscribeToAwayFromHome(light.TurnOn, thermostat.SetTemperature, alarm.Trigger);
            Console.WriteLine();

            smartHome.SubscribeToIntruderAlert(light.TurnOnTheFlashingLight, alarm.Trigger);
            user.SendIntruderAlert(smartHome);
            smartHome.UnsubscribeToIntruderAlert(light.TurnOnTheFlashingLight, alarm.Trigger);
            Console.WriteLine();

            Console.ReadKey();
        }
    }

    // Пользователь не должен знать о конкретных устройствах
    public class User
    {
        public string Name { get; set; }
        public User(string name) => Name = name;

        public void SendGoodMorningCommand(SmartHome smartHome)
        {
            Console.WriteLine($"{Name} говорит: 'GoodMorning'");
            smartHome.ExecuteGoodMorning();
        }

        public void SendAwayFromHomeCommand(SmartHome smartHome)
        {
            Console.WriteLine($"{Name} говорит: 'AwayFromHome'");
            smartHome.ExecuteAwayFromHome();
        }

        public void SendIntruderAlert(SmartHome smartHome)
        {
            Console.WriteLine($"{Name} говорит: 'IntruderAlert'");
            smartHome.ExecuteIntruderAlert();
        }
    }

    public class SmartHome
    {
        public delegate void LightAction(bool on); // Вкл/выкл свет (делегат)
        public delegate void TemperatureAction(float temp); // Изменить температуру
        public delegate void AlarmAction(); // Включить тревогу
        public delegate void VideoAction(string song);

        public LightAction onLightCommand; // Это еще не события, я поля-делегаты
        public TemperatureAction onTempCommand;
        public AlarmAction onAlarmCommand;
        public VideoAction onVideoCommand;

        public void SubscribeToGoodMorning(LightAction light, TemperatureAction temp, VideoAction video)
        {
            onLightCommand += light;
            onTempCommand += temp;
            onVideoCommand += video;
        }

        public void SubscribeToGoodMorning(LightAction light, TemperatureAction temp)
        {
            onLightCommand += light;
            onTempCommand += temp;
        }
        public void SubscribeToAwayFromHome(LightAction light, TemperatureAction temp, AlarmAction alarm)
        {
            onLightCommand += light;
            onTempCommand += temp;
            onAlarmCommand += alarm;
        }
        public void SubscribeToIntruderAlert(LightAction light, AlarmAction alarm)
        {
            onLightCommand += light;
            onAlarmCommand += alarm;
        }

        public void UnsubscribeToGoodMorning(LightAction light, TemperatureAction temp)
        {
            onLightCommand -= light;
            onTempCommand -= temp;
        }
        public void UnsubscribeToAwayFromHome(LightAction light, TemperatureAction temp, AlarmAction alarm)
        {
            onLightCommand -= light;
            onTempCommand -= temp;
            onAlarmCommand -= alarm;
        }
        public void UnsubscribeToIntruderAlert(LightAction light, AlarmAction alarm)
        {
            onLightCommand -= light;
            onAlarmCommand -= alarm;
        }

        public void ExecuteGoodMorning()
        {
            onLightCommand?.Invoke(true);
            onTempCommand?.Invoke(22.0f);
            onMusicCommand?.Invoke("Видео с котами");
        }

        public void ExecuteAwayFromHome()
        {
            onLightCommand?.Invoke(false);
            onTempCommand?.Invoke(18.0f);
            onAlarmCommand?.Invoke();
        }

        public void ExecuteIntruderAlert()
        {
            onLightCommand?.Invoke(true); // Включить свет для мигания
            onAlarmCommand?.Invoke();
        }
    }

    public class Light
    {
        public void TurnOn(bool on)
        {
            string output = on ? "Свет включен" : "Свет выключен";
            Console.WriteLine(output);
        }

        public void TurnOnTheFlashingLight(bool on)
        {
            Console.WriteLine("Свет мигает...");
        }
    }

    public class Thermostat
    {
        public void SetTemperature(float temp)
        {
            Console.WriteLine($"Температура установлена на {temp}°C");
        }
    }

    public class SecurityAlarm
    {
        public void Trigger()
        {
            Console.WriteLine("Сигнализация включена");
        }
    }

    public class VideoPlayer
    {
        public void PlayVideo(string song)
        {
            Console.WriteLine($"Играет видео: {song}");
        }
    }
}

using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyIoT.Simulator
{
    class Device
    {
        // config
        private readonly String _id;
        private readonly DeviceClient _deviceClient;

        // data
        private Timer _timer;
        private int _low = 30;
        private int _high = 32;
        private Random _random = new Random();
        private int _count = 0;
        private bool _enabled = true;
        private int _interval = 1000;

        public Device()
        {
            _id = ConfigurationManager.AppSettings["id"];
            string key = ConfigurationManager.AppSettings["key"];
            string uri = ConfigurationManager.AppSettings["iotHubUri"];
            _deviceClient = DeviceClient.Create(uri, new DeviceAuthenticationWithRegistrySymmetricKey(_id, key));
        }

        public void Start()
        {
            int dueTime = _enabled ? 0 : Timeout.Infinite;
            int period = _enabled ? _interval : Timeout.Infinite;
            _timer = new Timer(Update, null, dueTime, period);
            while (true)
            {
                Console.WriteLine(String.Format("Heartbeat: {0}", _id));
                Thread.Sleep(10000);
            }

        }

        protected virtual void Update(object state)
        {
            if (_count++ >= 60)
            {
                _count = 0;
                _low += 3;
                _high += 3;
            }

            var dataPoint = new
            {
                DeviceId = _id,
                Temperature = _random.Next(_low, _high),
                Humidity = 20,
                TimeStamp = DateTime.Now
            };

            var messageString = JsonConvert.SerializeObject(dataPoint);
            var message = new Message(Encoding.Default.GetBytes(messageString));

            _deviceClient.SendEventAsync(message);
            Console.WriteLine("Sending message: {0}", messageString);
        }
    }
}

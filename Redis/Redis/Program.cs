using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis
{
    public class RedisConnectorHelper
    {
        static RedisConnectorHelper()
        {
            RedisConnectorHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect("localhost");
            });
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();

            Console.WriteLine("Random Sayılar kayıt ediyoruz cache");
            program.SaveBigData();

            Console.WriteLine("Cache okuyoruz");
            program.ReadData();

            Console.ReadLine();
        }

        public void ReadData()
        {
            var cache = RedisConnectorHelper.Connection.GetDatabase();
            var devicesCount = 10000;
            for (int i = 0; i < devicesCount; i++)
            {
                var value = cache.StringGet($"giden:{i}");
                Console.WriteLine($"Değerler={value}");
            }
        }

        public void SaveBigData()
        {
            var devicesCount = 10000;
            var rnd = new Random();
            var cache = RedisConnectorHelper.Connection.GetDatabase();

            for (int i = 1; i < devicesCount; i++)
            {
                var value = rnd.Next(0, 10000);
                cache.StringSet($"giden:{i}", value);
            }
        }
    }
}

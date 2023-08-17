using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class RandomExtensions
    {
        public static Random _random = new Random();
        public static String RandomString(this Random random, int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomStringDigital(this Random random, int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomStringDigital(int length)
        {
            return RandomStringDigital(_random, length);

        }

        public static string RandomEmail(this Random random)
        {
            return $"{random.RandomStringDigital(random.Next(5, 32))}@{random.RandomString(random.Next(4, 9))}.{random.RandomString(random.Next(3, 9))}";
        }
        public static string RandomEmail()
        {
            return _random.RandomEmail();
        }
        public static string RandomStringDigital()
        {
            return _random.RandomStringDigital(_random.Next(6, 32));
        }

        public static int RandomDigital(int min, int max)
        {
            return _random.Next(min, max);
        }
        public static int Random(int max)
        {
            return _random.Next(max);
        }
        public static int Random(int min, int max)
        {
            return _random.Next(min, max);
        }
        public static string RandomString(int length)
        {
            return _random.RandomString(length);
        }

        public static DateTime RandomDateTime()
        {
            DateTime start = new DateTime(1970, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(_random.Next(range));
        }

    }
}

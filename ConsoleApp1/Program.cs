using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Raven.Client.Documents;
using Raven.Client.Documents.BulkInsert;

namespace ConsoleApplication6
{
    internal class Program
    {
        public class User
        {
            public int Score;
            public string Name;
            public DateTime CreatedAt;
        }

        private static char[] _buffer = new char[6];
        private static string RandomName(Random rand)
        {
            _buffer[0] = (char)rand.Next(65, 91);
            for (int i = 1; i < 6; i++)
            {
                _buffer[i] = (char)rand.Next(97, 123);
            }
            return new string(_buffer);
        }
        static void Main(string[] args)
        {
            using (var store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "bench"
            }.Initialize())
            {
                var sp = Stopwatch.StartNew();
                using (var bulk = store.BulkInsert())
                {
                    var rand = new Random();
                    for (int i = 0; i < 10000 * 1000; i++)
                    {
                        bulk.Store(new User
                        {
                            CreatedAt = DateTime.Today.AddDays(rand.Next(356)),
                            Score = rand.Next(0, 5000),
                            Name = RandomName(rand)
                        });
                    }
                }
                Console.WriteLine(sp.Elapsed);
                Console.ReadKey();
            }
        }
    }
}
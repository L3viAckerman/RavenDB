using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.BulkInsert;

namespace ConsoleApplication6
{
    public class Child
    {
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public Father Father { get; set; }
        public Mother Mother { get; set; }
    }

    public class Father
    {
        public string Name { get; set; }
    }

    public class Mother
    {
        public string Name { get; set; }
    }
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
                Database = "Test"
            }.Initialize())
            {
                var sp = Stopwatch.StartNew();
                //using (var bulk = store.BulkInsert())
                //{
                //    var rand = new Random();
                //    for (int i = 0; i < 10000 * 1000; i++)
                //    {
                //        bulk.Store(new User
                //        {
                //            CreatedAt = DateTime.Today.AddDays(rand.Next(356)),
                //            Score = rand.Next(0, 5000),
                //            Name = RandomName(rand)
                //        });
                //    }
                //}
                //Parallel.For(0, 5, i =>
                //{
                //    using (var bulk = store.BulkInsert())
                //    {
                //        var rand = new Random();
                //        for (int j = 0; j < 20000 * 1000; j++)
                //        {
                //            bulk.Store(new User
                //            {
                //                CreatedAt = DateTime.Today.AddDays(rand.Next(356)),
                //                Score = rand.Next(0, 5000),
                //                Name = RandomName(rand)
                //            });
                //        }
                //    }
                //});
                using (var bulk = store.BulkInsert())
                {
                    var rand = new Random();
                    for (int i = 0; i < 100 * 1000; i++)
                    {
                        bulk.Store(new Child
                        {
                            Name = RandomName(rand),
                            Birthday = DateTime.Today.AddDays(rand.Next(365)),
                            Father = new Father
                            {
                                Name = RandomName(rand)
                            },
                            Mother = new Mother
                            {
                                Name = RandomName(rand)
                            }
                        });
                    }
                }

                Console.WriteLine(sp.Elapsed);
                Console.ReadKey();
            }
        }
    }
}
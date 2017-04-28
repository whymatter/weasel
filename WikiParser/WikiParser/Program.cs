using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ConcurrentHashSetQueue;

namespace WikiParser {
    internal class Program {
        private static readonly ConcurrentHashSetQueue<string> Queue = new ConcurrentHashSetQueue<string>();
        private static readonly ConcurrentDictionary<string, int> Bag = new ConcurrentDictionary<string, int>();

        private static readonly ManualResetEvent PauseHandle = new ManualResetEvent(true);

        private static DateTime _lastPauseTime = DateTime.UtcNow;
        private const long PauseEverySeconds = 20;
        private const int PauseDurationSeconds = 10;

        private static readonly object LockObj = new object();
        private const int SqlCommandChunkSize = 1000;
        private const string LinkingStatsFile = "linkingstats.txt";

        private static int _mainThreadId;

        private static readonly Regex WikiLinkRegex = new Regex("(?<=\")\\/wiki\\/(?!Kategorie)(?!Spezial)(?!Diskussion)(?!Datei)[^\"]*");

        private const string ConnectionString = "Data Source=OLIVER-PC\\SQLEXPRESS2014;Initial Catalog=Wikipedia;Integrated Security=True";

        private static void Main(string[] args) {
            File.Delete(LinkingStatsFile);
            const string baseUrl = "https://de.wikipedia.org";
            _mainThreadId = Thread.CurrentThread.ManagedThreadId;

            var client = new HttpClient();
            var sqlConnection = new SqlConnection(ConnectionString);
            var textWriter = new StreamWriter("file_7.txt");

            Log("start");

            Bag.TryAdd("/wiki/Adolf_Hitler", 0);
            CrawlSite(client, sqlConnection, textWriter, baseUrl, "/wiki/Adolf_Hitler");

            for (var i = 0; i < 7; i++) {
                var i1 = i;
                Task.Factory.StartNew(() => CrawlLoop(new HttpClient(), new SqlConnection(ConnectionString), new StreamWriter($"file_{i1}.txt"), baseUrl));
            }

            CrawlLoop(client, sqlConnection, textWriter, baseUrl);

            Log("empty queue");

            Console.ReadLine();
        }


        private static void CrawlLoop(HttpClient client, SqlConnection connection, StreamWriter textWriter, string baseUrl) {
            while (true) {
                if (Thread.CurrentThread.ManagedThreadId == _mainThreadId) {
                    if (DateTime.UtcNow.Subtract(_lastPauseTime).TotalSeconds > PauseEverySeconds) {
                        PauseHandle.Reset();
                        Log("doing pause");
                        Thread.Sleep(PauseDurationSeconds * 1000);
                        Log("continue");
                        _lastPauseTime = DateTime.UtcNow;
                        PauseHandle.Set();
                    }
                }

                PauseHandle.WaitOne();

                var specific = Queue.Dequeue();
                if (specific == null) break;
                Bag.TryAdd(specific, 0);

                if (!CrawlSite(client, connection, textWriter, baseUrl, specific)) Bag.TryRemove(specific, out int value);

                //lock (_lockObj) {
                //File.AppendAllText(LinkingStatsFile, $"{Bag.Count} {Queue.Count()}\n");
                Log("crawled: " + Bag.Count + ", queue: " + Queue.Count());
                //}
            }
        }

        private static bool CrawlSite(HttpClient client, SqlConnection connection, StreamWriter textWriter, string baseUrl, string specific) {
            string s;

            try {
                s = client.GetStringAsync(baseUrl + specific).Result;
            }
            catch (Exception ex) {
                Log(specific + ": " + ex);
                return false;
            }

            var matches = WikiLinkRegex.Matches(s)
                .Cast<Match>()
                .Select(c => c.Value)
                .ToArray();

            foreach (var match in matches) {
                if (Queue.Count() == int.MaxValue - 1000) break;
                if (Bag.ContainsKey(match) || Queue.Contains(match)) continue;

                Queue.Enqueue(match);
            }

            InsertMappings(connection, specific, matches);
            //InsertMappings(textWriter, specific, matches);

            return true;
        }

        private static void Log(string message) {
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {message}");
        }

        private static void InsertMappings(TextWriter writer, string linkA, string[] linkB) {
            writer.Write(linkA + " ");
            writer.WriteLine(string.Join($"\n{linkA} ", linkB));
        }

        private static void InsertMappings(SqlConnection connection, string linkA, string[] linkB) {
            if (connection.State == ConnectionState.Closed) connection.Open();

            var valuesText = new StringBuilder();
            var linkSave = linkA.Replace("'", "''");

            for (var i = 1; i < linkB.Length; i++) {
                if (i % SqlCommandChunkSize == 1) {
                    valuesText.Append("INSERT INTO Links (LinkA, LinkB) VALUES ");
                }

                valuesText.AppendFormat("('{0}','{1}')", linkSave, linkB[i - 1].Replace("'", "''"));

                if (i % SqlCommandChunkSize != 0) {
                    valuesText.Append(",");
                }
                else {
                    using (var command = new SqlCommand(valuesText.ToString(), connection)) {
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }

                    valuesText.Clear();
                }
            }

            if (valuesText.Length > 0) {
                using (var command = new SqlCommand(valuesText.ToString().Trim(','), connection)) {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
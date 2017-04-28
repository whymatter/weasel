using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ConcurrentHashSetQueue;

namespace ListSpeedTest {
    internal class Program {
        private static void Main(string[] args) {
            LoadTestData();
            Console.WriteLine("loaded: " + testData.Length);
            Console.WriteLine("adding");

            //RunTestFor(TestList);
            //RunTestFor(TestConcurrentQueue);
            RunTestFor(TestHashSet);
            RunTestFor(TestSortedSet);
            //RunTestFor(TestSortedList);
            RunTestFor(TestConcurrentDictionary);
            RunTestFor(TestConcurrentHashSetQueue);

            Console.WriteLine("sorting");

            //RunTestFor(TestContainsList);
            //RunTestFor(TestContainsConcurrentQueue);
            RunTestFor(TestContainsHashSet);
            RunTestFor(TestContainsSortedSet);
            //RunTestFor(TestContainsSortedList);
            RunTestFor(TestContainsConcurrentDictionary);
            RunTestFor(TestContainsConcurrentHashSetQueue);


            Console.WriteLine("removing");

            RunTestFor(TestRemoveHashSet);
            RunTestFor(TestRemoveSortedSet);
            //RunTestFor(TestRemoveSortedList);
            RunTestFor(TestRemoveConcurrentDictionary);
            RunTestFor(TestRemoveConcurrentHashSetQueue);

            Console.ReadLine();
        }

        private static void RunTestFor(Action a) {
            var w = new Stopwatch();
            w.Restart();
            a();
            w.Stop();
            Console.WriteLine(a.Method.Name.Replace("Test", string.Empty).Replace("Contains", string.Empty) + ": " + w.ElapsedMilliseconds);
        }

        private static string[] testData;

        private static List<string> _testList;
        private static ConcurrentQueue<string> _concurrentQueue;
        private static HashSet<string> _hashSet;
        private static SortedSet<string> _sortedSet;
        private static SortedList<string, int> _sortedList;
        private static ConcurrentDictionary<string, int> _concurrentDictionary;
        private static ConcurrentHashSetQueue<string> _concurrentHashSetQueue;

        private static void LoadTestData() {
            testData = File.ReadAllLines("queue.txt");
        }

        private static void TestList() {
            _testList = new List<string>();

            foreach (var s in testData) {
                _testList.Add(s);
            }
        }

        private static void TestContainsList() {
            foreach (var s in testData) {
                if (!_testList.Contains(s)) {
                    Console.WriteLine("must be a failure ...");
                }
            }
        }

        private static void TestConcurrentQueue() {
            _concurrentQueue = new ConcurrentQueue<string>();

            foreach (var s in testData) {
                _concurrentQueue.Enqueue(s);
            }
        }

        private static void TestContainsConcurrentQueue() {
            foreach (var s in testData) {
                if (!_concurrentQueue.Contains(s)) {
                    Console.WriteLine("must be a failure ...");
                }
            }
        }

        private static void TestHashSet() {
            _hashSet = new HashSet<string>();

            foreach (var s in testData) {
                _hashSet.Add(s);
            }
        }

        private static void TestContainsHashSet() {
            foreach (var s in testData) {
                if (!_hashSet.Contains(s)) {
                    Console.WriteLine("must be a failure ...");
                }
            }
        }

        private static void TestRemoveHashSet() {
            foreach (var s in testData) {
                _hashSet.Remove(s);
            }
        }

        private static void TestSortedSet() {
            _sortedSet = new SortedSet<string>();

            foreach (var s in testData) {
                _sortedSet.Add(s);
            }
        }

        private static void TestContainsSortedSet() {
            foreach (var s in testData) {
                if (!_sortedSet.Contains(s)) {
                    Console.WriteLine("must be a failure ...");
                }
            }
        }

        private static void TestRemoveSortedSet() {
            foreach (var s in testData) {
                _sortedSet.Remove(s);
            }
        }

        private static void TestSortedList() {
            _sortedList = new SortedList<string, int>();

            foreach (var s in testData) {
                _sortedList.Add(s, 0);
            }
        }

        private static void TestContainsSortedList() {
            foreach (var s in testData) {
                if (!_sortedList.ContainsKey(s)) {
                    Console.WriteLine("must be a failure ...");
                }
            }
        }

        private static void TestRemoveSortedList() {
            foreach (var s in testData) {
                _sortedList.Remove(s);
            }
        }

        private static void TestConcurrentDictionary() {
            _concurrentDictionary = new ConcurrentDictionary<string, int>();

            foreach (var s in testData) {
                _concurrentDictionary.TryAdd(s, 0);
            }
        }

        private static void TestContainsConcurrentDictionary() {
            foreach (var s in testData) {
                if (!_concurrentDictionary.ContainsKey(s)) {
                    Console.WriteLine("must be a failure ...");
                }
            }
        }

        private static void TestRemoveConcurrentDictionary() {
            foreach (var s in testData) {
                _concurrentDictionary.TryRemove(s, out int i);
            }
        }

        private static void TestConcurrentHashSetQueue() {
            _concurrentHashSetQueue = new ConcurrentHashSetQueue<string>();

            foreach (var s in testData) {
                _concurrentHashSetQueue.Enqueue(s);
            }
        }

        private static void TestContainsConcurrentHashSetQueue() {
            foreach (var s in testData) {
                if (!_concurrentHashSetQueue.Contains(s)) {
                    Console.WriteLine("must be a failure ...");
                }
            }
        }

        private static void TestRemoveConcurrentHashSetQueue() {
            foreach (var s in testData) {
                _concurrentHashSetQueue.Dequeue();
            }
        }
    }
}
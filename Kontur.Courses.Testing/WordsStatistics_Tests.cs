using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using Kontur.Courses.Testing.Implementations;
using NUnit.Framework;

namespace Kontur.Courses.Testing
{
    public class WordsStatistics_Tests
    {
        public Func<IWordsStatistics> createStat = () => new WordsStatistics_CorrectImplementation();
            // меняется на разные реализации при запуске exe

        public IWordsStatistics stat;

        [SetUp]
        public void SetUp()
        {
            stat = createStat();
        }

        [Test]
        public void no_stats_if_no_words()
        {
            CollectionAssert.IsEmpty(stat.GetStatistics());
        }

        [Test]
        public void same_word_twice()
        {
            stat.AddWord("xxx");
            stat.AddWord("xxx");
            CollectionAssert.AreEqual(new[] {Tuple.Create(2, "xxx")}, stat.GetStatistics());
        }

        [Test]
        public void single_word()
        {
            stat.AddWord("hello");
            CollectionAssert.AreEqual(new[] {Tuple.Create(1, "hello")}, stat.GetStatistics());
        }

        [Test]
        public void two_same_words_one_other()
        {
            stat.AddWord("hello");
            stat.AddWord("world");
            stat.AddWord("world");
            CollectionAssert.AreEqual(new[] {Tuple.Create(2, "world"), Tuple.Create(1, "hello")}, stat.GetStatistics());
        }

        [Test]
        public void test_more_than_ten()
        {
            stat.AddWord("aAaAbBasdsddsad");
            CollectionAssert.AreEqual(new[] {Tuple.Create(1, "aaaabbasds")}, stat.GetStatistics());
        }

        [Test]
        public void test_frequency()
        {
            stat.AddWord("hello");
            stat.AddWord("hello");
            stat.AddWord("ololo");
            stat.AddWord("lol");
            stat.AddWord("kontur");
            stat.AddWord("kontur");
            stat.AddWord("Kontur");
            stat.AddWord("kontur");
            CollectionAssert.AreEqual(new[]
            {
                Tuple.Create(4, "kontur"),
                Tuple.Create(2, "hello"), Tuple.Create(1, "lol"),
                Tuple.Create(1, "ololo")
            }, stat.GetStatistics());
        }

        [Test]
        public void words_with_len_11()
        {
            for (var i = 0; i < 10000; i++)
            {
                stat.AddWord("aaaaaaaaaaC");
            }
            CollectionAssert.AreEqual(new[] {Tuple.Create(10000, "aaaaaaaaaa")}, stat.GetStatistics());
        }

        [Test, Timeout(500)]
        public void test_size()
        {
            for (var i = 0; i < 1500; i++)
                stat.AddWord(i.ToString());
            Assert.AreEqual(1500, stat.GetStatistics().Count());
        }

        [Test]
        public void word_with_zero_len()
        {
            stat.AddWord("Hell");

            stat.AddWord("");
            CollectionAssert.AreEqual(new[] {Tuple.Create(1, "hell")}, stat.GetStatistics());
        }

        [Test, Timeout(500)]
        public void test_dict()
        {
            for (var i = 0; i < 4; i++)
                for (var j = 0; j < 10000; j++)
                    stat.AddWord(j.ToString());
            var res = Enumerable.Range(0, 10000).Select(i => Tuple.Create(4, i.ToString()));
            res = res.OrderBy(item => item.Item2);
            CollectionAssert.AreEqual(res, stat.GetStatistics());

        }

        [Test]
        public void test_secondStat()
        {
            var stat1 = createStat();
            stat1.AddWord("hello");
            Assert.AreEqual(0, stat.GetStatistics().Count());
        }

        [Test]
        public void test_null()
        {
            stat.AddWord(null);
        }

        [Test, Timeout(500)]
        public void test()
        {
            var rand = new Random();

            for (var i = 0; i < 4; i++)
                for (var j = 0; j < 10000; j++)
                    if (rand.NextDouble() > 0.5)
                        stat.AddWord(j.ToString());
            var statistics = stat.GetStatistics();
            var expected = statistics.OrderByDescending(kv => kv.Item1).ThenBy(kv => kv.Item2).Select(kv => Tuple.Create(kv.Item1, kv.Item2));
            CollectionAssert.AreEqual(statistics, expected);
        }
    }
}
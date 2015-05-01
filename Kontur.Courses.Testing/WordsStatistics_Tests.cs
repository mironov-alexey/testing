using System;
using Kontur.Courses.Testing.Implementations;
using NUnit.Framework;

namespace Kontur.Courses.Testing
{
	public class WordsStatistics_Tests
	{
		public Func<IWordsStatistics> createStat = () => new WordsStatistics_CorrectImplementation(); // меняется на разные реализации при запуске exe
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
			CollectionAssert.AreEqual(new[] { Tuple.Create(2, "xxx") }, stat.GetStatistics());
		}

		[Test]
		public void single_word()
		{
			stat.AddWord("hello");
			CollectionAssert.AreEqual(new[] { Tuple.Create(1, "hello") }, stat.GetStatistics());
		}

		[Test]
		public void two_same_words_one_other()
		{
			stat.AddWord("hello");
			stat.AddWord("world");
			stat.AddWord("world");
			CollectionAssert.AreEqual(new[] { Tuple.Create(2, "world"), Tuple.Create(1, "hello") }, stat.GetStatistics());
		}

	    [Test]
	    public void test_more_than_ten()
	    {
	        stat.AddWord("aAaAbBasdsddsad");
            CollectionAssert.AreEqual(new[] { Tuple.Create(1, "aaaabbasds")}, stat.GetStatistics());
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
            CollectionAssert.AreEqual(new[] { Tuple.Create(4, "kontur"), 
                Tuple.Create(2, "hello"),Tuple.Create(1, "lol"),
                Tuple.Create(1, "ololo") }, stat.GetStatistics());
        }

        [Test]
	    public void words_with_len_11()
	    {
	        for (var i = 0; i < 10000; i++)
	        {
	            stat.AddWord("aaaaaaaaaaC");
	        }
//            stat.AddWord("b");
//            stat.AddWord("b");
//            stat.AddWord("C");
//            stat.AddWord("a");
//            stat.AddWord("A");
//            stat.AddWord("B");
            CollectionAssert.AreEqual(new[] { Tuple.Create(10000, "aaaaaaaaaa") }, stat.GetStatistics());
	    }

	    [Test]
	    public void word_with_zero_len()
	    {
	        stat.AddWord("Hell");

	        stat.AddWord("");
            CollectionAssert.AreEqual(new[] {Tuple.Create(1, "hell")}, stat.GetStatistics());
	    }

	}
}
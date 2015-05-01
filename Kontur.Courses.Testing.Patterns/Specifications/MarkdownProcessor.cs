using System;
using System.CodeDom;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Kontur.Courses.Testing.Patterns.Specifications
{
	public class MarkdownProcessor
	{
		public string Render(string input)
		{
            var emReplacer = new Regex(@"([^\w\\]|^)_(.*?[^\\])_(\W|$)");
            var strongReplacer = new Regex(@"([^\w\\]|^)__(.*?[^\\])__(\W|$)");
            input = strongReplacer.Replace(input,
                match => match.Groups[1].Value +
                    "<strong>" + match.Groups[2].Value + "</strong>" +
                    match.Groups[3].Value);
            input = emReplacer.Replace(input,
                match => match.Groups[1].Value +
                    "<em>" + match.Groups[2].Value + "</em>" +
                    match.Groups[3].Value);
            input = input.Replace(@"\_", "_");
            return input;
		}
	}

	[TestFixture]
	public class MarkdownProcessor_should
	{
		private readonly MarkdownProcessor md = new MarkdownProcessor();
		//TODO see Markdown.txt
	    [Test]
	    public void notChange_IfNoMarkup()
	    {
	        var input = "some string";
	        Assert.AreEqual(input, md.Render(input));
	    }

	    [TestCase("_a_", Result = "<em>a</em>")]
	    public string em_markup(string input)
	    {
	        return md.Render(input);
	    }

	    [TestCase("__a__", Result = "<strong>a</strong>")]
        [TestCase("_a __lol__", Result = "_a <strong>lol</strong>")]
	    public string strong_markup(string input)
	    {
	        return md.Render(input);
	    }

        [TestCase("_some __text__ is here_", Result = "<em>some <strong>text</strong> is here</em>")]
        [TestCase("_it __is__ it!_", Result = "<em>it <strong>is</strong> it!</em>")]
        [TestCase("_i __t __is__ it!_", Result = "<em>i <strong>t __is</strong> it!</em>")]
        public string strong_in_em(string input)
        {
            return md.Render(input);
        }

        [TestCase("_a", Result = "_a")]
        [TestCase("__a", Result = "__a")]
        [TestCase("a_", Result = "a_")]
        [TestCase("a__", Result = "a__")]
	    public string unpaired_underlines(string input)
	    {
	        return md.Render(input);
	    }

        [TestCase(@"\_a_", Result = "_a_")]
        [TestCase(@"\_\_a__", Result = "__a__")]
        [TestCase(@"_\_\_a__", Result = "<em>__a_</em>")]
	    public string shielded_underlines(string input)
	    {
	        return md.Render(input);
	    }
	}
}

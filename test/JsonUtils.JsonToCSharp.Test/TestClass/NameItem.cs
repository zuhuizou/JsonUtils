using System;
using System.Collections.Generic;
using System.Text;

namespace JsonUtils.JsonToCSharp.Test.TestClass
{
    public class NameItem
    {
        public NameItem()
        {
            Hello = new List<string>() { "1", "x" };
            Decimals = new List<decimal> { 1, 1.2M };
            Doubles = new List<double> { 1.1, 1.2 };
        }

        public string Name { get; set; }
        public bool AreYou { get; set; }
        public List<string> Hello { get; set; }
        public List<decimal> Decimals { get; set; }
        public List<double> Doubles { get; set; }
    }
}

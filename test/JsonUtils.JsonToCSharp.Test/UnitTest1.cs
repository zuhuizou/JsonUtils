using System;
using Xunit;
using Xunit.Abstractions;
using Newtonsoft;
using System.Text;
using System.Collections.Generic;
using JsonUtils.JsonToCSharp.Test.TestClass;
using Newtonsoft.Json;

namespace JsonUtils.JsonToCSharp.Test
{
    public class UnitTest1
    {
        ITestOutputHelper _log;

        public UnitTest1(ITestOutputHelper log)
        {
            _log = log;
        }

        [Fact]
        public void GenerateClassContent()
        {
            string json = JsonConvert.SerializeObject(new Target { Name = "Hello", Targets = new List<TargetItem> { new TargetItem { DateTime = DateTime.Now, Id = 110, NameItem = new NameItem { Name = "ss" } } } });

            List<StringBuilder> classSbs = new ClassBuilder().GetClassBuilder(json, "YES");

            _log.WriteLine(string.Join(System.Environment.NewLine, classSbs));
        }
    }
}

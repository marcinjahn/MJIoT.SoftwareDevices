using System;
using Xunit;
using Mj.CmdDashArgsReaderLibrary;
using System.Collections.Generic;

namespace CmdDashArgsReaderLibraryTest
{
    public class CmdDashArgsReaderTests
    {



        [Fact]
        public void Constructor_GiveProperDefinitions_DoesntThrowException()
        {
            var reader = GetReader();
        }

        [Fact]
        public void GetArgument_GiveProperDefinitionsAndRetrieveValues_GivesProperValues()
        {
            var reader = GetReader();

            Assert.Equal("true", reader.Get("global"));
            Assert.Equal("false", reader.Get("case"));
            Assert.Equal("mydata", reader.Get("data"));
            Assert.Equal("some-connection", reader.Get("SQL Connection"));
        }

        private CmdDashArgsReader GetReader()
        {
            var defs = GetProperArgumentDefinitions();
            var values = GetProperArgumentValues();

            return new CmdDashArgsReader(defs, values);
        }

        private List<ArgumentDefinition> GetProperArgumentDefinitions()
        {
            var result = new List<ArgumentDefinition>() {
                new ArgumentDefinition("d", "data"),
                new ArgumentDefinition("g", "global"),
                new ArgumentDefinition("c", "case"),
                new ArgumentDefinition("sql", "SQL Connection"),
            };

            return result;
        }

        private List<string> GetProperArgumentValues()
        {
            var result = new List<string> {
                "-d", "mydata", "-g", "true", "-c", "false", "-sql", "some-connection"
            };

            return result;
        }
    }
}

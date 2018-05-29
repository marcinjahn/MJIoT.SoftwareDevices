using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Mj.CmdDashArgsReaderLibrary
{
    public class CmdDashArgsReader
    {
        List<ArgumentDefinition> _argumentDefinitions;
        Dictionary<ArgumentDefinition, string> _argumentValues;

        public CmdDashArgsReader(List<ArgumentDefinition> argumentDefinitions, IEnumerable<string> arguments)
        {
            SetArgumentDefinitions(argumentDefinitions);
            SetArgumentValues(arguments);
        }

        public void SetArgumentValues(IEnumerable<string> arguments)
        {
            _argumentValues = new Dictionary<ArgumentDefinition, string>();

            if (arguments.Count() % 2 != 0)
                throw new ArgumentException("Arguments are wrong");

            for (var i = 0; i < arguments.Count()/2; i++)
            {
                if (!arguments.ElementAt(i * 2).StartsWith("-") && arguments.ElementAt(i * 2).Length < 2)
                    throw new ArgumentException("Arguments are wrong");

                var definition = _argumentDefinitions.Where(n => n.Argument == arguments.ElementAt(i * 2).Substring(1)).FirstOrDefault();

                if (definition == null)
                    throw new ArgumentException("Arguments are wrong");

                _argumentValues.Add(definition, arguments.ElementAt(i * 2 + 1));
            }
        }

        public void SetArgumentDefinitions(List<ArgumentDefinition> argumentDefinitions)
        {
            if (argumentDefinitions.Select(n => n.Argument).Distinct().ToList().Count == argumentDefinitions.Count && argumentDefinitions.Select(n => n.Name).Distinct().ToList().Count == argumentDefinitions.Count)
                _argumentDefinitions = argumentDefinitions;
            else
                throw new ArgumentException("Provided arguments are not unique");
        }

        public string Get(string argumentName)
        {
            string result = null;
            var argDef = _argumentDefinitions
                .Where(n => n.Name == argumentName)
                .FirstOrDefault();

            if (argDef != null)
                result = _argumentValues.GetValueOrDefault(argDef);

            return result;

        }

        public int GetCount()
        {
            return _argumentValues.Count;
        }

        public Dictionary<ArgumentDefinition, string> GetAllArguments()
        {
            return _argumentValues;
        }
    }
}

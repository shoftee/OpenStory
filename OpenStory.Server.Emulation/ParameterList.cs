using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenStory.Server.Emulation
{
    sealed class ParameterList
    {
        const RegexOptions ParamRegexOptions =
            RegexOptions.Compiled
            | RegexOptions.Singleline
            | RegexOptions.CultureInvariant
            | RegexOptions.ExplicitCapture;

        private static readonly Regex ParamRegex = new Regex(@"--(?<name>[A-Za-z][A-Za-z0-9]*)(=(?<value>""[^""]*""))?", ParamRegexOptions);

        private const char QuotationMark = '\"';
        private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;

        private readonly Dictionary<string, string> parameters;

        public ParameterList(IDictionary<string, string> parameters)
        {
            string error;
            var parsed = ParseParameters(parameters, out error);
            if (error != null)
            {
                throw new ArgumentException(error);
            }

            this.parameters = parsed;
        }

        public static Dictionary<string, string> ParseCommandLine(string commandLine)
        {
            var parsed = new Dictionary<string, string>();
            var matches = ParamRegex.Matches(commandLine);
            foreach (Match match in matches)
            {
                var captures = match.Captures;
                switch (captures.Count)
                {
                    case 1:
                        parsed.Add(captures[0].Value, string.Empty);
                        break;
                    case 2:
                        parsed.Add(captures[0].Value, captures[1].Value);
                        break;
                }
            }
            return parsed;
        }

        public string[] ToArgumentList()
        {
            var count = parameters.Count;
            var args = new string[count];
            int index = 0;

            var entries = parameters.OrderBy(item => item.Key, StringComparer.InvariantCulture);
            foreach (var entry in entries)
            {
                var name = entry.Key;
                var value = entry.Value;
                if (String.IsNullOrEmpty(value))
                {
                    const string NoValueEntry = @"--{0}";
                    args[index] = String.Format(InvariantCulture, NoValueEntry, name);
                }
                else
                {
                    const string ValueEntry = @"--{0}=""{1}""";
                    args[index] = String.Format(InvariantCulture, ValueEntry, name, value);
                }

                index++;
            }

            return args;
        }

        private static Dictionary<string, string> ParseParameters(IDictionary<string, string> parameters, out string error)
        {
            var parsed = new Dictionary<string, string>(parameters.Count, StringComparer.InvariantCulture);
            foreach (var entry in parameters)
            {
                string name = entry.Key.Trim();
                string value = entry.Value.Trim().Trim(QuotationMark);
                if (parsed.ContainsKey(name))
                {
                    const string DuplicateParameterNames =
                        "'{0}' : Parameter name duplicate after trimming white-space.";

                    error = String.Format(InvariantCulture, DuplicateParameterNames, name);
                }
                else if (name.Any(Char.IsWhiteSpace))
                {
                    const string NoWhiteSpaceInParameterName =
                        "'{0}' : Parameter names cannot contain white-space characters.";
                    error = String.Format(InvariantCulture, NoWhiteSpaceInParameterName, name);
                    return null;
                }
                else if (value.Any(c => c == QuotationMark))
                {
                    const string NoQuotationMarksInParameterValue =
                        "'{0}' : Parameter values cannot contain quotation marks.";

                    error = String.Format(InvariantCulture, NoQuotationMarksInParameterValue, value);
                    return null;
                }

                parsed.Add(name, value);
            }

            error = null;
            return parsed;
        }
    }
}

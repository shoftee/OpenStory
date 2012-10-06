using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenStory.Common.Tools
{
    /// <summary>
    /// Represents a command line parameter list.
    /// </summary>
    [Localizable(true)]
    public sealed class ParameterList
    {
        private const char QuotationMark = '\"';
        private const string ParameterRegexPattern = @"--(?<name>[A-Za-z][A-Za-z0-9\-]*)(=(?<value>""[^""]*""))?";
        
        private const RegexOptions ParamRegexOptions =
            RegexOptions.Compiled
            | RegexOptions.Singleline
            | RegexOptions.CultureInvariant
            | RegexOptions.ExplicitCapture;

        private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Regular Expression pattern for matching parameter key-value pairs.
        /// </summary>
        /// <remarks>
        /// This pattern will match strings that look like:
        /// * --some-flag-name
        /// * --some-key-name="some value"
        /// 
        /// General rules:
        /// * Do not put spaces in the key name.
        /// * Do not put spaces around the equals sign.
        /// * Always put values in quotation marks, even if there are no spaces in the value.
        /// * Do not put a digit as the first character of a key name. After that it's fine.
        /// * Keys are case-sensitive.
        /// </remarks>
        private static readonly Regex ParamRegex = new Regex(ParameterRegexPattern, ParamRegexOptions);

        private readonly Dictionary<string, string> parameters;

        /// <summary>
        /// Gets the value of a parameter.
        /// </summary>
        /// <param name="key">The name of the parameter.</param>
        /// <returns>
        /// the value of the parameter, <see cref="string.Empty"/> if the parameter has no value, or <c>null</c> if there is no such parameter.
        /// </returns>
        public string this[string key]
        {
            get
            {
                string value;
                this.parameters.TryGetValue(key, out value);
                return value;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ParameterList"/>.
        /// </summary>
        /// <param name="parameters">The parameter entries to initialize this list with.</param>
        private ParameterList(IDictionary<string, string> parameters)
        {
            this.parameters = new Dictionary<string, string>(parameters);
        }

        /// <summary>
        /// Parses the parameters from a provided command line.
        /// </summary>
        /// <remarks>
        /// The <see cref="Environment.CommandLine"/> property is useful for this.
        /// </remarks>
        /// <param name="commandLine">The command line to parse.</param>
        /// <returns>a <see cref="Dictionary{String,String}"/> of the parameter entries.</returns>
        private static Dictionary<string, string> ParseCommandLine(string commandLine)
        {
            if (commandLine == null)
            {
                throw new ArgumentNullException("commandLine");
            }

            var parsed = new Dictionary<string, string>();
            var matches = ParamRegex.Matches(commandLine);
            foreach (Match match in matches)
            {
                var groups = match.Groups;
                var key = groups[1].Value;
                var value = groups[2].Value;
                parsed.Add(key, value);
            }
            return parsed;
        }

        /// <summary>
        /// Constructs a string array parameter list.
        /// </summary>
        /// <remarks>
        /// This is useful when you need to use the list with the .NET methods to execute a process.
        /// </remarks>
        /// <returns>an array of <see cref="string"/> with each parameter entry corresponding to an array element.</returns>
        public string[] ToArgumentList()
        {
            var count = this.parameters.Count;
            var args = new string[count];
            int index = 0;

            var entries = this.parameters.OrderBy(item => item.Key, StringComparer.InvariantCulture);
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

        /// <summary>
        /// Gets the parameter list from the <see cref="Environment.CommandLine"/> variable.
        /// </summary>
        /// <returns>an instance of <see cref="ParameterList"/>.</returns>
        public static ParameterList FromEnvironment(out string error)
        {
            var parameters = ParseCommandLine(Environment.CommandLine);
            var parsed = ParseParameters(parameters, out error);
            if (error == null)
            {
                return new ParameterList(parsed);
            }
            else
            {
                return null;
            }
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
                    error = String.Format(Errors.DuplicateParameterNames, name);
                    return null;
                }
                else if (name.Any(Char.IsWhiteSpace))
                {
                    error = String.Format(Errors.NoWhiteSpaceInParameterName, name);
                    return null;
                }
                else if (value.Any(c => c == QuotationMark))
                {
                    error = String.Format(Errors.NoQuotationMarksInParameterValue, value);
                    return null;
                }

                parsed.Add(name, value);
            }

            error = null;
            return parsed;
        }
    }
}

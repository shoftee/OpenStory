using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenStory.Common
{
    /// <summary>
    /// Represents a command line parameter list.
    /// </summary>
    [Localizable(true)]
    public sealed class ParameterList
    {
        private const char DoubleQuotationMark = '\"';
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
        /// <para>
        /// This pattern will match strings that look like:
        /// * --some-flag-name
        /// * --some-key-name="some value"
        /// </para><para>
        /// General rules:
        /// * Do not put spaces in the key name.
        /// * Do not put spaces around the equals sign.
        /// * Always put values in quotation marks, even if there are no spaces in the value.
        /// * Do not put a digit as the first character of a key name. After that it's fine.
        /// * Keys are case-sensitive.
        /// </para>
        /// </remarks>
        private static readonly Regex ParamRegex = new Regex(ParameterRegexPattern, ParamRegexOptions);

        private readonly Dictionary<string, string> parameters;

        /// <summary>
        /// Gets the value of a parameter.
        /// </summary>
        /// <param name="key">The name of the parameter.</param>
        /// <returns>
        /// the value of the parameter, <see cref="string.Empty"/> if the parameter has no value, or <see langword="null"/> if there is no such parameter.
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
        /// Initializes a new instance of the <see cref="ParameterList"/> class.
        /// </summary>
        /// <param name="parameters">The parameter entries to initialize this list with.</param>
        private ParameterList(IDictionary<string, string> parameters)
        {
            this.parameters = new Dictionary<string, string>(parameters, StringComparer.OrdinalIgnoreCase);
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

            var entries = this.parameters.OrderBy(item => item.Key, StringComparer.OrdinalIgnoreCase);
            foreach (var entry in entries)
            {
                var name = entry.Key;
                var value = entry.Value;
                if (string.IsNullOrEmpty(value))
                {
                    const string NoValueEntry = @"--{0}";
                    args[index] = string.Format(InvariantCulture, NoValueEntry, name);
                }
                else
                {
                    const string ValueEntry = @"--{0}=""{1}""";
                    args[index] = string.Format(InvariantCulture, ValueEntry, name, value);
                }

                index++;
            }

            return args;
        }

        /// <summary>
        /// Gets the parameter list from the <see cref="Environment.CommandLine"/> variable.
        /// </summary>
        /// <exception cref="FormatException">Thrown if the provided parameter list has an invalid format.</exception>
        /// <returns>an instance of <see cref="ParameterList"/>.</returns>
        public static ParameterList FromEnvironment()
        {
            var commandLine = Environment.CommandLine;
            return Parse(commandLine);
        }

        /// <summary>
        /// Gets the parameter list from the provided string.
        /// </summary>
        /// <param name="parameterString">The string that contains the parameter information.</param>
        /// <exception cref="FormatException">Thrown if the provided parameter list has an invalid format.</exception>
        /// <returns>an instance of <see cref="ParameterList"/>.</returns>
        public static ParameterList Parse(string parameterString)
        {
            Guard.NotNull(() => parameterString, parameterString);

            var parameters = ParseInitial(parameterString);
            var parsed = ParseParameters(parameters);
            return new ParameterList(parsed);
        }

        /// <summary>
        /// Gets the parameter list from the <see cref="Environment.CommandLine"/> variable.
        /// </summary>
        /// <param name="error">A variable to hold any error messages.</param>
        /// <returns>an instance of <see cref="ParameterList"/>, or <see langword="null"/> if there were errors.</returns>
        public static ParameterList FromEnvironment(out string error)
        {
            var commandLine = Environment.CommandLine;
            return Parse(commandLine, out error);
        }

        /// <summary>
        /// Gets the parameter list from the provided string.
        /// </summary>
        /// <param name="parameterString">The string that contains the parameter information.</param>
        /// <param name="error">A variable to hold any error messages.</param>
        /// <returns>an instance of <see cref="ParameterList"/>, or <see langword="null"/> if there were errors.</returns>
        public static ParameterList Parse(string parameterString, out string error)
        {
            Guard.NotNull(() => parameterString, parameterString);

            var parameters = ParseInitial(parameterString);
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

        /// <summary>
        /// Parses the parameters from a provided parameter string.
        /// </summary>
        /// <remarks>
        /// The <see cref="Environment.CommandLine"/> property is useful for this.
        /// </remarks>
        /// <param name="parameterString">The parameter string to parse.</param>
        /// <returns>a <see cref="Dictionary{String,String}"/> of the parameter entries.</returns>
        private static Dictionary<string, string> ParseInitial(string parameterString)
        {
            var parsed = new Dictionary<string, string>();
            var matches = ParamRegex.Matches(parameterString);
            foreach (Match match in matches)
            {
                var groups = match.Groups;
                var key = groups["name"].Value;
                var value = groups["value"].Value;
                parsed.Add(key, value);
            }

            return parsed;
        }

        private static Dictionary<string, string> ParseParameters(IDictionary<string, string> parameters)
        {
            string error;
            var parsed = ParseParameters(parameters, out error);
            if (error != null)
            {
                throw new FormatException(error);
            }
            else
            {
                return parsed;
            }
        }

        private static Dictionary<string, string> ParseParameters(IDictionary<string, string> parameters, out string error)
        {
            var parsed = new Dictionary<string, string>(parameters.Count, StringComparer.OrdinalIgnoreCase);
            foreach (var entry in parameters)
            {
                string name = entry.Key.Trim();
                string value = entry.Value.Trim().Trim(DoubleQuotationMark);
                if (parsed.ContainsKey(name))
                {
                    error = string.Format(CommonStrings.DuplicateParameterNamesError, name);
                    return null;
                }
                else if (name.Any(char.IsWhiteSpace))
                {
                    error = string.Format(CommonStrings.NoWhiteSpaceInParameterNameError, name);
                    return null;
                }
                else if (value.Any(c => c == DoubleQuotationMark))
                {
                    error = string.Format(CommonStrings.NoQuotationMarksInParameterValueError, value);
                    return null;
                }

                parsed.Add(name, value);
            }

            error = null;
            return parsed;
        }
    }
}

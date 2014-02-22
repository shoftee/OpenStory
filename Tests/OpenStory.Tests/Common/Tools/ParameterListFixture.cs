using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace OpenStory.Common
{
    [TestFixture]
    public sealed class ParameterListFixture
    {
        [Test]
        public void Parse_Should_Return_Case_Insensitive_ParameterList()
        {
            var parameters = ParameterList.Parse(@"--parameter=""value""");
            parameters["parameter"].Should().Be("value");
            parameters["PARAMETER"].Should().Be("value");
        }

        [Test]
        public void Parse_Should_Support_Multiple_Parameter_Definitions()
        {
            var parameters = ParameterList.Parse(@"--parameter1=""value1"" --parameter2=""value2""");
            parameters["parameter1"].Should().Be("value1");
            parameters["parameter2"].Should().Be("value2");
        }

        [Test]
        public void ParameterList_Should_Return_Empty_String_For_No_Value_Parameters()
        {
            var parameters = ParameterList.Parse(@"--parameter");
            parameters["parameter"].Should().BeEmpty();
        }

        [Test]
        public void ParameterList_Should_Return_Null_For_Missing_Parameters()
        {
            var parameters = ParameterList.Parse(@"--parameter=""value""");
            parameters["other-parameter"].Should().BeNull();
        }

        [Test]
        public void Parse_Should_Support_Spaces_In_Parameter_Values()
        {
            var parameters = ParameterList.Parse(@"--parameter=""value with all kinds of spaces""");
            parameters["parameter"].Should().Be("value with all kinds of spaces");
        }

        [Test]
        public void Parse_Should_Support_Hyphens_In_Parameter_Names()
        {
            var parameters = ParameterList.Parse(@"--parameter-with-hyphens=""value""");
            parameters["parameter-with-hyphens"].Should().Be("value");
        }

        [TestCase("1invalid-because-starts-with-number")]
        public void Parse_Should_Skip_Invalid_Parameter_Definitions(string parameterName)
        {
            const string InvalidParameterFormat = @"--{0}=""don't mind me""";
            var parameters = ParameterList.Parse(String.Format(InvalidParameterFormat, parameterName));
            parameters[parameterName].Should().Be(null);
        }
    }
}

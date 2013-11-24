using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace OpenStory.Common
{
    [TestFixture]
    [Category("OpenStory.Common.IO.PacketCodeTable")]
    public sealed class PacketCodeTableFixture
    {
        [Test]
        public void TryGetOutgoingCode_Should_Throw_On_Null_Label()
        {
            var table = new TestTable();

            table
                .Invoking(
                    t =>
                    {
                        ushort code;
                        t.TryGetOutgoingCode(null, out code);
                    })
                .ShouldThrow<ArgumentNullException>();
        }
        [Test]
        public void TryGetOutgoingCode_Should_Throw_On_Empty_Label()
        {
            var table = new TestTable();

            table
                .Invoking(
                    t =>
                    {
                        ushort code;
                        t.TryGetOutgoingCode("", out code);
                    })
                .ShouldThrow<ArgumentException>();
        }

        [Test]
        public void LoadPacketCodes_Should_Not_Throw()
        {
            var table = new TestTable();

            table
                .Invoking(t => t.LoadPacketCodes())
                 .ShouldNotThrow();
        }

        [Test]
        public void AddOut_Should_Throw_On_Null_Label()
        {
            var table = new TestTable();

            table
                .Invoking(t => t.AddOut(null, 0x0000))
                .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void AddOut_Should_Throw_On_Empty_Label()
        {
            var table = new TestTable();

            table
                .Invoking(t => t.AddOut("", 0x0000))
                .ShouldThrow<ArgumentException>();
        }

        [Test]
        public void AddIn_Should_Throw_On_Null_Label()
        {
            var table = new TestTable();

            table
                .Invoking(t => t.AddIn(0x0000, null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void AddIn_Should_Throw_On_Empty_Label()
        {
            var table = new TestTable();

            table
                .Invoking(t => t.AddIn(0x0000, ""))
                .ShouldThrow<ArgumentException>();
        }

        [Test]
        public void LoadPacketCodesInternal_Should_Be_Called()
        {
            var table = new TestTable();

            table.LoadPacketCodes();

            table.LoadPacketCodesCallCount.Should().Be(1);
        }

        [Test]
        public void GetIncomingLabel_Should_Throw_KeyNotFoundException_For_Missing_Code()
        {
            const int Zero = 0x0000;
            const int One = 0x0001;

            var table = new TestTable();
            table.AddIn(Zero, "Zero");

            table
                .Invoking(t => t.GetIncomingLabel(One))
                .ShouldThrow<KeyNotFoundException>();
        }

        [Test]
        public void TryGetIncomingLabel_Should_Return_False_For_Missing_Code()
        {
            const int Zero = 0x0000;
            const int One = 0x0001;

            var table = new TestTable();
            table.AddIn(Zero, "Zero");

            string s;
            table.TryGetIncomingLabel(One, out s).Should().BeFalse();
        }

        [Test]
        public void TryGetIncomingLabel_Should_Return_True_For_Existing_Code()
        {
            const int Zero = 0x0000;

            var table = new TestTable();
            table.AddIn(Zero, "Zero");

            string s;
            table.TryGetIncomingLabel(Zero, out s).Should().BeTrue();
        }

        [Test]
        public void TryGetIncomingLabel_Should_Retrieve_Correct_Label()
        {
            const int One = 0x0001;
            const string OneString = "One";

            var table = new TestTable();
            table.AddIn(One, OneString);

            string label;
            table.TryGetIncomingLabel(One, out label);
            label.Should().Be(OneString);
        }

        [Test]
        public void GetIncomingLabel_Should_Return_Correct_Label()
        {
            const int One = 0x0001;
            const string OneString = "One";
            
            var table = new TestTable();
            table.AddIn(One, OneString);

            table.GetIncomingLabel(One).Should().Be(OneString);
        }

        [Test]
        public void TryGetOutgoingCode_Should_Return_False_For_Missing_Label()
        {
            const int Zero = 0x0000;

            var table = new TestTable();
            table.AddOut("Zero", Zero);

            ushort code;
            table.TryGetOutgoingCode("One", out code).Should().BeFalse();
        }

        [Test]
        public void TryGetOutgoingCode_Should_Return_True_For_Existing_Label()
        {
            const int Zero = 0x0000;
            const string ZeroString = "Zero";

            var table = new TestTable();
            table.AddOut(ZeroString, Zero);

            ushort code;
            table.TryGetOutgoingCode(ZeroString, out code).Should().BeTrue();
        }

        [Test]
        public void TryGetOutgoingCode_Should_Retrieve_Correct_Code()
        {
            const int One = 0x0001;
            const string OneString = "One";

            var table = new TestTable();
            table.AddOut(OneString, One);

            ushort code;
            table.TryGetOutgoingCode(OneString, out code);
            code.Should().Be(One);
        }

        [Test]
        public void GetOutgoingCode_Should_Return_Correct_Code()
        {
            const int One = 0x0001;
            const string OneString = "One";

            var table = new TestTable();
            table.AddOut(OneString, One);

            table.GetOutgoingCode(OneString).Should().Be(One);
        }

        [Test]
        public void GetOutgoingCode_Should_Throw_For_Missing_Label()
        {
            const int One = 0x0001;
            const string ZeroString = "Zero";
            const string OneString = "One";

            var table = new TestTable();
            table.AddOut(OneString, One);

            table
                .Invoking(t => t.GetOutgoingCode(ZeroString))
                .ShouldThrow<KeyNotFoundException>();
        }
        
        [Test]
        public void AddIn_Should_Return_True_On_Adding_New_Code()
        {
            const int One = 0x0001;
            var table = new TestTable();

            table.AddIn(One, "One").Should().BeTrue();
        }

        [Test]
        public void AddIn_Should_Return_False_On_Adding_Existing_Code()
        {
            const int One = 0x0001;
            const string OneString = "One";

            var table = new TestTable();
            table.AddIn(One, OneString);

            table.AddIn(One, OneString).Should().BeFalse();
        }

        [Test]
        public void AddOut_Should_Return_True_On_Adding_New_Label()
        {
            const int One = 0x0001;
            var table = new TestTable();

            table.AddOut("One", One).Should().BeTrue();
        }

        [Test]
        public void AddOut_Should_Return_False_On_Adding_Existing_Label()
        {
            const int One = 0x0001;
            const string OneString = "One";

            var table = new TestTable();
            table.AddOut(OneString, One);

            table.AddOut(OneString, One).Should().BeFalse();
        }

        private sealed class TestTable : PacketCodeTable
        {
            public int LoadPacketCodesCallCount { get; private set; }

            public bool AddIn(ushort code, string label)
            {
                return this.AddIncoming(code, label);
            }

            public bool AddOut(string label, ushort code)
            {
                return this.AddOutgoing(label, code);
            }

            #region Overrides of PacketCodeTable

            protected override void LoadPacketCodesInternal()
            {
                this.LoadPacketCodesCallCount++;
            }

            #endregion
        }
    }
}

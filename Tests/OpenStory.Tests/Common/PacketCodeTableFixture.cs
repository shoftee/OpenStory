using System;
using FluentAssertions;
using NUnit.Framework;

namespace OpenStory.Common
{
    [TestFixture]
    [Category("OpenStory.Common.IO.PacketCodeTable")]
    public sealed class PacketCodeTableFixture
    {
        [Test]
        public void TryGetOutgoingOpCode_Should_Throw_On_Null_Label()
        {
            var table = new TestTable();

            table.Invoking(
                t =>
                {
                    ushort code;
                    t.TryGetOutgoingCode(null, out code);
                }).ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void LoadOpCodes_Should_Not_Throw()
        {
            var table = new TestTable();

            table.Invoking(t => t.LoadPacketCodes())
                 .ShouldNotThrow();
        }

        [Test]
        public void AddOut_Should_Throw_On_Null_Label()
        {
            var table = new TestTable();

            table.Invoking(t => t.AddOut(null, 0x0000))
                 .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void AddIn_Should_Throw_On_Null_Label()
        {
            var table = new TestTable();

            table.Invoking(t => t.AddIn(0x0000, null))
               .ShouldThrow<ArgumentNullException>();
        }
        
        [Test]
        public void LoadOpCodesInternal_Should_Be_Called()
        {
            var table = new TestTable();

            table.LoadPacketCodes();

            table.LoadOpCodesInternalCount.Should().Be(1);
        }
        
        [Test]
        public void TryGetIncomingLabel_Should_Return_False_For_Missing_Code()
        {
            var table = new TestTable();
            table.AddIn(0x0000, "Zero");

            string s;
            table.TryGetIncomingLabel(0x0001, out s).Should().BeFalse();
        }

        [Test]
        public void TryGetIncomingLabel_Should_Return_True_For_Existing_Code()
        {
            var table = new TestTable();
            table.AddIn(0x0000, "Zero");

            string s;
            table.TryGetIncomingLabel(0x0000, out s).Should().BeTrue();
        }

        [Test]
        public void TryGetIncomingLabel_Should_Retrieve_Correct_Label()
        {
            var table = new TestTable();
            table.AddIn(0x0001, "One");

            string label;
            table.TryGetIncomingLabel(0x0001, out label);
            label.Should().Be("One");
        }

        [Test]
        public void TryGetOutgoingOpCode_Should_Return_False_For_Missing_Label()
        {
            var table = new TestTable();
            table.AddOut("Zero", 0x0000);

            ushort code;
            table.TryGetOutgoingCode("One", out code).Should().BeFalse();
        }

        [Test]
        public void TryGetOutgoingOpCode_Should_Return_True_For_Existing_Label()
        {
            var table = new TestTable();
            table.AddOut("Zero", 0x0000);

            ushort code;
            table.TryGetOutgoingCode("Zero", out code).Should().BeTrue();
        }

        [Test]
        public void TryGetOutgoingOpCode_Should_Retrieve_Correct_OpCode()
        {
            var table = new TestTable();
            table.AddOut("One", 0x0001);

            ushort code;
            table.TryGetOutgoingCode("One", out code);
            code.Should().Be(0x0001);
        }
        
        [Test]
        public void AddIn_Should_Return_True_On_Adding_New_Code()
        {
            var table = new TestTable();

            table.AddIn(0x0001, "One").Should().BeTrue();
        }

        [Test]
        public void AddIn_Should_Return_False_On_Adding_Existing_Code()
        {
            var table = new TestTable();
            table.AddIn(0x0001, "One");

            table.AddIn(0x0001, "One").Should().BeFalse();
        }

        [Test]
        public void AddOut_Should_Return_True_On_Adding_New_Label()
        {
            var table = new TestTable();
            
            table.AddOut("One", 0x0001).Should().BeTrue();
        }

        [Test]
        public void AddOut_Should_Return_False_On_Adding_Existing_Label()
        {
            var table = new TestTable();
            table.AddOut("One", 0x0001);

            table.AddOut("One", 0x0001).Should().BeFalse();
        }

        private sealed class TestTable : PacketCodeTable
        {
            public int LoadOpCodesInternalCount { get; private set; }

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
                this.LoadOpCodesInternalCount++;
            }

            #endregion
        }
    }
}

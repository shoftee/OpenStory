using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenStory.Common.Data;

namespace OpenStory.Tests
{
    [TestFixture(Category = "OpenStory.Common.IO", Description = "OpCodeTable tests.")]
    public sealed class OpCodeTableFixture
    {
        [Test]
        public void ReturnsFalseOnMissingCode()
        {
            var table = new TestTable();
            table.AddIn(0x0000, "Zero");

            string s;
            Assert.IsFalse(table.TryGetIncomingLabel(0x0001, out s));
        }

        [Test]
        public void ReturnsTrueOnExistingCode()
        {
            var table = new TestTable();
            table.AddIn(0x0000, "Zero");

            string s;
            Assert.IsTrue(table.TryGetIncomingLabel(0x0000, out s));
        }

        [Test]
        public void ReturnsFalseOnMissingLabel()
        {
            var table = new TestTable();
            table.AddOut("Zero", 0x0000);

            ushort code;
            Assert.IsFalse(table.TryGetOutgoingOpCode("One", out code));
        }

        [Test]
        public void ReturnsTrueOnExistingLabel()
        {
            var table = new TestTable();
            table.AddOut("Zero", 0x0000);

            ushort code;
            Assert.IsTrue(table.TryGetOutgoingOpCode("Zero", out code));
        }

        [Test]
        public void ThrowsOnAddingNullLabel()
        {
            var table = new TestTable();
            Assert.Throws<ArgumentNullException>(() => table.AddOut(null, 0x0000));
            Assert.Throws<ArgumentNullException>(() => table.AddIn(0x0000, null));
        }

        [Test]
        public void ReturnsTrueOnAddingNewCode()
        {
            var table = new TestTable();
            Assert.IsTrue(table.AddIn(0x0001, "One"));
        }

        [Test]
        public void ReturnsFalseOnAddingExistingCode()
        {
            var table = new TestTable();
            table.AddIn(0x0001, "One");
            Assert.IsFalse(table.AddIn(0x0001, "One"));
        }

        [Test]
        public void ReturnsFalseOnAddingNewLabel()
        {
            var table = new TestTable();
            Assert.IsTrue(table.AddOut("One", 0x0001));
        }

        [Test]
        public void ReturnsFalseOnAddingExistingLabel()
        {
            var table = new TestTable();
            table.AddOut("One", 0x0001);
            Assert.IsFalse(table.AddOut("One", 0x0001));
        }

        [Test]
        public void ThrowsAneOnGettingNullLabel()
        {
            var table = new TestTable();
            ushort opCode;
            Assert.Throws<ArgumentNullException>(() => table.TryGetOutgoingOpCode(null, out opCode));
        }

        [Test]
        public void LoadOpCodesDoesNotThrow()
        {
            var table = new TestTable();
            table.LoadOpCodes();
        }

        private sealed class TestTable : OpCodeTable
        {
            public bool AddIn(ushort code, string label)
            {
                return base.AddIncoming(code, label);
            }

            public bool AddOut(string label, ushort code)
            {
                return base.AddOutgoing(label, code);
            }

            #region Overrides of OpCodeTable

            protected override void LoadOpCodesInternal()
            {
            }

            #endregion
        }
    }
}

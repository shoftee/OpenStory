using System;
using System.Net.Sockets;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace OpenStory.Networking
{
    [Category("OpenStory.Common.Networking")]
    [TestFixture]
    public sealed class ReceiveDescriptorFixture
    {
        [Test]
        public void DataArrived_Should_Throw_When_Delegate_Already_Bound()
        {
            var descriptor = new ReceiveDescriptor(Mock.Of<IDescriptorContainer>());
            descriptor.DataArrived += (o, e) => { };

            descriptor
                .Invoking(d => d.DataArrived += (o, e) => { })
                .ShouldThrow<InvalidOperationException>()
                .WithMessage(CommonStrings.EventMustHaveOnlyOneSubscriber);
        }

        #region StartReceive()

        [Test]
        public void StartReceive_Should_Throw_If_DataArrived_Has_No_Subscribers()
        {
            var descriptor = new ReceiveDescriptor(Mock.Of<IDescriptorContainer>());

            descriptor
                .Invoking(d => d.StartReceive())
                .ShouldThrow<InvalidOperationException>()
                .WithMessage(CommonStrings.ReceiveEventHasNoSubscribers);
        }

        [Test]
        public void StartReceive_Should_Check_Container_IsActive()
        {
            var container = new Mock<IDescriptorContainer>();
            container.SetupGet(c => c.IsActive).Returns(false);

            var descriptor = new ReceiveDescriptor(container.Object);
            descriptor.DataArrived += (sender, args) => { };
            descriptor.StartReceive();

            container.Verify(c => c.IsActive, Times.Once());
        }

        #endregion
    }
}

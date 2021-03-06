﻿using System;
using Microsoft.Practices.Prism.Events;
using TascheAtWork.Core.Infrastructure;
using TascheAtWork.Core.Services;

namespace TascheAtWork.Core
{
    public class TascheBase
    {
        private IMessageAggregator _messageAggregator;

        private IMessageAggregator MessageAggregator {
            get { return _messageAggregator ?? (_messageAggregator = ServiceLocator.Resolve<IMessageAggregator>()); }
        }
        

        public void SubscribeViaMessageAggregator<TMessage>(Action<TMessage> methodToCall, ThreadOption threadOption, bool strongReference)
        {
            var compositePresentationEvent = MessageAggregator.GetEvent<CompositePresentationEvent<TMessage>>();
            compositePresentationEvent.Subscribe(methodToCall, threadOption, strongReference);
        }

        public void PublishViaMessageAggregator<TMessage>(TMessage message)
        {
            var compositePresentationEvent = MessageAggregator.GetEvent<CompositePresentationEvent<TMessage>>();
            compositePresentationEvent.Publish(message);
        }

    }
}

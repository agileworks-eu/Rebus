﻿using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using System.Linq;
using Rebus.Configuration;

namespace Rebus.Unity
{
    public class UnityContainerAdapter : IContainerAdapter
    {
        readonly IUnityContainer unityContainer;

        public UnityContainerAdapter(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        public IEnumerable<IHandleMessages> GetHandlerInstancesFor<T>()
        {
            return unityContainer.ResolveAll<IHandleMessages<T>>().ToArray();
        }

        public void Release(IEnumerable handlerInstances)
        {
            foreach (var disposable in handlerInstances.OfType<IDisposable>())
            {
                disposable.Dispose();
            }
        }

        public void SaveBusInstances(IBus bus)
        {
            unityContainer.RegisterInstance(typeof(IBus), bus);
            unityContainer.RegisterType<IMessageContext>(new InjectionFactory(c => MessageContext.GetCurrent()));
        }
    }
}

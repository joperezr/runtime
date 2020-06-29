﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.DotNet.RemoteExecutor;
using Xunit;

namespace System.Runtime.Tests
{
    public class UseResourceKeysTest
    {
        [ConditionalFact(typeof(RemoteExecutor), nameof(RemoteExecutor.IsSupported))]
        public void RetrnsResourceKeyWhenFeatureSwitchIsEnabled()
        {
            RemoteInvokeOptions options = new RemoteInvokeOptions();
            options.RuntimeConfigurationOptions.Add("System.Resources.UseSystemResourceKeys", true);

            RemoteExecutor.Invoke(() =>
            {
                try
                {
                    throw new AggregateException();
                }
                catch (Exception e)
                {
                    Assert.Equal("AggregateException_ctor_DefaultMessage", e.Message);
                }
            }, options).Dispose();
        }

        [ConditionalFact(typeof(RemoteExecutor), nameof(RemoteExecutor.IsSupported))]
        public void RetrnsResourceWhenFeatureSwitchIsDisabled()
        {
            RemoteInvokeOptions options = new RemoteInvokeOptions();
            options.RuntimeConfigurationOptions.Add("System.Resources.UseSystemResourceKeys", false);

            RemoteExecutor.Invoke(() =>
            {
                try
                {
                    throw new ArgumentException();
                }
                catch (Exception e)
                {
                    Assert.NotEqual("AggregateException_ctor_DefaultMessage", e.Message);
                }
            }, options).Dispose();
        }
    }
}

﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Graph.Providers
{
    /// <summary>
    /// Creates a new provider instance for the provided configuration and sets the GlobalProvider.
    /// </summary>
    public static class Graph
    {
        /// <summary>
        /// Gets the Graph Config property value.
        /// </summary>
        /// <param name="target">
        /// The target object to retrieve the property value from.
        /// </param>
        /// <returns>
        /// The value of the property on the target.
        /// </returns>
        public static IGraphConfig GetConfig(ResourceDictionary target)
        {
            return (IGraphConfig)target.GetValue(ConfigProperty);
        }

        /// <summary>
        /// Sets the GraphConfig property value.
        /// </summary>
        /// <param name="target">
        /// The target object to set the value on.
        /// </param>
        /// <param name="value">
        /// The value to apply to the target property.
        /// </param>
        public static void SetConfig(ResourceDictionary target, IGraphConfig value)
        {
            target.SetValue(ConfigProperty, value);
        }

        /// <summary>
        /// Identifies the Config dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Config dependency property.
        /// </returns>
        public static readonly DependencyProperty ConfigProperty =
            DependencyProperty.RegisterAttached("Config", typeof(IGraphConfig), typeof(Graph), new PropertyMetadata(null, OnConfigChanged));

        private static void OnConfigChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ResourceDictionary rd)
            {
                IGraphConfig config = GetConfig(rd);

                Type configType = config.GetType();
                if (_providers.ContainsKey(configType))
                {
                    var providerFactory = _providers[configType];
                    ProviderManager.Instance.GlobalProvider = providerFactory.Invoke(config);
                }
                else if (config is MockConfig mockConfig)
                {
                    ProviderManager.Instance.GlobalProvider = new MockProvider(mockConfig);
                }
            }
            else
            {
                ProviderManager.Instance.GlobalProvider = null;
            }
        }

        private static readonly Dictionary<Type, Func<IGraphConfig, IProvider>> _providers = new Dictionary<Type, Func<IGraphConfig, IProvider>>();

        /// <summary>
        /// Register a provider to be available for declaration in XAML using the ConfigProperty.
        /// Use in the static constructor of an IGraphConfig implementation.
        /// </summary>
        /// <code>
        /// static MsalConfig()
        /// {
        ///     Graph.RegisterConfig(typeof(MsalConfig), (c) => MsalProvider.Create(c as MsalConfig));
        /// }.
        /// </code>
        /// <param name="configType">
        /// The Type of the IGraphConfig implementation associated with provider.
        /// </param>
        /// <param name="providerFactory">
        /// A factory function for creating a new instance of the IProvider implementation.
        /// </param>
        public static void RegisterConfig(Type configType, Func<IGraphConfig, IProvider> providerFactory)
        {
            if (!_providers.ContainsKey(configType))
            {
                _providers.Add(configType, providerFactory);
            }
        }
    }
}

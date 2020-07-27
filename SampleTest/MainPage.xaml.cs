// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Graph;
using Microsoft.Graph.Extensions;
using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Controls;

namespace SampleTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public DateTime Today => DateTimeOffset.Now.Date.ToUniversalTime();

        public DateTime ThreeDaysFromNow => Today.AddDays(3);

        public MainPage()
        {
            this.InitializeComponent();
        }

        public static string ToLocalTime(DateTimeTimeZone value)
        {
            return value.ToDateTimeOffset().LocalDateTime.ToString("g");
        }

        public static string RemoveWhitespace(string value)
        {
            //// Workaround for https://github.com/microsoft/microsoft-ui-xaml/issues/2654
            return Regex.Replace(value, @"\t|\r|\n", " ");
        }
    }
}

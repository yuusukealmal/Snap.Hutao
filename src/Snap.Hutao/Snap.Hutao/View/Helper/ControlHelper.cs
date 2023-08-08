﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Control;

namespace Snap.Hutao.View.Helper;

[SuppressMessage("", "SH001")]
[DependencyProperty("SquareLength", typeof(double), 0D, nameof(OnSquareLengthChanged), IsAttached = true, AttachedType = typeof(Microsoft.UI.Xaml.Controls.Control))]
public sealed partial class ControlHelper
{
    private static void OnSquareLengthChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
    {
        Microsoft.UI.Xaml.Controls.Control control = (Microsoft.UI.Xaml.Controls.Control)dp;
        control.Width = (double)e.NewValue;
        control.Height = (double)e.NewValue;
    }
}
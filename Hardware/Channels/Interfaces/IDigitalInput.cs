﻿using Core;

namespace Hardware
{
    /// <summary>
    /// Define a generic digital input
    /// </summary>
    public interface IDigitalInput : IDigitalChannel, IReadOnlyProperty<bool>
    {
    }
}
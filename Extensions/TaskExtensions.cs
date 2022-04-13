﻿using System;
using System.Threading.Tasks;

namespace Extensions
{
    /// <summary>
    /// Provide <see cref="Task"/> (and <see cref="Task{TResult}"/>)-related extension methods
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Start a <see cref="Task"/> with a <paramref name="timeout"/>
        /// </summary>
        /// <param name="source">The <see cref="Task"/> to start</param>
        /// <param name="timeout">The <paramref name="source"/> timeout (in milliseconds) to complete</param>
        /// <returns>
        /// The (async) <see cref="Task{TResult}"/> (<see langword="true"/> if the <see cref="Task"/> completed,
        /// <see langword="false"/> if timeout occurred)
        /// </returns>
        public static async Task<bool> StartWithTimeout(this Task source, int timeout)
        {
            bool completed = await Task.WhenAny(source, Task.Delay(timeout)) == source;
            return completed;
        }

        /// <summary>
        /// Start a <see cref="Task"/> with a <paramref name="timeout"/>
        /// </summary>
        /// <param name="source">The <see cref="Task"/> to start</param>
        /// <param name="timeout">The <paramref name="source"/> timeout (as <see cref="TimeSpan"/>) to complete</param>
        /// <returns>
        /// The (async) <see cref="Task{TResult}"/> (<see langword="true"/> if the <see cref="Task"/> completed,
        /// <see langword="false"/> if timeout occurred)
        /// </returns>
        public static async Task<bool> StartWithTimeout(this Task source, TimeSpan timeout)
            => await source.StartWithTimeout(timeout.Milliseconds);
    }
}
﻿using NWrath.Synergy.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace NWrath.Logging
{
    public static class PipeLoggerExtensions
    {
        public static PipeLogger<TLogger> ClearPipes<TLogger>(this PipeLogger<TLogger> source)
            where TLogger : ILogger
        {
            source.Pipes.Clear();

            return source;
        }

        public static PipeLogger<TLogger> UsePipes<TLogger>(
            this PipeLogger<TLogger> source,
            IPipe<PipeLoggerContext<TLogger>>[] collection
            )
            where TLogger : ILogger
        {
            source.Pipes.AddRange(collection);

            return source;
        }

        public static PipeLogger<TLogger> UsePipes<TLogger>(
            this PipeLogger<TLogger> source,
            params Action<PipeLoggerContext<TLogger>, Action<PipeLoggerContext<TLogger>>>[] collection
            )
            where TLogger : ILogger
        {
            source.Pipes.AddRange(collection);

            return source;
        }

        public static PipeLogger<TLogger> UseDefaultWriterPipe<TLogger>(this PipeLogger<TLogger> source)
            where TLogger : ILogger
        {
            source.Pipes.Add(PipeLogger<TLogger>.LogWriterPipe);

            return source;
        }
    }
}
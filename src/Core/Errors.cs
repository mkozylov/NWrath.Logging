using System;

namespace NWrath.Logging
{
    public static class Errors
    {
        public static ArgumentException NO_LOGGERS = new ArgumentException("You need set one or more loggers!");

        public static ArgumentNullException NULL_BASE_LOGGER = new ArgumentNullException("You need set base logger!");

        public static ArgumentNullException NULL_STREAM = new ArgumentNullException("You need set stream!");

        public static ArgumentNullException NULL_LAMBDA = new ArgumentNullException("You need set lambda action!");

        public static ArgumentException NO_FILE_PROVIDER = new ArgumentException("You need set file provider!");

        public static ArgumentException NO_CONNECTION_STRING = new ArgumentException("You need set db connection string!");

        public static ArgumentException NO_LOG_LEVELS = new ArgumentException("You must specify at least one log level");

        public static ArgumentException WRONG_LOG_LEVELS = new ArgumentException("The min level can not be higher than the max level");
    }
}
using StackExchange.Redis;
using System;
using System.Threading;

namespace SHAutomation.Core.StaticClasses
{
    public static class RedisManager
    {

        private static long _lastReconnectTicks = DateTimeOffset.MinValue.UtcTicks;
        private static DateTimeOffset _firstError = DateTimeOffset.MinValue;
        private static DateTimeOffset _previousError = DateTimeOffset.MinValue;
        private static object _reconnectLock = new object();
        private static Lazy<ConnectionMultiplexer> _multiplexer = CreateMultiplexer();


        public static string EndPoint { get; set; }
        public static string Password { get; set; }
        public static int Port { get; set; }
        public static bool UseSSL { get; set; }

        // In general, let StackExchange.Redis handle most reconnects, 
        // so limit the frequency of how often this will actually reconnect.
        public static TimeSpan ReconnectMinFrequency = TimeSpan.FromSeconds(60);

        // if errors continue for longer than the below threshold, then the 
        // multiplexer seems to not be reconnecting, so re-create the multiplexer
        public static TimeSpan ReconnectErrorThreshold = TimeSpan.FromSeconds(30);



        public static ConnectionMultiplexer Connection { get { return _multiplexer.Value; } }

        public static ConfigurationOptions GetConfigurationOptions()
        {

            if (string.IsNullOrEmpty(EndPoint) || Port == 0)
            {
                throw new InvalidOperationException("Redis endpoint and port must be set");
            }

            return new ConfigurationOptions()
            {
                ConnectRetry = 5,
                EndPoints =
                {
                    { EndPoint, Port }
                },
                Password = Password,
                Ssl = UseSSL

            };

        }


        public static void ForceReconnect()
        {
            var utcNow = DateTimeOffset.UtcNow;
            var previousTicks = Interlocked.Read(ref _lastReconnectTicks);
            var previousReconnect = new DateTimeOffset(previousTicks, TimeSpan.Zero);
            var elapsedSinceLastReconnect = utcNow - previousReconnect;

            // If mulitple threads call ForceReconnect at the same time, we only want to honor one of them.
            if (elapsedSinceLastReconnect > ReconnectMinFrequency)
            {
                lock (_reconnectLock)
                {
                    utcNow = DateTimeOffset.UtcNow;
                    elapsedSinceLastReconnect = utcNow - previousReconnect;

                    if (_firstError == DateTimeOffset.MinValue)
                    {
                        // We haven't seen an error since last reconnect, so set initial values.
                        _firstError = utcNow;
                        _previousError = utcNow;
                        return;
                    }

                    if (elapsedSinceLastReconnect < ReconnectMinFrequency)
                        return; // Some other thread made it through the check and the lock, so nothing to do.

                    var elapsedSinceFirstError = utcNow - _firstError;
                    var elapsedSinceMostRecentError = utcNow - _previousError;

                    var shouldReconnect =
                        elapsedSinceFirstError >= ReconnectErrorThreshold   // make sure we gave the multiplexer enough time to reconnect on its own if it can
                        && elapsedSinceMostRecentError <= ReconnectErrorThreshold; //make sure we aren't working on stale data (e.g. if there was a gap in errors, don't reconnect yet).

                    // Update the previousError timestamp to be now (e.g. this reconnect request)
                    _previousError = utcNow;

                    if (shouldReconnect)
                    {
                        _firstError = DateTimeOffset.MinValue;
                        _previousError = DateTimeOffset.MinValue;

                        var oldMultiplexer = _multiplexer;
                        CloseMultiplexer(oldMultiplexer);
                        _multiplexer = CreateMultiplexer();
                        Interlocked.Exchange(ref _lastReconnectTicks, utcNow.UtcTicks);
                    }
                }
            }
        }

        private static Lazy<ConnectionMultiplexer> CreateMultiplexer()
        {
            return new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(GetConfigurationOptions()));
        }

        private static void CloseMultiplexer(Lazy<ConnectionMultiplexer> oldMultiplexer)
        {
            if (oldMultiplexer != null)
            {
                try
                {
                    oldMultiplexer.Value.Close();
                }
                catch (Exception)
                {
                    // Example error condition: if accessing old.Value causes a connection attempt and that fails.
                }
            }
        }
    }
}

using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace mars_rover_client
{
    public static class RateLimitWatcher
    {
        public const long RateLimit = 1000;

        public static long RateLimitRemaining { get; private set; } = RateLimit;

        public static bool IsBlocked => RateLimitRemaining == 0;

        private static readonly SemaphoreSlim _rateLimitSemaphore = new SemaphoreSlim(1, 1);

        public static async Task SetFromResponseAsync(HttpResponseMessage response)
        {
            var remainingHeaderVal = response.Headers.GetValues("X-RateLimit-Remaining")?.FirstOrDefault();

            if (string.IsNullOrEmpty(remainingHeaderVal) ||
                !long.TryParse(remainingHeaderVal, out long remaining))
            {
                return;
            }

            await _rateLimitSemaphore.WaitAsync();

            if (remaining > 0)
            {
                RateLimitRemaining = remaining;
            }
            else if (response.IsSuccessStatusCode)
            {
                Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(TimeSpan.FromHours(1));

                    await _rateLimitSemaphore.WaitAsync();

                    RateLimitRemaining = 0;

                    _rateLimitSemaphore.Release();
                }, TaskCreationOptions.LongRunning);
            }

            _rateLimitSemaphore.Release();
        }
    }
}

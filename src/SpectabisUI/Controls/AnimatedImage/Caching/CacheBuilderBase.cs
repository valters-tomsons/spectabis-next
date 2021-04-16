/// This source file is derived from https://github.com/launchdarkly/dotnet-cache/
/// Under the terms of Apache 2.0 License.
using System;

namespace SpectabisUI.Controls.AnimatedImage.Caching
{
    /// <summary>
    /// Basic builder methods common to all caches.
    /// </summary>
    /// <typeparam name="B">the specific builder subclass</typeparam>
    internal class CacheBuilderBase<B> where B : CacheBuilderBase<B>
    {
        internal TimeSpan? Expiration { get; private set; }
        internal TimeSpan? PurgeInterval { get; private set; }
        internal bool? DoSlidingExpiration { get; private set; }

        /// <summary>
        /// <para>
        /// Sets the maximum time (TTL) that any value will be retained in the cache. This time is
        /// counted from the time when the value was last written (added or updated).
        /// </para>
        /// <para>If this is null, values will never expire.</para>
        /// </summary>
        /// <param name="expiration">the expiration time, or null if values should never expire</param>
        /// <returns></returns>
        public B WithExpiration(TimeSpan? expiration)
        {
            Expiration = expiration;
            return (B)this;
        }

        public B WithSlidingExpiration()
        {
            DoSlidingExpiration = true;
            return (B)this;
        }

        /// <summary>
        /// <para>Sets the interval in between automatic purges of expired values.</para>
        /// <para>
        /// If this is not null, then a background task will run at that frequency to sweep the cache for
        /// all expired values.
        /// </para>
        /// <para>If it is null, expired values will be removed only at the time when you try to access them.</para>
        /// <para>This value is ignored if the expiration time (<see cref="WithExpiration(TimeSpan?)"/>) is null.</para>
        /// </summary>
        /// <param name="purgeInterval">the purge interval, or null to turn off automatic purging</param>
        /// <returns></returns>
        public B WithBackgroundPurge(TimeSpan? purgeInterval)
        {
            PurgeInterval = purgeInterval;
            return (B)this;
        }
    }
}

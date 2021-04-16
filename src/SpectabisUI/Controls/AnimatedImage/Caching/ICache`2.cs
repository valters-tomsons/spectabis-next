/// This source file is derived from https://github.com/launchdarkly/dotnet-cache/
/// Under the terms of Apache 2.0 License.
using System;

namespace SpectabisUI.Controls.AnimatedImage.Caching
{
    /// <summary>
    /// Interface for a key-value cache.
    /// </summary>
    /// <typeparam name="K">the key type</typeparam>
    /// <typeparam name="V">the value type</typeparam>
    internal interface ICache<K, V> : IDisposable
    {
        /// <summary>
        /// <para>Returns true if the cache can provide a value for the given key.</para>
        /// <para>
        /// In a read-through cache, this method will always return true, since calling
        /// <see cref="Get(K)"/> will always call the loader function to acquire a value if the
        /// value was not already cached.
        /// </para>
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>true if the key has an associated value</returns>
        bool ContainsKey(K key);

        /// <summary>
        /// <para>Attempts to get the value associated with the given key.</para>
        /// <para>
        /// In a read-through cache, if there is no cached value for the key, the cache will call
        /// the loader function to provide a value; thus, a value is always available.
        /// </para>
        /// <para>
        /// If it is not a read-through cache and no value is available, the cache does not throw
        /// an exception (unlike IDictionary). Instead, it returns the default value for type V
        /// (null, if it is a reference type). Note that any value (including null, for reference
        /// types) can be cached, so if you need to distinguish between the lack of a value and a
        /// default value you must use <see cref="ContainsKey(K)"/> or <see cref="TryGetValue(K, out V)"/>.
        /// </para>
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>the associated value, or <c>default(V)</c></returns>
        V Get(K key);

        /// <summary>
        /// Attempts to get the value associated with the given key. If successful, sets
        /// <c>value</c> to the value and returns true; otherwise, sets <c>value</c>
        /// to <c>default(V)</c> <para>and returns false.</para>
        /// <para>
        /// In a read-through cache, if there is no cached value for the key, the cache will call
        /// the loader function to provide a value; thus, it will always return true.
        /// </para>
        /// <para>
        /// This is the same as calling <see cref="ContainsKey(K)"/> followed by <see cref="Get(K)"/>
        /// except that it is an atomic operation.
        /// </para>
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>true if there is a value</returns>
        bool TryGetValue(K key, out V value);

        /// <summary>
        /// <para>Stores a value associated with the given key.</para>
        /// <para>Note that any value of type V can be cached, including null for reference types.</para>
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="value">the value</param>
        void Set(K key, V value);

        /// <summary>
        /// Removes the value associated with the given key, if any.
        /// </summary>
        /// <param name="key">the key</param>
        void Remove(K key);

        /// <summary>
        /// Removes all cached values.
        /// </summary>
        void Clear();
    }
}

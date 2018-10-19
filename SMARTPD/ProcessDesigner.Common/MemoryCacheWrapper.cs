using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Common
{
    /// <summary>
    /// Memory cache wrapper.
    /// </summary>
    /// <typeparam name="T">Type of cache - for generic set object</typeparam>
    public interface IMemoryCacheWrapper<T>
    {
        /// <summary>
        /// Gets the name of a cache.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets  cache memory limit in bytes.
        /// </summary>
        long CacheMemoryLimitInBytes { get; }

        /// <summary>
        /// Gets physical memory limit.
        /// </summary>
        long PhysicalMemoryLimit { get; }

        /// <summary>
        /// Gets polling interval.
        /// </summary>
        TimeSpan PollingInterval { get; }

        /// <summary>
        /// Gets or sets cache policy.
        /// </summary>
        CacheItemPolicy CacheItemPolicy { get; set; }

        /// <summary>
        /// Gets a value indicating whether the object is disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets a count on number of cache item.
        /// </summary>
        long Count { get; }

        /// <summary>
        /// Add or update the cache.
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache value</param>
        void AddOrUpdate(string key, T value);

        /// <summary>
        /// Try to get the value from the Cache.
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Reference to cache value.</param>
        /// <returns>on success return true.</returns>
        bool TryGetValue(string key, out T value);

        /// <summary>
        /// Try to remove the value from the cache and set cache value in the out object.
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Reference to cache value.</param>
        /// <returns>on success return true.</returns>
        bool TryRemove(string key, out T value);

        /// <summary>
        /// Remove the cache item.
        /// </summary>
        /// <param name="key">cache key</param>
        void Remove(string key);

        /// <summary>
        /// Check if the cache contains a key.
        /// </summary>
        /// <param name="key">Cache key.</param>
        /// <returns>On success return true.</returns>
        bool ContainsKey(string key);

        /// <summary>
        /// Dispose item.
        /// </summary>
        void Dispose();
    }

    /// <summary>
    /// Memory cache wrapper.
    /// </summary>
    /// <typeparam name="T">Type of cache - for generic set object</typeparam>
    public class MemoryCacheWrapper<T> : IMemoryCacheWrapper<T>
    {
        /// <summary>
        /// Memory cache.
        /// </summary>
        private readonly MemoryCache memoryCache;

        /// <summary>
        /// Default cache item policy set to 24 hours.
        /// </summary>
        private CacheItemPolicy cacheItemPolicy;

        /// <summary>
        /// Indicate is disposing.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the MemoryCacheWrapper class.
        /// </summary>
        /// <param name="name">Memory cache name</param>
        /// <param name="config">Configuration object</param>
        public MemoryCacheWrapper(string name, NameValueCollection config = null)
        {
            this.memoryCache = config != null ? new MemoryCache(name, config) : new MemoryCache(name);
            this.isDisposed = false;
            this.CacheItemPolicy = new CacheItemPolicy { SlidingExpiration = new TimeSpan(24, 0, 0) };
        }

        /// <summary>
        /// Finalizes an instance of the MemoryCacheWrapper class.
        /// </summary>
        ~MemoryCacheWrapper()
        {
            // Garbage collection has kicked in tidy up your object.
            this.Dispose(false);
        }

        /// <summary>
        /// Gets the name of a cache.
        /// </summary>
        public string Name
        {
            get { return this.memoryCache.Name; }
        }

        /// <summary>
        /// Gets  cache memory limit in bytes.
        /// </summary>
        public long CacheMemoryLimitInBytes
        {
            get { return this.memoryCache.CacheMemoryLimit; }
        }

        /// <summary>
        /// Gets physical memory limit.
        /// </summary>
        public long PhysicalMemoryLimit
        {
            get { return this.memoryCache.PhysicalMemoryLimit; }
        }

        /// <summary>
        /// Gets polling interval.
        /// </summary>
        public TimeSpan PollingInterval
        {
            get { return this.memoryCache.PollingInterval; }
        }

        /// <summary>
        /// Gets or sets cache policy.
        /// </summary>
        public CacheItemPolicy CacheItemPolicy
        {
            get
            {
                return this.cacheItemPolicy;
            }

            set
            {
                if (value != null)
                {
                    this.cacheItemPolicy = value;
                }
            }
        }

        /// <summary>
        /// Gets a count on number of cache item.
        /// </summary>
        public long Count
        {
            get
            {
                return this.memoryCache.GetCount();
            }
        }

        /// <summary>
        /// Gets a value indicating whether object is disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return this.isDisposed; }
        }

        /// <summary>
        /// Add or update the cache.
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache value</param>
        public void AddOrUpdate(string key, T value)
        {
            this.memoryCache.Set(key, value, CacheItemPolicy);
        }

        /// <summary>
        /// Try to get the value from the Cache.
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Reference to cache value.</param>
        /// <returns>on success return true.</returns>
        public bool TryGetValue(string key, out T value)
        {
            bool result = false;
            value = default(T);

            object item = this.memoryCache.Get(key);
            if (item != null)
            {
                value = (T)item;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Try to remove the value from the cache and set cache value in the out object.
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Reference to cache value.</param>
        /// <returns>on success return true.</returns>
        public bool TryRemove(string key, out T value)
        {
            bool result = false;
            value = default(T);

            object item = this.memoryCache.Remove(key);
            if (item != null)
            {
                result = true;
                value = (T)item;
            }

            return result;
        }

        /// <summary>
        /// Remove the cache item.
        /// </summary>
        /// <param name="key">cache key</param>
        public void Remove(string key)
        {
            this.memoryCache.Remove(key);
        }

        /// <summary>
        /// Check if the cache contains a key.
        /// </summary>
        /// <param name="key">Cache key.</param>
        /// <returns>On success return true.</returns>
        public bool ContainsKey(string key)
        {
            return this.memoryCache.Contains(key);
        }

        /// <summary>
        /// Implement IDisposable.
        /// </summary>
        public void Dispose()
        {
            // Dispose has been called clean up your object and members that
            // are disposable.
            this.Dispose(true);

            // Now Make sure that you don't call the cleanup again via the Finalizer
            // Effectively you are taking over garbage collection so make sure you
            // don't do it again
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose object.
        /// </summary>
        /// <param name="disposing">indicates if it is disposing.</param>
        private void Dispose(bool disposing)
        {
            // Only do this once.
            if (!this.isDisposed)
            {
                // If called via IDispose interface then clean up sub-objects.
                // That implement the IDispose interface.
                if (disposing)
                {
                    // Dispose managed resources.
                    this.memoryCache.Dispose();
                }
            }

            this.isDisposed = true;
        }
    }
}

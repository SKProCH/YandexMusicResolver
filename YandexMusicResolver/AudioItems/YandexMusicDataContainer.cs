using System;
using System.Threading.Tasks;

namespace YandexMusicResolver.AudioItems {
    /// <summary>
    /// Represents class that contains data which could be loaded
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class YandexMusicDataContainer<T> {
        private Func<Task<T>> loadDataFactory;
        private Task<T>? _loadDataTask;

        /// <summary>
        /// Create instance of <see cref="YandexMusicDataContainer{T}"/>
        /// </summary>
        /// <param name="loadDataFactory">Data creation factory</param>
        public YandexMusicDataContainer(Func<Task<T>> loadDataFactory) {
            this.loadDataFactory = loadDataFactory;
        }

        /// <summary>
        /// Create instance of <see cref="YandexMusicDataContainer{T}"/>
        /// </summary>
        /// <param name="data">Target data</param>
        public YandexMusicDataContainer(T data) {
            var task = Task.FromResult(data);
            loadDataFactory = () => task;
            _loadDataTask = task;
        }

        private Task<T> LoadDataTask => _loadDataTask ??= loadDataFactory();

        /// <summary>
        /// Load target data
        /// </summary>
        /// <returns>Task</returns>
        public Task<T> LoadDataAsync() {
            return LoadDataTask;
        }

        /// <summary>
        /// Return true if data already loaded
        /// </summary>
        public bool IsDataLoaded => _loadDataTask?.IsCompleted ?? false;

        /// <summary>
        /// Synchronously wait for data loading
        /// </summary>
        public T Data => LoadDataAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }
}

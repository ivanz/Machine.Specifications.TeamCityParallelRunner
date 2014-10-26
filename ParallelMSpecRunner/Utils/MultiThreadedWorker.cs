using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelMSpecRunner.Utils
{
    public class MultiThreadedWorker<TWorkItem, TWorkResult>
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly BlockingCollection<TWorkItem> _workLoad;
        private readonly uint _threads;
        private readonly Func<TWorkItem, TWorkResult> _worker;
        private int _hasRan;
        private readonly object _lockObject = new object();

        public MultiThreadedWorker(IEnumerable<TWorkItem> workload, 
                                   Func<TWorkItem, TWorkResult> worker, 
                                   uint threads)
        {
            if (workload == null)
                throw new ArgumentNullException("workload", "workload is null.");
            if (worker == null)
                throw new ArgumentNullException("worker", "worker is null.");

            _cancellationTokenSource = new CancellationTokenSource();
            _threads = threads;
            _workLoad = new BlockingCollection<TWorkItem>();
            _worker = worker;

            foreach (TWorkItem item in workload)
                _workLoad.Add(item);

            // This means that when the collection is emtpy and TryTake is called - it will throw.
            // Also means that when the collection is empty TryTake will return false rather than
            // block until an item is available (or timeout)
            _workLoad.CompleteAdding();
        }

        public async Task<IEnumerable<TWorkResult>> Run()
        {
            lock (_lockObject) {
                if (Interlocked.CompareExchange(ref _hasRan, 1, 0) == 1)
                    throw new InvalidOperationException("This worker has already ran.");
            }

            List<Task<List<TWorkResult>>> tasks = new List<Task<List<TWorkResult>>>();

            for (int i = 0; i < _threads; i++) {
                tasks.Add(Task.Run(() => {
                    List<TWorkResult> results = new List<TWorkResult>();

                    while (true) {
                        TWorkItem workItem;
                        try {
                            if (!_workLoad.TryTake(out workItem, Timeout.Infinite, _cancellationTokenSource.Token))
                                break; // no more work items in the collection
                        } catch (OperationCanceledException) {
                            break; // cancellation token triggered
                        }

                        results.Add(_worker(workItem));
                    }

                    return results;
                }));
            }

            List<TWorkResult>[] completedWork;
            try {
                completedWork = await Task.WhenAll(tasks);
            } finally {
                Dispose();
            }

            return completedWork.SelectMany(item => item);
        }

        private void Dispose()
        {
            _workLoad.Dispose();
            _cancellationTokenSource.Dispose();
        }
    }
}

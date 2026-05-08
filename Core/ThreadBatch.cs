using System.Collections.Concurrent;

namespace GalensUnified.CubicGrid.Core;

/// <summary>
/// An always Ready batch of threads that process queued work.
/// Optional <see cref="ThreadPriority"/> at construction.
/// If you want other jobs to have a different priority make another batch with the desired priority.
/// </summary>
/// <remarks>
/// Provides better performance over normal Task.Run,
/// primarily because Task.Run automatically destroys threads causing lag spikes
/// that also means it has to spin up a new threads before working.<br/>
/// A thread is called a worker, see <see cref="WorkerCount"/>.<br/>
/// A Task or enqueued action is a Job, see <see cref="JobCount"/>.<br/>
/// Does not handle job switching. If two jobs are queued that require each other to complete it will cause a deadlock.
/// </remarks>
public class ThreadBatch : IDisposable
{
    /// <summary>Number of threads this batch was instantiated with.</summary>
    public readonly int WorkerCount;
    /// <summary>Number of actions currently in the queue.</summary>
    /// <remarks>This might be 0 but a worker is still processing it.</remarks>
    public int JobCount => jobs.Count;
    public readonly ThreadPriority Priority;

    /// <summary>
    /// The cancellation token for the entire batch.
    /// IsCancellationRequested becomes true when <see cref="Dispose"/> is called.
    /// </summary>
    public readonly CancellationToken GetCancellationToken;
    private readonly CancellationTokenSource CTS;

    private record WorkItem(Action Work, TaskCompletionSource TCS);
    private readonly ConcurrentQueue<WorkItem> jobs = [];
    private readonly SemaphoreSlim signal = new(0);

    /// <summary>Enqueues a job for a worker to process.</summary>
    /// <returns>A task that is completed when the worker finishes this job.</returns>
    /// <remarks>Don't queue up two jobs that require each other to complete.</remarks>
    public Task EnqueueJob(Action work)
    {
        TaskCompletionSource tcs = new();
        jobs.Enqueue(new WorkItem(work, tcs));
        signal.Release();
        return tcs.Task;
    }

    public void Dispose()
    {
        jobs.Clear();
        CTS.Cancel();
    }

    private void TaskConsumer()
    {
        while (!CTS.Token.IsCancellationRequested)
        {
            try { signal.Wait(GetCancellationToken); }
            catch (OperationCanceledException) { break; }

            if (jobs.TryDequeue(out WorkItem? job))
            {
                try
                {
                    job.Work();
                    job.TCS.SetResult();
                }
                catch (Exception e)
                {
                    job.TCS.SetException(e);
                }
            }
        }
    }

    /// <remarks>
    /// Unless your jobs are constantly waiting for other external things to finish
    /// keep <paramref name="count"/> at or bellow
    /// <see cref="Environment.ProcessorCount"/> to minimize context switching.
    /// </remarks>
    public ThreadBatch(int count, ThreadPriority priority = ThreadPriority.Normal)
    {
        WorkerCount = count;
        Priority = priority;
        CTS = new();
        GetCancellationToken = CTS.Token;
        for (int i = 0; i < count; i++)
            new Thread(TaskConsumer)
            {
                Name = $"{priority}Priority_ThreadBatchThread_{i}",
                Priority = priority
            }.Start();
    }
}

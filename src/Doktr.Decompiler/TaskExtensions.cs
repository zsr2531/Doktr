namespace Doktr.Decompiler;

internal static class TaskExtensions
{
    internal static T WaitForResult<T>(this Task<T> task) => task.GetAwaiter().GetResult();
}
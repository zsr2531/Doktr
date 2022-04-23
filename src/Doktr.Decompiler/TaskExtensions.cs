namespace Doktr.Decompiler;

public static class TaskExtensions
{
    public static T WaitForResult<T>(this Task<T> task) => task.GetAwaiter().GetResult();
}
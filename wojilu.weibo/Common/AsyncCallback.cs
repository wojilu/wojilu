namespace wojilu.weibo.Common
{
    /// <summary>
    ///   Represents the delegate of a async call's callback with the specified type of result data.
    /// </summary>
    /// <typeparam name="T"> The type of the result data. </typeparam>
    /// <param name="result"> The result of async call. </param>
    public delegate void AsyncCallback<T>(AsyncCallResult<T> result);

    /// <summary>
    ///   Represents the delegate of a async call's callback.
    /// </summary>
    /// <param name="result"> The result of async call. </param>
    public delegate void VoidAsyncCallback(AsyncCallResult result);
}
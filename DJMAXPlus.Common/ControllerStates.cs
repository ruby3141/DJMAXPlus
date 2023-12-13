namespace DJMAXPlus.Common
{

    /// <summary>
    /// Enum which represent Controller's state.
    /// While "-ing" state, its internal state is locked and some methods can be failed or being blocked.
    /// </summary>
    public enum ControllerStates
    {
        Uninitialized,
        Loading,
        Ready,
        Running,
        Failed,
    }
}

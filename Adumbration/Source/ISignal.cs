namespace Adumbration
{
    /// <summary>
    /// Interface used for grouping and activating of signaled objects
    /// </summary>
    internal interface ISignal
    {
        /// <summary>
        /// Integer signal associated with this object, should only have values from 0-9
        /// </summary>
        public int SignalNum { get; }
    }
}

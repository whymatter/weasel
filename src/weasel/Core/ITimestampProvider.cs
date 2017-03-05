namespace weasel.Core {
    internal interface ITimestampProvider {
        /// <summary>
        ///     Creates a new timestamp with the current time.
        ///     The UTC time is used.
        /// </summary>
        /// <returns></returns>
        string GetTimestampFromNow();
    }
}
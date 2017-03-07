using System;
using weasel.Internal.Core;

namespace weasel.Internal {
    /// <summary>
    ///     This class creates timestamps.
    /// </summary>
    internal class TimestampProvider : ITimestampProvider {
        /// <summary>
        ///     Creates a new timestamp with the current time.
        ///     The UTC time is used.
        /// </summary>
        /// <returns></returns>
        public string GetTimestampFromNow() {
            return Math.Abs(DateTime.UtcNow.ToBinary()).ToString();
        }
    }
}
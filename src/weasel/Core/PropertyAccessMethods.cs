namespace weasel.Core {
    /// <summary>
    ///     Defines the ways a property can be accessed
    /// </summary>
    internal enum PropertyAccessMethods {
        /// <summary>
        ///     Get the value from a property
        /// </summary>
        Get,

        /// <summary>
        ///     Sets the value of a property
        /// </summary>
        Set,

        /// <summary>
        ///     Defines both, get and set methods
        /// </summary>
        Global
    }
}
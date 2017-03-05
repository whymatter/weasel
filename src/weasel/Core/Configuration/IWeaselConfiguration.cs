namespace weasel.Core.Configuration {
    /// <summary>
    ///     Configuration Settings Template
    /// </summary>
    public interface IWeaselConfiguration {
        /// <summary>
        ///     Configure if the methods of the base Classes (the base.CallMe()) should be covered by the proxy
        /// </summary>
        bool OverrideBaseClasses { get; }

        /// <summary>
        ///     Configure if the property getter should be covered by the proxy
        /// </summary>
        bool OverridePropertyGet { get; }

        /// <summary>
        ///     Configure if the property setter should be covered by the proxy
        /// </summary>
        bool OverridePropertySet { get; }
    }
}
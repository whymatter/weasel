namespace weasel.Core.Configuration {
    /// <summary>
    ///     This Configuration can be used if all properties should be false
    /// </summary>
    public class DefaultConfiguration : IWeaselConfiguration {
        public bool OverrideBaseClasses {
            get { return false; }
        }

        public bool OverridePropertyGet {
            get { return false; }
        }

        public bool OverridePropertySet {
            get { return false; }
        }
    }
}
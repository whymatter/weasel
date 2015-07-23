namespace weasel.Core.Configuration {
    /// <summary>
    ///     This Configuration can be uses if all properties should be false
    /// </summary>
    public class DefaultConfiguration : IWeaselConfiguration {
        public bool OverrideNonVirtuals {
            get { return false; }
        }

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
﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PortalForgeX.Shared.Features.Checkouts {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class CheckoutResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CheckoutResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PortalForgeX.Shared.Features.Checkouts.CheckoutResources", typeof(CheckoutResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Device Code can have max 50 chars..
        /// </summary>
        public static string DeviceCode_MaximumLength {
            get {
                return ResourceManager.GetString("DeviceCode_MaximumLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Device Code is a required field..
        /// </summary>
        public static string DeviceCode_NotEmpty {
            get {
                return ResourceManager.GetString("DeviceCode_NotEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Remarks can have max 2000 chars..
        /// </summary>
        public static string Remarks_MaximumLength {
            get {
                return ResourceManager.GetString("Remarks_MaximumLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Software Version can have max 10 chars..
        /// </summary>
        public static string SoftwareVersion_MaximumLength {
            get {
                return ResourceManager.GetString("SoftwareVersion_MaximumLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Software Version is a required field..
        /// </summary>
        public static string SoftwareVersion_NotEmpty {
            get {
                return ResourceManager.GetString("SoftwareVersion_NotEmpty", resourceCulture);
            }
        }
    }
}

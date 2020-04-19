namespace AssistanceRequestApp.Common
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;

    /// <summary>
    /// Defines the <see cref="AppUtility" />.
    /// </summary>
    public static class AppUtility
    {
        /// <summary>
        /// The GetAppSettingsValue.
        /// </summary>
        /// <param name="appSettingId">The appSettingId<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetAppSettingsValue(string appSettingId)
        {
            if (!string.IsNullOrWhiteSpace(appSettingId))
            {
                return Convert.ToString(ConfigurationManager.AppSettings[appSettingId]);
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// The GetFileName.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetFileName()
        {
            return DateTime.Now.ToString("yyyyMMddhhmmss");
        }

        /// <summary>
        /// Defines the <see cref="CurrentDateAttribute" />.
        /// </summary>
        public class CurrentDateAttribute : ValidationAttribute
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CurrentDateAttribute"/> class.
            /// </summary>
            public CurrentDateAttribute()
            {
            }

            /// <summary>
            /// The IsValid.
            /// </summary>
            /// <param name="value">The value<see cref="object"/>.</param>
            /// <returns>The <see cref="bool"/>.</returns>
            public override bool IsValid(object value)
            {
                if (value != null)
                {
                    var dt = (DateTime)value;
                    if (dt >= DateTime.Now.Date)
                    {
                        return true;
                    }
                    return false;
                }
                return true;
            }
        }
    }
}

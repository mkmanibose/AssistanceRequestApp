namespace AssistanceRequestApp.Common
{
    /// <summary>
    /// Defines the <see cref="CommonEnum" />.
    /// </summary>
    public class CommonEnum
    {
        /// <summary>
        /// Defines the Status.
        /// </summary>
        public enum Status
        {
            /// <summary>
            /// Defines the Reported.
            /// </summary>
            Reported = 1,

            /// <summary>
            /// Defines the Open.
            /// </summary>
            Open = 1,

            /// <summary>
            /// Defines the InProgress.
            /// </summary>
            InProgress = 2,

            /// <summary>
            /// Defines the Closed.
            /// </summary>
            Closed = 3,

            /// <summary>
            /// Defines the Withdrawn.
            /// </summary>
            Withdrawn = 4,

            /// <summary>
            /// Defines the Rejected.
            /// </summary>
            Rejected = 5
        }
    }
}

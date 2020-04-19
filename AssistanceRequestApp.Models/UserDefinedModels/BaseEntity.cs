namespace AssistanceRequestApp.Models.UserDefinedModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="BaseEntity" />.
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Display(Name = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDate.
        /// </summary>
        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedDate.
        /// </summary>
        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the IPAddress.
        /// </summary>
        [Display(Name = "IP Address")]
        [StringLength(100)]
        public string IPAddress { get; set; }
    }
}

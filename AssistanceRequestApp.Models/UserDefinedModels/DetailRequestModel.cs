namespace AssistanceRequestApp.Models.UserDefinedModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="DetailRequestModel" />.
    /// </summary>
    public class DetailRequestModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the EmployeeId.
        /// </summary>
        [Display(Name = "Employee Id")]
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the EmailId.
        /// </summary>
        [Display(Name = "Email Id")]
        public string EmailId { get; set; }

        /// <summary>
        /// Gets or sets the Phone.
        /// </summary>
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the ApplicationName.
        /// </summary>
        [Display(Name = "Application Name")]
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the NatureofRequest.
        /// </summary>
        [Display(Name = "Nature Of Request")]
        public string NatureofRequest { get; set; }

        /// <summary>
        /// Gets or sets the RelatedEnvironment.
        /// </summary>
        [Display(Name = "Related Environment")]
        public string RelatedEnvironment { get; set; }

        /// <summary>
        /// Gets or sets the DescriptionofRequest.
        /// </summary>
        [Display(Name = "Description of Request")]
        public string DescriptionofRequest { get; set; }

        /// <summary>
        /// Gets or sets the AssignedTo.
        /// </summary>
        [Display(Name = "Assigned To")]
        public string AssignedTo { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        [Display(Name = "Status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the DateAssigned.
        /// </summary>
        [Display(Name = "Date Assigned")]
        public DateTime? DateAssigned { get; set; }

        /// <summary>
        /// Gets or sets the EstimatedHours.
        /// </summary>
        [Display(Name = "Estimated Hours")]
        public int? EstimatedHours { get; set; }

        /// <summary>
        /// Gets or sets the DateCompleted.
        /// </summary>
        [Display(Name = "Date Completed")]
        public DateTime? DateCompleted { get; set; }

        /// <summary>
        /// Gets or sets the ActualHours.
        /// </summary>
        [Display(Name = "Actual Hours")]
        public int? ActualHours { get; set; }

        /// <summary>
        /// Gets or sets the ResolutionComments.
        /// </summary>
        [Display(Name = "Resolution Comments")]
        public string ResolutionComments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsActive.
        /// </summary>
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}

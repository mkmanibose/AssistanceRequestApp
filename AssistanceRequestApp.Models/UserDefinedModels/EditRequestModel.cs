namespace AssistanceRequestApp.Models.UserDefinedModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static AssistanceRequestApp.Common.AppUtility;

    /// <summary>
    /// Defines the <see cref="EditRequestModel" />.
    /// </summary>
    public class EditRequestModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [Required(ErrorMessage = "Enter Name")]
        [Display(Name = "Name")]
        [StringLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the EmployeeId.
        /// </summary>
        [Required(ErrorMessage = "Enter Employee Id")]
        [Display(Name = "Employee Id")]
        [StringLength(100)]
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the EmailId.
        /// </summary>
        [Required(ErrorMessage = "Enter Email Id")]
        [Display(Name = "Email Id")]
        [StringLength(100)]
        public string EmailId { get; set; }

        /// <summary>
        /// Gets or sets the Phone.
        /// </summary>
        [Display(Name = "Phone")]
        [StringLength(100)]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the ApplicationName.
        /// </summary>
        [Required(ErrorMessage = "Enter Application Name")]
        [Display(Name = "Application Name")]
        [StringLength(100)]
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the NatureofRequest.
        /// </summary>
        [Required(ErrorMessage = "Select Nature Of Request")]
        [Display(Name = "Nature Of Request")]
        [StringLength(200)]
        public string NatureofRequest { get; set; }

        /// <summary>
        /// Gets or sets the RelatedEnvironment.
        /// </summary>
        [Required(ErrorMessage = "Select Related Environment")]
        [Display(Name = "Related Environment")]
        [StringLength(200)]
        public string RelatedEnvironment { get; set; }

        /// <summary>
        /// Gets or sets the DescriptionofRequest.
        /// </summary>
        [Required(ErrorMessage = "Enter Description of Request")]
        [Display(Name = "Description of Request")]
        public string DescriptionofRequest { get; set; }

        /// <summary>
        /// Gets or sets the AssignedTo.
        /// </summary>
        [Required(ErrorMessage = "Enter Assigned To")]
        [Display(Name = "Assigned To")]
        [StringLength(100)]
        public string AssignedTo { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        [Required(ErrorMessage = "Select Status")]
        [Display(Name = "Status")]
        [StringLength(100)]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the DateAssigned.
        /// </summary>
        [Display(Name = "Date Assigned")]
        [DataType(DataType.Date)]
        [CurrentDate(ErrorMessage = "Date Assigned must be after or equal to current date")]
        public DateTime? DateAssigned { get; set; }

        /// <summary>
        /// Gets or sets the EstimatedHours.
        /// </summary>
        [Display(Name = "Estimated Hours")]
        public int? EstimatedHours { get; set; }

        /// <summary>
        /// Gets or sets the DateCompleted.
        /// </summary>
        [Required(ErrorMessage = "Select Date Completed")]
        [Display(Name = "Date Completed")]
        [DataType(DataType.Date)]
        [CurrentDate(ErrorMessage = "Date Completed must be after or equal to current date")]
        public DateTime? DateCompleted { get; set; }

        /// <summary>
        /// Gets or sets the ActualHours.
        /// </summary>
        [Display(Name = "Actual Hours")]
        public int? ActualHours { get; set; }

        /// <summary>
        /// Gets or sets the ResolutionComments.
        /// </summary>
        [Required(ErrorMessage = "Enter Resolution Comments")]
        [Display(Name = "Resolution Comments")]
        public string ResolutionComments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsActive.
        /// </summary>
        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }
    }
}

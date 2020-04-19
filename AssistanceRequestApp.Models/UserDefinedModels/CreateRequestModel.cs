namespace AssistanceRequestApp.Models.UserDefinedModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="CreateRequestModel" />.
    /// </summary>
    public class CreateRequestModel : BaseEntity
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
        [Required(ErrorMessage = "Select Nature of Request")]
        [Display(Name = "Nature Of Request")]
        public string NatureofRequest { get; set; }

        /// <summary>
        /// Gets or sets the RelatedEnvironment.
        /// </summary>
        [Required(ErrorMessage = "Select Related Environment")]
        [Display(Name = "Related Environment")]
        public string RelatedEnvironment { get; set; }

        /// <summary>
        /// Gets or sets the DescriptionofRequest.
        /// </summary>
        [Required(ErrorMessage = "Enter Description of Request")]
        [Display(Name = "Description of Request")]
        public string DescriptionofRequest { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        [Display(Name = "Status")]
        [StringLength(100)]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsActive.
        /// </summary>
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}

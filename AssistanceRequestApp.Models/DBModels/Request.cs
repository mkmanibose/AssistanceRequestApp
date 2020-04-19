namespace AssistanceRequestApp.Models.DBModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Defines the <see cref="Request" />.
    /// </summary>
    [Table("Request", Schema = "dbo")]
    public class Request : BaseEntity
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [Required]
        [Display(Name = "Name")]
        [Column(TypeName = "varchar(200)")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the EmployeeId.
        /// </summary>
        [Required]
        [Display(Name = "Employee Id")]
        [Column(TypeName = "varchar(100)")]
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the EmailId.
        /// </summary>
        [Required]
        [Display(Name = "Email Id")]
        [Column(TypeName = "varchar(100)")]
        public string EmailId { get; set; }

        /// <summary>
        /// Gets or sets the Phone.
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [Display(Name = "Phone")]
        [Column(TypeName = "varchar(100)")]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the ApplicationName.
        /// </summary>
        [Required]
        [Display(Name = "Application Name")]
        [Column(TypeName = "varchar(200)")]
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the NatureofRequest.
        /// </summary>
        [Required]
        [Display(Name = "Nature Of Request")]
        [Column(TypeName = "varchar(200)")]
        public string NatureofRequest { get; set; }

        /// <summary>
        /// Gets or sets the RelatedEnvironment.
        /// </summary>
        [Required]
        [Display(Name = "Related Environment")]
        [Column(TypeName = "varchar(200)")]
        public string RelatedEnvironment { get; set; }

        /// <summary>
        /// Gets or sets the DescriptionofRequest.
        /// </summary>
        [Required]
        [Display(Name = "Description of Request")]
        [Column(TypeName = "nvarchar(MAX)")]
        public string DescriptionofRequest { get; set; }

        /// <summary>
        /// Gets or sets the AssignedTo.
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [Display(Name = "Assigned To")]
        [Column(TypeName = "varchar(100)")]
        public string AssignedTo { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [Display(Name = "Status")]
        [Column(TypeName = "varchar(100)")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the DateAssigned.
        /// </summary>
        [Display(Name = "Date Assigned")]
        [Column(TypeName = "datetime")]
        public DateTime? DateAssigned { get; set; }

        /// <summary>
        /// Gets or sets the EstimatedHours.
        /// </summary>
        [Display(Name = "Estimated Hours")]
        [Column(TypeName = "int")]
        public int? EstimatedHours { get; set; }

        /// <summary>
        /// Gets or sets the DateCompleted.
        /// </summary>
        [Display(Name = "Date Completed")]
        [Column(TypeName = "datetime")]
        public DateTime? DateCompleted { get; set; }

        /// <summary>
        /// Gets or sets the ActualHours.
        /// </summary>
        [Display(Name = "Actual Hours")]
        [Column(TypeName = "int")]
        public int? ActualHours { get; set; }

        /// <summary>
        /// Gets or sets the ResolutionComments.
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [Display(Name = "Resolution Comments")]
        [Column(TypeName = "nvarchar(MAX)")]
        public string ResolutionComments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsActive.
        /// </summary>
        [Display(Name = "IsActive")]
        [Column(TypeName = "bit")]
        public bool IsActive { get; set; }
    }
}

namespace AssistanceRequestApp.Models.DBModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Defines the <see cref="BaseEntity" />.
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [Column(TypeName = "int")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDate.
        /// </summary>
        [Display(Name = "Created Date")]
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedDate.
        /// </summary>
        [Display(Name = "Modified Date")]
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the IPAddress.
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [Display(Name = "IP Address")]
        [Column(TypeName = "varchar(100)")]
        public string IPAddress { get; set; }
    }
}

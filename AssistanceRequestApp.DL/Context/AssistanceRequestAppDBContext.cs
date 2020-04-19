namespace AssistanceRequestApp.DL.Context
{
    using AssistanceRequestApp.Models.DBModels;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Defines the <see cref="AssistanceRequestAppDBContext" />.
    /// </summary>
    public class AssistanceRequestAppDBContext : DbContext
    {
        //public AssistanceRequestAppDBContext() : base()
        //{
        //    //CreateDatabaseIfNotExists
        //}
        /// <summary>
        /// Initializes a new instance of the <see cref="AssistanceRequestAppDBContext"/> class.
        /// </summary>
        /// <param name="options">The options<see cref="DbContextOptions{AssistanceRequestAppDBContext}"/>.</param>
        public AssistanceRequestAppDBContext(DbContextOptions<AssistanceRequestAppDBContext> options) : base(options)
        {
        }

        /// <summary>
        /// The OnConfiguring.
        /// </summary>
        /// <param name="optionsBuilder">The optionsBuilder<see cref="DbContextOptionsBuilder"/>.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// The OnModelCreating.
        /// </summary>
        /// <param name="modelBuilder">The modelBuilder<see cref="ModelBuilder"/>.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Request>().Property(r => r.Phone).IsRequired(false);
            modelBuilder.Entity<Request>().Property(r => r.AssignedTo).IsRequired(false);
            modelBuilder.Entity<Request>().Property(r => r.ResolutionComments).IsRequired(false);
            modelBuilder.Entity<Request>().Property(r => r.IPAddress).IsRequired(false);
            modelBuilder.Entity<Request>().ToTable("Request");
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Gets or sets the Requests.
        /// </summary>
        public DbSet<Request> Requests { get; set; }
    }
}

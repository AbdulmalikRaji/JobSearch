﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseLayer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class jobsearchDBEntities : DbContext
    {
        public jobsearchDBEntities()
            : base("name=jobsearchDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AccountStatusTable> AccountStatusTables { get; set; }
        public virtual DbSet<CertificateTable> CertificateTables { get; set; }
        public virtual DbSet<CompanyTable> CompanyTables { get; set; }
        public virtual DbSet<CountryTable> CountryTables { get; set; }
        public virtual DbSet<CurrentJobStatusTable> CurrentJobStatusTables { get; set; }
        public virtual DbSet<EducationTable> EducationTables { get; set; }
        public virtual DbSet<EmployeeTable> EmployeeTables { get; set; }
        public virtual DbSet<EventTable> EventTables { get; set; }
        public virtual DbSet<JobApplyStatusTable> JobApplyStatusTables { get; set; }
        public virtual DbSet<JobApplyTable> JobApplyTables { get; set; }
        public virtual DbSet<JobCategoryTable> JobCategoryTables { get; set; }
        public virtual DbSet<JobStatusTable> JobStatusTables { get; set; }
        public virtual DbSet<JobTable> JobTables { get; set; }
        public virtual DbSet<LanguageTable> LanguageTables { get; set; }
        public virtual DbSet<PostJobTable> PostJobTables { get; set; }
        public virtual DbSet<SkillTable> SkillTables { get; set; }
        public virtual DbSet<UserTable> UserTables { get; set; }
        public virtual DbSet<UserTypeTable> UserTypeTables { get; set; }
        public virtual DbSet<WorkExperienceTable> WorkExperienceTables { get; set; }
    }
}

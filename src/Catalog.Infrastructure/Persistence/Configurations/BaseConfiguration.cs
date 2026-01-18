using Catalog.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations
{
    public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : EntityBase
    {
        #region Members

        private const string DEFAULT_DATABASE_SCHEMA = "dbo";

        protected const string INTEGER_COLUMN_TYPE = "int";
        protected const string DECIMAL_COLUMN_TYPE = "decimal";
        protected const string DECIMAL_LIMIT_COLUMN_TYPE = "decimal({0},{1})";
        protected const string SMALLINT_COLUMN_TYPE = "smallint";
        protected const string TINYINT_COLUMN_TYPE = "tinyint";
        protected const string VARCHAR_COLUMN_TYPE = "varchar";
        protected const string VARCHAR_LIMIT_COLUMN_TYPE = "varchar({0})";
        protected const string VARCHAR_MAX_COLUMN_TYPE = "varchar(max)";
        protected const string NVARCHAR_MAX_COLUMN_TYPE = "nvarchar(max)";
        protected const string NVARCHAR_COLUMN_TYPE = "nvarchar";
        protected const string NCHAR_COLUMN_TYPE = "nchar";
        protected const string CHAR_COLUMN_TYPE = "char";
        protected const string BIT_COLUMN_TYPE = "bit";
        protected const string DATETIME_COLUMN_TYPE = "datetime";
        protected const string UNIQUEIDENTIFIER_COLUMN_TYPE = "uniqueidentifier";

        #endregion

        #region Constructors
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            if (DefaultConfiguration)
            {
                var entityName = typeof(TEntity).Name;
                var tableName = entityName.StartsWith("tb") ? entityName : $"tb{entityName}";

                builder.ToTable(tableName, DEFAULT_DATABASE_SCHEMA);
                builder.HasKey(e => e.Id);
                builder.Property(e => e.Id).HasColumnType(INTEGER_COLUMN_TYPE).IsRequired().UseIdentityColumn();
                builder.Property(e => e.CreatedAt).HasColumnType(DATETIME_COLUMN_TYPE).IsRequired();
                builder.Property(e => e.UpdatedAt).HasColumnType(DATETIME_COLUMN_TYPE).IsRequired();
            }

            ConfigureEntity(builder);
        }

        #endregion

        #region Properties
        protected virtual bool DefaultConfiguration => true;

        #endregion

        #region Methods

        protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);

        #endregion
    }
}

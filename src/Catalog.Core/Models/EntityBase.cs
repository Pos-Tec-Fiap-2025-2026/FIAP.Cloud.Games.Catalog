namespace Catalog.Core.Models
{
    public abstract class EntityBase
    {
        #region Constructor

        public EntityBase() => CreatedAt = UpdatedAt = DateTime.Now;

        #endregion

        #region Properties

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        #endregion
    }
}

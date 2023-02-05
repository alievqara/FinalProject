using System;

namespace FinalProject.Entities.Base
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletorName { get; set; }
        public DateTime? DeletedTime { get; set; }
        public string? CreatorName { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string? EditorName { get; set; }
        public DateTime? EditedTime { get; set; }
    }
}

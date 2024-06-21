﻿namespace DAL.Entities.Interfaces
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public void Undo()
        {
            IsDeleted = false;
            DeletedAt = null;
        }
    }
}

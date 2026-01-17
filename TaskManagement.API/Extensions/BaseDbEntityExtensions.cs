using TaskManagement.API.Entities;
using TaskManagement.API.Enums;

namespace TaskManagement.API.Extensions;

public static class BaseDbEntityExtensions
{
    public static T SetMetaData<T>(this T obj, Guid currentUserId, long currentTimestamp, EntityState state =  EntityState.Added) where T : BaseEntity
    {
        obj.LastUpdatedOn = currentTimestamp;
        obj.LastUpdatedBy = currentUserId;
        
        if(state == EntityState.Added)
        {
            obj.CreatedOn = currentTimestamp;
            obj.CreatedBy = currentUserId;
        }

        return obj;
    }

    public static IQueryable<T> ById<T>(this IQueryable<T> query, Guid id) where T : BaseEntity
    {
        return query.Where(entity => entity.Id == id);
    }
}
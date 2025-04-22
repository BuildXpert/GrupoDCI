namespace Build_Xpert.Repository.Base
{
    /// <summary>
    /// Interface for basic repository operations.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface IRepositoryBase<T, TKey>
    {
        /// <summary>
        /// Creates a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
        Task<bool> CreateAsync(T entity);

        /// <summary>
        /// Deletes an existing entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        /// Reads all entities of type T asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities.</returns>
        Task<IEnumerable<T>> ReadAsync();

        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// Updates multiple entities asynchronously.
        /// </summary>
        /// <param name="entities">The collection of entities to be updated.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
        Task<bool> UpdateManyAsync(IEnumerable<T> entities);

        /// <summary>
        /// Checks if an entity exists asynchronously.
        /// </summary>
        /// <param name="entity">The entity to check for existence.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating if the entity exists.</returns>
        Task<bool> BExistsAsync(T entity);

        IQueryable<T> ReadQueriableAsync();

        Task<T> ReadOneAsync(TKey id);
    }
}
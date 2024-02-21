using TestProj.Data.Models;
using TestProj.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using TestProj.Exceptions;

namespace TestProj.Data.Services
{
	public class EntityService
    {
        private readonly AppDbContext _context;

        public EntityService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddEntityAsync(EntityVM entity, CancellationToken cancellationToken = default)
        {
            if (!await ClassifierExistsAsync(entity.TypeGuid, cancellationToken))
            {
                throw new GuidNotFoundException($"Classifier with id: {entity.TypeGuid} not found.");
            }

            var _entity = new Entity
            {
                Title = entity.Title,
                Description = entity.Description,
                TypeGuid = entity.TypeGuid,
            };

            _context.Entities.Add(_entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Entity>> GetAllEntitiesAsync(CancellationToken cancellationToken = default)
            => await _context.Entities.ToListAsync(cancellationToken);

        public async Task<Entity> GetEntityByIdAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var _entity = await _context.Entities.FirstOrDefaultAsync(n => n.Guid == guid, cancellationToken);

            if (_entity == null)
            {
                throw new GuidNotFoundException($"Entity with id: {guid} not found");
            }
            return _entity;
        }
        public async Task<Entity> UpdateEntityByIdAsync(Guid entityId, EntityVM entity, CancellationToken cancellationToken = default)
        {
            if (!await EntityExistsAsync(entityId, cancellationToken))
            {
                throw new GuidNotFoundException($"Entity with id: {entityId} not found");
            }

            if (!await ClassifierExistsAsync(entity.TypeGuid, cancellationToken))
            {
                throw new GuidNotFoundException($"Classifier with id: {entity.TypeGuid} not found.");
            }

            var _entity = await _context.Entities.FirstOrDefaultAsync(c => c.Guid == entityId, cancellationToken);

            _entity.Title = entity.Title;
            _entity.Description = entity.Description;
            _entity.TypeGuid = entity.TypeGuid;
            await _context.SaveChangesAsync(cancellationToken);
            return _entity;
        }

        public async Task DeleteEntityByIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            if (!await EntityExistsAsync(entityId, cancellationToken))
            {
                throw new GuidNotFoundException($"Entity with id: {entityId} not found");
            }

            var _entity = await _context.Entities.FirstOrDefaultAsync(n => n.Guid == entityId, cancellationToken);
            _context.Entities.Remove(_entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task<bool> EntityExistsAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await _context.Entities.AnyAsync(n => n.Guid == entityId, cancellationToken);
        }

        private async Task<bool> ClassifierExistsAsync(Guid typeGuid, CancellationToken cancellationToken = default)
        {
            return await _context.Classifiers.AnyAsync(c => c.Guid == typeGuid, cancellationToken);
        }
    }
}
using TestProj.Data.Models;
using TestProj.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using TestProj.Exceptions;

namespace TestProj.Data.Services
{
	public class ClassifierService
    {
        private readonly AppDbContext _context;

        public ClassifierService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddClassifierAsync(ClassifierVM classifier, CancellationToken cancellationToken = default)
        {
            var newClassifier = new Classifier
            {
                Title = classifier.Title,
            };

            _context.Classifiers.Add(newClassifier);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Classifier>> GetAllClassifiersAsync(CancellationToken cancellationToken = default)
            => await _context.Classifiers.ToListAsync(cancellationToken);

        public async Task<Classifier> GetClassifierByIdAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var classifier = await _context.Classifiers
                .SingleOrDefaultAsync(c => c.Guid == guid, cancellationToken);

            if (classifier is null)
            {
                throw new GuidNotFoundException($"Classifier with id: {guid} not found");
            }

            return classifier;
        }

        public async Task<Classifier> UpdateClassifierByIdAsync(Guid classifierId, ClassifierVM classifier, CancellationToken cancellationToken = default)
        {
            var existingClassifier = await _context.Classifiers
                .SingleOrDefaultAsync(c => c.Guid == classifierId, cancellationToken);

            if (existingClassifier is null)
            {
                throw new GuidNotFoundException($"Classifier with id: {classifierId} not found.");
            }

            existingClassifier.Title = classifier.Title;
            await _context.SaveChangesAsync(cancellationToken);

            return existingClassifier;
        }

        public async Task DeleteClassifierByIdAsync(Guid classifierId, CancellationToken cancellationToken = default)
        {
            var classifierToDelete = await _context.Classifiers
                .SingleOrDefaultAsync(n => n.Guid == classifierId, cancellationToken);

            if (classifierToDelete is null)
            {
                throw new GuidNotFoundException($"Classifier with id: {classifierId} not found.");
            }

            _context.Classifiers.Remove(classifierToDelete);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
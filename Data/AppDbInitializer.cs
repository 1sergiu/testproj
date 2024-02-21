using TestProj.Data.Models;

namespace TestProj.Data
{
	public static class AppDbInitializer
	{
		public static async Task SeedAsync(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

				if (!context.Classifiers.Any() && !context.Entities.Any())
				{
					var classifier1 = new Classifier { Guid = Guid.NewGuid(), Title = "imobil" };
					var classifier2 = new Classifier { Guid = Guid.NewGuid(), Title = "transport" };

					var entities = new[]
					{
						new Entity
						{
							Guid = Guid.NewGuid(),
							Title = "Apartament, cu 3 camere",
							TypeGuid = classifier1.Guid,
							Description = "mun.Chisinau, sec.Centru, bloc nou, etajul 3",
							Classifier = classifier1
						},
						new Entity
						{
							Guid = Guid.NewGuid(),
							Title = "Casa de locuit",
							TypeGuid = classifier1.Guid,
							Description = "mun.Chisinau, sat.Truseni, 2 nivele",
							Classifier = classifier1
						},
						new Entity
						{
							Guid = Guid.NewGuid(),
							Title = "Toyota RAV4",
							TypeGuid = classifier2.Guid,
							Description = "anul:2019, combustibil:motorina, locuri:5",
							Classifier = classifier2
						}
					};

					context.Classifiers.AddRange(classifier1, classifier2);
					context.Entities.AddRange(entities);
					await context.SaveChangesAsync();
				}
			}
		}
	}
}

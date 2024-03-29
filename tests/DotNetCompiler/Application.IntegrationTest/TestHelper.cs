﻿using Core.Exceptions;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTest
{
    [SetUpFixture]
    public class TestHelper
    {
        private static IServiceScopeFactory _scopeFactory;
        [OneTimeSetUp]
        public void RunApplication()
        {
            var factory = new CustomWebApplicationFactory();
            _scopeFactory = factory.Services.CreateScope().ServiceProvider.GetRequiredService<IServiceScopeFactory>();
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            return await mediator.Send(request);
        }

        public static async Task AddAsync<TEntity>(TEntity entity) where TEntity : IDbEntity
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Add(entity);

            await context.SaveChangesAsync();
        }
        public static bool IsErrorExists(string propertyName, string errorMessage, ValidationFailureException validationError)
        {
            return validationError.ErrorResponse.Errors.Any(s => s.PropertyName == propertyName && s.ErrorMessage == errorMessage);
        }
    }
}

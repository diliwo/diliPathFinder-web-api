﻿using System.Reflection;
using DiliBeneficiary.Application.Beneficiaries.Commands.SociabiliDBChangeBeneficiaryMessage.Config;
using DiliBeneficiary.Application.Common.Behaviours;
using DiliBeneficiary.Application.Common.Helpers;
using DiliBeneficiary.Application.Common.Interfaces;
using DiliBeneficiary.Application.Common.Services;
using DiliBeneficiary.Application.Configuration;
using DiliBeneficiary.Application.SchoolRegistations.Common;
using DiliBeneficiary.Application.Supports.Commands.SendReferentChangedNotification.Messaging.Config;
using DiliBeneficiary.Core.Common.Dto;
using DiliBeneficiary.Core.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiliBeneficiary.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddTransient<IDocumentGeneratorService, DocumentGeneratorService>();
            services.AddScoped<ISortHelper<MyConsultantSupportDto>, SortHelper<MyConsultantSupportDto>>();
            services.AddScoped<ISortHelper<SchoolRegistrationDto>, SortHelper<SchoolRegistrationDto>>();

            return services;
        }

        public static IServiceCollection RegisterFluidProvider(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            services.Configure<FluidServiceConfiguration>(configurationSection.Bind);
            services.AddSingleton<IConverter>(sp => new SynchronizedConverter(new PdfTools()));
            return services;
        }

        public static IServiceCollection AddRabbitMqService(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
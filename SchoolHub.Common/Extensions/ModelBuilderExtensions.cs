using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Common.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Entities<T>(this ModelBuilder builder, DbContext instance, string methodName)
        {
            var method = instance.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            var typesStatus = builder.Model.GetEntityTypes().Where(type => typeof(T).IsAssignableFrom(type.ClrType));
            foreach (var type in typesStatus)
            {
                var builderType = typeof(EntityTypeBuilder<>).MakeGenericType(type.ClrType);
                var buildMethod = method.MakeGenericMethod(type.ClrType);
                var buildAction = typeof(Action<>).MakeGenericType(builderType);
                var buildDelegate = Delegate.CreateDelegate(buildAction, instance, buildMethod);
                var buildEntity = typeof(ModelBuilder).GetMethods()
                    .Single(m => m.Name == "Entity" && m.GetGenericArguments().Any() && m.GetParameters().Any())
                    .MakeGenericMethod(type.ClrType);

                buildEntity.Invoke(builder, new[] { buildDelegate });
            }
        }
    }
}

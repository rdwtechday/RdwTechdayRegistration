using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace RdwTechdayRegistration.Utility
{
    // this class provides a way to get a smaller scope service from within a larger scope service
    // i.e. get transient within a singleton
    // see http://morgwai.pl/articles/aspdi.html
    public interface IProvider<T>
    {
        T Get();
    }

    public class Provider<T> : IProvider<T>
    {

        IHttpContextAccessor contextAccessor;

        public Provider(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        T IProvider<T>.Get()
        {
            return contextAccessor.HttpContext.RequestServices.GetService<T>();
        }
    }
}
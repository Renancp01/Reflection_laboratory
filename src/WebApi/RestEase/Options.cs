//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Text;
//using Microsoft.Extensions.DependencyInjection;
//using WebApi.RestEase;

//namespace RestEase.HttpClientFactory
//{
//    /// <summary>
//    /// Additional options which can be passed to
//    /// <see cref="WebApi.RestEase.HttpClientFactoryExtensions.AddRestEaseClient(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Type,RestEase.HttpClientFactory.AddRestEaseClientOptions)"/>
//    /// and similar overloads
//    /// </summary>
//    public class AddRestEaseClientOptions
//    {
//        /// <summary>
//        /// Optional delegate to configure the <see cref="RestClient"/> (to set serializers, etc)
//        /// </summary>
//        public Action<RestClient>? RestClientConfigurer { get; set; }

//        /// <summary>
//        /// Optional delegate to use to modify all requests
//        /// </summary>
//        public RequestModifier? RequestModifier { get; set; }

//        /// <summary>
//        /// Optional factory to create an <see cref="IRequester"/> instance to use
//        /// </summary>
//        /// <remarks>
//        /// If specified, <see cref="RestClientConfigurer"/> must be null, or an exception will be thrown
//        /// </remarks>
//        public Func<HttpClient, IRequester>? RequesterFactory { get; set; }
//    }

//    /// <summary>
//    /// Additional options which can be passed to
//    /// <see cref="WebApi.RestEase.HttpClientFactoryExtensions.AddRestEaseClient{T}(Microsoft.Extensions.DependencyInjection.IServiceCollection,RestEase.HttpClientFactory.AddRestEaseClientOptions{T})"/>
//    /// and similar overloads
//    /// </summary>
//    public class AddRestEaseClientOptions<T> where T : class
//    {
//        /// <summary>
//        /// Optional delegate to configure the <see cref="RestClient"/> (to set serializers, etc)
//        /// </summary>
//        public Action<RestClient>? RestClientConfigurer { get; set; }

//        /// <summary>
//        /// Optional delegate to configure the <typeparamref name="T"/> (to set properties, etc)
//        /// </summary>
//        public Action<T>? InstanceConfigurer { get; set; }

//        /// <summary>
//        /// Optional delegate to use to modify all requests
//        /// </summary>
//        public RequestModifier? RequestModifier { get; set; }

//        /// <summary>
//        /// Optional factory to create an <see cref="IRequester"/> instance to use
//        /// </summary>
//        /// <remarks>
//        /// If specified, <see cref="RestClientConfigurer"/> must be null, or an exception will be thrown
//        /// </remarks>
//        public Func<HttpClient, IRequester>? RequesterFactory { get; set; }
//    }

//    /// <summary>
//    /// Additional options which can be passed to
//    /// <see cref="WebApi.RestEase.HttpClientFactoryExtensions.UseWithRestEaseClient(Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,System.Type,RestEase.HttpClientFactory.UseWithRestEaseClientOptions)"/>
//    /// and similar overloads
//    /// </summary>
//    public class UseWithRestEaseClientOptions
//    {
//        /// <summary>
//        /// Optional delegate to configure the <see cref="RestClient"/> (to set serializers, etc)
//        /// </summary>
//        public Action<RestClient>? RestClientConfigurer { get; set; }

//        /// <summary>
//        /// Optional delegate to use to modify all requests
//        /// </summary>
//        public RequestModifier? RequestModifier { get; set; }

//        /// <summary>
//        /// Optional factory to create an <see cref="IRequester"/> instance to use
//        /// </summary>
//        /// <remarks>
//        /// If specified, <see cref="RestClientConfigurer"/> must be null, or an exception will be thrown
//        /// </remarks>
//        public Func<HttpClient, IRequester>? RequesterFactory { get; set; }
//    }

//    /// <summary>
//    /// Additional options which can be passed to
//    /// <see cref="WebApi.RestEase.HttpClientFactoryExtensions.UseWithRestEaseClient{T}(Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,RestEase.HttpClientFactory.UseWithRestEaseClientOptions{T})"/>
//    /// and similar overloads
//    /// </summary>
//    public class UseWithRestEaseClientOptions<T> where T : class
//    {
//        /// <summary>
//        /// Optional delegate to configure the <see cref="RestClient"/> (to set serializers, etc)
//        /// </summary>
//        public Action<RestClient>? RestClientConfigurer { get; set; }

//        /// <summary>
//        /// Optional delegate to configure the <typeparamref name="T"/> (to set properties, etc)
//        /// </summary>
//        public Action<T>? InstanceConfigurer { get; set; }

//        /// <summary>
//        /// Optional delegate to use to modify all requests
//        /// </summary>
//        public RequestModifier? RequestModifier { get; set; }

//        /// <summary>
//        /// Optional factory to create an <see cref="IRequester"/> instance to use
//        /// </summary>
//        /// <remarks>
//        /// If specified, <see cref="RestClientConfigurer"/> must be null, or an exception will be thrown
//        /// </remarks>
//        public Func<HttpClient, IRequester>? RequesterFactory { get; set; }
//    }
//}

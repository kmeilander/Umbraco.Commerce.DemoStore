using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using System.Linq;
using Umbraco.Extensions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Umbraco.Commerce.Cms.Services;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core;
using Umbraco.Commerce.Core.Finders;
using System;
using Umbraco.Cms.Core.Web;
using Umbraco.Commerce.Cms.Finders;
using Umbraco.Commerce.Cms.Helpers;

namespace Umbraco.Commerce.DemoStore.Web
{
    /// <summary>
    /// This watches the save event and stores the Kalahari Product Stock in the product stock database table.  
    /// </summary>
    public class SchedulePublishBreakingSavingNotificationHandler : INotificationHandler<ContentSavingNotification>, INotificationHandler
    {
        private readonly UmbracoNodeStoreFinderCollection _storeFinderCollection;
        UmbracoPublishedContentStoreFinder finder;
        private readonly Lazy<IUmbracoContextFactory> _umbracoContextFactory;
        private readonly Lazy<PublishedContentHelperAccessor> _publishedContentHelperAccessor;
        IPublishedValueFallback _publishedValueFallback;
        IUmbracoStoreService _storeService;


        public SchedulePublishBreakingSavingNotificationHandler(IUmbracoStoreService storeService, UmbracoNodeStoreFinderCollection storeFinderCollection, Lazy<IUmbracoContextFactory> umbracoContextFactory, Lazy<PublishedContentHelperAccessor> publishedContentHelperAccessor, IPublishedValueFallback publishedValueFallback)
        {
            _storeFinderCollection = storeFinderCollection;
            finder = new UmbracoPublishedContentStoreFinder(umbracoContextFactory, publishedContentHelperAccessor);
            _umbracoContextFactory = umbracoContextFactory;
            _publishedContentHelperAccessor = publishedContentHelperAccessor;
            _publishedValueFallback = publishedValueFallback;
            _storeService = storeService;
        }

        public void Handle(ContentSavingNotification notification)
        {
            var siteRootId = int.Parse(notification.SavedEntities.First().Path.Split(",").ElementAt(1));
            using UmbracoContextReference umbracoContextReference = _umbracoContextFactory.Value.EnsureUmbracoContext();
            var node = umbracoContextReference.UmbracoContext.Content.GetById(siteRootId);

            IPublishedProperty property22 = node.GetProperty("store");
            var value = property22.GetValue();


            var store = _storeService.FindStoreByNodeId(notification.SavedEntities.First().Id);

      
        }
    }
}
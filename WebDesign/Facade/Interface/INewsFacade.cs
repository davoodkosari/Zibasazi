using System;
using System.Collections.Generic;
using System.Web;
using Radyn.Framework;
using Radyn.News.DataStructure;

namespace Radyn.WebDesign.Facade.Interface
{
public interface INewsFacade : IBaseFacade<DataStructure.News>
{
    bool Insert(Guid websiteId, News.DataStructure.News news, NewsContent newsContent, NewsProperty newsproperty, HttpPostedFileBase file);
    IEnumerable<News.DataStructure.News> TopCount(Guid websiteId, int? top);
}
}

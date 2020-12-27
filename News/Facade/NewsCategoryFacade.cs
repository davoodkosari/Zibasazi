﻿using System;
using System.Data;
using Radyn.Common;
using Radyn.Common.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.News.BO;
using Radyn.News.DataStructure;
using Radyn.News.Facade.Interface;

namespace Radyn.News.Facade
{
    internal sealed class NewsCategoryFacade : NewsBaseFacade<NewsCategory>, INewsCategoryFacade
    {
        internal NewsCategoryFacade()
        {
        }

        internal NewsCategoryFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

      
       
       
       
    }
}

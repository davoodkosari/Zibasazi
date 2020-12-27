﻿using System;
using System.Data;
using Radyn.Common;
using Radyn.Common.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Graphic.BO;
using Radyn.Graphic.DataStructure;
using Radyn.Graphic.Facade.Interface;

namespace Radyn.Graphic.Facade
{
    internal sealed class ThemeFacade : GraphicBaseFacade<Theme>, IThemeFacade
    {
        internal ThemeFacade() { }

        internal ThemeFacade(IConnectionHandler connectionHandler)
        : base(connectionHandler)
        { }
       

        
    }
}

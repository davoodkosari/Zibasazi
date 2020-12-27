using System;
using System.Collections.Generic;
using System.Linq;
using Radyn.FormGenerator.DAL;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.FormGenerator.BO
{
    internal class FormAssigmentBO : BusinessBase<FormAssigment>
    {


        public override bool Insert(IConnectionHandler connectionHandler, FormAssigment obj)
        {
            obj.Url = obj.Url.ToLower();
            return base.Insert(connectionHandler, obj);
        }


     

    }
}

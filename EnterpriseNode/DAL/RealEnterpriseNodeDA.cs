using System;
using System.Collections.Generic;
using Radyn.EnterpriseNode.DataStructure;
using Radyn.EnterpriseNode.Tools;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.EnterpriseNode.DAL
{
    public sealed class RealEnterpriseNodeDA : DALBase<RealEnterpriseNode>
    {
        public RealEnterpriseNodeDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

    }
    internal class RealEnterpriseNodeCommandBuilder
    {
       
    }
}

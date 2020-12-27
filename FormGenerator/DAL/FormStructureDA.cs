using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using System;
using System.Data;

namespace Radyn.FormGenerator.DAL
{
    public sealed class FormStructureDA : DALBase<FormStructure>
    {
        public FormStructureDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

      
    }
    internal class FormStructureCommandBuilder
    {
      
    }
}

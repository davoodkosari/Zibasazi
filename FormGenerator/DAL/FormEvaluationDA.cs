using System;
using System.Data;
using System.Data.SqlTypes;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.FormGenerator.DAL
{
    public sealed class FormEvaluationDA : DALBase<FormEvaluation>
    {
        public FormEvaluationDA(IConnectionHandler connectionHandler)
          : base(connectionHandler)
        { }

       
    }
    internal class FormEvaluationCommandBuilder
    {
        
    }

}

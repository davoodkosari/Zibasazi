using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Congress.Tools;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.Facade
{
    internal sealed class RefereeCartableFacade : CongressBaseFacade<RefereeCartable>, IRefereeCartableFacade
    {
        internal RefereeCartableFacade()
        {
        }

        internal RefereeCartableFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

       
        public bool SpecialRefereeAssignArticle(Guid congressId, RefereeCartable refereeCartable, Guid answeredrefreeId, string comments,
            HttpPostedFileBase attachment, List<Guid> refreeIdlist)
        {
            ModelView.ModifyResult<RefereeCartable> keyValuePairs;
            var refereeBo = new RefereeBO();
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                keyValuePairs = new RefereeCartableBO().SpecialRefereeAssignArticle(this.ConnectionHandler,
                    this.FileManagerConnection,
                    congressId, refereeCartable, answeredrefreeId, comments, attachment, refreeIdlist);
                   
                this.ConnectionHandler.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            try
            {
                if (keyValuePairs.SendInform)
                {
                    refereeBo.InformRefereeAddArticle(this.ConnectionHandler, congressId, keyValuePairs.InformList);
                }
            }
            catch (Exception)
            {


            }
            return keyValuePairs.Result;
        }

        public bool AssigneArticleToRefreeCartabl(Guid congressId,Guid articleId, Guid flowsender, List<Guid> refereesId)
        {
            ModelView.ModifyResult<RefereeCartable> keyValuePairs ;
            
            var refereeBo = new RefereeBO();
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                keyValuePairs = new RefereeCartableBO().AssigneArticleToRefreeCartabl(this.ConnectionHandler,this.FileManagerConnection,articleId, flowsender, refereesId);
                this.ConnectionHandler.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
               
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            
            try
            {
                if (keyValuePairs.SendInform)
                {
                    refereeBo.InformRefereeAddArticle(this.ConnectionHandler, congressId, keyValuePairs.InformList);
                }
            }
            catch (Exception)
            {


            }
            return keyValuePairs.Result;

        }
        public bool ModifyCartable(Guid articleId, Guid refreeId, byte state, bool visited, bool active )
        {
            try
            {
                return new RefereeCartableBO().ModifyCartabl(this.ConnectionHandler, articleId, refreeId, state, visited, active);
            }

            catch (KnownException ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }


        public bool AnswerArticle(Guid congressId, RefereeCartable refereeCartable, Guid answeredrefreeId, string comments,
            HttpPostedFileBase attachment)
        {

            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FileManagerConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (
                    !new RefereeCartableBO().AnswerArticle(this.ConnectionHandler, this.FileManagerConnection,
                        congressId, refereeCartable, answeredrefreeId, comments, attachment))
                    return false;
                this.ConnectionHandler.CommitTransaction();
                this.FileManagerConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FileManagerConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }

        }

        public bool Refereeopinion(RefereeCartable refereeCartable, FormStructure postFormData)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (
                    !new RefereeCartableBO().Refereeopinion(this.ConnectionHandler, this.FormGeneratorConnection,
                        refereeCartable, postFormData))
                    return false;
                this.ConnectionHandler.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }
        public bool DeleteFromRefreeCartable(List<Guid> list, Guid refreeId)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.FormGeneratorConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (
                    !new RefereeCartableBO().DeleteFromRefreeCartable(this.ConnectionHandler, list, refreeId))
                    return false;
                this.ConnectionHandler.CommitTransaction();
                this.FormGeneratorConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.FormGeneratorConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

       
    }
}

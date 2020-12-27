using System;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Tools;
using Radyn.ContentManager;
using Radyn.ContentManager.DataStructure;
using Radyn.Framework;
using Radyn.Framework.DbHelper;

namespace Radyn.Congress.BO
{
    internal class SupportTypeBO : BusinessBase<SupportType>
    {
        

        public bool ModifySupportTypes(IConnectionHandler connectionHandler, IConnectionHandler contentManagerConnection, Guid congressId, SupportType supportType)
        {

            var configurationSupportTypeBo = new ConfigurationSupportTypeBO();
            var byFilter = configurationSupportTypeBo.Get(connectionHandler, congressId, supportType.Id);
            if (byFilter == null)
            {
                var configurationSupportType = new ConfigurationSupportType
                {

                    SupportTypeId = supportType.Id,
                    CongressId = congressId
                };
                if (!configurationSupportTypeBo.Insert(connectionHandler, configurationSupportType))
                    throw new Exception(Resources.Congress.ErrorInSaveSupporterPartialInConfiguration);
            }
            var partialsTransactionalFacade = ContentManagerComponent.Instance.PartialsTransactionalFacade(contentManagerConnection);
            var pl = new Partials
            {
                Enabled = true,
                Title = supportType.Title,
                Url = supportType.PartialUrl(),
                ContextName = "همایشات",
                Id = Guid.NewGuid(),
                OperationId = Radyn.Common.Constants.OperationId.CongressOperationId,
                RefId = congressId.ToString(),
                CurrentUICultureName = supportType.CurrentUICultureName

            };
            var firstOrDefault = partialsTransactionalFacade.FirstOrDefault(x => x.Url.ToLower() == pl.Url.ToLower());
            if (firstOrDefault != null)
            {
                pl.Id = firstOrDefault.Id;
                if (!partialsTransactionalFacade.Update(pl))
                    throw new Exception(Resources.Congress.ErrorInSaveSupporterPartialInConfiguration);
                return true;
            }

            if (!partialsTransactionalFacade.Insert(pl))
                throw new Exception(Resources.Congress.ErrorInSaveSupporterPartialInConfiguration);

            return true;

        }
    }
}

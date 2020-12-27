using System;
using System.Data;
using Radyn.Congress.BO;
using Radyn.Congress.DataStructure;
using Radyn.Congress.Facade.Interface;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Slider;
using Radyn.Slider.DataStructure;

namespace Radyn.Congress.Facade
{
    internal sealed class CongressSlideFacade : CongressBaseFacade<CongressSlide>, ICongressSlideFacade
    {
        internal CongressSlideFacade()
        {
        }

        internal CongressSlideFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

      

        public bool Insert(Guid congressId, Slide slide, string usefor)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.SliderConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                slide.IsExternal = true;

                if (!SliderComponent.Instance.SlideTransactionalFacade(this.SliderConnection).Insert(slide))
                    throw new Exception("خطایی درذخیره گالری وجود دارد");
                var congessSlide = new CongressSlide { SlideId = slide.Id, CongressId = congressId };
                if (!new CongressSlideBO().Insert(this.ConnectionHandler, congessSlide))
                    throw new Exception(Resources.Congress.ErrorInSaveCongressGallery);
                if (!string.IsNullOrEmpty(usefor))
                {
                    var configurationBo = new ConfigurationBO();
                    var config = configurationBo.ValidConfig(this.ConnectionHandler, congressId);
                    switch (usefor)
                    {
                        case "1":
                            config.BigSlideId = slide.Id;
                            break;
                        case "0":
                            config.AverageSlideId = slide.Id;
                            break;
                        case "-1":
                            config.MiniSlideId = slide.Id;
                            break;
                    }
                    if (!configurationBo.Update(this.ConnectionHandler, config))
                        throw new Exception(Resources.Congress.ErrorInEditConfuguration);
                }
                this.ConnectionHandler.CommitTransaction();
                this.SliderConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.SliderConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.SliderConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Update(Guid congressId, Slide slide, string usefor)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.SliderConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                if (!SliderComponent.Instance.SlideTransactionalFacade(this.SliderConnection).Update(slide))
                    throw new Exception("خطایی درذخیره گالری وجود دارد");
                var configurationBo = new ConfigurationBO();
                var config = configurationBo.ValidConfig(this.ConnectionHandler, congressId);
                if (!string.IsNullOrEmpty(usefor))
                {
                    switch (usefor)
                    {
                        case "1":
                            config.BigSlideId = slide.Id;
                            if (config.AverageSlideId == slide.Id)
                                config.AverageSlideId = null;
                            if (config.MiniSlideId == slide.Id)
                                config.MiniSlideId = null;
                            break;
                        case "0":
                            config.AverageSlideId = slide.Id;
                            if (config.BigSlideId == slide.Id)
                                config.BigSlideId = null;
                            if (config.MiniSlideId == slide.Id)
                                config.MiniSlideId = null;
                            break;
                        case "-1":
                            config.MiniSlideId = slide.Id;
                            if (config.AverageSlideId == slide.Id)
                                config.AverageSlideId = null;
                            if (config.BigSlideId == slide.Id)
                                config.BigSlideId = null;
                            break;
                    }
                }
                else
                {
                    if (config.BigSlideId == slide.Id)
                        config.BigSlideId = null;
                    if (config.AverageSlideId == slide.Id)
                        config.AverageSlideId = null;
                    if (config.MiniSlideId == slide.Id)
                        config.MiniSlideId = null;
                }
                if (!configurationBo.Update(this.ConnectionHandler, config))
                    throw new Exception(Resources.Congress.ErrorInEditConfuguration);
                this.ConnectionHandler.CommitTransaction();
                this.SliderConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.SliderConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.SliderConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

        public override bool Delete(CongressSlide obj)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.SliderConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var configurationBo = new ConfigurationBO();
                
                var config = configurationBo.ValidConfig(this.ConnectionHandler, obj.CongressId);
                if (config.BigSlideId == obj.SlideId)
                    config.BigSlideId = null;
                if (config.AverageSlideId == obj.SlideId)
                    config.AverageSlideId = null;
                if (config.MiniSlideId == obj.SlideId)
                    config.MiniSlideId = null;

                if (!configurationBo.Update(this.ConnectionHandler, config))
                    throw new Exception(Resources.Congress.ErrorInEditConfuguration);
                if (!new CongressSlideBO().Delete(this.ConnectionHandler, obj))
                    throw new Exception("خطایی در حذف اسلاید وجود دارد");
                if (!SliderComponent.Instance.SlideTransactionalFacade(this.GalleryConnection).Delete(obj.SlideId))
                    return false;
                this.ConnectionHandler.CommitTransaction();
                this.SliderConnection.CommitTransaction();
                return true;
            }
            catch (KnownException ex)
            {
                this.ConnectionHandler.RollBack();
                this.SliderConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.SliderConnection.RollBack();
                Log.Save(ex.Message, LogType.ApplicationError, ex.Source, ex.StackTrace);
                throw new KnownException(ex.Message, ex);
            }
        }

      

    }
}

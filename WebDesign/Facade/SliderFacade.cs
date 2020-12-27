using System;
using System.Data;
using Radyn.Framework;
using Radyn.Framework.DbHelper;
using Radyn.Slider;
using Radyn.Slider.DataStructure;
using Radyn.WebDesign.BO;
using Radyn.WebDesign.Facade.Interface;

namespace Radyn.WebDesign.Facade
{
    internal sealed class SliderFacade : WebDesignBaseFacade<DataStructure.Slider>, ISliderFacade
    {
        internal SliderFacade() { }

        internal SliderFacade(IConnectionHandler connectionHandler)
            : base(connectionHandler) { }



        public override bool Delete(params object[] keys)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.SliderConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                var configurationBo = new ConfigurationBO();
                var sliderBo = new SliderBO();
                var obj = sliderBo.Get(this.ConnectionHandler, keys);
                var config = configurationBo.Get(this.ConnectionHandler, obj.WebId);
                if (config == null) return false;
                if (config.BigSlideId == obj.SlideId)
                    config.BigSlideId = null;
                if (config.AverageSlideId == obj.SlideId)
                    config.AverageSlideId = null;
                if (config.MiniSlideId == obj.SlideId)
                    config.MiniSlideId = null;

                if (!configurationBo.Update(this.ConnectionHandler, config))
                    throw new Exception("خطایی درذخیره تنظیمات وجود دارد");
                if (!sliderBo.Delete(this.ConnectionHandler, obj.WebId, obj.SlideId))
                    throw new Exception("خطایی در حذف اسلاید وجود دارد");
                if (!SliderComponent.Instance.SlideTransactionalFacade(this.GalleryConnection).Delete(obj.SlideId))
                    return false;
                this.ConnectionHandler.CommitTransaction();
                this.SliderConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.SliderConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.SliderConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }
        

        public bool Insert(Guid websiteId, Slide slide, string url)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.SliderConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                slide.IsExternal = true;

                if (!SliderComponent.Instance.SlideTransactionalFacade(this.SliderConnection).Insert(slide))
                    throw new Exception("خطایی درذخیره گالری وجود دارد");
                var congessSlide = new DataStructure.Slider() { SlideId = slide.Id, WebId = websiteId };
                if (!new SliderBO().Insert(this.ConnectionHandler, congessSlide))
                    throw new Exception("خطایی درذخیره گالری وجود دارد");
                if (!string.IsNullOrEmpty(url))
                {
                    var configurationBo = new ConfigurationBO();
                    var config = configurationBo.Get(this.ConnectionHandler, websiteId);
                    if (config == null) return false;
                    switch (url)
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
                        throw new Exception("خطایی درذخیره تنظیمات وجود دارد");
                }
                this.ConnectionHandler.CommitTransaction();
                this.SliderConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.SliderConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.SliderConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }

        public bool Update(Guid websiteId, Slide slide, string url)
        {
            try
            {
                this.ConnectionHandler.StartTransaction(IsolationLevel.ReadUncommitted);
                this.SliderConnection.StartTransaction(IsolationLevel.ReadUncommitted);
                slide.IsExternal = true;
                
                if (!SliderComponent.Instance.SlideTransactionalFacade(this.SliderConnection).Update(slide))
                    throw new Exception("خطایی درذخیره گالری وجود دارد");
                var configurationBo = new ConfigurationBO();
                var config = configurationBo.Get(this.ConnectionHandler, websiteId);
                if (config == null) return false;
                if (!string.IsNullOrEmpty(url))
                {
                    switch (url)
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
                    throw new Exception("خطایی درذخیره تنظیمات وجود دارد");
                this.ConnectionHandler.CommitTransaction();
                this.SliderConnection.CommitTransaction();
                return true;
            }
            catch (KnownException knownException)
            {
                this.ConnectionHandler.RollBack();
                this.SliderConnection.RollBack();
                throw new KnownException(knownException.Message, knownException);
            }
            catch (Exception ex)
            {
                this.ConnectionHandler.RollBack();
                this.SliderConnection.RollBack();
                throw new KnownException(ex.Message, ex);
            }
        }
    }
}

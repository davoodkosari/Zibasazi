using System;
using Radyn.FormGenerator.DataStructure;
using Radyn.Framework;
using Radyn.WebDesign.DataStructure;

namespace Radyn.WebDesign.Facade.Interface
{
public interface IFormsFacade : IBaseFacade<Forms>
{
    bool Insert(Guid websiteId, FormStructure formStructure);
}
}

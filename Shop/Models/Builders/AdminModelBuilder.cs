using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.DataService;

namespace Shop.Models.Builders
{
    public interface IAdminModelBuilder
    {
        AdminModel Build();
    }

    public class AdminModelBuilder : MenuBuilder,IAdminModelBuilder
    {
        public AdminModelBuilder(IDataService dataService):base(dataService)
        {
            
        }
        public AdminModel Build()
        {
            var model = new AdminModel();
            model.menu = BuildMenu();
            return model;
        }

    }
}
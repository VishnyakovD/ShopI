using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.DataService;

namespace Shop.Models.Builders
{

    public interface IRegisterBuilder
    {
        RegisterModel Build();
    }

    public class RegisterBuilder : MenuBuilder, IRegisterBuilder
    {
        public RegisterBuilder(IDataService dataService) : base(dataService)
        {
        }

        public RegisterModel Build()
        {
            var model = new RegisterModel();
            model.menu = BuildMenu();
            return model;
        }
    }
}
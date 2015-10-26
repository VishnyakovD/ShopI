using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Shop.DataService;
using Shop.db.Entities;

namespace Shop.Models.Builders
{


    public class MenuBuilder 
    {
        public IDataService dataService;
        public MenuBuilder(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public List<MenuItem> BuildMenu()
        {
       
              return dataService.ListStaticCategoryes()
                    .Select(
                        cat =>
                            new MenuItem()
                            {
                                idCat = cat.id,
                                name = cat.name,
                                url =string.Empty /*new UrlHelper(new RequestContext()).Action("ListSkuByCategory", "Sku", new {idCat = cat.id})*/
                            }).ToList();
           
           
        }

        public List<MenuItem> BuildMenu(List<StaticCategory> listStaticCategory)
        {

            return listStaticCategory.Select(
                      cat =>
                          new MenuItem()
                          {
                              idCat = cat.id,
                              name = cat.name,
                              url = string.Empty
                          }).ToList();


        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Shop.DataService;
using Shop.Filters;
using Shop.Logger;
using Shop.Models.Builders;
using Shop.Modules;

namespace Shop.Controllers
{
      [InitializeSimpleMembership]
    public class HomeController : BaseController
    {
        private ISkuViewerBuilder skuViewerBuilder { set; get; }

        public HomeController(ILogger logger, IAdminModelBuilder adminModelBuilder, IDataService dataService, IImagesPath imagesPath, ISkuViewerBuilder SkuViewerBuilder, ISKUModelBuilder SKUModelBuilder)
            : base(logger, adminModelBuilder, dataService, imagesPath, SKUModelBuilder)
        {
            skuViewerBuilder = SkuViewerBuilder;
        }

        public ActionResult ListSkuOnCategory(long idCat, int? sort)
        {
            if (!sort.HasValue)
            {
                sort = -1;
            }
            Session["sort"] = sort;      
            var model = skuViewerBuilder.Build(idCat,sort.Value);
            return View("ListSkuOnCategory", model);
        }

        public ActionResult SkuInfo(long idSku)
        {
            var model = skuViewerBuilder.BuildSkuModel(idSku);
            return View("SkuInfo", model);
        }

        public ActionResult Index()
        {
            var model = skuViewerBuilder.Build(1,-1);
            return View("ListSkuOnCategory", model);
    
        }



    }
}

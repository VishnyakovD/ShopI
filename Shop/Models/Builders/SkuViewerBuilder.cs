using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.DataService;
using Shop.db.Entities;
using Shop.Modules;
using WebMatrix.WebData;

namespace Shop.Models.Builders
{

    public interface ISkuViewerBuilder
    {
        SkuViewerModel Build(long idCat, int sort);
        SKUModel BuildSkuModel(long idSlu);
    }

    public class SkuViewerBuilder : MenuBuilder, ISkuViewerBuilder
    {
        private IImagesPath imagesPath;
        private ISKUModelBuilder SKUModelBuilder;
        private IAccountAdminModelBuilder AccountAdminModelBuilder;
        public SkuViewerBuilder(IDataService dataService, IImagesPath imagesPath, ISKUModelBuilder iSKUModelBuilder, IAccountAdminModelBuilder iAccountAdminModelBuilder)
            : base(dataService)
        {
            this.imagesPath = imagesPath;
            this.SKUModelBuilder = iSKUModelBuilder;
            this.AccountAdminModelBuilder = iAccountAdminModelBuilder;
        }

        private IEnumerable<ShortSKUModel> ListSkuByCategory(StaticCategory cat)
        {
            var u = AccountAdminModelBuilder.BuildOneUser(WebSecurity.CurrentUserName);
            if (u!=null&&u.Discount>0)
            {
                return dataService.ListSkuByCategory(cat).Select(sku => new ShortSKUModel()
                {
                    id = sku.id,
                    brandId = sku.brand.id,
                    brandName = sku.brand.name,
                    categotyId = cat.id,
                    categotyName = cat.name,
                    description = sku.description,
                    name = sku.name,
                    price = sku.price,
                    priceAct = sku.priceAct-((sku.priceAct / 100) * u.Discount),
                    smalPhotoPath = string.Format("{0}/{1}", imagesPath.GetImagesPath(), (sku.smalPhoto ?? new Photo() { path = "box.png" }).path)
                });
            }

                return dataService.ListSkuByCategory(cat).Select(sku => new ShortSKUModel()
                {
                    id = sku.id,
                    brandId = sku.brand.id,
                    brandName = sku.brand.name,
                    categotyId = cat.id,
                    categotyName = cat.name,
                    description = sku.description,
                    name = sku.name,
                    price = sku.price,
                    priceAct = sku.priceAct,
                    smalPhotoPath = string.Format("{0}/{1}", imagesPath.GetImagesPath(), (sku.smalPhoto ?? new Photo() { path = "box.png" }).path)
                }); 
           
           
        }

        public SkuViewerModel Build(long idCat, int sort)
        {
            var model = new SkuViewerModel();
            var cat = dataService.GetStaticCategoryById(idCat);
            if (cat!=null)
            {
                model.IdCat = cat.id;
                model.Name=cat.name;
                switch (sort)
                {
                    case 1://сорт от А до Я
                        model.skuList = ListSkuByCategory(cat).OrderBy(it => it.name).ToList();
                        break;
                    case 2://сорт от Я до А
                        model.skuList = ListSkuByCategory(cat).OrderByDescending(it=>it.name).ToList();
                        break;
                    case 3://сорт по цене Возростание
                        model.skuList = ListSkuByCategory(cat).OrderBy(it => it.priceAct).ToList();
                        break;
                    case 4://сорт по цене Убывание
                        model.skuList = ListSkuByCategory(cat).OrderByDescending(it => it.priceAct).ToList();
                        break;
                    default:
                        model.skuList = ListSkuByCategory(cat).ToList();
                        break;
                }
            }
            model.menu = BuildMenu();
            return model;
        }

        public SKUModel BuildSkuModel(long idSku)
        {
            var skuModel = new SKUModel();
            var sku = dataService.GetSkuById(idSku);
            if (sku!=null)
            {     
                 var u = AccountAdminModelBuilder.BuildOneUser(WebSecurity.CurrentUserName);
                 skuModel = SKUModelBuilder.ConvertSkuBDToSkuModel(sku);
                if (u != null && u.Discount > 0)
                {
                    skuModel.priceAct = skuModel.priceAct - ((skuModel.priceAct/100)*u.Discount);
                }
    
            }
            skuModel.menu = BuildMenu();
            return skuModel;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Shop.DataService;
using Shop.db.Entities;
using Shop.Modules;
using WebMatrix.WebData;

namespace Shop.Models.Builders
{
    public interface ISKUModelBuilder
    {
        SKUModel GetEmptySku();
        SKUModel ConvertSkuBDToSkuModel(Sku sku);
    }

    public class SKUModelBuilder :MenuBuilder, ISKUModelBuilder
    {
        private IAccountAdminModelBuilder AccountAdminModelBuilder;
        private IDataService dataService;
        private IImagesPath imagesPath;

        public SKUModelBuilder(IDataService dataService, IImagesPath imagesPath, IAccountAdminModelBuilder iAccountAdminModelBuilder)
            : base(dataService)
        {
            this.dataService = dataService;
            this.imagesPath = imagesPath;
            this.AccountAdminModelBuilder = iAccountAdminModelBuilder;
        }
        public SKUModel GetEmptySku()
        {
            var sku = new SKUModel();
            sku.listStaticCategory = dataService.ListStaticCategoryes();
            sku.listStaticSpecification = dataService.ListStaticSpecification();
            sku.listStaticBrand = dataService.ListBrands();
            sku.menu = BuildMenu(sku.listStaticCategory);

            sku.listCategory=new List<Category>();
            sku.listComment=new List<Comment>();
            sku.listPhoto=new List<PhotoBig>();
            sku.listSpecification=new List<Specification>();

            return sku;
        }

        public SKUModel ConvertSkuBDToSkuModel(Sku sku)
        {
            var u = AccountAdminModelBuilder.BuildOneUser(WebSecurity.CurrentUserName);
            var skuModel = GetEmptySku();
            if (sku != null)
            {
                skuModel.id = sku.id;
                skuModel.name = sku.name;
                skuModel.price = sku.price;

                if (u != null && u.Discount > 0)
                {
                    skuModel.priceAct = sku.priceAct - ((sku.priceAct / 100) * u.Discount);
                }
                else
                {
                    skuModel.priceAct = sku.priceAct;
                }

                skuModel.priceAct = sku.priceAct;

                skuModel.description = sku.description;
                if (sku.brand!=null)
                {
                    skuModel.brandId = sku.brand.id;
                    skuModel.brandName = sku.brand.name;
                }
                
                if (sku.smalPhoto!=null)
                {
                    skuModel.smalPhotoId = sku.smalPhoto.id;
                    skuModel.smalPhotoPath =  string.Format("{0}/{1}",imagesPath.GetImagesPath(), sku.smalPhoto.path);
                }
                
                skuModel.listCategory = sku.listCategory;
                skuModel.listSpecification = sku.listSpecification;
                skuModel.listPhoto = sku.listPhoto.Select(im => new PhotoBig() { id = im.id, name = im.name, path = string.Format("{0}/{1}", imagesPath.GetImagesPath(), im.path),skuId = im.skuId}).ToList();

            }
            return skuModel;
        }
    }
}
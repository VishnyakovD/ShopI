using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NLog;
using Shop.DataService;
using Shop.db;
using Shop.db.Entities;
using Shop.Logger;
using Shop.Models;
using Shop.Models.Builders;
using Shop.Modules;
using Shop.Filters;

namespace Shop.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AdminController : BaseController
    {


        public AdminController(ILogger logger, IAdminModelBuilder adminModelBuilder, IDataService dataService, IImagesPath imagesPath, ISKUModelBuilder SKUModelBuilder)
            : base(logger, adminModelBuilder, dataService, imagesPath, SKUModelBuilder)
        {

        }


        public ActionResult Administrator()
        {
            var model = adminModelBuilder.Build();

           
            return View("Administrator",model);
        }

        [HttpPost]
        public ActionResult AddOrUpdateBrand(Brand brand)
        {
            var message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (dataService.AddOrUpdateBrand(brand))
                    {
                        return Content("Бренд сохранен", "text/html");
                    }
                }
            }
            catch (Exception err)
            {
                message = err.Message;
            }

            return Content("Бренд НЕ сохранен " + message, "text/html");  
        }

        [HttpPost]
        public ActionResult AddOrUpdateCategory(Category category)
        {
            var message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (dataService.AddOrUpdateCategory(category))
                    {
                        return Content("Категория сохранена", "text/html");
                    }
                }
            }
            catch (Exception err)
            {
                message = err.Message;
            }

            return Content("Категория НЕ сохранена " + message, "text/html");
        }


        public ActionResult ListBrands()
        {
            List<BrandModel> brands = null;
            try
            {
                brands = dataService.ListBrands().Select(br=>new BrandModel(){id=br.id,name = br.name}).ToList();
            }
            catch (Exception err)
            {
                return PartialView("MessagesPartial", "Ошибка " + err.Message);
            }

            return PartialView("ListBrandsPartial", brands);
        }


        [HttpPost]
        public ActionResult AddOrUpdateStaticCategory(StaticCategory category)
        {
            var message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (dataService.AddOrUpdateStaticCategory(category))
                    {
                        return Content("Категория сохранена", "text/html");
                    }
                }
            }
            catch (Exception err)
            {
                message = err.Message;
            }

            return Content("Категория НЕ сохранена " + message, "text/html");
        }

        public ActionResult ListStaticCategoryes()
        {
            List<StaticCategory> categoryes = null;
            try
            {
                categoryes = dataService.ListStaticCategoryes();
            }
            catch (Exception err)
            {
                return PartialView("MessagesPartial", "Ошибка " + err.Message);
            }

            return PartialView("ListStaticCategoryesPartial", categoryes);
        }


        public ActionResult ListStaticSpecification()
        {
            List<StaticSpecification> specifications = null;
            try
            {
                specifications = dataService.ListStaticSpecification();
            }
            catch (Exception err)
            {
                return PartialView("MessagesPartial", "Ошибка " + err.Message);
            }

            return PartialView("ListStaticSpecificationPartial", specifications);
        }

        [HttpPost]
        public ActionResult AddOrUpdateStaticSpecification(StaticSpecification spec)
        {
            var message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (dataService.AddOrUpdateStaticSpecification(spec))
                    {
                        return Content("Спецификация сохранена", "text/html");
                    }
                }
            }
            catch (Exception err)
            {
                message = err.Message;
            }

            return Content("Спецификация НЕ сохранена " + message, "text/html");
        }

        public ActionResult SkuData(long? id)
        {
            var result = new SKUModel();
       
            try
            {
                if (id.HasValue)
                {
                    var skuDB = dataService.GetSkuById(id.Value);

                    result = skuDB != null ? skuModelBuilder.ConvertSkuBDToSkuModel(skuDB) : skuModelBuilder.GetEmptySku();
                }
                else
                {
                    result = skuModelBuilder.GetEmptySku();
                }

            }
            catch (Exception err)
            {
                return View("MessagesPartial", "Ошибка " + err.Message);
            }

            return View("SkuData", result);
        }

        [HttpPost]
        public ActionResult AddOrUpdateSku(SKUModel sku)
        {
            var message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var id = dataService.AddOrUpdateSKU(sku.GetSKUDB());
                    if (id >0)
                    {
                        return RedirectToAction("SkuData",  new {id=id});
                    }
                }
            }
            catch (Exception err)
            {
                message = err.Message;
            }

            return Content("Товар НЕ сохранен " + message, "text/html");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadSmalPhoto(long id, HttpPostedFileBase smalPhotoFile)
        {
            try
            {
                if (smalPhotoFile != null)
                {
                        if (smalPhotoFile.ContentLength > 0)
                        {
                            var path = UploadPhoto(id, smalPhotoFile);
                            if (!string.IsNullOrEmpty(path))
                            {
                                var pho=new Photo(){name = string.Empty, path = path, skuId = id};
                                if (dataService.AddSmalPhotoToSKU(id, pho))
                                {
                                    return RedirectToAction("SkuData", "Admin", new {id=id}); 
                                }
                            }
                        }
                }
                else
                {
                    return Content("Фото НЕ сохранено ", "text/html");
                }
            }
            catch (Exception err)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Content("Фото НЕ сохранено " + err, "text/html");
            }

            return RedirectToAction("SkuData", "Admin", new { id = id }); 
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadBigPhoto(long id, HttpPostedFileBase photoFile)
        {
            try
            {
                if (photoFile != null)
                {
                    if (photoFile.ContentLength > 0)
                    {
                        var path = UploadPhoto(id, photoFile);
                        if (!string.IsNullOrEmpty(path))
                        {
                            var pho=new PhotoBig(){name = string.Empty, path = path, skuId = id};
                            if (dataService.AddBigPhotoToSKU(id, pho))
                            {
                                return RedirectToAction("SkuData", "Admin", new {id=id}); 
                            }
                        }
                    }
                }
                else
                {
                    return Content("Фото НЕ сохранено ", "text/html");
                }
            }
            catch (Exception err)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Content("Фото НЕ сохранено " + err, "text/html");
            }

            return RedirectToAction("SkuData", "Admin", new { id = id }); 
        }

        public ActionResult RemoveBigPhotoFromSKU(long idSku, long idPhoto)
        {
            SKUModel result = null;
            try
            {
                if (dataService.RemoveBigPhotoFromSKU(idSku, idPhoto))
                {
                    var skuDB = dataService.GetSkuById(idSku);
                    result = skuDB != null ? skuModelBuilder.ConvertSkuBDToSkuModel(skuDB) : skuModelBuilder.GetEmptySku();
                }
                else
                {
                    return Content("Фото НЕ удалено", "text/html");
                }
            }
            catch (Exception err)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Фото НЕ удалено " + err, "text/html");
            }

            return PartialView("SkuListPhotosPartial", result.listPhoto);
        }

        private string UploadPhoto(long id, HttpPostedFileBase file)
        {
            var fileName = Path.GetFileNameWithoutExtension(file.FileName.Trim(' ').Replace(' ', '_'));//имя файла
            var fileExt = Path.GetExtension(file.FileName);//расширение файла
            var path = string.Format(@"{0}\{1}", Server.MapPath("~"+imagesPath.GetImagesPath()), id);
            var ticks = DateTime.Now.Ticks;
            if (Directory.Exists(path))
            {
                path = Path.Combine(path, string.Format("{0}-{1}{2}", fileName, ticks, fileExt));
            }
            else
            {
                /*DirectoryInfo di =*/ Directory.CreateDirectory(path);
                if (Directory.Exists(path))
                {
                    path = Path.Combine(path, string.Format("{0}-{1}{2}", fileName, ticks, fileExt));
                }
                else
                {
                    return string.Empty;
                }
            }
            file.SaveAs(path);
           
            return string.Format(@"{0}/{1}",id,string.Format("{0}-{1}{2}", fileName, ticks, fileExt));
        }

        public ActionResult AddSKUFromCategory(long idSku, long catId)
        {
            SKUModel result = null;
            try
            {
                if (dataService.AddCategoryToSKU(idSku, catId))
                {
                    var skuDB = dataService.GetSkuById(idSku);
                    result = skuDB != null ? skuModelBuilder.ConvertSkuBDToSkuModel(skuDB) : skuModelBuilder.GetEmptySku();
                }
                else
                {
                    return Content("Категория НЕ добавлена", "text/html");
                }
            }
            catch (Exception err)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Content("Категория НЕ добавлена " + err, "text/html");
            }

            return PartialView("SkuListCategoriesPartial", result.listCategory); 
        }

        public ActionResult RemoveSKUFromCategory(long idSku, long idCat)
        {
            SKUModel result = null;
            try
            {
                if (dataService.RemoveSKUFromCategory(idSku, idCat))
                {
                    var skuDB = dataService.GetSkuById(idSku);
                    result = skuDB != null ? skuModelBuilder.ConvertSkuBDToSkuModel(skuDB) : skuModelBuilder.GetEmptySku();
                }
                else
                {
                    return Content("Категория НЕ удалена", "text/html");
                }
            }
            catch (Exception err)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Content("Категория НЕ удалена " + err, "text/html");
            }

            return PartialView("SkuListCategoriesPartial", result.listCategory); 
        }

        public ActionResult AddSpecificationToSku(long idSku, long specId, string specValue)
        {
            SKUModel result = null;
            try
            {
                if (dataService.AddSpecificationToSKU(idSku, specId, specValue))
                {
                    var skuDB = dataService.GetSkuById(idSku);
                    result = skuDB != null ? skuModelBuilder.ConvertSkuBDToSkuModel(skuDB) : skuModelBuilder.GetEmptySku();
                }
                else
                {
                    return Content("Спецификация НЕ добавлена", "text/html");
                }
            }
            catch (Exception err)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Спецификация НЕ добавлена " + err, "text/html");
            }

            return PartialView("SkuListSpecificationPartial", result.listSpecification);
        }

        public ActionResult RemoveSpecificationFromSKU(long idSku, long idSpec)
        {
            SKUModel result = null;
            try
            {
                if (dataService.RemoveSpecificationFromSKU(idSku, idSpec))
                {
                    var skuDB = dataService.GetSkuById(idSku);
                    result = skuDB != null ? skuModelBuilder.ConvertSkuBDToSkuModel(skuDB) : skuModelBuilder.GetEmptySku();
                }
                else
                {
                    return Content("Спецификация НЕ удалена", "text/html");
                }
            }
            catch (Exception err)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Content("Спецификация НЕ удалена " + err, "text/html");
            }

            return PartialView("SkuListSpecificationPartial", result.listSpecification);
        }


        
    }
}

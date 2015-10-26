using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Shop.db;
using Shop.db.Entities;
using Shop.db.Repository;
using Shop.Logger;

namespace Shop.DataService
{
    public interface IDataService
    {
        bool AddOrUpdateBrand(Brand brand);
        bool AddOrUpdateCategory(Category category);
        bool AddOrUpdateStaticCategory(StaticCategory category);
        bool AddOrUpdateStaticSpecification(StaticSpecification spec);
        long AddOrUpdateSKU(Sku sku);
        bool AddSmalPhotoToSKU(long id, Photo photo);
        bool AddBigPhotoToSKU(long id, PhotoBig photo);
        bool AddCategoryToSKU(long id, long idCat);
        bool AddSpecificationToSKU(long id, long specId, string specValue);
        bool RemoveSKUFromCategory(long id, long idCat);
        bool RemoveSpecificationFromSKU(long id, long idSpec);
        bool RemoveBigPhotoFromSKU(long id, long idPhoto);
        bool RemoveSkuCartId(long idCart, long idsku);
        Sku GetSkuById(long id);
        StaticCategory GetStaticCategoryById(long idCat);

        List<Sku> ListSkuByCategory(StaticCategory cat);


        List<CartState> ListCartState();
        Cart AddorUpdateSkuToCart(Cart cart);
        Cart GetCartById(long idCart);
        List<Cart> GetCartsByDateAndStatus(DateTime start, DateTime end, int state);

        List<Brand> ListBrands();
        List<Category> ListCategoryes();

        List<StaticCategory> ListStaticCategoryes();
        List<StaticSpecification> ListStaticSpecification();

        bool AddComment(Comment comment);
        bool AddSku(Sku sku);
    }

    public class DataService:IDataService
    {
        private IDb dbService;
        private ILogger logger;
        public DataService(IDb dbService, ILogger logger)
        {
            this.dbService = dbService;
            this.logger = logger;
        }

        public bool AddOrUpdateBrand(Brand brand)
        {
            bool result = false;
            try
            {
                dbService.Run(db =>
                {
                    var brandDB= db.GetRepository<Brand>().TryOne(brand.id);
                    if (brandDB==null)
                    {
                        db.GetRepository<Brand>().Add(brand);
                    }
                    else
                    {
                        brandDB.name = brand.name;
                        db.GetRepository<Brand>().Update(brandDB);
                    }
                    
                });
                result = true;
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;

        }

        public bool AddOrUpdateStaticSpecification(StaticSpecification spec)
        {
            bool result = false;
            try
            {
                dbService.Run(db =>
                {
                    var specDB = db.GetRepository<StaticSpecification>().TryOne(spec.id);
                    if (specDB == null)
                    {
                        db.GetRepository<StaticSpecification>().Add(spec);
                    }
                    else
                    {
                        specDB.name = spec.name;
                        db.GetRepository<StaticSpecification>().Update(specDB);
                    }

                });
                result = true;
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;

        }

        public bool AddOrUpdateStaticCategory(StaticCategory category)
        {
            bool result = false;
            try
            {
                dbService.Run(db =>
                {
                    var categoryDB = db.GetRepository<StaticCategory>().TryOne(category.id);
                    if (categoryDB == null)
                    {
                        db.GetRepository<StaticCategory>().Add(category);
                    }
                    else
                    {
                        categoryDB.name = category.name;
                        db.GetRepository<StaticCategory>().Update(categoryDB);
                    }

                });
                result = true;
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;

        }

        public bool AddOrUpdateCategory(Category category)
        {
            bool result = false;
            try
            {
                dbService.Run(db =>
                {
                    var categoryDB = db.GetRepository<Category>().TryOne(category.id);
                    if (categoryDB == null)
                    {
                        db.GetRepository<Category>().Add(category);
                    }
                    else
                    {
                        categoryDB.staticcat = category.staticcat;
                        categoryDB.description = category.description;
                        categoryDB.photoPath = category.photoPath;
                        db.GetRepository<Category>().Update(categoryDB);
                    }

                });
                result = true;
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;

        }

        public long AddOrUpdateSKU(Sku sku)
        {
            long result = 0;
            try
            {
                dbService.Run(db =>
                {
                    var SkuDB = db.GetRepository<Sku>().TryOne(sku.id);
                    if (SkuDB == null)
                    {
                      result=  db.GetRepository<Sku>().Add(sku).id;
                    }
                    else
                    {
                        SkuDB.name = sku.name;
                        SkuDB.brand = sku.brand;
                        SkuDB.description = sku.description;
                        SkuDB.price = sku.price;
                        SkuDB.priceAct = sku.priceAct;
                        //SkuDB.smalPhoto = sku.smalPhoto;
                        SkuDB.listCategory = sku.listCategory;
                        SkuDB.listComment = sku.listComment;
                        SkuDB.listPhoto = sku.listPhoto;
                        SkuDB.listSpecification = sku.listSpecification;
                        db.GetRepository<Sku>().Update(SkuDB);
                        result = sku.id;
                    }

                });
                
            }
            catch (Exception err)
            {
                result = 0;
                logger.Error(err.Message);
            }
            return result;
        }

        public bool AddSmalPhotoToSKU(long id, Photo photo)
        {
            var result = false;

            try
            {
                dbService.Run(db =>
                {
                    var SkuDB = db.GetRepository<Sku>().TryOne(id);
                    if (SkuDB != null)
                    {
                        SkuDB.smalPhoto = photo;
                        db.GetRepository<Sku>().Update(SkuDB);
                        result = true;
                    }
                });
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }

            return result;
        }

        public bool AddBigPhotoToSKU(long id, PhotoBig photo)
        {
            var result = false;

            try
            {
                dbService.Run(db =>
                {
                    var sku = db.GetRepository<Sku>().TryOne(id);
                    if (sku != null)
                    {
                        if (sku.listPhoto == null)
                        {
                            sku.listPhoto = new List<PhotoBig>();
                        }

                        sku.listPhoto.Add(new PhotoBig() { name = photo.name,path = photo.path,skuId = sku.id });
                        db.GetRepository<Sku>().Update(sku);
                        result = true;

                    }
                });
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }

            return result;
        }

        public bool RemoveBigPhotoFromSKU(long id, long idPhoto)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var pho = db.GetRepository<PhotoBig>().TryOne(idPhoto);
                    if (pho != null)
                    {
                        db.GetRepository<PhotoBig>().Remove(pho);
                        result = true;
                    }
                });

            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public List<Brand> ListBrands()
        {
            var result = new List<Brand>();
            try
            {
                dbService.Run(db =>
                {
                   result= db.GetRepository<Brand>().All();
                });
          
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public List<Category> ListCategoryes()
        {
            var result = new List<Category>();
            try
            {
                dbService.Run(db =>
                {
                    result = db.GetRepository<Category>().All();
                });

            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public List<StaticCategory> ListStaticCategoryes()
        {
            var result = new List<StaticCategory>();
            try
            {
                dbService.Run(db =>
                {
                    result = db.GetRepository<StaticCategory>().All();
                });

            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public List<StaticSpecification> ListStaticSpecification()
        {
            var result = new List<StaticSpecification>();
            try
            {
                dbService.Run(db =>
                {
                    result = db.GetRepository<StaticSpecification>().All();
                });

            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public Sku GetSkuById(long id)
        {
            var result = new Sku();
            try
            {
                dbService.Run(db =>
                {
                    result = db.GetRepository<Sku>().TryOne(id);
                });

            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public StaticCategory GetStaticCategoryById(long idCat)
        {
            var result = new StaticCategory();
            try
            {
                dbService.Run(db =>
                {
                    result = db.GetRepository<StaticCategory>().TryOne(idCat);
                });

            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public bool AddComment(Comment comment)
        {
            bool result = false;
            try
            {
                dbService.Run(db =>
                {
                    db.GetRepository<Comment>().Add(comment);
                });
                result = true;
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;
        }

        public bool AddSku(Sku sku)
        {
            bool result = false;
            try
            {
                dbService.Run(db =>
                {
                    db.GetRepository<Sku>().Add(sku);
                });
                result = true;
            }
            catch (Exception err)
            {
                result = false;
                logger.Error(err.Message);
            }
            return result;
        }

        public bool AddSpecificationToSKU(long id, long specId, string specValue)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var sku = db.GetRepository<Sku>().TryOne(id);
                    var spec = db.GetRepository<StaticSpecification>().TryOne(specId);
                    if (sku != null && spec != null)
                    {
                        if (sku.listSpecification == null)
                        {
                            sku.listSpecification = new List<Specification>();
                        }

                        if (sku.listSpecification.Any(s => s.staticspec.id == spec.id))
                        {
                            result = false;
                        }
                        else
                        {
                            sku.listSpecification.Add(new Specification() { staticspec = spec, skuId = sku.id, value = specValue });
                            db.GetRepository<Sku>().Update(sku);
                            result = true;  
                        }

                    }
                });

            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public bool RemoveSpecificationFromSKU(long id, long idSpec)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var spec = db.GetRepository<Specification>().TryOne(idSpec);
                    if (spec != null)
                    {
                        db.GetRepository<Specification>().Remove(spec);
                        result = true;
                    }
                });

            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public bool AddCategoryToSKU(long id, long idCat)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var sku = db.GetRepository<Sku>().TryOne(id);
                    var category = db.GetRepository<StaticCategory>().TryOne(idCat);
                    if (sku!=null&&category!=null)
                    {
                        if (sku.listCategory == null)
                        {
                            sku.listCategory=new List<Category>();
                        }

                        if (sku.listCategory.Any(s => s.staticcat.id == category.id))
                        {
                            result = false;
                        }
                        else
                        {

                            sku.listCategory.Add(new Category() {staticcat = category, skuId = sku.id});
                            db.GetRepository<Sku>().Update(sku);
                            result = true;
                        }
                    }
                });

            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public bool RemoveSKUFromCategory(long id,long idCat)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {
                    var cat = db.GetRepository<Category>().TryOne(idCat);
                    if (cat!=null)
                    {
                        db.GetRepository<Category>().Remove(cat);
                        result = true;
                    }
                });

            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }
        
        public List<Sku> ListSkuByCategory(StaticCategory cat)
        {
            var result = new List<Sku>();
            try
            {
                dbService.Run(db =>
                {
                    result = ((SkuRepository)db.GetRepository<Sku>()).ListSkuByCategory(cat).ToList();
                });

            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }
        
        public List<CartState> ListCartState()
        {
            var result = new List<CartState>();
            try
            {
                dbService.Run(db =>
                {
                    result = db.GetRepository<CartState>().All();
                });

            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public Cart AddorUpdateSkuToCart(Cart cart)
        {
            Cart result = null;
            try
            {
                dbService.Run(db =>
                {
                    var cartDB = db.GetRepository<Cart>().TryOne(cart.id);
                    if (cartDB == null)
                    {
                        result = db.GetRepository<Cart>().Add(cart);   
                    }
                    else
                    {
                        cartDB.id = cart.id;
                        cartDB.name = cart.name;
                        cartDB.nameClient = cart.nameClient;
                        cartDB.phone = cart.phone;
                        cartDB.email = cart.email;
                        cartDB.city = cart.city;
                        cartDB.street = cart.street;
                        cartDB.numHome = cart.numHome;
                        cartDB.numFlat = cart.numFlat;
                        cartDB.comment = cart.comment;
                        cartDB.state = cart.state;
                        //cartDB.createDate = cart.createDate;
                        if (cart.listSku.Any())
                        {
                            cartDB.totalCount = cart.listSku.Sum(s => s.count);
                        }
                        else
                        {
                            cartDB.totalCount = cartDB.listSku.Sum(s => s.count);
                        }
                        db.GetRepository<Cart>().Update(cartDB);
                        result = cart;
                    }

                });


                    foreach (var item in cart.listSku)
                    {
                        dbService.Run(db =>
                        {
                            db.GetRepository<CartItem>().Remove(item);
                        });
                    }

            

                dbService.Run(db =>
                {
                    foreach (var item in cart.listSku)
                    {
                        item.CartId = result.id;
                    }
                    db.GetRepository<CartItem>().AddMany(cart.listSku.ToList());
                });
            }
            catch (Exception err)
            {
                result = cart;
                logger.Error(err.Message);
            }
            return result;
        }

       public Cart GetCartById(long idCart)
        {
            Cart result = null;
            try
            {
                dbService.Run(db =>
                {
                    result = db.GetRepository<Cart>().TryOne(idCart);
                });
                dbService.Run(db =>
                {
                    result.listSku = ((CartItemRepository)db.GetRepository<CartItem>()).AllByCartId(result.id).ToList();
                });

            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }

        public List<Cart> GetCartsByDateAndStatus(DateTime start, DateTime end, int state)
        {
            var result = new List<Cart>();
            try
            {
                dbService.Run(db =>
                {
                    var cState = ListCartState().First(s => s.value == state);
                    result = ((CartRepository)db.GetRepository<Cart>()).GetCartsByDateAndStatus(start, end, cState).ToList();
                });

            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result;
        }


        public bool RemoveSkuCartId(long idCart, long idsku)
        {
            var result = false;
            try
            {
                dbService.Run(db =>
                {

                    result = ((CartItemRepository)db.GetRepository<CartItem>()).RemoveSkuCartId(idCart, idsku);
                });

            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }
            return result; 
        }
      }
}

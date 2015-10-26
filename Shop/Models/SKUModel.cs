using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Shop.db.Entities;

namespace Shop.Models
{
    public class SKUModel:MenuModel
    {

        [DisplayName("ид")]
        public long id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [DisplayName("Название")]
        public string name { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [DisplayName("Цена")]
        public decimal price { get; set; }

        [DisplayName("Спец. цена")]
        public decimal priceAct { get; set; }

        public long brandId { get; set; }
        [DisplayName("Бренд")]
        public string brandName { get; set; }

        public long categotyId { get; set; }
        [DisplayName("Категория")]
        public string categotyName { get; set; }

        [DisplayName("Описание")]
        public string description { get; set; }

        
        public long smalPhotoId { get; set; }
        [DisplayName("Фотография")]
        public string smalPhotoPath { get; set; }



        public IList<PhotoBig> listPhoto { get; set; }
        public IList<Category> listCategory { get; set; }
        public IList<Specification> listSpecification { get; set; }
        public IList<Comment> listComment { get; set; }

        public List<StaticCategory> listStaticCategory { get; set; }
        public List<StaticSpecification> listStaticSpecification { get; set; }
        public List<Brand> listStaticBrand { get; set; }



        public SKUModel()
        {
            price = 0;
            priceAct = 0;
            brandId = 0;
            brandName = string.Empty;
            description = string.Empty;
            smalPhotoId = 0;
            smalPhotoPath = string.Empty;
            categotyId = 0;
            categotyName = string.Empty;
            listPhoto=new List<PhotoBig>();
            listCategory=new List<Category>();
            listSpecification=new List<Specification>();
            listComment=new List<Comment>();
            listStaticCategory=new List<StaticCategory>();
            listStaticSpecification = new List<StaticSpecification>();
            listStaticBrand = new List<Brand>();
        }
        public Sku GetSKUDB()
        {
            return new Sku() { id = this.id, name = this.name, brand = new Brand() { id = this.brandId, name = this.brandName }, description = this.description, price = this.price, priceAct = this.priceAct, smalPhoto =  /*new Photo() { id = this.smalPhotoId, path = this.smalPhotoPath, skuId = this.id}*/null };
        }

       
    }
}

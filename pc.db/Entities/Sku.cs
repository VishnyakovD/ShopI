using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class Sku:idName
    {
        public virtual decimal price { get; set; }
        public virtual decimal priceAct { get; set; }
        public virtual Brand brand { get; set; }
        public virtual string description { get; set; }
        public virtual Photo smalPhoto { get; set; }
        public virtual IList<PhotoBig> listPhoto { get; set; }
        public virtual IList<Category> listCategory { get; set; }
        public virtual IList<Specification> listSpecification { get; set; }
        public virtual IList<Comment> listComment { get; set; }

        public Sku()
        {
            brand=new Brand();
            smalPhoto=new Photo();
            listPhoto=new List<PhotoBig>();
            listCategory=new List<Category>();
            listSpecification=new List<Specification>();
            listComment = new List<Comment>();
        }

    }
}

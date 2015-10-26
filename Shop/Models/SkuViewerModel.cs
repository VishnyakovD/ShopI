using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class SkuViewerModel:MenuModel
    {
        public long IdCat { set; get; }
        public string Name { set; get; }
        public List<ShortSKUModel> skuList { set; get; }

        public SkuViewerModel()
        {
            skuList=new List<ShortSKUModel>();
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class Photo : idName
    {
        public virtual long skuId { get; set; }
        public virtual string path { get; set; }

        public Photo()
        {

        }
    }
}

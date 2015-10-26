using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using Shop.db.Entities;

namespace Shop.db.Repository
{
    public class SkuRepository : Repository<Sku>
    {
        public SkuRepository(ISession session)
            : base(session)
        {
        }

        public IEnumerable<Sku> ListSkuByCategory(StaticCategory cat)
        {
            var ids = session.QueryOver<Category>().Where(i => i.staticcat == cat).List().Select(i => i.skuId).ToArray();
            var retval = session.QueryOver<Sku>().Where(s => s.id.IsIn(ids)).List();
            return retval;
        }

        public IEnumerable<Sku> AllByCategory(Category category)
        {
            return session.QueryOver<Sku>()
                .Where(a => (a.listCategory.Contains(category)))
                .List();
        }

        public IEnumerable<Sku> AllByBrand(Brand brand)
        {
            return session
                .QueryOver<Sku>()
                .Where(a => a.brand==brand)
                .List();
        }

    }
}

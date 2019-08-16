using System;
using System.Collections.Generic;
using System.Text;
using WebShop.Messaging;
using WebShop.Products.Common.Dtos;

namespace WebShop.Products.Common.Queries
{
    public class GetProductsQuery: IQuery<IEnumerable<ProductViewDto>>
    {
    }
}

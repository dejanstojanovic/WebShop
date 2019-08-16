using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShop.Common.Validation;
using WebShop.Products.Common.Commands;
using WebShop.Products.Common.Dtos;
using WebShop.Products.Common.Queries;

namespace WebShop.Products.ApiControllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewDto>>> GetProducts([FromQuery]GetProductsQuery query)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetailsViewDto>> GetProduct([FromRoute]Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody]CreateProductCommand product)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct([FromBody]UpdateProductCommand productUpdate)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct([FromRoute]DeleteProductCommand productDelete)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{id}/image")]
        public async Task<ActionResult> SetProductImage(
            [FromRoute]Guid id,
            [FromForm(Name = "photo"), AllowedFileTypes(fileTypes: new String[] { ".jpg", ".jpeg" })]IFormFile file
            )
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}/image")]
        public async Task<ActionResult> DeleteProductImage([FromRoute]DeleteProductImageCommand productImage)
        {
            throw new NotImplementedException();
        }
    }
}

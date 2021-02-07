using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InventoryManager.API.Data.Dto.Product;
using InventoryManager.API.Data.Interfaces;
using InventoryManager.API.Data.Models;
using System;

namespace InventoryManager.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public ProductController(IProductService productService, IMapper mapper, ILoggerManager logger)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get products list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ProductDto>> Get()
        {
            _logger.LogInfo("Getting all products");

            var products = await _productService.GetProducts();
            var dto = _mapper.Map<List<ProductDto>>(products);

            _logger.LogInfo($"Returning {products.Count} products");

            return Ok(dto);
        }

        /// <summary>
        /// Get product details
        /// </summary>
        /// <param name="productId">The model.</param>
        /// <returns></returns>
        [HttpGet("{productId}")]
        public async Task<ActionResult<ProductDto>> Get([FromRoute] int productId)
        {
            _logger.LogInfo("Getting product by id");

            var product = await _productService.GetProduct(productId);
            if (product == null)
                return NotFound();

            var dto = _mapper.Map<ProductDto>(product);

            _logger.LogInfo($"Returning product {product.Id}");

            return Ok(dto);
        }

        /// <summary>
        /// Add new product
        /// </summary>
        /// <param name="product">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Product product)
        {
            _logger.LogInfo("Creating new product");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _productService.AddProduct(product);

            _logger.LogInfo($"Product id = {product.Id} created");

            return Ok();
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="product">The model.</param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPut("{productId}")]
        public async Task<ActionResult> Put([FromBody] Product product, [FromRoute] int productId)
        {
            _logger.LogInfo($"Updating product id = {productId}");

            var result = await _productService.UpdateProduct(product, productId);

            if (result)
            {
                return Ok();
            }

            _logger.LogInfo($"Updated product id = {productId}");

            return NotFound();
        }

        /// <summary>
        /// Remove product
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <returns></returns>
        [HttpDelete("{productId}")]
        public async Task<ActionResult<bool>> Delete([FromRoute] int productId)
        {
            _logger.LogInfo($"Deleting product id = {productId}");

            return await _productService.RemoveProduct(productId);
        }
    }
}

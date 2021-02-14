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
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            _logger.LogInfo("Getting all products");

            var products = await _productService.GetProducts();
            var dto = _mapper.Map<List<ResponseDto>>(products);

            _logger.LogInfo($"Returning {products.Count} products");

            return Ok(dto);
        }

        /// <summary>
        /// Get product details
        /// </summary>
        [HttpGet("{productId}")]
        public async Task<ActionResult<ResponseDto>> Get([FromRoute] int productId)
        {
            _logger.LogInfo("Getting product by id");

            var product = await _productService.GetProduct(productId);
            if (product == null)
                return NotFound();

            var dto = _mapper.Map<ResponseDto>(product);

            _logger.LogInfo($"Returning product {product.Id}");

            return Ok(dto);
        }

        /// <summary>
        /// Add new product
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RequestDto request)
        {
            _logger.LogInfo("Creating new product");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = _mapper.Map<Product>(request);

            await _productService.AddProduct(product);

            _logger.LogInfo($"Product id = {product.Id} created");

            return Ok();
        }

        /// <summary>
        /// Update product
        /// </summary>
        [HttpPut("{productId}")]
        public async Task<ActionResult> Put([FromBody] RequestDto request, [FromRoute] int productId)
        {
            _logger.LogInfo($"Updating product id = {productId}");

            var product = _mapper.Map<Product>(request);

            var result = await _productService.UpdateProduct(product, productId);

            if (result)
            {
                _logger.LogInfo($"Updated product id = {productId}");

                return Ok();
            }

            _logger.LogInfo($"Product not found product id = {productId}");

            return NotFound();
        }

        /// <summary>
        /// Remove product
        /// </summary>
        [HttpDelete("{productId}")]
        public async Task<ActionResult<bool>> Delete([FromRoute] int productId)
        {
            _logger.LogInfo($"Deleting product id = {productId}");

            return await _productService.RemoveProduct(productId);
        }
    }
}

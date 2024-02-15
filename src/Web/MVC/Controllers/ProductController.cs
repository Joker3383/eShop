using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services.Interfaces;
using Newtonsoft.Json;

namespace MVC.Controllers;

public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public ProductController(IProductService productService, IMapper mapper)
        {
             _productService = productService;
             _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _productService.GetAllProductsAsync();
            List<ProductDto> productList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));

            return View(productList);
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductRequest addProductRequest)
        {
            var product = _mapper.Map<ProductDto>(addProductRequest);
            await _productService.CreateProductsAsync(product);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var response = await _productService.GetProductByIdAsync(Id);
            var product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            var viewProduct = _mapper.Map<ProductRequest>(product);
            return View(viewProduct);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(ProductRequest updateProductRequest)
        {
            
            var product = _mapper.Map<ProductDto>(updateProductRequest);
            await _productService.UpdateProductsAsync(product);
            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductsAsync(id);
            return RedirectToAction("Index");
        }
    }
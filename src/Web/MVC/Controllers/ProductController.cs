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

        public async Task<IActionResult> Index()
        {
            var response = await _productService.GetAllProductsAsync();
            if (!response.IsSuccess)
            {
                TempData["error"] = response.Message;
                return RedirectToAction("Error", "Home");
            }

            return View(JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result)));
        }

        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductRequest addProductRequest)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            var product = _mapper.Map<ProductDto>(addProductRequest);
            var response = await _productService.CreateProductsAsync(product);
            if (!response.IsSuccess)
            {
                TempData["error"] = response.Message;
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int Id)
        {
            var response = await _productService.GetProductByIdAsync(Id);
            if (!response.IsSuccess)
            {
                TempData["error"] = response.Message;
                return RedirectToAction("Error", "Home");
            }
            var product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

            if (product == null)
            {
                return RedirectToAction("Edit");
            }

            var viewProduct = _mapper.Map<ProductRequest>(product);
            return View(viewProduct);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(ProductRequest updateProductRequest)
        {
            
            var product = _mapper.Map<ProductDto>(updateProductRequest);
            var response = await _productService.UpdateProductsAsync(product);
            if (!response.IsSuccess)
            {
                TempData["error"] = response.Message;
                return RedirectToAction("Error", "Home"); 
            }
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productService.DeleteProductsAsync(id);
            if (!response.IsSuccess)
            {
                TempData["error"] = response.Message;
                return RedirectToAction("Error", "Home"); 
            }
            return RedirectToAction("Index");
        }
    }
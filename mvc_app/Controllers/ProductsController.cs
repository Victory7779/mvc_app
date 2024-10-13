using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using mvc_app.Models;
//using mvc_app.Service;


    public class ProductsController : Controller
    {
        private readonly IServiceProduct? _serviceProduct;
        private readonly ProductContext? _productContext;
        public ProductsController(IServiceProduct? serviceProduct, ProductContext? productContext)
        {
            _serviceProduct = serviceProduct;
            _productContext = productContext;
            _serviceProduct._productContext = _productContext;
        }

        //Get: http://localhost:[port]/products
        public ViewResult Index()
        {
            var products = _serviceProduct?.Read();
            return View(products);
        }
        //Get: http://localhost:[port]/products/details/{id}
        public ViewResult Details(int id) => View(_serviceProduct?.GetById(id));
    //POST: http://localhost:[port]/products/create
    [HttpGet]
    [Authorize(Roles = "MyAdminRole")]
        public ViewResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Price,Discription")]Product product)
        {
            if (ModelState.IsValid)
            {
                _ = _serviceProduct?.Craete(product);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(product);
            }
        }
    //POST: http://localhost:[port]/products/update/{id}
    [HttpGet]
    [Authorize(Roles = "MyAdminRole")]
    public ActionResult Update() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, [Bind("Id,Name,Price,Discription")] Product product)
        {
            if (ModelState.IsValid)
            {
                _ = _serviceProduct?.Update(id, product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
    //POST: http://localhost:[port]/products/delete/{id}
    [HttpGet]
    [Authorize(Roles = "MyAdminRole")]
    public ActionResult Delete() => View();
    [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
           _= _serviceProduct?.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }


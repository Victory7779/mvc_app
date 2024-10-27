using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using mvc_app.Models;
//using mvc_app.Service;


    public class ProductsController : Controller
    {
        private readonly IServiceProduct? _serviceProduct;
        public ProductsController(IServiceProduct? serviceProduct)
        {
            _serviceProduct = serviceProduct;
        }

        //Get: http://localhost:[port]/products
        public async Task<ViewResult> Index()
        {
            var products = await _serviceProduct.ReadAsync();
            return View(products);
        }
    //Get: http://localhost:[port]/products/details/{id}
    public async Task<ViewResult> Details(int id)
    {
        var product = await _serviceProduct.GetByIdAsync(id);
       // ViewBag.Name =
        return View(product);
    }
    //POST: http://localhost:[port]/products/create
    [HttpGet]
    [Authorize(Roles = "admin")]
        public ViewResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Discription")]Product product)
        {
            if (ModelState.IsValid)
            {
                await _serviceProduct.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(product);
            }
        }
    //POST: http://localhost:[port]/products/update/{id}
    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult Update() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [Bind("Id,Name,Price,Discription")] Product product)
        {
            if (ModelState.IsValid)
            {
                 await _serviceProduct.UpdateAsync(id, product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
    //POST: http://localhost:[port]/products/delete/{id}
    [HttpGet]
    [Authorize(Roles = "admin")]
    public ActionResult Delete() => View();
    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
           await _serviceProduct.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }


using Microsoft.EntityFrameworkCore;
using mvc_app.Models;


    public interface IServiceProduct 
    {
        public ProductContext? _productContext { get; set; }
        public Product? Craete(Product? product);
        public IEnumerable<Product>? Read();
        public Product? GetById(int id);
        public Product? Update(int id, Product? product);
        public bool Delete(int id);
    }

    public class ServiceProducts : IServiceProduct
    {
        public ProductContext? _productContext { get; set; }

        public Product? Craete(Product? product)
        {
            _productContext?.Add(product);
            _productContext?.SaveChanges();
            return product;
        }

        public bool Delete(int id)
        {
            var product = _productContext?.Products
                .FirstOrDefault(p => p.Id == id);
            if(product==null)
            {
                return false;
            }
            else
            {
                _productContext?.Products.Remove(product);
                _productContext?.SaveChanges();
                return true;
            }
        }

        public Product? GetById(int id)
        {
            var product = _productContext?.Products
                .FirstOrDefault(p => p.Id == id);
            return product;
        }

        public IEnumerable<Product>? Read()
        {
            return _productContext?.Products.ToList();
        }

        public Product? Update(int id, Product? product)
        {
           if(id!=product?.Id)
            {
                return null;
            }
            else
            {
                try
                {
                    _productContext?.Products.Update(product);
                    _productContext?.SaveChanges();
                    return product;
                }
                catch(DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    return null;
                }
            }
            
        }
    }


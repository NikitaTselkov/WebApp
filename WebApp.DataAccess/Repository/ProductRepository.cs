using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Data;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;

namespace WebApp.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Update(Product product)
        {
            //var objFromDb = _dbContext.Products.FirstOrDefault(p => p.Id == product.Id);
            //if (objFromDb != null)
            //{
            //    objFromDb.Title = product.Title;
            //    objFromDb.Description = product.Description;
            //    objFromDb.ISBN = product.ISBN;
            //    objFromDb.Price = product.Price;
            //    objFromDb.Price100 = product.Price100;
            //    objFromDb.Price50 = product.Price50;
            //    objFromDb.ListPrice = product.ListPrice;
            //    objFromDb.CategoryId = product.CategoryId;
            //    objFromDb.Author = product.Author;

            //    if (!string.IsNullOrEmpty(product.ImageUrl))
            //        objFromDb.ImageUrl = product.ImageUrl;
            //}

            _dbContext.Update(product);
        }
    }
}

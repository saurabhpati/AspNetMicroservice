using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
        }

        public Task CreateProduct(Product product)
        {
            return _catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var result = await _catalogContext.Products.DeleteOneAsync(id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var result = await _catalogContext.Products.Find<Product>(p => true).ToListAsync();
            return result;
        }

        public Task<Product> GetProducts(string id)
        {
            return _catalogContext.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryName(string category)
        {
            var result = await _catalogContext.Products
                .Find(Builders<Product>.Filter.Eq(p => p.Category, category))
                .ToListAsync();

            return result;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var result = await _catalogContext.Products.ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);

            return result.IsAcknowledged && result.IsModifiedCountAvailable;
        }
    }
}

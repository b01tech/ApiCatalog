using ApiCatalog.Data;
using ApiCatalog.Repositories.Interfaces;

namespace ApiCatalog.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProductRepository _productRepo;
        private ICategoryRepository _categoryRepo;
        public AppDbContext Context { get; set; }

        public UnitOfWork(AppDbContext context)
        {
            Context = context;
        }

        public IProductRepository productRepository
        {
            get { return _productRepo ?? new ProductRepository(Context); }
        }
        public ICategoryRepository categoryRepository
        {
            get { return _categoryRepo ?? new CategoryRepository(Context); }
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}

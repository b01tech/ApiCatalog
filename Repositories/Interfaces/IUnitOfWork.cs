namespace ApiCatalog.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IProductRepository productRepository { get; }
        ICategoryRepository categoryRepository { get; }

        void Commit();
    }
}

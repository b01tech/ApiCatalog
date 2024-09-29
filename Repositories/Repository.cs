using ApiCatalog.Data;
using ApiCatalog.Pagination;
using ApiCatalog.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApiCatalog.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public PageList<T> GetList(PageParameter pageParams)
    {
        var items = GetAll().AsQueryable();
        var itemsPaged = PageList<T>.ToPageList(items, pageParams.PageSize, pageParams.PageNumber);
        return itemsPaged;
    }

    public IEnumerable<T> GetAll()
    {
       return _context.Set<T>().AsNoTracking().ToList();
    }
    public T? Get(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().AsNoTracking().FirstOrDefault(expression);
    }
    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity;
    }
    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);        
        return entity;
    }
    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);       
        return entity;
    }

}


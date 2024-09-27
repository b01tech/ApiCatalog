using ApiCatalog.Data;
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

    public IEnumerable<T> GetAll()
    {
       return _context.Set<T>().AsNoTracking().Take(10).ToList();
    }
    public T? Get(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().AsNoTracking().FirstOrDefault(expression);
    }
    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        _context.SaveChanges();
        return entity;
    }
    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        _context.SaveChanges();
        return entity;
    }
    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        _context.SaveChanges();
        return entity;
    }

}


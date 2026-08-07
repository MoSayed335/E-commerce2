using System.Linq.Expressions;

namespace ECommerce2.Utilities
{
    /// <summary>
    /// عمليات CRUD المشتركة بين كل الـ Repositories، عشان متكررش نفس الكود.
    /// أي Repository متخصص (زي IProductRepository) بيورث من هنا ويضيف بس
    /// الاستعلامات الخاصة بيه (ISP - الـ Interface الأساسي فاضل صغير ومركّز).
    /// </summary>
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }

    /// <summary>
    /// Unit of Work بيضمن إن كل التغييرات (في أكتر من Repository) تتحفظ في
    /// Transaction واحدة، مثلًا: إنشاء Order + تقليل Stock + مسح Cart كلهم مع بعض
    /// أو كلهم يفشلوا مع بعض.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
    }
}

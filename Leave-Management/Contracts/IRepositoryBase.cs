using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Management.Contracts
{
    /*
     * Section #4, class #9 of Udemy
     * CRUD base creation
     */
    public interface IRepositoryBase <T> where T : class
    {
        ICollection<T> Findall();
        T FindByID(int id);
        bool isExists(int id);
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        bool Save();
    }
}

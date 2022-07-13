using BriefResume.DataBase;
using BriefResume.IService;
using System;
using System.Linq.Expressions;


namespace BriefResume.Service
{
    /// <summary>
    /// 业务处理服务具体实现。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly UserDbContext _userDbContext;
        public BaseService(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        /// <summary>
        /// 添加数据。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Create(T entity)
        {
            _userDbContext.Add(entity);
            var result =  _userDbContext.SaveChanges();
            return result>0;
        }

        /// <summary>
        /// 删除数据。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(T entity) 
        {
            _userDbContext.Remove(entity);
            var result = _userDbContext.SaveChanges();
            return result > 0;
        }
        /// <summary>
        /// 获取单个数据。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Find(Guid id)
        {
            T ObjectFromRepo = _userDbContext.Find<T>(id);
            var result = _userDbContext.SaveChanges();
            return ObjectFromRepo;
        }
        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            //此处entity必须可追踪
            _userDbContext.Update<T>(entity);
            var result = _userDbContext.SaveChanges();
            return result > 0;
        }
    }
}
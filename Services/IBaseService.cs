using System.Linq.Expressions;

namespace BriefResume.IService
{
    /// <summary>
    /// 业务处理服务基接口。
    /// </summary>
    public interface IBaseService<T> where T : class
    {
        /// <summary>
        /// 添加数据。
        /// </summary>
        /// <param name="entity">数据实体。</param>
        /// <returns>是否添加成功。</returns>
        bool Create(T entity);

        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <param name="entity">数据实体。</param>
        /// <returns>是否更新成功。</returns>
        bool Update(T entity);

        /// <summary>
        /// 删除数据。
        /// </summary>
        /// <param name="entity">数据实体。</param>
        /// <returns>是否删除成功。</returns>
        bool Delete(T entity);

        /// <summary>
        /// 获取单个数据。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Find(string id);
    }
}
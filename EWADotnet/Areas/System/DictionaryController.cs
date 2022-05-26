using EWA.Sugar;
using EWADotnet.Authorize;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace EWADotnet.Areas.System
{
    [ApiController]
    [Route("/api/system/dictionary")]
    public class DictionaryController : BaseController
    {
        public DictionaryController(ISqlSugarClient _db) : base(_db)
        {
        }

        /// <summary>
        /// 查询全部字典
        /// </summary>
        /// <returns></returns>
        [PreAuthorize("sys:dict:list"), OperLog("字典管理", "查询全部字典")]
        public async Task<CommonResult> Get()
        {
            var exp = Expressionable.Create<SysDictionary>();
            exp.And(x => x.deleted == 0);
            var list = await db.Queryable<SysDictionary>().Where(exp.ToExpression()).OrderBy(x => x.sortNumber).ToListAsync();
            return Result.Success(list);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <returns></returns>
        [PreAuthorize("sys:dict:save"), OperLog("字典管理", "添加字典")]
        public async Task<CommonResult> Post(SysDictionary dictionary)
        {
            dictionary.createTime = DateTime.Now;
            var row = await db.Insertable(dictionary).ExecuteCommandAsync();
            return Result.Judge(row > 0);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [PreAuthorize("sys:dict:update"), OperLog("字典管理", "修改字典")]
        public async Task<CommonResult> Put(SysDictionary dictionary)
        {
            dictionary.updateTime = DateTime.Now;
            var row = await db.Updateable(dictionary).ExecuteCommandAsync();
            return Result.Judge(row > 0);
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dictDataId"></param>
        /// <returns></returns>
        [PreAuthorize("sys:dict:remove"), OperLog("字典管理", "删除字典")]
        public async Task<CommonResult> Delete([FromRoute] int dictId)
        {
            //先查询有无子项
            var anyson = await db.Queryable<SysDictionaryData>().Where(x => x.dictId == dictId && x.deleted == 0).AnyAsync();
            if (anyson)
            {
                return Result.Error("项目包含子项，不可直接删除！");
            }
            var row = await db.Updateable<SysDictionary>()
                .SetColumns(x => new SysDictionary() { deleted = 1, updateTime = DateTime.Now })
                .Where(x => x.dictId == dictId)
                .ExecuteCommandAsync();
            return Result.Judge(row > 0);
        }

    }
}

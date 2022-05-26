using EWA.Sugar;
using EWADotnet.Authorize;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace EWADotnet.Areas.System
{
    [ApiController]
    [Route("/api/system/dictionary-data")]
    public class DictionaryDataController : BaseController
    {
        public DictionaryDataController(ISqlSugarClient _db) : base(_db)
        {

        }

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [PreAuthorize("sys:dict:list"), OperLog("字典数据管理", "分页查询字典数据")]
        public async Task<CommonPageResult> GetPage([FromQuery] SysDictionaryDataParam param)
        {
            var exp = Expressionable.Create<SysDictionaryData>();
            exp.And(x => x.deleted == 0);
            exp.AndIF(param.dictId != null, x => x.dictId == param.dictId);
            exp.AndIF(!string.IsNullOrEmpty(param.keywords), x => x.dictDataCode.Contains(param.keywords.Trim()) || x.dictDataName.Contains(param.keywords.Trim()));
            RefAsync<int> total = 0;
            var list = await db.Queryable<SysDictionaryData>()
                .Where(exp.ToExpression())
                .OrderBy(x => x.sortNumber)
                .ToPageListAsync(param.page, param.limit, total);
            return Result.PageSuccess(list, total);
        }

        /// <summary>
        /// 查询单个
        /// </summary>
        /// <param name="dictCode"></param>
        /// <returns></returns>
        [PreAuthorize("sys:dict:list"), OperLog("字典数据管理", "查询全部字典数据")]
        public async Task<CommonResult> Get([FromQuery] string dictCode)
        {
            var list = await db.Queryable<SysDictionary>()
                .LeftJoin<SysDictionaryData>((a, b) => a.dictId == b.dictId)
                .Where((a, b) => a.dictCode == dictCode)
                .Select((a, b) => new
                {
                    comments = b.comments ?? "",
                    createTime = b.createTime,
                    updateTime = b.updateTime,
                    deleted = b.deleted,
                    dictDataCode = b.dictDataCode,
                    dictDataId = b.dictDataId,
                    dictDataName = b.dictDataName,
                    sortNumber = b.sortNumber,
                    dictCode = a.dictCode,
                    dictId = a.dictId,
                    dictName = a.dictName
                })
                .ToListAsync();
            return Result.Success(list);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <returns></returns>
        [PreAuthorize("sys:dict:save"), OperLog("字典数据管理", "添加字典数据")]
        public async Task<CommonResult> Post(SysDictionaryData dictionarydata)
        {
            var any = await db.Queryable<SysDictionaryData>()
                .Where(x => x.dictDataCode == dictionarydata.dictDataCode && x.dictId == dictionarydata.dictId && x.dictDataId != dictionarydata.dictDataId)
                .AnyAsync();
            if (any)
            {
                return Result.Error("已存在相同字典标识！");
            }
            dictionarydata.createTime = DateTime.Now;
            var row = await db.Insertable(dictionarydata).ExecuteCommandAsync();
            return Result.Judge(row > 0);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [PreAuthorize("sys:dict:update"), OperLog("字典数据管理", "修改字典数据")]
        public async Task<CommonResult> Put(SysDictionaryData dictionarydata)
        {
            var any = await db.Queryable<SysDictionaryData>()
                .Where(x => x.dictDataCode == dictionarydata.dictDataCode && x.dictId == dictionarydata.dictId && x.dictDataId != dictionarydata.dictDataId)
                .AnyAsync();
            if (any)
            {
                return Result.Error("已存在相同字典标识！");
            }
            dictionarydata.updateTime = DateTime.Now;
            var row = await db.Updateable(dictionarydata).ExecuteCommandAsync();
            return Result.Judge(row > 0);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dictDataId"></param>
        /// <returns></returns>
        [PreAuthorize("sys:dict:remove"), OperLog("字典数据管理", "删除字典数据")]
        public async Task<CommonResult> Delete([FromRoute] int dictDataId)
        {
            var row = await db.Updateable<SysDictionaryData>()
                .SetColumns(x => new SysDictionaryData() { deleted = 1, updateTime = DateTime.Now })
                .Where(x => x.dictDataId == dictDataId)
                .ExecuteCommandAsync();
            return Result.Judge(row > 0);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [PreAuthorize("sys:dict:remove"), OperLog("字典数据管理", "批量删除字典数据")]
        public async Task<CommonResult> DeleteBatch([FromBody] List<int> ids)
        {
            var row = await db.Updateable<SysDictionaryData>()
                .SetColumns(x => new SysDictionaryData() { deleted = 1, updateTime = DateTime.Now })
                .Where(x => ids.Contains(x.dictDataId))
                .ExecuteCommandAsync();
            return Result.Judge(row > 0);
        }
    }
}

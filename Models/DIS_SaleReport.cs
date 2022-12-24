using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
namespace Models
{

    [SQLite.Table("DISSaleReport")]
    [JsonObject(MemberSerialization.OptOut)]
    public class DIS_SaleReport
    {
        [JsonProperty(PropertyName = "序")]
        public string 序 { get; set; } = "";

        [JsonProperty(PropertyName = "单据状态")]

        public string 单据状态 { get; set; } = "";

        [JsonProperty(PropertyName = "单据类型")]

        public string 单据类型 { get; set; } = "";

        [JsonProperty(PropertyName = "单据来源")]

        public string 单据来源 { get; set; } = "";

        [JsonProperty(PropertyName = "原始订单号")]

        public string 原始订单号 { get; set; } = "";

        [JsonProperty(PropertyName = "订单单号")]

        public string 订单单号 { get; set; } = "";

        [JsonProperty(PropertyName = "单据项次")]

        public string 单据项次 { get; set; } = "";

        [JsonProperty(PropertyName = "订单日期")]

        public string 订单日期 { get; set; } = "";

        [JsonProperty(PropertyName = "导购员")]

        public string 导购员 { get; set; } = "";

        [JsonProperty(PropertyName = "导购员名称")]

        public string 导购员名称 { get; set; } = "";

        [JsonProperty(PropertyName = "业务员")]

        public string 业务员 { get; set; } = "";

        [JsonProperty(PropertyName = "业务员名称")]

        public string 业务员名称 { get; set; } = "";

        [JsonProperty(PropertyName = "所属部门")]

        public string 所属部门 { get; set; } = "";

        [JsonProperty(PropertyName = "所属部门名称")]

        public string 所属部门名称 { get; set; } = "";

        [JsonProperty(PropertyName = "所属组织")]

        public string 所属组织 { get; set; } = "";

        [JsonProperty(PropertyName = "所属组织名称")]

        public string 所属组织名称 { get; set; } = "";

        [JsonProperty(PropertyName = "发货组织")]

        public string 发货组织 { get; set; } = "";

        [JsonProperty(PropertyName = "发货组织名称")]

        public string 发货组织名称 { get; set; } = "";

        [JsonProperty(PropertyName = "代理公司名称")]

        public string 代理公司名称 { get; set; } = "";

        [JsonProperty(PropertyName = "门店")]

        public string 门店 { get; set; } = "";

        [JsonProperty(PropertyName = "门店名称")]

        public string 门店名称 { get; set; } = "";

        [JsonProperty(PropertyName = "销售片区")]

        public string 销售片区 { get; set; } = "";

        [JsonProperty(PropertyName = "销售片区名称")]

        public string 销售片区名称 { get; set; } = "";

        [JsonProperty(PropertyName = "项目")]

        public string 项目 { get; set; } = "";

        [JsonProperty(PropertyName = "项目名称")]

        public string 项目名称 { get; set; } = "";

        [JsonProperty(PropertyName = "渠道商")]

        public string 渠道商 { get; set; } = "";

        [JsonProperty(PropertyName = "渠道商名称")]

        public string 渠道商名称 { get; set; } = "";

        [JsonProperty(PropertyName = "渠道板块划分")]

        public string 渠道板块划分 { get; set; } = "";

        [JsonProperty(PropertyName = "总部渠道分类")]

        public string 总部渠道分类 { get; set; } = "";

        [JsonProperty(PropertyName = "总部渠道分类名称")]

        public string 总部渠道分类名称 { get; set; } = "";

        [JsonProperty(PropertyName = "顾客姓名")]

        public string 顾客姓名 { get; set; } = "";

        [JsonProperty(PropertyName = "手机号")]

        public string 手机号 { get; set; } = "";

        [JsonProperty(PropertyName = "顾客邮箱")]

        public string 顾客邮箱 { get; set; } = "";

        [JsonProperty(PropertyName = "家庭电话")]

        public string 家庭电话 { get; set; } = "";

        [JsonProperty(PropertyName = "地址")]

        public string 地址 { get; set; } = "";

        [JsonProperty(PropertyName = "预约日期")]

        public string 预约日期 { get; set; } = "";

        [JsonProperty(PropertyName = "出库日期")]

        public string 出库日期 { get; set; } = "";

        [JsonProperty(PropertyName = "IMS出库单号")]

        public string IMS出库单号 { get; set; } = "";

        [JsonProperty(PropertyName = "入库日期")]

        public string 入库日期 { get; set; } = "";

        [JsonProperty(PropertyName = "IMS入库单号")]

        public string IMS入库单号 { get; set; } = "";

        [JsonProperty(PropertyName = "配送单号")]

        public string 配送单号 { get; set; } = "";

        [JsonProperty(PropertyName = "单据性质")]

        public string 单据性质 { get; set; } = "";

        [JsonProperty(PropertyName = "发货状态")]

        public string 发货状态 { get; set; } = "";

        [JsonProperty(PropertyName = "退货状态")]

        public string 退货状态 { get; set; } = "";

        [JsonProperty(PropertyName = "票据型号")]

        public string 票据型号 { get; set; } = "";

        [JsonProperty(PropertyName = "票据型号名称")]

        public string 票据型号名称 { get; set; } = "";

        [JsonProperty(PropertyName = "票据销售码")]

        public string 票据销售码 { get; set; } = "";

        [JsonProperty(PropertyName = "票据价格")]

        public string 票据价格 { get; set; } = "";

        [JsonProperty(PropertyName = "销售指导价")]

        public string 销售指导价 { get; set; } = "";

        [JsonProperty(PropertyName = "建议销售价")]

        public string 建议销售价 { get; set; } = "";

        [JsonProperty(PropertyName = "实际型号")]

        public string 实际型号 { get; set; } = "";

        [JsonProperty(PropertyName = "实际型号名称")]

        public string 实际型号名称 { get; set; } = "";

        [JsonProperty(PropertyName = "实际销售码")]

        public string 实际销售码 { get; set; } = "";

        [JsonProperty(PropertyName = "特征值")]

        public string 特征值 { get; set; } = "";

        [JsonProperty(PropertyName = "特征值说明")]

        public string 特征值说明 { get; set; } = "";

        [JsonProperty(PropertyName = "数量")]

        public string 数量 { get; set; } = "";

        [JsonProperty(PropertyName = "已送数量")]

        public string 已送数量 { get; set; } = "";

        [JsonProperty(PropertyName = "未送数量")]

        public string 未送数量 { get; set; } = "";

        [JsonProperty(PropertyName = "实际价格")]

        public string 实际价格 { get; set; } = "";

        [JsonProperty(PropertyName = "金额")]

        public string 金额 { get; set; } = "";

        [JsonProperty(PropertyName = "差额")]

        public string 差额 { get; set; } = "";

        [JsonProperty(PropertyName = "渠道SKU")]

        public string 渠道SKU { get; set; } = "";

        [JsonProperty(PropertyName = "活动方案")]

        public string 活动方案 { get; set; } = "";

        [JsonProperty(PropertyName = "活动方案说明")]

        public string 活动方案说明 { get; set; } = "";

        [JsonProperty(PropertyName = "套餐编号")]

        public string 套餐编号 { get; set; } = "";

        [JsonProperty(PropertyName = "品类")]

        public string 品类 { get; set; } = "";

        [JsonProperty(PropertyName = "品类说明")]

        public string 品类说明 { get; set; } = "";

        [JsonProperty(PropertyName = "商品类型")]

        public string 商品类型 { get; set; } = "";

        [JsonProperty(PropertyName = "商品状态")]

        public string 商品状态 { get; set; } = "";

        [JsonProperty(PropertyName = "出库仓库")]

        public string 出库仓库 { get; set; } = "";

        [JsonProperty(PropertyName = "出库仓库名称")]

        public string 出库仓库名称 { get; set; } = "";

        [JsonProperty(PropertyName = "库存管理特征")]

        public string 库存管理特征 { get; set; } = "";

        [JsonProperty(PropertyName = "出库SN码")]

        public string 出库SN码 { get; set; } = "";

        [JsonProperty(PropertyName = "入库SN码")]

        public string 入库SN码 { get; set; } = "";

        [JsonProperty(PropertyName = "行号")]

        public string 行号 { get; set; } = "";

        [JsonProperty(PropertyName = "平库码")]

        public string 平库码 { get; set; } = "";

        [JsonProperty(PropertyName = "平库否")]

        public string 平库否 { get; set; } = "";

        [JsonProperty(PropertyName = "对账码")]

        public string 对账码 { get; set; } = "";

        [JsonProperty(PropertyName = "结算状态")]

        public string 结算状态 { get; set; } = "";

        [JsonProperty(PropertyName = "最近结算日期")]

        public string 最近结算日期 { get; set; } = "";

        [JsonProperty(PropertyName = "最近结算人员")]

        public string 最近结算人员 { get; set; } = "";

        [JsonProperty(PropertyName = "最近结算人员名称")]

        public string 最近结算人员名称 { get; set; } = "";

        [JsonProperty(PropertyName = "定金所在")]

        public string 定金所在 { get; set; } = "";

        [JsonProperty(PropertyName = "现金收款")]

        public string 现金收款 { get; set; } = "";

        [JsonProperty(PropertyName = "待收货款")]

        public string 待收货款 { get; set; } = "";

        [JsonProperty(PropertyName = "审核人")]

        public string 审核人 { get; set; } = "";

        [JsonProperty(PropertyName = "审核人名称")]

        public string 审核人名称 { get; set; } = "";

        [JsonProperty(PropertyName = "返券额")]

        public string 返券额 { get; set; } = "";

        [JsonProperty(PropertyName = "返利类型")]

        public string 返利类型 { get; set; } = "";

        [JsonProperty(PropertyName = "扣率")]

        public string 扣率 { get; set; } = "";

        [JsonProperty(PropertyName = "介绍来源")]

        public string 介绍来源 { get; set; } = "";

        [JsonProperty(PropertyName = "介绍来源电话")]

        public string 介绍来源电话 { get; set; } = "";

        [JsonProperty(PropertyName = "创建人")]

        public string 创建人 { get; set; } = "";

        [JsonProperty(PropertyName = "创建人名称")]

        public string 创建人名称 { get; set; } = "";

        [JsonProperty(PropertyName = "创建日期")]

        public string 创建日期 { get; set; } = "";

        [JsonProperty(PropertyName = "退换货处理方")]

        public string 退换货处理方 { get; set; } = "";

        [JsonProperty(PropertyName = "拒审原因说明")]

        public string 拒审原因说明 { get; set; } = "";

        [JsonProperty(PropertyName = "备注")]

        public string 备注 { get; set; } = "";

        [JsonProperty(PropertyName = "其他备注")]

        public string 其他备注 { get; set; } = "";

        [JsonProperty(PropertyName = "核算工资月份")]

        public string 核算工资月份 { get; set; } = "";

        [JsonProperty(PropertyName = "待补状态")]

        public string 待补状态 { get; set; } = "";

        [JsonProperty(PropertyName = "实际价格1")]

        public string 实际价格1 { get; set; } = "";

        [JsonProperty(PropertyName = "厨房是否已有烟机或灶具")]

        public string 厨房是否已有烟机或灶具 { get; set; } = "";

    }

    [JsonObject(MemberSerialization.OptOut)]
    public class DIS_SaleReport_Header
    {
        [JsonProperty(PropertyName = "单据状态")]

        public string 单据状态 { get; set; } = "";

        [JsonProperty(PropertyName = "单据类型")]

        public string 单据类型 { get; set; } = "";

        [JsonProperty(PropertyName = "单据来源")]

        public string 单据来源 { get; set; } = "";

        [JsonProperty(PropertyName = "原始订单号")]

        public string 原始订单号 { get; set; } = "";

        [JsonProperty(PropertyName = "订单单号")]

        public string 订单单号 { get; set; } = "";

        [JsonProperty(PropertyName = "单据项次")]

      

        public string 订单日期 { get; set; } = "";

        [JsonProperty(PropertyName = "导购员")]

        public string 导购员 { get; set; } = "";

        [JsonProperty(PropertyName = "导购员名称")]

        public string 导购员名称 { get; set; } = "";

        [JsonProperty(PropertyName = "业务员")]

        public string 业务员 { get; set; } = "";

        [JsonProperty(PropertyName = "业务员名称")]

        public string 业务员名称 { get; set; } = "";

        [JsonProperty(PropertyName = "所属部门")]

        public string 所属部门 { get; set; } = "";

        [JsonProperty(PropertyName = "所属部门名称")]

        public string 所属部门名称 { get; set; } = "";

        [JsonProperty(PropertyName = "所属组织")]

        public string 所属组织 { get; set; } = "";

        [JsonProperty(PropertyName = "所属组织名称")]

        public string 所属组织名称 { get; set; } = "";

        [JsonProperty(PropertyName = "发货组织")]

        public string 发货组织 { get; set; } = "";

        [JsonProperty(PropertyName = "发货组织名称")]

        public string 发货组织名称 { get; set; } = "";

        [JsonProperty(PropertyName = "代理公司名称")]

        public string 代理公司名称 { get; set; } = "";

        [JsonProperty(PropertyName = "门店")]

        public string 门店 { get; set; } = "";

        [JsonProperty(PropertyName = "门店名称")]

        public string 门店名称 { get; set; } = "";

        [JsonProperty(PropertyName = "销售片区")]

        public string 销售片区 { get; set; } = "";

        [JsonProperty(PropertyName = "销售片区名称")]

        public string 销售片区名称 { get; set; } = "";

        [JsonProperty(PropertyName = "项目")]

        public string 项目 { get; set; } = "";

        [JsonProperty(PropertyName = "项目名称")]

        public string 项目名称 { get; set; } = "";

        [JsonProperty(PropertyName = "渠道商")]

        public string 渠道商 { get; set; } = "";

        [JsonProperty(PropertyName = "渠道商名称")]

        public string 渠道商名称 { get; set; } = "";

        [JsonProperty(PropertyName = "渠道板块划分")]

        public string 渠道板块划分 { get; set; } = "";

        [JsonProperty(PropertyName = "总部渠道分类")]

        public string 总部渠道分类 { get; set; } = "";

        [JsonProperty(PropertyName = "总部渠道分类名称")]

        public string 总部渠道分类名称 { get; set; } = "";

        [JsonProperty(PropertyName = "顾客姓名")]

        public string 顾客姓名 { get; set; } = "";

        [JsonProperty(PropertyName = "手机号")]

        public string 手机号 { get; set; } = "";

        [JsonProperty(PropertyName = "顾客邮箱")]

        public string 顾客邮箱 { get; set; } = "";

        [JsonProperty(PropertyName = "家庭电话")]

        public string 家庭电话 { get; set; } = "";

        [JsonProperty(PropertyName = "地址")]

        public string 地址 { get; set; } = "";
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class DIS_SaleReport_Entity
    {
        [JsonProperty(PropertyName = "单据项次")]
        public string 单据项次 { get; set; } = "";

        [JsonProperty(PropertyName = "预约日期")]

        public string 预约日期 { get; set; } = "";

        [JsonProperty(PropertyName = "出库日期")]

        public string 出库日期 { get; set; } = "";

        [JsonProperty(PropertyName = "IMS出库单号")]

        public string IMS出库单号 { get; set; } = "";

        [JsonProperty(PropertyName = "入库日期")]

        public string 入库日期 { get; set; } = "";

        [JsonProperty(PropertyName = "IMS入库单号")]

        public string IMS入库单号 { get; set; } = "";

        [JsonProperty(PropertyName = "配送单号")]

        public string 配送单号 { get; set; } = "";

        [JsonProperty(PropertyName = "单据性质")]

        public string 单据性质 { get; set; } = "";

        [JsonProperty(PropertyName = "发货状态")]

        public string 发货状态 { get; set; } = "";

        [JsonProperty(PropertyName = "退货状态")]

        public string 退货状态 { get; set; } = "";

        [JsonProperty(PropertyName = "票据型号")]

        public string 票据型号 { get; set; } = "";

        [JsonProperty(PropertyName = "票据型号名称")]

        public string 票据型号名称 { get; set; } = "";

        [JsonProperty(PropertyName = "票据销售码")]

        public string 票据销售码 { get; set; } = "";

        [JsonProperty(PropertyName = "票据价格")]

        public string 票据价格 { get; set; } = "";

        [JsonProperty(PropertyName = "销售指导价")]

        public string 销售指导价 { get; set; } = "";

        [JsonProperty(PropertyName = "建议销售价")]

        public string 建议销售价 { get; set; } = "";

        [JsonProperty(PropertyName = "实际型号")]

        public string 实际型号 { get; set; } = "";

        [JsonProperty(PropertyName = "实际型号名称")]

        public string 实际型号名称 { get; set; } = "";

        [JsonProperty(PropertyName = "实际销售码")]

        public string 实际销售码 { get; set; } = "";

        [JsonProperty(PropertyName = "特征值")]

        public string 特征值 { get; set; } = "";

        [JsonProperty(PropertyName = "特征值说明")]

        public string 特征值说明 { get; set; } = "";

        [JsonProperty(PropertyName = "数量")]

        public string 数量 { get; set; } = "";

        [JsonProperty(PropertyName = "已送数量")]

        public string 已送数量 { get; set; } = "";

        [JsonProperty(PropertyName = "未送数量")]

        public string 未送数量 { get; set; } = "";

        [JsonProperty(PropertyName = "实际价格")]

        public string 实际价格 { get; set; } = "";

        [JsonProperty(PropertyName = "金额")]

        public string 金额 { get; set; } = "";

        [JsonProperty(PropertyName = "差额")]

        public string 差额 { get; set; } = "";

        [JsonProperty(PropertyName = "渠道SKU")]

        public string 渠道SKU { get; set; } = "";

        [JsonProperty(PropertyName = "活动方案")]

        public string 活动方案 { get; set; } = "";

        [JsonProperty(PropertyName = "活动方案说明")]

        public string 活动方案说明 { get; set; } = "";

        [JsonProperty(PropertyName = "套餐编号")]

        public string 套餐编号 { get; set; } = "";

        [JsonProperty(PropertyName = "品类")]

        public string 品类 { get; set; } = "";

        [JsonProperty(PropertyName = "品类说明")]

        public string 品类说明 { get; set; } = "";

        [JsonProperty(PropertyName = "商品类型")]

        public string 商品类型 { get; set; } = "";

        [JsonProperty(PropertyName = "商品状态")]

        public string 商品状态 { get; set; } = "";

        [JsonProperty(PropertyName = "出库仓库")]

        public string 出库仓库 { get; set; } = "";

        [JsonProperty(PropertyName = "出库仓库名称")]

        public string 出库仓库名称 { get; set; } = "";

        [JsonProperty(PropertyName = "库存管理特征")]

        public string 库存管理特征 { get; set; } = "";

        [JsonProperty(PropertyName = "出库SN码")]

        public string 出库SN码 { get; set; } = "";

        [JsonProperty(PropertyName = "入库SN码")]

        public string 入库SN码 { get; set; } = "";

        [JsonProperty(PropertyName = "行号")]

        public string 行号 { get; set; } = "";

        [JsonProperty(PropertyName = "平库码")]

        public string 平库码 { get; set; } = "";

        [JsonProperty(PropertyName = "平库否")]

        public string 平库否 { get; set; } = "";

        [JsonProperty(PropertyName = "对账码")]

        public string 对账码 { get; set; } = "";

        [JsonProperty(PropertyName = "结算状态")]

        public string 结算状态 { get; set; } = "";

        [JsonProperty(PropertyName = "最近结算日期")]

        public string 最近结算日期 { get; set; } = "";

        [JsonProperty(PropertyName = "最近结算人员")]

        public string 最近结算人员 { get; set; } = "";

        [JsonProperty(PropertyName = "最近结算人员名称")]

        public string 最近结算人员名称 { get; set; } = "";

        [JsonProperty(PropertyName = "定金所在")]

        public string 定金所在 { get; set; } = "";

        [JsonProperty(PropertyName = "现金收款")]

        public string 现金收款 { get; set; } = "";

        [JsonProperty(PropertyName = "待收货款")]

        public string 待收货款 { get; set; } = "";

        [JsonProperty(PropertyName = "审核人")]

        public string 审核人 { get; set; } = "";

        [JsonProperty(PropertyName = "审核人名称")]

        public string 审核人名称 { get; set; } = "";

        [JsonProperty(PropertyName = "返券额")]

        public string 返券额 { get; set; } = "";

        [JsonProperty(PropertyName = "返利类型")]

        public string 返利类型 { get; set; } = "";

        [JsonProperty(PropertyName = "扣率")]

        public string 扣率 { get; set; } = "";

        [JsonProperty(PropertyName = "介绍来源")]

        public string 介绍来源 { get; set; } = "";

        [JsonProperty(PropertyName = "介绍来源电话")]

        public string 介绍来源电话 { get; set; } = "";

        [JsonProperty(PropertyName = "创建人")]

        public string 创建人 { get; set; } = "";

        [JsonProperty(PropertyName = "创建人名称")]

        public string 创建人名称 { get; set; } = "";

        [JsonProperty(PropertyName = "创建日期")]

        public string 创建日期 { get; set; } = "";

        [JsonProperty(PropertyName = "退换货处理方")]

        public string 退换货处理方 { get; set; } = "";

        [JsonProperty(PropertyName = "拒审原因说明")]

        public string 拒审原因说明 { get; set; } = "";

        [JsonProperty(PropertyName = "备注")]

        public string 备注 { get; set; } = "";

        [JsonProperty(PropertyName = "其他备注")]

        public string 其他备注 { get; set; } = "";

        [JsonProperty(PropertyName = "核算工资月份")]

        public string 核算工资月份 { get; set; } = "";

        [JsonProperty(PropertyName = "待补状态")]

        public string 待补状态 { get; set; } = "";

        [JsonProperty(PropertyName = "实际价格1")]

        public string 实际价格1 { get; set; } = "";

        [JsonProperty(PropertyName = "厨房是否已有烟机或灶具")]

        public string 厨房是否已有烟机或灶具 { get; set; } = "";
    }
}

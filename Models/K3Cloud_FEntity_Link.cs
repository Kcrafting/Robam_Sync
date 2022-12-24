namespace Models
{
    public class K3Cloud_FEntity_Link
    {
        /// <summary>
        /// 单据转换内码，必录，（在转换规则设计界面中的唯一标识）
        /// </summary>
        public string FRuleId { get; set; }
        /// <summary>
        /// 需关联的上游单据实体表名，必录，（注意大小写敏感，需跟表定义表t_bf_tableDefine中的表编码一致）
        /// </summary>
        public string FSTableName { get; set; }
        /// <summary>
        /// 上游单据内码，必录
        /// </summary>
        public int FSBillId { get; set; }
        /// <summary>
        /// 关联的上游单据实体内码，必录，（一般为分录内码）
        /// </summary>
        public string FSId { get; set; }
        /// <summary>
        /// 上游字段携带值，有则必录（根据转换规则字段映射对应的上游字段）
        /// </summary>
        public decimal RemainQtyOld { get; set; }
        /// <summary>
        /// 控制字段的反写值，有则必录（单据关联配置中的控制字段）
        /// </summary>
        public decimal RemainQty { get; set; }
        /// <summary>
        /// 上游字段携带值，有则必录（根据转换规则字段映射对应的上游字段）
        /// </summary>
        public decimal BaseQtyOld { get; set; }
        /// <summary>
        /// 控制字段的反写值，有则必录（单据关联配置中的控制字段）
        /// </summary>
        public decimal BaseQty { get; set; }
    }
}

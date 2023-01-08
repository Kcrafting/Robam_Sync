using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robam_Sync.Models
{
    //internal class Sqlite_Models_Result_TableMessage
    //{
    //}


    public class Result_TableMessage_ColumnType
    {
        public string name { get; set; }
        public string dataIndex { get; set; }
        public string key { get; set; }
        public bool resizable { get; set; } = true;
        public string type { get; set; }
        public int width { get; set; }
        //public string formatter { get; set; }
        public string headerCellClass { get; set; }
        //public string headerRenderer { get; set; }
    }
    public class Result_TableMessage_RowData
    {
        public string key { get; set; }
        public int index { get; set; }
        public string errorTime { get; set; }
        public bool isError { get; set; }
        public string description { get; set; }

    }

    public class Result_TableMessage_syncMessage
    {
        public bool IsDone { get; set; }
        public string Tips { get; set; }
    }
    public class Sqlite_Models_Result_TableMessage
    {
        public List<Result_TableMessage_ColumnType> columnType { get; set; } = new List<Result_TableMessage_ColumnType> {
            new Result_TableMessage_ColumnType(){
                name = "序号",
                dataIndex = "index",
                key = "index",
                resizable = true,
                width = 30,
                //formatter = "FUNCTION_FLAG function formatter ({ row, onRowChange, isCellSelected }) {\r\n        return (\r\n          <SelectCellFormatter\r\n            value={row.available}\r\n            onChange={() => {\r\n              onRowChange({ ...row, available: !row.available });\r\n            }}\r\n            isCellSelected={isCellSelected}\r\n          />\r\n        );\r\n      }",
                headerCellClass = "filter-cell",
            },
            new Result_TableMessage_ColumnType(){
                name = "错误时间",
                dataIndex = "errorTime",
                key = "errorTime",
                resizable = true,
                width = 250,
                //formatter = "FUNCTION_FLAG function formatter ({ row, onRowChange, isCellSelected }) {\r\n        return (\r\n          <SelectCellFormatter\r\n            value={row.available}\r\n            onChange={() => {\r\n              onRowChange({ ...row, available: !row.available });\r\n            }}\r\n            isCellSelected={isCellSelected}\r\n          />\r\n        );\r\n      }",
                headerCellClass = "filter-cell",

            },
            new Result_TableMessage_ColumnType(){
                name = "是否错误",
                dataIndex = "isError",
                key = "isError",
                resizable = true,
                width = 250,
                //formatter = "FUNCTION_FLAG function formatter ({ row, onRowChange, isCellSelected }) {\r\n        return (\r\n          <SelectCellFormatter\r\n            value={row.available}\r\n            onChange={() => {\r\n              onRowChange({ ...row, available: !row.available });\r\n            }}\r\n            isCellSelected={isCellSelected}\r\n          />\r\n        );\r\n      }",
                headerCellClass = "filter-cell",
                //headerRenderer = "FUNCTION_FLAG function headerRenderer(row){\r\n                                  return ( <>\r\n                                    <div style={{\r\n                                        height:'35px',\r\n                                        maxHeight:'35px',\r\n                                        flex:1,\r\n                                        lineHeight:'35px',\r\n                                        width:'100%',\r\n                                        borderBlockEnd:'1px solid',\r\n                                        paddingBlock:'0px',\r\n                                        paddingInline:'8px',\r\n                                        borderBlockEndColor:'var(--rdg-border-color)'\r\n                                        }}>\r\n                                    <span><b>{row.column.name}</b></span>\r\n                                    </div>\r\n                                    <div style={{height:'35px',maxHeight:'35px',flex:1,lineHeight:'35px',paddingBlock:'0px',paddingInline:'8px',padding:'1px'}}>\r\n                          <Select\r\n                                defaultValue=\"all\"\r\n                                style={{ width:'100%' }}\r\n                                onChange={(value: string)=>{\r\n                                    if(value==='all'){\r\n                                    dispatch({ type: 'ColumnsDataAction_Act', value: data.rowData/*.filter((item)=>(item.isError?.includes(txt.currentTarget.value as string)))*/});\r\n                                    }else if (value === 'checked'){\r\n                                        dispatch({ type: 'ColumnsDataAction_Act', value: data.rowData.filter((item)=>(item.isError))});\r\n                                    }else if(value === 'unchecked'){\r\n                                        dispatch({ type: 'ColumnsDataAction_Act', value: data.rowData.filter((item)=>(!item.isError))});\r\n                                    }\r\n                                }}\r\n                                options={[\r\n                                {\r\n                                    label: '单选',\r\n                                    options: [\r\n                                    { label: '勾选', value: 'checked' },\r\n                                    { label: '未勾选', value: 'unchecked' },\r\n                                    ],\r\n                                },\r\n                                {\r\n                                    label: '全选',\r\n                                    options: [{ label: '全选', value: 'all' }],\r\n                                },\r\n                                ]}\r\n                            />\r\n                                    </div>\r\n                                    </>)\r\n                              }",
            },
            new Result_TableMessage_ColumnType(){
                name = "信息",
                dataIndex = "description",
                key = "description",
                resizable = true,
                width = 250,
                //formatter = "FUNCTION_FLAG function formatter ({ row, onRowChange, isCellSelected }) {console.log('row__ ',row);}",
                headerCellClass = "filter-cell",
            },
        };
        public List<Result_TableMessage_RowData> rowData { get; set; }
        public Result_TableMessage_syncMessage syncMessage { get; set; }

    }
}

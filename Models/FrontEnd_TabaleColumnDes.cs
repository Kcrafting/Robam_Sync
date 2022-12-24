namespace Models
{
    public class FrontEnd_TabaleColumnDes
    {
        [SQLite.Column("field")]
        public string FField { get; set; }
        [SQLite.Column("headerName")]
        public string FHeaderName { get; set; } 
        [SQLite.Column("type")]
        public string FType { get; set; } = "string";
        [SQLite.Column("width")] 
        public int FWidth { get; set; } = 100;
        [SQLite.Column("editable")]
        public bool FEditable { get; set; } = false;
    }
}

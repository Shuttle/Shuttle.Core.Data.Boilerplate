using System.Data;

namespace Shuttle.Core.Data.Boilerplate
{
    public class Columns
    {
        public static readonly Column<string> ColumnName = new Column<string>("COLUMN_NAME", DbType.AnsiString);
        public static readonly Column<string> DataType = new Column<string>("DATA_TYPE", DbType.AnsiString);
        public static readonly Column<string> IsNullable = new Column<string>("IS_NULLABLE", DbType.AnsiString);
        public static readonly Column<int> OrdinalPosition = new Column<int>("ORDINAL_POSITION", DbType.Int32);
    }
}
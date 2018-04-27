using System.Data;

namespace Shuttle.Core.Data.Boilerplate
{
    public class Columns
    {
        public static readonly MappedColumn<string> ColumnName = new MappedColumn<string>("COLUMN_NAME", DbType.AnsiString);
        public static readonly MappedColumn<string> DataType = new MappedColumn<string>("DATA_TYPE", DbType.AnsiString);
        public static readonly MappedColumn<string> IsNullable = new MappedColumn<string>("IS_NULLABLE", DbType.AnsiString);
        public static readonly MappedColumn<int> OrdinalPosition = new MappedColumn<int>("ORDINAL_POSITION", DbType.Int32);
    }
}
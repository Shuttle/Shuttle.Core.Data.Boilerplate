using System;
using System.CodeDom;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.CSharp;
using Shuttle.Core.Contract;

namespace Shuttle.Core.Data.Boilerplate
{
    public class Column
    {
        private readonly string _dataType;

        public Column(DataRow row)
        {
            Guard.AgainstNull(row, "row");

            _dataType = Columns.DataType.MapFrom(row);
            ColumnName = Columns.ColumnName.MapFrom(row);
            IsNullable = Columns.IsNullable.MapFrom(row).Equals("YES", StringComparison.InvariantCultureIgnoreCase);
            OrdinalPosition = Columns.OrdinalPosition.MapFrom(row);
        }

        public string ColumnName { get; }
        public bool IsNullable { get; }
        public int OrdinalPosition { get; }

        public Type SystemType()
        {
            var dbType = new SqlParameter("x", (SqlDbType) Enum.Parse(typeof(SqlDbType), _dataType, true)).DbType;

            var metaClrType = Type.GetType(
                "System.Data.SqlClient.MetaType, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                true,
                true
            );

            var metaType = metaClrType.InvokeMember(
                "GetMetaTypeFromDbType",
                BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic,
                null,
                null,
                new object[] {dbType}
            );

            return (Type) metaClrType.InvokeMember(
                "ClassType",
                BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                metaType,
                null
            );
        }

        public string SystemTypeName()
        {
            return SystemType().Name;
        }

        public string MappedName(bool camelCase = false)
        {
            var result = (OrdinalPosition == 1 && ColumnName.EndsWith("_ID")
                ? "Id"
                : (ColumnName.EndsWith("ID")
                    ? string.Concat(ColumnName.Substring(0, ColumnName.Length - 2), "Id")
                    : ColumnName)).Replace("_", string.Empty);

            if (camelCase)
            {
                return $"{result.Substring(0, 1).ToLower()}{result.Substring(1)}";
            }

            return result;
        }

        public string DbType()
        {
            var dbType = new SqlParameter("x", (SqlDbType) Enum.Parse(typeof(SqlDbType), _dataType, true)).DbType.ToString();

            if (dbType.Equals("String", StringComparison.CurrentCultureIgnoreCase))
            {
                dbType = "AnsiString";
            }

            return dbType;
        }

        public string CSharpTypeName()
        {
            using (var provider = new CSharpCodeProvider())
            {
                return provider.GetTypeOutput(new CodeTypeReference(SystemType()));
            }
        }

        public string Nullable(string typeName)
        {
            return string.Concat(typeName, IsNullable && !typeName.Equals("string", StringComparison.InvariantCultureIgnoreCase) ? "?" : string.Empty);
        }

        public string ColumnsClassName(string className)
        {
            Guard.AgainstNullOrEmptyString(className, "className");

            var match = ColumnName.ToLower();

            foreach (var mapping in MainView.ColumnMapping)
            {
                if (mapping.Value.Contains(match))
                {
                    return mapping.Key;
                }
            }

            return string.Concat(className, "Columns"); 
        }
    }

    internal class TypeMap
    {
        public TypeMap(string type, string dbType)
        {
            Type = type;
            DbType = dbType;
        }

        public string Type { get; }
        public string DbType { get; }
    }
}
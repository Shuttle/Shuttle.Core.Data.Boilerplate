using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shuttle.Core.Data.Boilerplate
{
    public partial class MainView : Form
    {
        internal static readonly Dictionary<string, List<string>> ColumnMapping = new Dictionary<string, List<string>>
        {
            {
                "Columns",
                new List<string>
                {
                    "Id"
                }
            }
        };

        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IDatabaseGateway _databaseGateway;
        private readonly List<string> _tables = new List<string>();

        public MainView()
        {
            InitializeComponent();

            _databaseContextFactory = new DatabaseContextFactory(new ConnectionConfigurationProvider(),  new DbConnectionFactory(), new DbCommandFactory(),
                new ThreadStaticDatabaseContextCache());
            _databaseGateway = new DatabaseGateway();
        }

        private void Connect()
        {
            if (string.IsNullOrEmpty(ConnectionString.Text))
            {
                return;
            }

            _tables.Clear();

            using (_databaseContextFactory.Create("System.Data.SqlClient", ConnectionString.Text))
            {
                foreach (
                    var row in
                    _databaseGateway.GetRowsUsing(
                        RawQuery.Create("select TABLE_NAME from INFORMATION_SCHEMA.TABLES order by 1")))
                {
                    _tables.Add(row[0].ToString());
                }
            }

            FilterTables();
        }

        private void Table_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateCode();
        }

        private void GenerateOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateCode();
        }

        private void GenerateCode()
        {
            Result.Text = string.Empty;

            if (string.IsNullOrEmpty(Table.Text)
                ||
                string.IsNullOrEmpty(GenerateOption.Text))
            {
                return;
            }

            switch (GenerateOption.Text.ToLower())
            {
                case "mappedcolumns":
                {
                    GenerateMappedColumns();

                    break;
                }
                case "mapfrom":
                {
                    GenerateMapFrom();

                    break;
                }
                case "propertymappedfrom":
                {
                    GeneratePropertyMappedFrom();

                    break;
                }
                case "contains":
                {
                    GenerateContains();

                    break;
                }
                case "select":
                {
                    GenerateSelect();

                    break;
                }
                case "insert":
                {
                    GenerateInsert();

                    break;
                }
                case "update":
                {
                    GenerateUpdate();

                    break;
                }
                case "queryclass":
                {
                    GenerateQueryClass();

                    break;
                }
            }
        }

        private void GenerateQueryClass()
        {
            var result = new StringBuilder();

            foreach (var column in GetColumns())
            {
                result.AppendLine(
                    $"public {column.Nullable(column.CSharpTypeName())} {column.MappedName()} {{ get; set; }}");
            }

            Result.Text = result.ToString();
        }

        private void GenerateContains()
        {
            var result = new StringBuilder();
            var primary = GetColumns().First();

            result.AppendLine("\t\treturn RawQuery.Create(@\"");
            result.AppendLine("if exists");
            result.AppendLine("(");
            result.AppendLine("\tselect");
            result.AppendLine("\t\tnull");
            result.AppendLine("\tfrom");
            result.AppendLine($"\t\t{TableName()}");
            result.AppendLine("\twhere");
            result.AppendLine(string.Format("\t\t{0} = @{0}", primary.ColumnName));
            result.AppendLine(")");
            result.AppendLine("\tselect 1");
            result.AppendLine("else");
            result.AppendLine("\tselect 0");

            result.AppendLine("\")");
            result.AppendLine(string.Format("\t\t.AddParameterValue({0}.{1}, {2}.{1});",
                primary.ColumnsClassName(ClassName()), primary.MappedName(), ObjectName()));

            Result.Text = result.ToString();
        }

        private void GeneratePropertyMappedFrom()
        {
            var result = new StringBuilder();

            foreach (var column in GetColumns())
            {
                result.AppendLine($"{column.MappedName()} = {column.ColumnsClassName(ClassName())}.{column.MappedName()}.MapFrom(row),");
            }

            Result.Text = result.ToString();
        }

        private void GenerateMapFrom()
        {
            var result = new StringBuilder();

            foreach (var column in GetColumns())
            {
                result.AppendLine($"{column.ColumnsClassName(ClassName())}.{column.MappedName()}.MapFrom(row),");
            }

            Result.Text = result.ToString();
        }

        private void GenerateSelect()
        {
            var result = new StringBuilder();
            Column primary = null;

            result.AppendLine("\t\treturn RawQuery.Create(@\"");
            result.AppendLine("select");

            var columns = GetColumns();

            foreach (var column in columns)
            {
                if (column.OrdinalPosition == 1)
                {
                    primary = column;
                }

                result.AppendLine(
                    $"\t{column.ColumnName}{(column.OrdinalPosition < columns.Count ? "," : string.Empty)}");
            }

            result.AppendLine("from");
            result.AppendLine($"\t{Table.Text}");
            result.AppendLine("where");
            result.AppendLine(string.Format("\t{0} = @{0}", primary.ColumnName));
            result.AppendLine("\")");
            result.AppendLine(
                $"\t\t.AddParameterValue({primary.ColumnsClassName(ClassName())}.{primary.MappedName()}, id);");

            Result.Text = result.ToString();
        }

        private void GenerateInsert()
        {
            var result = new StringBuilder();
            var values = new StringBuilder();

            result.AppendLine($"\t\tGuard.AgainstNull({ObjectName()}, nameof({ObjectName()}));");
            result.AppendLine();
            result.AppendLine("\t\treturn RawQuery.Create(@\"");
            result.AppendLine($"insert into {Table.Text}");
            result.AppendLine("(");

            var columns = GetColumns();

            foreach (var column in columns)
            {
                //if (column.OrdinalPosition == 1)
                //{
                //    continue;
                //}

                result.AppendLine(
                    $"\t{column.ColumnName}{(column.OrdinalPosition < columns.Count ? "," : string.Empty)}");

                values.AppendLine(
                    $"\t@{column.ColumnName}{(column.OrdinalPosition < columns.Count ? "," : string.Empty)}");
            }

            result.AppendLine(")");
            result.AppendLine("values");
            result.AppendLine("(");
            result.Append(values);
            result.AppendLine(");");
            //result.AppendLine("select cast(scope_identity() as int);");
            result.AppendLine("\")");

            foreach (var column in columns)
            {
                result.AppendLine(string.Format("\t\t\t.AddParameterValue({0}.{1}, {2}.{1}){3}",
                    column.ColumnsClassName(ClassName()), column.MappedName(), ObjectName(),
                    column.OrdinalPosition < columns.Count ? string.Empty : ";"));
            }

            Result.Text = result.ToString();
        }

        private void GenerateUpdate()
        {
            var result = new StringBuilder();

            Column primary = null;

            result.AppendLine($"\t\tGuard.AgainstNull({ObjectName()}, nameof({ObjectName()}));");
            result.AppendLine();
            result.AppendLine("\t\treturn RawQuery.Create(@\"");
            result.AppendLine("update");
            result.AppendLine($"\t{Table.Text}");
            result.AppendLine("set");

            var columns = GetColumns();

            foreach (var column in columns)
            {
                if (column.OrdinalPosition == 1)
                {
                    primary = column;

                    continue;
                }

                result.AppendLine(string.Format("\t{0} = @{0}{1}", column.ColumnName,
                    column.OrdinalPosition < columns.Count ? "," : string.Empty));
            }

            result.AppendLine("where");
            result.AppendLine(string.Format("\t{0} = @{0}", primary.ColumnName));
            result.AppendLine("\")");

            foreach (var column in columns)
            {
                result.AppendLine(string.Format("\t\t\t.AddParameterValue({0}.{1}, {2}.{1}){3}",
                    column.ColumnsClassName(ClassName()), column.MappedName(), ObjectName(),
                    column.OrdinalPosition < columns.Count ? string.Empty : ";"));
            }

            Result.Text = result.ToString();
        }

        private string ObjectName()
        {
            var instance = ClassName();

            return string.Concat(instance.Substring(0, 1).ToLower(), instance.Substring(1));
        }

        private string ClassName()
        {
            var table = TableName();
            var pos = table.IndexOf("_", StringComparison.InvariantCultureIgnoreCase);

            return (pos > -1 ? table.Substring(pos + 1) : table).Replace("_", string.Empty);
        }

        private string TableName()
        {
            return Table.Text ?? string.Empty;
        }

        private void GenerateMappedColumns()
        {
            var result = new StringBuilder();

            foreach (var column in GetColumns())
            {
                if (IsMappedColumn(column.ColumnName))
                {
                    continue;
                }

                result.AppendLine(
                    string.Format(
                        "public static readonly MappedColumn<{0}> {1} = new MappedColumn<{0}>(\"{2}\", DbType.{3});",
                        column.Nullable(column.CSharpTypeName()), column.MappedName(), column.ColumnName,
                        column.DbType()));
            }

            Result.Text = result.ToString();
        }

        private bool IsMappedColumn(string columnName)
        {
            var match = columnName.ToLower();

            foreach (var mapping in ColumnMapping)
            {
                if (mapping.Value.Contains(match))
                {
                    return true;
                }
            }

            return false;
        }

        private List<Column> GetColumns()
        {
            var result = new List<Column>();

            if (string.IsNullOrEmpty(ConnectionString.Text))
            {
                return result;
            }

            using (_databaseContextFactory.Create("System.Data.SqlClient", ConnectionString.Text))
            {
                foreach (
                    var row in
                    _databaseGateway.GetRowsUsing(
                        RawQuery.Create(
                            $"select COLUMN_NAME, DATA_TYPE, IS_NULLABLE, ORDINAL_POSITION from  INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '{Table.Text}' order by ORDINAL_POSITION")))
                {
                    result.Add(new Column(row));
                }
            }

            return result;
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Result.Text);
        }

        private void Filter_TextChanged(object sender, EventArgs e)
        {
            FilterTables();
        }

        private void FilterTables()
        {
            Table.Items.Clear();

            var filter = Filter.Text.ToLower();

            foreach (var table in _tables)
            {
                if (!table.ToLower().Contains(filter))
                {
                    continue;
                }

                Table.Items.Add(table);
            }
        }

        private void Copy_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Copy.Text.ToLower())
            {
                case "classname":
                {
                    Clipboard.SetText(ClassName());
                    break;
                }
                case "objectname":
                {
                    Clipboard.SetText(ObjectName());
                    break;
                }
                case "*columns":
                {
                    Clipboard.SetText(ColumnsClassName());
                    break;
                }
            }
        }

        private string ColumnsClassName()
        {
            return string.Concat(ClassName(), "Columns");
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            Connect();
        }
    }
}
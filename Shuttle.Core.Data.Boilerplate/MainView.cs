using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Shuttle.Core.Data.Boilerplate;

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

    private readonly string _connectionStringPath;

    private readonly IDatabaseContextFactory _databaseContextFactory;
    private readonly IDatabaseGateway _databaseGateway;
    private readonly List<string> _tables = new List<string>();

    public MainView()
    {
        InitializeComponent();

        var services = new ServiceCollection();

        services.AddDataAccess();

        var provider = services.BuildServiceProvider();

        _databaseContextFactory = provider.GetRequiredService<IDatabaseContextFactory>();
        _databaseGateway = provider.GetRequiredService<IDatabaseGateway>();

        _connectionStringPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".connection-string");

        if (File.Exists(_connectionStringPath))
        {
            ConnectionString.Text = File.ReadAllText(_connectionStringPath);
        }
    }

    private string ClassName()
    {
        var table = TableName();
        var pos = table.IndexOf("_", StringComparison.InvariantCultureIgnoreCase);

        return (pos > -1 ? table.Substring(pos + 1) : table).Replace("_", string.Empty);
    }

    private string ColumnsClassName()
    {
        return string.Concat(ClassName(), "Columns");
    }

    private async Task Connect()
    {
        if (string.IsNullOrEmpty(ConnectionString.Text))
        {
            return;
        }

        _tables.Clear();

        try
        {
            using (await _databaseContextFactory.Create("Microsoft.Data.SqlClient", ConnectionString.Text))
            {
                foreach (
                    var row in
                    await _databaseGateway.GetRows("select TABLE_NAME from INFORMATION_SCHEMA.TABLES order by 1"))
                {
                    _tables.Add(row[0].ToString());
                }
            }

            FilterTables();

            SaveConnectionString();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    private async void ConnectButton_Click(object sender, EventArgs e)
    {
        await Connect();
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

    private async Task GenerateArguments()
    {
        var result = new StringBuilder();

        foreach (var column in await GetColumns())
        {
            result.Append(
                $"{column.Nullable(column.CSharpTypeName())} {column.MappedName(true)},");
        }

        Result.Text = result.ToString();
    }

    private async Task GenerateCode()
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
            case "columns":
            {
                await GenerateColumns();

                break;
            }
            case "value":
            {
                await GenerateMapFrom();

                break;
            }
            case "propertymappedfrom":
            {
                await GeneratePropertyMappedFrom();

                break;
            }
            case "contains":
            {
                await GenerateContains();

                break;
            }
            case "select":
            {
                await GenerateSelect();

                break;
            }
            case "insert":
            {
                await GenerateInsert();

                break;
            }
            case "update":
            {
                await GenerateUpdate();

                break;
            }
            case "properties":
            {
                await GenerateProperties();

                break;
            }
            case "arguments":
            {
                await GenerateArguments();

                break;
            }
            case "object":
            {
                await GenerateObject();

                break;
            }
        }
    }

    private async Task GenerateColumns()
    {
        var result = new StringBuilder();

        foreach (var column in await GetColumns())
        {
            if (IsMappedColumn(column.ColumnName))
            {
                continue;
            }

            result.AppendLine(
                $"public static readonly Column<{column.Nullable(column.CSharpTypeName())}> {column.MappedName()} = new (\"{column.ColumnName}\", DbType.{column.DbType()});");
        }

        Result.Text = result.ToString();
    }

    private async Task GenerateContains()
    {
        var result = new StringBuilder();
        var columns = await GetColumns();
        var primary = columns.First();

        result.AppendLine("\t\treturn RawQuery.Create(@\"");
        result.AppendLine("if exists");
        result.AppendLine("(");
        result.AppendLine("\tselect");
        result.AppendLine("\t\tnull");
        result.AppendLine("\tfrom");
        result.AppendLine($"\t\t{TableName()}");
        result.AppendLine("\twhere");
        result.AppendLine($"\t\t{primary.ColumnName} = @{primary.ColumnName}");
        result.AppendLine(")");
        result.AppendLine("\tselect 1");
        result.AppendLine("else");
        result.AppendLine("\tselect 0");

        result.AppendLine("\")");
        result.AppendLine($"\t\t.AddParameterValue(Columns.{primary.MappedName()}, {ObjectName()}.{primary.MappedName()});");

        Result.Text = result.ToString();
    }

    private async Task GenerateInsert()
    {
        var result = new StringBuilder();
        var values = new StringBuilder();

        result.AppendLine($"\t\tGuard.AgainstNull({ObjectName()}, nameof({ObjectName()}));");
        result.AppendLine();
        result.AppendLine("\t\treturn RawQuery.Create(@\"");
        result.AppendLine($"insert into {Table.Text}");
        result.AppendLine("(");

        var columns = await GetColumns();

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
            result.AppendLine($"\t\t\t.AddParameterValue(Columns.{column.MappedName()}, {ObjectName()}.{column.MappedName()}){(column.OrdinalPosition < columns.Count ? string.Empty : "; ")}");
        }

        Result.Text = result.ToString();
    }

    private async Task GenerateMapFrom()
    {
        var result = new StringBuilder();

        foreach (var column in await GetColumns())
        {
            result.AppendLine($"Columns.{column.MappedName()}.Value(row),");
        }

        Result.Text = result.ToString();
    }

    private async Task GenerateObject()
    {
        var result = new StringBuilder();

        result.AppendLine($"new {ClassName()} {{");

        foreach (var column in await GetColumns())
        {
            result.AppendLine(
                $"{column.MappedName()} = {column.MappedName(true)},");
        }

        result.AppendLine("}");

        Result.Text = result.ToString();
    }

    private async void GenerateOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        await GenerateCode();
    }

    private async Task GenerateProperties()
    {
        var result = new StringBuilder();

        foreach (var column in await GetColumns())
        {
            result.AppendLine(
                $"public {column.Nullable(column.CSharpTypeName())} {column.MappedName()} {{ get; set; }}");
        }

        Result.Text = result.ToString();
    }

    private async Task GeneratePropertyMappedFrom()
    {
        var result = new StringBuilder();

        foreach (var column in await GetColumns())
        {
            result.AppendLine($"{column.MappedName()} = Columns.{column.MappedName()}.Value(row),");
        }

        Result.Text = result.ToString();
    }

    private async Task GenerateSelect()
    {
        var result = new StringBuilder();
        Column primary = null;

        result.AppendLine("\t\treturn RawQuery.Create(@\"");
        result.AppendLine("select");

        var columns = await GetColumns();

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
        result.AppendLine($"\t{primary.ColumnName} = @{primary.ColumnName}");
        result.AppendLine("\")");
        result.AppendLine(
            $"\t\t.AddParameterValue(Columns.{primary.MappedName()}, id);");

        Result.Text = result.ToString();
    }

    private async Task GenerateUpdate()
    {
        var result = new StringBuilder();

        Column primary = null;

        result.AppendLine($"\t\tGuard.AgainstNull({ObjectName()}, nameof({ObjectName()}));");
        result.AppendLine();
        result.AppendLine("\t\treturn RawQuery.Create(@\"");
        result.AppendLine("update");
        result.AppendLine($"\t{Table.Text}");
        result.AppendLine("set");

        var columns = await GetColumns();

        foreach (var column in columns)
        {
            if (column.OrdinalPosition == 1)
            {
                primary = column;

                continue;
            }

            result.AppendLine($"\t{column.ColumnName} = @{column.ColumnName}{(column.OrdinalPosition < columns.Count ? "," : string.Empty)}");
        }

        result.AppendLine("where");
        result.AppendLine($"\t{primary.ColumnName} = @{primary.ColumnName}");
        result.AppendLine("\")");

        foreach (var column in columns)
        {
            result.AppendLine($"\t\t\t.AddParameterValue(Columns.{column.MappedName()}, {ObjectName()}.{column.MappedName()}){(column.OrdinalPosition < columns.Count ? string.Empty : ";")}");
        }

        Result.Text = result.ToString();
    }

    private async Task<List<Column>> GetColumns()
    {
        var result = new List<Column>();

        if (string.IsNullOrEmpty(ConnectionString.Text))
        {
            return result;
        }

        using (await _databaseContextFactory.Create("Microsoft.Data.SqlClient", ConnectionString.Text))
        {
            foreach (
                var row in
                await _databaseGateway.GetRows($"select COLUMN_NAME, DATA_TYPE, IS_NULLABLE, ORDINAL_POSITION from  INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '{Table.Text}' order by ORDINAL_POSITION"))
            {
                result.Add(new Column(row));
            }
        }

        return result;
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

    private string ObjectName()
    {
        var instance = ClassName();

        return string.Concat(instance.Substring(0, 1).ToLower(), instance.Substring(1));
    }

    private void SaveConnectionString()
    {
        File.WriteAllText(_connectionStringPath, ConnectionString.Text);
    }

    private void ShowMessage(string message)
    {
        MessageBox.Show(message, @"Message", MessageBoxButtons.OK);
    }

    private async void Table_SelectedIndexChanged(object sender, EventArgs e)
    {
        await GenerateCode();
    }

    private string TableName()
    {
        return Table.Text ?? string.Empty;
    }
}
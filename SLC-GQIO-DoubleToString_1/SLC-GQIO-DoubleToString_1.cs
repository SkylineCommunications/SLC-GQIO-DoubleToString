using System;
using Skyline.DataMiner.Analytics.GenericInterface;

[GQIMetaData(Name = "DoubleToString")]
public class DoubleToString : IGQIRowOperator, IGQIInputArguments, IGQIColumnOperator
{
    private readonly GQIColumnDropdownArgument _doubleColumnArg = new GQIColumnDropdownArgument("Double Column") { IsRequired = true, Types = new[] { GQIColumnType.Double } };
    private readonly GQIStringArgument _newColumnNameArg = new GQIStringArgument("New Column Name") { IsRequired = true };

    private string _newColumnName;
    private GQIColumn<double> _doubleColumn;
    private GQIColumn<string> stringColumn;

    public GQIArgument[] GetInputArguments()
    {
        return new GQIArgument[] { _doubleColumnArg, _newColumnNameArg };
    }

    public OnArgumentsProcessedOutputArgs OnArgumentsProcessed(OnArgumentsProcessedInputArgs args)
    {
        _newColumnName = args.GetArgumentValue(_newColumnNameArg);
        _doubleColumn = args.GetArgumentValue(_doubleColumnArg) as GQIColumn<double>;
        if (_doubleColumn != null)
            stringColumn = new GQIStringColumn($"{_newColumnNameArg}");

        return new OnArgumentsProcessedOutputArgs();
    }

    public void HandleColumns(GQIEditableHeader header)
    {
        if (stringColumn != null)
            header.AddColumns(stringColumn);
    }

    public void HandleRow(GQIEditableRow row)
    {
        if (stringColumn == null || _doubleColumn == null)
            return;

        double number = row.GetValue<double>(_doubleColumn);
        row.SetValue(stringColumn, Convert.ToString(number).Replace(",", string.Empty));
    }
}
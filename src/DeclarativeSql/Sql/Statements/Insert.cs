﻿using Cysharp.Text;
using DeclarativeSql.Mapping;



namespace DeclarativeSql.Sql.Statements
{
    /// <summary>
    /// Represents insert statement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class Insert<T> : Statement<T>, IInsert<T>
    {
        #region Properties
        /// <summary>
        /// Gets the value priority of CreatedAt column.
        /// </summary>
        private ValuePriority CreatedAtPriority { get; }
        #endregion


        #region Constructors
        /// <summary>
        /// Creates instance.
        /// </summary>
        /// <param name="createdAtPriority"></param>
        public Insert(ValuePriority createdAtPriority)
            => this.CreatedAtPriority = createdAtPriority;
        #endregion


        #region override
        /// <inheritdoc/>
        internal override void Build(DbProvider dbProvider, ref Utf16ValueStringBuilder builder, ref BindParameter bindParameter)
        {
            var bracket = dbProvider.KeywordBracket;
            var prefix = dbProvider.BindParameterPrefix;
            var table = TableInfo.Get<T>(dbProvider.Database);

            builder.Append("insert into ");
            builder.AppendLine(table.FullName);
            builder.Append("(");
            foreach (var x in table.Columns)
            {
                if (x.IsAutoIncrement)
                    continue;

                builder.AppendLine();
                builder.Append("    ");
                builder.Append(bracket.Begin);
                builder.Append(x.ColumnName);
                builder.Append(bracket.End);
                builder.Append(',');
            }
            builder.Advance(-1);
            builder.AppendLine();
            builder.AppendLine(")");
            builder.AppendLine("values");
            builder.Append("(");
            foreach (var x in table.Columns)
            {
                if (x.IsAutoIncrement)
                    continue;

                builder.AppendLine();
                builder.Append("    ");
                if (this.CreatedAtPriority == ValuePriority.Default)
                {
                    if (x.IsCreatedAt && x.DefaultValue != null)
                    {
                        builder.Append(x.DefaultValue);
                        builder.Append(',');
                        continue;
                    }
                    if (x.IsModifiedAt && x.DefaultValue != null)
                    {
                        builder.Append(x.DefaultValue);
                        builder.Append(',');
                        continue;
                    }
                }
                builder.Append(prefix);
                builder.Append(x.MemberName);
                builder.Append(',');
                //bindParameter.Add(x.MemberName, null);
            }
            builder.Advance(-1);
            builder.AppendLine();
            builder.Append(")");
        }
        #endregion
    }
}

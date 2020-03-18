﻿using DeclarativeSql.Sql;
using DeclarativeSql.Tests.Models;
using FluentAssertions;
using Xunit;



namespace DeclarativeSql.Tests.Cases
{
    public class CountTest
    {
        private DbProvider DbProvider { get; } = DbProvider.SqlServer;


        [Fact]
        public void Create()
        {
            var actual = QueryBuilder.Count<Person>().Build(this.DbProvider);
            var expect = "select count(*) as Count from [dbo].[Person]";
            actual.Statement.Should().Be(expect);
            actual.BindParameter.Should().BeNull();
        }
    }
}

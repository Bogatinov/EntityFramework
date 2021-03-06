﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Data.Entity.FunctionalTests;
using Microsoft.Data.Entity.Relational.FunctionalTests;
using Xunit;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class SqlServerGearsOfWarQueryTest : GearsOfWarQueryTestBase<SqlServerTestStore, SqlServerGearsOfWarQueryFixture>
    {
        public SqlServerGearsOfWarQueryTest(SqlServerGearsOfWarQueryFixture fixture)
            : base(fixture)
        {
        }

        public override void Include_multiple_one_to_one_and_one_to_many()
        {
            base.Include_multiple_one_to_one_and_one_to_many();

            Assert.EndsWith(
@"SELECT [w].[Id], [w].[Name], [w].[OwnerNickname], [w].[OwnerSquadId], [w].[SynergyWithId]
FROM [Weapon] AS [w]
INNER JOIN (
    SELECT DISTINCT [t].[Id], [g].[Nickname], [g].[SquadId]
    FROM [CogTag] AS [t]
    LEFT JOIN [Gear] AS [g] ON ([t].[GearNickName] = [g].[Nickname] AND [t].[GearSquadId] = [g].[SquadId])
) AS [t] ON ([w].[OwnerNickname] = [t].[Nickname] AND [w].[OwnerSquadId] = [t].[SquadId])
ORDER BY [t].[Id]", Sql);
        }

        public override void Include_multiple_one_to_one_and_one_to_many_self_reference()
        {
            base.Include_multiple_one_to_one_and_one_to_many_self_reference();

            Assert.EndsWith(
@"SELECT [g].[AssignedCityId], [g].[CityOrBirthName], [g].[FullName], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Nickname], [g].[Rank], [g].[SquadId]
FROM [Gear] AS [g]
INNER JOIN (
    SELECT DISTINCT [t].[Id], [g].[Nickname], [g].[SquadId]
    FROM [CogTag] AS [t]
    LEFT JOIN [Gear] AS [g] ON ([t].[GearNickName] = [g].[Nickname] AND [t].[GearSquadId] = [g].[SquadId])
) AS [t] ON ([g].[LeaderNickname] = [t].[Nickname] AND [g].[LeaderSquadId] = [t].[SquadId])
ORDER BY [t].[Id]", Sql);
        }

        public override void Include_multiple_one_to_one_and_one_to_one_and_one_to_many()
        {
            base.Include_multiple_one_to_one_and_one_to_one_and_one_to_many();

            Assert.EndsWith(
@"TBD", Sql);
        }

        public override void Include_multiple_one_to_one_optional_and_one_to_one_required()
        {
            base.Include_multiple_one_to_one_optional_and_one_to_one_required();

            Assert.EndsWith(
@"TBD", Sql);
        }

        public override void Include_multiple_circular()
        {
            base.Include_multiple_circular();

            Assert.EndsWith(
@"SELECT [g].[AssignedCityId], [g].[CityOrBirthName], [g].[FullName], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Nickname], [g].[Rank], [g].[SquadId]
FROM [Gear] AS [g]
INNER JOIN (
    SELECT DISTINCT [g].[Nickname], [g].[SquadId], [c].[Name]
    FROM [Gear] AS [g]
    INNER JOIN [City] AS [c] ON [g].[CityOrBirthName] = [c].[Name]
) AS [g0] ON [g].[AssignedCityId] = [g0].[Name]
ORDER BY [g].[Nickname], [g].[SquadId]", Sql);
        }

        public override void Include_multiple_circular_with_filter()
        {
            base.Include_multiple_circular_with_filter();

            Assert.EndsWith(
@"SELECT [g].[AssignedCityId], [g].[CityOrBirthName], [g].[FullName], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Nickname], [g].[Rank], [g].[SquadId]
FROM [Gear] AS [g]
INNER JOIN (
    SELECT DISTINCT [g].[Nickname], [g].[SquadId], [c].[Name]
    FROM [Gear] AS [g]
    INNER JOIN [City] AS [c] ON [g].[CityOrBirthName] = [c].[Name]
    WHERE [g].[Nickname] = @p0
) AS [g0] ON [g].[AssignedCityId] = [g0].[Name]
ORDER BY [g].[Nickname], [g].[SquadId]", Sql);
        }

        private static string Sql
        {
            get { return TestSqlLoggerFactory.Sql; }
        }
    }
}

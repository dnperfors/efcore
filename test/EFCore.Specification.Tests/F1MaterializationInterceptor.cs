﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.TestModels.ConcurrencyModel;

namespace Microsoft.EntityFrameworkCore;

public class F1MaterializationInterceptor : IMaterializationInterceptor
{
    public InterceptionResult<object> CreatingInstance(
        MaterializationInterceptionData materializationData,
        InterceptionResult<object> result)
        => materializationData.EntityType.ClrType.Name switch
        {
            nameof(Chassis) => InterceptionResult<object>.SuppressWithResult(
                new Chassis.ChassisProxy(
                    materializationData.GetPropertyValue<ILazyLoader>("_loader"),
                    materializationData.GetPropertyValue<int>(nameof(Chassis.TeamId)),
                    materializationData.GetPropertyValue<string>(nameof(Chassis.Name)))),
            nameof(Driver) => InterceptionResult<object>.SuppressWithResult(
                new Driver.DriverProxy(
                    materializationData.GetPropertyValue<ILazyLoader>("_loader"),
                    materializationData.GetPropertyValue<int>(nameof(Driver.Id)),
                    materializationData.GetPropertyValue<string>(nameof(Driver.Name)),
                    materializationData.GetPropertyValue<int?>(nameof(Driver.CarNumber)),
                    materializationData.GetPropertyValue<int>(nameof(Driver.Championships)),
                    materializationData.GetPropertyValue<int>(nameof(Driver.Races)),
                    materializationData.GetPropertyValue<int>(nameof(Driver.Wins)),
                    materializationData.GetPropertyValue<int>(nameof(Driver.Podiums)),
                    materializationData.GetPropertyValue<int>(nameof(Driver.Poles)),
                    materializationData.GetPropertyValue<int>(nameof(Driver.FastestLaps)),
                    materializationData.GetPropertyValue<int>(nameof(Driver.TeamId)))),
            nameof(Engine) => InterceptionResult<object>.SuppressWithResult(
                new Engine.EngineProxy(
                    materializationData.GetPropertyValue<ILazyLoader>("_loader"),
                    materializationData.GetPropertyValue<int>(nameof(Engine.Id)),
                    materializationData.GetPropertyValue<string>(nameof(Engine.Name)))),
            nameof(EngineSupplier) => InterceptionResult<object>.SuppressWithResult(
                new EngineSupplier.EngineSupplierProxy(
                    materializationData.GetPropertyValue<ILazyLoader>("_loader"),
                    materializationData.GetPropertyValue<string>(nameof(EngineSupplier.Name)))),
            nameof(Gearbox) => InterceptionResult<object>.SuppressWithResult(
                new Gearbox.GearboxProxy(
                    materializationData.GetPropertyValue<int>(nameof(Gearbox.Id)),
                    materializationData.GetPropertyValue<string>(nameof(Gearbox.Name)))),
            nameof(Location) => InterceptionResult<object>.SuppressWithResult(
                new Location.LocationProxy(
                    materializationData.GetPropertyValue<double>(nameof(Location.Latitude)),
                    materializationData.GetPropertyValue<double>(nameof(Location.Longitude)))),
            nameof(Sponsor) => InterceptionResult<object>.SuppressWithResult(new Sponsor.SponsorProxy()),
            nameof(SponsorDetails) => InterceptionResult<object>.SuppressWithResult(
                new SponsorDetails.SponsorDetailsProxy(
                    materializationData.GetPropertyValue<int>(nameof(SponsorDetails.Days)),
                    materializationData.GetPropertyValue<decimal>(nameof(SponsorDetails.Space)))),
            nameof(Team) => InterceptionResult<object>.SuppressWithResult(
                new Team.TeamProxy(
                    materializationData.GetPropertyValue<ILazyLoader>("_loader"),
                    materializationData.GetPropertyValue<int>(nameof(Team.Id)),
                    materializationData.GetPropertyValue<string>(nameof(Team.Name)),
                    materializationData.GetPropertyValue<string>(nameof(Team.Constructor)),
                    materializationData.GetPropertyValue<string>(nameof(Team.Tire)),
                    materializationData.GetPropertyValue<string>(nameof(Team.Principal)),
                    materializationData.GetPropertyValue<int>(nameof(Team.ConstructorsChampionships)),
                    materializationData.GetPropertyValue<int>(nameof(Team.DriversChampionships)),
                    materializationData.GetPropertyValue<int>(nameof(Team.Races)),
                    materializationData.GetPropertyValue<int>(nameof(Team.Victories)),
                    materializationData.GetPropertyValue<int>(nameof(Team.Poles)),
                    materializationData.GetPropertyValue<int>(nameof(Team.FastestLaps)),
                    materializationData.GetPropertyValue<int?>(nameof(Team.GearboxId)))),
            nameof(TeamSponsor) => InterceptionResult<object>.SuppressWithResult(new TeamSponsor.TeamSponsorProxy()),
            nameof(TestDriver) => InterceptionResult<object>.SuppressWithResult(
                new TestDriver.TestDriverProxy(
                    materializationData.GetPropertyValue<ILazyLoader>("_loader"),
                    materializationData.GetPropertyValue<int>(nameof(Driver.Id)),
                    materializationData.GetPropertyValue<string>(nameof(Driver.Name)),
                    materializationData.GetPropertyValue<int?>(nameof(Driver.CarNumber)),
                    materializationData.GetPropertyValue<int>(nameof(Driver.Championships)),
                    materializationData.GetPropertyValue<int>(nameof(Driver.Races)),
                    materializationData.GetPropertyValue<int>(nameof(Driver.Wins)),
                    materializationData.GetPropertyValue<int>(nameof(Driver.Podiums)),
                    materializationData.GetPropertyValue<int>(nameof(Driver.Poles)),
                    materializationData.GetPropertyValue<int>(nameof(Driver.FastestLaps)),
                    materializationData.GetPropertyValue<int>(nameof(Driver.TeamId)))),
            nameof(TitleSponsor) => InterceptionResult<object>.SuppressWithResult(
                new TitleSponsor.TitleSponsorProxy(
                    materializationData.GetPropertyValue<ILazyLoader>("_loader"))),
            _ => result
        };

    public object CreatedInstance(MaterializationInterceptionData materializationData, object instance)
    {
        Assert.True(instance is IF1Proxy);

        ((IF1Proxy)instance).CreatedCalled = true;

        return instance;
    }

    public InterceptionResult InitializingInstance(
        MaterializationInterceptionData materializationData,
        object instance,
        InterceptionResult result)
    {
        Assert.True(instance is IF1Proxy);

        ((IF1Proxy)instance).InitializingCalled = true;

        if (instance is Sponsor sponsor)
        {
            sponsor.Id = materializationData.GetPropertyValue<int>(nameof(sponsor.Id));
            sponsor.Name = "Intercepted: " + materializationData.GetPropertyValue<string>(nameof(sponsor.Name));

            return InterceptionResult.Suppress();
        }

        return result;
    }

    public object InitializedInstance(MaterializationInterceptionData materializationData, object instance)
    {
        Assert.True(instance is IF1Proxy);

        ((IF1Proxy)instance).InitializedCalled = true;

        if (instance is Sponsor.SponsorProxy sponsor)
        {
            return new Sponsor.SponsorDoubleProxy(sponsor);
        }

        return instance;
    }
}

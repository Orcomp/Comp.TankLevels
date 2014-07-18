Stockpile Mangement
==

We have a buffer tank that has liquid put into and taken away from it at different points in time. The buffer tank has a min and max limit.

The picture below shows how the levels in the tank change over time:


![Tank level over time](doc/img/StockpileManagement.png) 


## Aim

We need a function which will tell us If and when we can add a new filling or consuming operation to the tank so that it does not go over (or under) the max (or min) limit.

The method signature should look something like this:

```csharp
Result CheckOperation(DateTime startTime, TimeSpan duration, double quantity, TankLevels tankLevels);
```

The "Result" should have two properties:
- IsSucess : bool
- StartTime : DateTime (NOTE: the Result.StartTime may be equal or greater to the startTime supplied to the CheckOperation() method. Part of the challenge of this problem is finding the earliest time we can start the operation without breaching the limits.)

## Parameters

- "StartTime": Is the time we would like to start adding the operation to the tank.
- "Duration": is the duration of the operation
- "Quantity": is how much liquid will be added to the tank during the whole duration of the operation. (If the value is negative then, liquid will be taken away from the tank.) 
* "TankLevels": is a collection of points which give you the level in the tank at different times.

## Starter playground

This repository is prepared to jump start to the imlpementation instantly. The Visual Studio solution consist of three projects:

* TankLevels
* TankLevels.Tests
* TankLevels.PerformanceTests

**TankLevels project** 

The TankLevels project defines the

* ITank interface with the CheckOperation method.
* TankLevel entity 
* CheckOperationResult entity

It also defines two dummy imlementations for demonstrating how a future implementation fits to the unit test and performance test infrastructure.
The two dummy imlementations are:
* DummyTank
* OtherDummyTank

**TankLevels.Tests project** 

The TankLevels.Tests project contains the unit test infrastucture and the actual unit tests. Start with reading the detailed summary documentation of the TankLevel class.
The only dependency of the project is the NuGet NUnit package.

**TankLevels.PerformanceTests project** 

The TankLevels.Tests project contains the performance test infrastucture and the actual performance tests. 
The perfomance tests implemented using NunitBenchmarker (https://github.com/Orcomp/NUnitBenchmarker). Start with the TankPerformanceTests class and run the tests just as you would run an ordinary unit test.

To get an instant picture how cool things should happen when you run the performance test see the picture below:

![Performance testing](doc/img/nunitbenchmarker.png) 

**Getting the dependencies** 

All dependency managed by the NuGet package manager.
The simplest way to get all the dependencies is using the provided RestorePackages.bat in the repository root.

## Support

You can ask for support on our mailing list: https://groups.google.com/forum/to_be_created

## License

This project is open source and released under the [MIT license.](License.txt)


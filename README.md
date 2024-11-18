# Crypto Exchange Task

## Prerequisites

In order to run the code inside this repository make sure that the following software is installed on your computer:

1. [.NET SDK >= 8.0.400](https://dotnet.microsoft.com/en-us/download/dotnet)

## Task 1

### Problem Statement

Your task is to implement a meta exchange that always gives the user the best possible price if he is buying or selling a certain amount of BTC. Technically, you will be given n order books [from n different
cryptoexchanges], the type of order (buy or sell ), and the amount of BTC that our user wants to buy or sell. Your algorithm needs to output one or more buy or sell orders.

In effect, our user buys the specified amount of BTC for the lowest possible price or sells the specified amount of BTC for the highest possible price.

To make life a bit more complicated, each cryptoexchange has EUR and BTC balance. Your algorithm needs to achieve the best price within these constraints. The algorithm cannot transfer any money or crypto between cryptoexchanges , that means you can only sell what you have stored on that cryptoexchange account (EUR or BTC).

Together with this task, you will receive a bunch of JSON files with order books you can use to test your solution In each file, you will also find the given limit (EUR/BTC) of this cryptoexchange.

Your solution should be a relatively simple .NET Core console mode application, which reads the order books with limits order amounts and order type , and outputs a set of orders to execute against the given order books (exchanges).

### Solution

The task at hand is a manifestation of the well-known knapsack problem from the field of combinatorial optimization wich can be summarized like this: 

> Given a set of items, each with a weight and a value, determine which items to include in the collection so that the total weight is less than or equal to a given limit and the total value is as large as possible.
>
> Source: [Wikipedia: Knapsack problem](https://en.wikipedia.org/wiki/Knapsack_problem)

The implementation to solve the task is inside the project `CryptoExchangeBestValue.Library`. This library exposes the class `BestValueCalculator` which looks as follows:

```csharp
public interface IBestValueCalculator
{
	BestValueResult Buy(double availableEuros, IEnumerable<Order> askOrders);

	BestValueResult Sell(double availableCryptos, IEnumerable<Order> bidOrders);
}
```

Using the `Buy` method we can calculate the highest amount of cryptos we can buy from a number of ask orders given a specific amount of euros. Using the `Sell` method we can calculate the highest amount of euros we can obtain from selling a specific amount of cryptos and chosing from a set of bid orders.

Internally both the `Buy` and the `Sell` method use the same 0-1 knapsack problem solving algorithm which is based on the [method of dynamic programming](https://en.wikipedia.org/wiki/Dynamic_programming). In order to do this we need to map the properties of our real world example to the properties of the knapsack problem. This is shown in the table below:

|Knapsack properties|Buy             |Sell               |
|-------------------|----------------|-------------------|
| Capacity          |Available euros | Available cryptos |
| Value             |Amount (Ask)    | Price (Bid)       |
| Weight            |Price (Ask)     | Amound (Bid)      |

(!) The following technical constraint was applied to the algorithm implementation: The dynamic programming approach for 0-1 knapsack problems requires the capacity to be of type integer, or better it requires incremental steps of increasing the capacity for its calculation. For the values of the cryptos we are facing a value ranging from two decimal places before and eigth decimal places after the comma. In order to keep the memory consumption within a achievable range the implementation only considers up to six fractional digits of the crypto value. This value is then multiplied with a scale factor to be used as capacity in the best value calculation. This limitation can be mitigated in a next step, by optimizing the memory consumption of the algorithm, e.g. by memoization or using a 1-D array instead of a 2-D array for the dynamic programming computation.

### Run unit tests

Unit tests for the implemented algorithm are available in the project `CryptoExchangeBestValue.Library.Tests`. To execute the unit tests do the following:

1. In a terminal navigate to `./CryptoExchangeBestValue.Library.Tests`
1. Run `dotnet test`
 
### Run the console application

The implemented algorithm is available for use on the example input data in the project `CryptoExchangeBestValue.Console.` To execute the application do the following

1. In a terminal navigate to `./CryptoExchangeBestValue.Console`
1. Run the project `dotnet run ..\exchanges\exchange-06.json`

## Task 2

### Problem Statement

Implement a Web service (Kestrel, .NET Core API), and expose the implemented functionality through it. Implement an endpoint that will receive the required parameters ( order amount, order type and return a JSON response with the "best execution" plan.

### Solution

The best value calculation algorithm is provided as a web service inside the `CryptoExchangeBestValue.WebService` project. This web service is a [Minimal API application](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/overview) which exposes a single endpoint `POST /best-value` and expects the request body to be a JSON document equal to the provided exchange sample data.

To use the application do the following:

1. Start the application
   1. either by running `dotnet run` inside the `CryptoExchangeBestValue.WebService` directory,
   1. or by starting the application in an IDE of your choice (Visual Studio, Rider)
1. Go to a browser and open the `/swagger` path of the application
1. Click on the 'POST /best-value' section to open up the details
1. Click on the "Try it out" button
1. Copy the content from one of the example exchange json files and paste it into the multiline text field in the browser
1. Hit the "Execute" button and wait for the computation to finish
1. When the results are returned you can see it in the section named 'Responses'

A Docker file is provided to build and run the application as a container. This is used by Visual Studio or can be used to manually create a Docker image with the application and then create and start a Docker container from this image.

# Crypto Exchange Task

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

## Task 2

### Problem Satement

Implement a Web service (Kestrel, .NET Core API), and expose the implemented functionality through it. Implement an endpoint that will receive the required parameters ( order amount, order type and return a JSON response with the "best execution" plan.

### Solution
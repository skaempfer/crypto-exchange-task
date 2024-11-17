namespace CryptoExchangeBestValue.Library;

/// <summary>
/// Calculates the best possible value a user can achieve with buying or selling cryptos given a set of orders and a specific amount of Euros or Cryptos.
/// </summary>
public class BestValueCalculator : IBestValueCalculator
{

	/// <inheritdoc cref="IBestValueCalculator.Buy"/>
	public BestValueResult Buy(double availableEuros, IEnumerable<Order> askOrders)
	{
		KnapsackItem[] knapsackItems = askOrders.Select(x => new KnapsackItem()
		{
			Id = x.Id,
			Value = x.Amount,
			Weight = x.Price,
		})
		.ToArray();

		(double bestValue, string[] itemIds) = Solve01KnapsackProblem(availableEuros, knapsackItems, 100);

		return new BestValueResult(bestValue, itemIds);
	}

	/// <inheritdoc cref="IBestValueCalculator.Sell"/>
	public BestValueResult Sell(double availableCryptos, IEnumerable<Order> bidOrders)
	{
		KnapsackItem[] knapsackItems = bidOrders.Select(x => new KnapsackItem()
			{
				Id = x.Id,
				Value = x.Price,
				Weight = x.Amount,
			})
			.ToArray();

		(double bestValue, string[] itemIds) = Solve01KnapsackProblem(availableCryptos, knapsackItems, 1_000_000);

		return new BestValueResult(bestValue, itemIds);
	}

	/// <summary>
	/// A 0-1 knapsack problem solver that uses dynamic programming.
	/// </summary>
	/// <remarks>
	/// Since the dynamic programming approach for solving the knapsack problem only works with a capacity of
	/// type integer, we are additionally providing a scale factor to turn decimal capacities into integer capacities.
	/// </remarks>
	/// <seealso href="https://en.wikipedia.org/wiki/Knapsack_problem"/>
	/// <seealso href="https://youtu.be/cJ21moQpofY"/>
	/// <param name="capacity">The capacity of our knapsack.</param>
	/// <param name="items">The list of items that are available for being put into the knapsack.</param>
	/// <param name="scaleFactor">A scale factor to turn a decimal capacity value into an integer capacity value.</param>
	/// <returns></returns>
	private (double bestValue, string[] itemIds) Solve01KnapsackProblem(
		double capacity,
		KnapsackItem[] items,
		int scaleFactor)
	{
		int n = items.Length;

		int scaledCapacity = (int)(capacity * scaleFactor);

		// Dynamic programming table: row index for items, column index for capacity
		double[,] dp = new double[n + 1, scaledCapacity + 1];

		// Fill the table to find the best value
		for (int i = 1; i <= n; i++)
		{
			for (int w = 0; w <= scaledCapacity; w++)
			{
				double currentWeight = items[i - 1].Weight * scaleFactor;
				if (currentWeight <= w)
				{
					dp[i, w] = Math.Max(dp[i - 1, w],
						dp[i - 1, (int)(w - currentWeight)] + items[i - 1].Value);
				}
				else
				{
					dp[i, w] = dp[i - 1, w];
				}
			}
		}

		// Backtrack the table to find the chosen items
		double remainingCapacity = scaledCapacity;
		var chosenItems = new List<string>();
		for (int i = n; i > 0; i--)
		{
			if (dp[i, (int)remainingCapacity] == dp[i - 1, (int)remainingCapacity])
			{
				continue;
			}

			chosenItems.Add(items[i - 1].Id);
			remainingCapacity -= items[i - 1].Weight * scaleFactor;
		}

		chosenItems.Reverse();

		double bestValue = dp[n, scaledCapacity];

		return (bestValue, chosenItems.ToArray());
	}

	private struct KnapsackItem
	{
		public string Id { get; init; }

		public double Value { get; init; }

		public double Weight { get; init; }
	}
}

/// <summary>
/// Contains the result of a <see cref="BestValueCalculator.Sell"/> or <see cref="BestValueCalculator.Buy"/> operation.
/// </summary>
/// <param name="Result">The best possible value that can be achieved given a set of orders.</param>
/// <param name="OrderIds">A list of order identifiers that need to be used in order to achieve <see cref="Result"/>.</param>
public record BestValueResult(double Result, string[] OrderIds);

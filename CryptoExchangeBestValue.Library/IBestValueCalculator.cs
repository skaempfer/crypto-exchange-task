namespace CryptoExchangeBestValue.Library;

public interface IBestValueCalculator
{
	/// <summary>
	/// Calculates the highest amount of cryptos we can buy.
	/// </summary>
	/// <param name="availableEuros">The amount of euros at our disposal for buying cryptos.</param>
	/// <param name="askOrders">A list of ask orders selling cryptos.</param>
	/// <returns>A <see cref="BestValueResult"/> containing the result of the calculation.</returns>
	BestValueResult Buy(double availableEuros, IEnumerable<Order> askOrders);

	/// <summary>
	/// Calculates the highest amount of euros we can achieve by selling cryptos.
	/// </summary>
	/// <param name="availableCryptos">The amount of cryptos at out disposal to sell.</param>
	/// <param name="bidOrders">A list of bid orders buying cryptos.</param>
	/// <returns>A <see cref="BestValueResult"/> containing the result of the calculation.</returns>
	BestValueResult Sell(double availableCryptos, IEnumerable<Order> bidOrders);
}

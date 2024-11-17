namespace CryptoExchangeBestValue.Library;

/// <summary>
/// The container object providing all necessary data to calculate a crypto exchange.
/// </summary>
public class CryptoExchangeInput
{
	/// <summary>
	/// The identifier of the crypto exchange input.
	/// </summary>
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// The resources we have at our disposal in order to sell or buy during this exchange
	/// </summary>
	public UserFunds AvailableFunds { get; set; } = new UserFunds();

	/// <summary>
	/// A list of offers to buy or sell crypto resources.
	/// </summary>
	public OrderBook OrderBook { get; set; } = new OrderBook();
}

/// <summary>
/// A balance of <see cref="Crypto"/> and <see cref="Euro"/> resources.
/// </summary>
public class UserFunds
{
	/// <summary>
	/// Amount of Crypto resources that can be sold.
	/// </summary>
	public double Crypto { get; set; }
	
	/// <summary>
	/// Amount of Euro resources that can be used to buy <see cref="Crypto"/> resources.
	/// </summary>
	public double Euro { get; set; }
}

/// <summary>
/// Contains offers to buy and sell crypto resources.
/// </summary>
public class OrderBook
{
	/// <summary>
	/// A list of all offers to buy crypto resources.
	/// </summary>
	public List<OrderBookEntry> Bids { get; set; } = [];

	/// <summary>
	/// A list of all offers to sell crypto resources.
	/// </summary>
	public List<OrderBookEntry> Asks { get; set; } = [];
}

/// <summary>
/// Container for a single offer to buy or sell crypto resources.
/// </summary>
public class OrderBookEntry
{
	/// <summary>
	/// The offer to sell or buy crypto resources.
	/// </summary>
	public Order Order { get; set; } = new();
}

/// <summary>
/// An offer to sell or buy crypto resources.
/// </summary>
public class Order
{
	/// <summary>
	/// The identifier of this <see cref="Order"/>.
	/// </summary>
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// The time at which this order was places
	/// </summary>
	/// <remarks>
	/// Presumably. Not enough information in task
	/// </remarks>
	public string Time { get; set; } = string.Empty;

	/// <summary>
	/// The type of order this <see cref="Order"/> represents (bid or ask).
	/// </summary>
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// Specifies the meaning of <see cref="Amount"/> and <see cref="Price"/> information of this <see cref="Order"/>.
	/// </summary>
	/// <remarks>
	/// Presumably. Not enough information in task
	/// </remarks>
	public string Kind { get; set; } = string.Empty;
	
	/// <summary>
	/// The amount of cryptos the provider of this order is willing to trade.
	/// </summary>
	public double Amount { get; set; }
	
	/// <summary>
	/// The amount of money the provider of this <see cref="Order"/> is willing to trade <see cref="Amount"/> for.
	/// </summary>
	public double Price { get; set; }
}

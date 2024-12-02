using FluentAssertions;

namespace CryptoExchangeBestValue.Library.Tests;

public class BestValueCalculatorTests
{
	private BestValueCalculator sut = new();

	public static IEnumerable<object[]> BuyData =>
	[
		[
			110,
			new List<Order>()
			{
				CreateAskOrder("1", amount: 0.1, price: 50.5),
				CreateAskOrder("2", amount: 0.15, price: 55.03),
				CreateAskOrder("3", amount: 0.01, price: 40.1),
			},
			0.25,
			new List<string>() { "1", "2" },
		],
		[
			61,
			new List<Order>()
			{
				CreateAskOrder("1", amount: 0.1, price: 57),
				CreateAskOrder("2", amount: 0.1, price: 56),
				CreateAskOrder("3", amount: 0.01, price: 4),
			},
			0.11,
			new List<string>() { "1", "3" },
		],
		[
			// Notice how the chosen items change when we get to the capacity limit
			// as opposed to the previous example that does not reach the capacity limit
			60,
			new List<Order>()
			{
				CreateAskOrder("1", amount: 0.1, price: 57),
				CreateAskOrder("2", amount: 0.1, price: 56),
				CreateAskOrder("3", amount: 0.01, price: 4),
			},
			0.11,
			new List<string>() { "2", "3" },
		]
	];

	[Theory]
	[MemberData(nameof(BuyData))]
	public void Buy_ValidFundsValidOrders_CalculatesBestCryptoBuyResult(
		double availableEuros,
		List<Order> askOrders,
		double expectedResultValue,
		List<string> expectedResultItems)
	{
		BestValueResult actual = this.sut.Buy(availableEuros, askOrders);

		actual.Result.Should().Be(expectedResultValue);
		actual.OrderIds.Should().BeEquivalentTo(expectedResultItems);
	}

	[Fact]
	public void Buy_DataFromTaskExample_CalculatesBestCryptoBuyResult()
	{
		var askOrders = new List<Order>
		{
			CreateAskOrder("3k_1", amount: 1, price: 3000),
			CreateAskOrder("3k_2", amount: 1, price: 3000),
			CreateAskOrder("3k_3", amount: 1, price: 3000),
			CreateAskOrder("3k_4", amount: 1, price: 3000),
			CreateAskOrder("3k_5", amount: 1, price: 3000),
			CreateAskOrder("3k_6", amount: 1, price: 3000),
			CreateAskOrder("3k_7", amount: 1, price: 3000),
			CreateAskOrder("3.3k_1", amount: 1, price: 3300),
			CreateAskOrder("3.3k_2", amount: 1, price: 3300),
			CreateAskOrder("3.3k_3", amount: 1, price: 3300),
			CreateAskOrder("3.3k_4", amount: 1, price: 3300),
			CreateAskOrder("3.5k_1", amount: 1, price: 3500),
			CreateAskOrder("3.5k_2", amount: 1, price: 3500),
			CreateAskOrder("3.5k_3", amount: 1, price: 3500),
			CreateAskOrder("3.5k_4", amount: 1, price: 3500),
			CreateAskOrder("3.5k_5", amount: 1, price: 3500),
			CreateAskOrder("3.5k_6", amount: 1, price: 3500),
			CreateAskOrder("3.5k_7", amount: 1, price: 3500),
			CreateAskOrder("3.5k_8", amount: 1, price: 3500),
			CreateAskOrder("3.5k_9", amount: 1, price: 3500)
		};

		BestValueResult actual = this.sut.Buy(28000, askOrders);

		actual.Result.Should().Be(9);
		actual.OrderIds.Should().BeEquivalentTo("3k_1", "3k_2", "3k_3", "3k_4", "3k_5", "3k_6", "3k_7", "3.3k_1", "3.3k_2");
	}

	public static IEnumerable<object[]> SellData =>
	[
		[
			6,
			new List<Order>()
			{
				CreateBidOrder("1", amount: 0.5, price: 100),
				CreateBidOrder("2", amount: 6, price: 100),
				CreateBidOrder("3", amount: 4, price: 300),
				CreateBidOrder("4", amount: 0.5, price: 15),
				CreateBidOrder("5", amount: 7, price: 100000),
				CreateBidOrder("6", amount: 3, price: 100),
			},
			415,
			new List<string>() { "1", "3", "4" },
		]
	];

	[Theory]
	[MemberData(nameof(SellData))]
	public void Sell_ValidFundsValidOrders_CalculatesBestCryptoBuyResult(
		double availableCryptos,
		List<Order> bidOrders,
		double expectedResultValue,
		List<string> expectedResultItems
		)
	{
		BestValueResult actual = this.sut.Sell(availableCryptos, bidOrders);

		actual.Result.Should().Be(expectedResultValue);
		actual.OrderIds.Should().BeEquivalentTo(expectedResultItems);
	}

	private static Order CreateBidOrder(string id, double amount, double price) => CreateOrder(id, amount, price, "Buy");

	private static Order CreateAskOrder(string id, double amount, double price) => CreateOrder(id, amount, price, "Sell");

	private static Order CreateOrder(string id, double amount, double price, string type) => new Order
	{
		Id = id,
		Time = "2024-03-01T14:41:06.563Z",
		Type = type,
		Kind = "Limit",
		Amount = amount,
		Price = price,
	};
}

using System.Text.Json;

using CommandLine;

using CryptoExchangeBestValue.Console;
using CryptoExchangeBestValue.Library;

Parser parser = new(config =>
{
	config.CaseInsensitiveEnumValues = true;
	config.AutoHelp = true;
	config.HelpWriter = Parser.Default.Settings.HelpWriter;
});
ParserResult<Options> parserResult = parser.ParseArguments<Options>(args);

await parserResult.WithParsedAsync(async (options) =>
{
	await using FileStream fileStream = File.OpenRead(options.InputPath);
	CryptoExchangeInput exchangeInput = (await JsonSerializer.DeserializeAsync<CryptoExchangeInput>(fileStream))!;

	BestValueCalculator bestValueCalculator = new();

	Console.WriteLine($"Buy and sell results for {options.InputPath}:");

	var buyResult = bestValueCalculator.Buy(
		exchangeInput.AvailableFunds.Euro,
		exchangeInput.OrderBook.Asks.Select(x => x.Order));

	Console.WriteLine("- Buy");
	Console.WriteLine($"   - Available euros: {exchangeInput.AvailableFunds.Euro}");
	Console.WriteLine($"   - Cryptos to gain: {buyResult.Result}");
	Console.WriteLine($"   - Ask orders to use: {string.Join(", ", buyResult.OrderIds)}");

	var sellResult = bestValueCalculator.Sell(
		exchangeInput.AvailableFunds.Crypto,
		exchangeInput.OrderBook.Bids.Select(x => x.Order));

	Console.WriteLine("- Sell");
	Console.WriteLine($"   - Available cryptos: {exchangeInput.AvailableFunds.Crypto}");
	Console.WriteLine($"   - Euros to gain: {sellResult.Result}");
	Console.WriteLine($"   - Bid orders to use: {string.Join(", ", sellResult.OrderIds)}");
});

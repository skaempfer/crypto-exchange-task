using CryptoExchangeBestValue.Library;

using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IBestValueCalculator, BestValueCalculator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/best-value", (IBestValueCalculator bestValueCalculator, [FromBody]CryptoExchangeInput exchangeInput) =>
	{
	var buyResult = bestValueCalculator.Buy(
		exchangeInput.AvailableFunds.Euro,
		exchangeInput.OrderBook.Asks.Select(x => x.Order));

	var sellResult = bestValueCalculator.Sell(
		exchangeInput.AvailableFunds.Crypto,
		exchangeInput.OrderBook.Bids.Select(x => x.Order));

	return Results.Json(
		new
		{
			ExchangeId = exchangeInput.Id,
			BuyResult = new { Cryptos = buyResult.Result, OrderIds = buyResult.OrderIds, },
			SellResult = new { Euros = sellResult.Result, OrderIds = sellResult.OrderIds, },
		});
	})
.WithName("Calculate crypto exchange best value")
.WithOpenApi();

app.Run();

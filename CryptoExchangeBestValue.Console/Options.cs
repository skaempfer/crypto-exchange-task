using CommandLine;
using CommandLine.Text;

namespace CryptoExchangeBestValue.Console;

/// <summary>
/// Command line options for the console application.
/// </summary>
public class Options
{
	[Value(0, MetaName = "InputPath", Required = true,
		HelpText = "Path to JSON file containing input data for the crypto exchange.")]
	public string InputPath { get; set; } = string.Empty;

	[Usage(ApplicationAlias = "best-value")]
	public static IEnumerable<Example> Examples
	{
		get
		{
			yield return new Example(
				"Read the input from the specified JSON file and calculate the best result for buy and sell operations.",
				new Options { InputPath = @".\exchanges\exchange-01.json", });
		}
	}
}

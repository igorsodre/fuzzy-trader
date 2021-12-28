using FuzzyTrader.Server.Services.Iterfaces;

namespace FuzzyTrader.Server.Services;

public class HtmlService : IHtmlService
{
	public string GetDocumentBodyOpeneningTags()
	{
		return @"
                <!DOCTYPE html>
                <html lang=""en"">
	                <head>
		                <meta charset=""UTF-8"" />
		                <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
		                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
		                <meta http-equiv=""content-type"" content=""text/html; charset=utf-8"" />
		                <title>Document</title>
	                </head>
	                <body>
                ";
	}

	public string GetDocumentBodyClosingTags()
	{
		return @"
					</body>
				</html>";
	}
}
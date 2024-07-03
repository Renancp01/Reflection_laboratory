using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Contracts;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using WebApi;
using WebApi.Models;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<CardServiceBenchmark>();
    }
    public class CardServiceBenchmark
    {
        private CardService _cardService;
        private ICardApi _cardApi;

        [GlobalSetup]
        public void Setup()
        {
            _cardApi = Substitute.For<ICardApi>();
            var cardResponse = new CardResponse { CardHolderCards = new List<Card>(
            {
                new Card()
                {
                    Shortcuts = 
                }
            }};


            _cardApi.GetCardAsync().Returns(Task.FromResult(cardResponse));

            var configuration = Substitute.For<IConfiguration>();
            _cardService = new CardService(configuration, _cardApi);
        }

        [Benchmark]
        public async Task GetCardAsyncBenchmark()
        {
            await _cardService.GetCardAsync();
        }
    }
}
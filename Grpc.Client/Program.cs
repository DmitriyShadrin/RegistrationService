using Grpc.Client.SignGenerator;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace Grpc.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new SignLicense.SignLicenseClient(channel);

            var call = client.ReadLicense(new ReadLicenseRequest(), new CallOptions());

            Console.WriteLine("Started.");

            while (true)
            {
                while (await call.ResponseStream.MoveNext())
                {
                    Console.WriteLine("Receveid: " + call.ResponseStream.Current);
                    var response = call.ResponseStream.Current;
                    await client.SendSignedLicenseAsync(new SignedRequest { CorrelationId = response.CorrelationId, Code = SigningCode.Ok });
                }
            }
        }
    }
}

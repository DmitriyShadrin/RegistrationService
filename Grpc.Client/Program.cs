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

            var headers = new Metadata();
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibG9naW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJyb2xlIiwibmJmIjoxNjAyMzM2OTA0LCJleHAiOjE2MDI5MzY5MDQsImlzcyI6IlJlZ2lzdHJhdGlvblNlcnZpY2UiLCJhdWQiOiJMaWNlbnNlU2lnbkdlbmVyYXRvciJ9.7Pu7EdWn4fcjtJD1Z-UNLWQFensbps2z_2EX6AG0ZKY";
            headers.Add("Authorization", $"Bearer {token}");

            var call = client.ReadLicense(new ReadLicenseRequest(), headers);

            Console.WriteLine("Started.");

            while (true)
            {
                while (await call.ResponseStream.MoveNext())
                {
                    Console.WriteLine("Receveid: " + call.ResponseStream.Current);
                    var response = call.ResponseStream.Current;
                    await client.SendSignedLicenseAsync(new SignedRequest { CorrelationId = response.CorrelationId, Code = SigningCode.Ok }, headers);
                }
            }
        }
    }
}

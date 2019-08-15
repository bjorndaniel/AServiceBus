using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace Sender
{
    class Program
    {
        private const string ServiceBusConnectionString = "<your_connection_string>";
        private const string TopicName = "<your_topic_name>";
        private static ITopicClient _topicClient;

        static async Task Main(string[] args)
        {
            const int numberOfMessages = 5;
            _topicClient = new TopicClient(ServiceBusConnectionString, TopicName);
            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after sending all the messages or any other key to resend.");
            Console.WriteLine("======================================================");

            // Send messages.
            await SendMessagesAsync(numberOfMessages);

            var key = Console.ReadKey();
            while (key.Key != ConsoleKey.Enter)
            {
                await SendMessagesAsync(numberOfMessages);
                key = Console.ReadKey();
            }
            await _topicClient.CloseAsync();
        }

        private static async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            try
            {
                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    // Create a new message to send to the topic.
                    string messageBody = $"Message { i } sent at { DateTime.Now.ToString("yyyy-MM-dd HH:mm") }";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    // Write the body of the message to the console.
                    Console.WriteLine($"Sending message: { messageBody }");

                    // Send the message to the topic.
                    await _topicClient.SendAsync(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($" { DateTime.Now }::Exception: { exception.Message }");
            }
        }
    }
}
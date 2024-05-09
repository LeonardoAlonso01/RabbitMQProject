using EasyNetQ;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

const string EXCHANGE = "curso-rabbitmq";
const string QUEUE = "person-created";
const string ROUTING_KEY = "hr.person-created";

var person = new Person("Leonardo", 22, "leooalonso@gmail.com");


//RabbitMQ.Client
/*var connectionFactory = new ConnectionFactory
{
    HostName = "localhost"
};

var connection = connectionFactory.CreateConnection("curso-rabbitmq");
var channel = connection.CreateModel();

var json = JsonSerializer.Serialize(person);
var byteArray = Encoding.UTF8.GetBytes(json);

//channel.BasicPublish(EXCHANGE, "hr.person-created", null, byteArray);

//Console.WriteLine($"Mensagem publicada {json}");

var consumerChannel = connection.CreateModel();
var consumer = new EventingBasicConsumer(consumerChannel);

consumer.Received += async (sender, eventArgs) =>
{
    var contentArray = eventArgs.Body.ToArray();
    var contentString = Encoding.UTF8.GetString(contentArray);

    var message = JsonSerializer.Deserialize<Person>(contentString);
    Console.WriteLine($"Mensagem recebida: {contentString}");

    consumerChannel.BasicAck(eventArgs.DeliveryTag, false);
};
consumerChannel.BasicConsume("person-created", false, consumer);
Console.ReadLine();
*/

//EasyNetQ
var bus = RabbitHutch.CreateBus("host=localhost");
var advanced = bus.Advanced;

var exchange = advanced.ExchangeDeclare(EXCHANGE, "topic");
var queue = advanced.QueueDeclare(QUEUE);

advanced.Publish(exchange, ROUTING_KEY, true, new Message<Person>(person));
advanced.Publish(exchange, ROUTING_KEY, true, new Message<Person>(person));
advanced.Publish(exchange, ROUTING_KEY, true, new Message<Person>(person));
advanced.Publish(exchange, ROUTING_KEY, true, new Message<Person>(person));
advanced.Publish(exchange, ROUTING_KEY, true, new Message<Person>(person));
advanced.Publish(exchange, ROUTING_KEY, true, new Message<Person>(person));

advanced.Consume<Person>(queue, (msg, info) =>
{
     var json = JsonConvert.SerializeObject(msg.Body);
     Console.WriteLine(json);
});

//await bus.PubSub.PublishAsync(person);
//await bus.PubSub.SubscribeAsync<Person>("marketing", msg =>
//{
// var json = JsonConvert.SerializeObject(msg);
// Console.WriteLine(json);
//});

Console.ReadLine();

class Person
{
    public string FullName { get; private set; }
    public int Age { get; private set; }
    public string Email { get; private set; }

    public Person(string fullName, int age, string email)
    {
        FullName = fullName;
        Age = age;
        Email = email;
    }
}

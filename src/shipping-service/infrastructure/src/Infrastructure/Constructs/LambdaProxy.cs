using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.Events;
using Amazon.CDK.AWS.Lambda;
using AssetOptions = Amazon.CDK.AWS.S3.Assets.AssetOptions;
using Constructs;
using Amazon.CDK.AWS.SQS;
using Amazon.CDK.AWS.Lambda.EventSources;
using Amazon.CDK.AWS.Events.Targets;

namespace Infrastructure
{
    public class LambdaProxyProps
    {
        public string ProxyIdentifier { get; set; }

        public string ForwardingUrl { get; set; }

        public Amazon.CDK.AWS.Events.IEventBus EventBus { get; set; }

        public EventPattern EventPattern { get; set; }

        public IVpc Vpc { get; set; }

        public ISubnetSelection Subnets { get; set; }
    }

    public class LambdaProxy : Construct
    {
        public LambdaProxy(Construct scope, string id, LambdaProxyProps props) : base(scope, id)
        {
            var commands = new[]
            {
                "cd /asset-input",
                "export DOTNET_CLI_HOME=\"/tmp/DOTNET_CLI_HOME\"",
                "export PATH=\"$PATH:/tmp/DOTNET_CLI_HOME/.dotnet/tools\"",
                "dotnet tool install -g Amazon.Lambda.Tools",
                "dotnet lambda package -o output.zip",
                "unzip -o -d /asset-output output.zip"
            };

            var deadLetterQueue = new Queue(this, $"{props.ProxyIdentifier}-dlq", new QueueProps
            {
                QueueName = $"{props.ProxyIdentifier}-dlq",
                VisibilityTimeout = Duration.Minutes(1)
            });

            var queue = new Queue(this, $"{props.ProxyIdentifier}-queue", new QueueProps
            {
                QueueName = $"{props.ProxyIdentifier}-queue",
                VisibilityTimeout = Duration.Minutes(1),
                DeadLetterQueue = new DeadLetterQueue()
                {
                    Queue = deadLetterQueue,
                    MaxReceiveCount = 2
                },
            });

            var function = new Function(this,
                "proxy-function",
                new FunctionProps
                {
                    FunctionName = $"{props.ProxyIdentifier}-proxy-function",
                    Runtime = Runtime.DOTNET_6,
                    Environment = new Dictionary<string, string>
                    {
                        {"FORWARDING_URL", props.ForwardingUrl},
                        {"DEAD_LETTER_QUEUE_URL", deadLetterQueue.QueueUrl},
                    },
                    Vpc = props.Vpc,
                    VpcSubnets = props.Subnets,
                    Code = Code.FromAsset("./src/Infrastructure/Constructs/LambdaProxyHandler", new AssetOptions
                    {
                        Bundling = new BundlingOptions
                        {
                            Image = Runtime.DOTNET_6.BundlingImage,
                            Command = new[]
                      {
                          "bash", "-c", string.Join(" && ", commands)
                      }
                        }
                    }),
                    Handler = "LambdaProxyHandler::LambdaProxyHandler.Function::FunctionHandler",
                });

            function.AddEventSource(new SqsEventSource(queue, new SqsEventSourceProps()
            {
                Enabled = true,
                ReportBatchItemFailures = true
            }));

            queue.GrantConsumeMessages(function);
            deadLetterQueue.GrantSendMessages(function);

            var rule = new Rule(this, $"{props.ProxyIdentifier}-rule", new RuleProps()
            {
                EventBus = props.EventBus,
                EventPattern = props.EventPattern,
                RuleName = $"{props.ProxyIdentifier}-rule",
                Targets = new[]
                {
                    new SqsQueue(queue)
                }
            });
        }
    }
}

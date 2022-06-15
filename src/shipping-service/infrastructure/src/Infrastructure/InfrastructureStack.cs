using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ElasticLoadBalancingV2;
using Amazon.CDK.AWS.Events;
using Amazon.CDK.AWS.SSM;
using Constructs;

namespace Infrastructure
{
    public class InfrastructureStack : Stack
    {
        internal InfrastructureStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var table = new Table(this, "ShippingTable", new TableProps()
            {
                BillingMode = BillingMode.PAY_PER_REQUEST,
                TableName = "eda-shipping-table",
                PartitionKey = new Attribute()
                {
                    Name = "PK",
                    Type = AttributeType.STRING
                },
                SortKey = new Attribute()
                {
                    Name = "SK",
                    Type = AttributeType.STRING
                },
            });

            var secret = new StringParameter(this, "ShippingTableNameParameter", new StringParameterProps()
            {
                ParameterName = "/eda/dotnet/shipping/table_name",
                StringValue = table.TableName,
                DataType = ParameterDataType.TEXT,
                Type = ParameterType.STRING
            });

            var centralEventBus = EventBus.FromEventBusName(this, "CentralEventBus", "eda-dotnet-bus");

            var existingVpc = Vpc.FromLookup(this, "AppVpc", new VpcLookupOptions()
            {
                VpcName = "copilot-event-driven-dotnet-test",
                IsDefault = false,
            });

            var targetGroup = new NetworkTargetGroup(this, "NetworkTargetGroup", new NetworkTargetGroupProps()
            {
                TargetGroupName = "nlb-target-group",
                Vpc = existingVpc,
                TargetType = TargetType.ALB,
                Port = 80,
                Protocol = Amazon.CDK.AWS.ElasticLoadBalancingV2.Protocol.TCP
            });

            var nlb = new NetworkLoadBalancer(this, "AppNLB", new NetworkLoadBalancerProps()
            {
                Vpc = existingVpc,
                VpcSubnets = new SubnetSelection()
                {
                    Subnets = existingVpc.IsolatedSubnets
                },
                InternetFacing = false,
                LoadBalancerName = "shipping-nlb"
            });

            var vpcLink = new VpcLink(this, "ShippingApiLink", new VpcLinkProps()
            {
                VpcLinkName = "shipping-apigw-link",
                Targets = new INetworkLoadBalancer[1] { existingNlb },
                Description = "VPC link to provide access to EDA Dotnet API VPC"
            });
        }
    }
}

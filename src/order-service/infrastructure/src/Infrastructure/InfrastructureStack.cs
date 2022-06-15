using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.Events;
using Amazon.CDK.AWS.SSM;
using Constructs;

namespace Infrastructure
{
    public class InfrastructureStack : Stack
    {
        internal InfrastructureStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var table = new Table(this, "OrderTable", new TableProps()
            {
                BillingMode = BillingMode.PAY_PER_REQUEST,
                TableName = "eda-order-table",
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

            var eventBus = new EventBus(this, "CentralEventBus", new EventBusProps()
            {
                EventBusName = "eda-dotnet-bus",
            });

            var secret = new StringParameter(this, "TableNameParameter", new StringParameterProps()
            {
                ParameterName = "/eda/dotnet/orders/table_name",
                StringValue = table.TableName,
                DataType = ParameterDataType.TEXT,
                Type = ParameterType.STRING
            });

            var eventBusNameParameter = new StringParameter(this, "EventBusNameParameter", new StringParameterProps()
            {
                ParameterName = "/eda/dotnet/shared/event_bus",
                StringValue = eventBus.EventBusName,
                DataType = ParameterDataType.TEXT,
                Type = ParameterType.STRING
            });
        }
    }
}

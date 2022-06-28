# Event Driven Archiecture with .NET API's

This sample project demonstrates how to build an event driven architecture on AWS using ECS Fargate, .NET 6 REST API's and AWS CoPilot. AWS Lambda requires a paradigm shift for developers, both in programming and deployment. Whilst Lambda is the recommended compute approach for event driven compute, it is still possible to build a serverless event driven application using REST API's.

## Architecture

![](./assets/architecture.png)

1. A request is received to a public facing application load balancer. This request is routed to a task running on ECS Fargate.
2. The receiver process the request and publishes an order-created event to ECS Fargate.
3. An event bridge rule is deployed by the consumer that routes the event to an SQS queue. SQS is used here to provide backpressure if an unexpectedly high load of events come into the consumer
4. An AWS Lambda functions processes the inbound requests from the queue. A Route53 private hosted zone is used to provide DNS resolution to the internal service
5. The Lambda function POST's the event to the receiving API running on ECS Fargate.

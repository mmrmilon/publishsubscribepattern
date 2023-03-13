# Publish and Subscribe Pattern
The Publish-Subscribe (Pub/Sub) pattern is a messaging pattern that allows different components or services of a system to communicate with each other without being directly
aware of each other's existence. In this pattern, a message broker acts as an intermediary between publishers (senders) and subscribers (receivers) of messages.

The Pub/Sub pattern is commonly used in distributed systems, event-driven architectures, and microservices-based architectures. Some popular message brokers that implement 
the Pub/Sub pattern include Apache Kafka, RabbitMQ, and Google Cloud Pub/Sub.

# Agenda
Implement the concept of Publish/Subscribe pattern. It should:-
 > - Take an input of data
 > - Transform that data in some way
 > - Transport that data to a set of subscribers
 > - The subscribers should be able to display the transformed data

# Solutioning
To implement the Pub/Sub pattern based on our agenda, we will use RabbitMQ as a message broker. The .Net Core Web API will act as the publisher, and we will have at least two console applications serving as subscribers. As an addition to our implementation of the Pub/Sub pattern using RabbitMQ as the message broker, let me provide a part of real scenario from the current application I'm working on. For this scenario, we have three applications:

  > - ResourceEngagement(publisher) - manages resource engagement information and publishes it to the subscribers
  > - IdentityGovernance(subscriber) - consumes the published message and creates a resource profile in the organization's active directory 
  > - UserTimewrite(subscriber) - consumes the message to store engagement details and verify the resources' input hours.

Please find below the process diagram illustrating how pubsub is working:
![alt tag](https://github.com/mmrmilon/publishsubscribepattern/blob/main/images/messaging_broker.jpg)

# Running the Application
Since we have already implemented the publisher and subscribers applications based on our agenda and scenarios in this repository, let's now look at how we can run them on our local machine.
 - **Running RabbitMQ in Docker** - Assuming that we have already set up Docker on our machine if not then find instructions from [**here**](https://www.docker.com/get-started/)
    > - To spin up RabbitMQ server open command-line and use the <code>docker run</code> command as below
    >
    >   <code>docker run -d --hostname pubsub-rabbitmq-server --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management</code>
    >
    > - We are using the <code>rabbitmq:3-management</code> image from DockerHub which will provide us a UI available on port **15672** to view our queues/messages.
    > - In order to access the management UI, we need to open a browser and navigate to <code>localhost:15672</code>, using the default login of **guest/guest**.
 
 - **Running Application** - Ensuring we have both applications set to run on startup befor running applications.
    > - Simply run by clicking on **Start** in visula studio, it will open a console two windows (Subscribers) and a browser with the Swagger UI (Publisher). 
    > - I'm using **Postman** here as publisher instead of using browser swagger.

# Simulation Result
  > **Step-01** - RabbitMQ management login UI
  >  ![alt tag](https://github.com/mmrmilon/publishsubscribepattern/blob/main/images/rabbitmq_management_ui_login.png)
  > **Step-02** - RabbitMQ management dashboaed UI
  >  ![alt tag](https://github.com/mmrmilon/publishsubscribepattern/blob/main/images/rabbitmq_management_ui.png)
  > **Step-03** - Publisher swagger UI
  >  ![alt tag](https://github.com/mmrmilon/publishsubscribepattern/blob/main/images/publisher_swagger_ui.png)
  > **Step-04** - Subscribers and Publisher **(Postman)**
  >  ![alt tag](https://github.com/mmrmilon/publishsubscribepattern/blob/main/images/publisher_subscriber_running_01.png)
  > **Step-05** - Create a engagement and published 
  >  ![alt tag](https://github.com/mmrmilon/publishsubscribepattern/blob/main/images/publisher_subscriber_running_02.png)
  > **Step-06** - Create another engagement and published 
  >  ![alt tag](https://github.com/mmrmilon/publishsubscribepattern/blob/main/images/publisher_subscriber_running_03.png)
  > **Step-07** - RabbitMQ queue dashboard
  >  ![alt tag](https://github.com/mmrmilon/publishsubscribepattern/blob/main/images/rabbitmq_management_ui_01.png)
  > **Step-08** - RabbitMQ dashboard summary
  >  ![alt tag](https://github.com/mmrmilon/publishsubscribepattern/blob/main/images/rabbitmq_management_ui_01.png)

# Routing exercise Answers



Complete this document with your answers.



----

### Candidate Name: Ousmane MBALLO

### Date: 04/24/2025

-----


## A. Implement SubscriptionIndex

- A2.  Write the names of the tests you added here:
  - `ShouldExcludeSubscriptionWithContentPatern(string contentPattern)`
  - `ShouldIncludeSubscriptionWithContentPatern(string contentPattern)`
  - `ShouldRemoveListOfSubscriptions()`
  - `ShouldRemoveSubscriptionByConsumer()`

- A3.  Briefly document your approach here (max: 500 words)
  
  To improve `MessageRouter.GetConsumers` performance, I have try to analyse the method structur at first, to see if we can improve his implementation and there others methods wich it depend (`_subscriptionIndex.FindSubscriptions`). I also needed a tool to mesure the performance of `GetConsumers` method before and after improvment.
    1. I have used the **BenchmarkDotNet** library wich was already referenced in the `AbcArbitrageHomeworkBenchmarks` project. I have setuped a Benchmark config to be able to benchmark the `GetConsumers` method on different dotnet platform (`net8.0 and net9.0`) to get a mesurable values of performance.
    2. I have launch the `AbcArbitrageHomeworkBenchmarks` to bench the `GetConsumers` method before improvment. As we can see the result on the screen short, before the improvment `GetConsumers` method execution take `59.09ms` on the `.NET8.0` platform and `54.62ms` on the `.NET9.0` platform.<br/>

    **Current GetConsumers method**:<br/>
    ```cs
    public IEnumerable<ClientId> GetConsumers(IMessage message)
    {
        var messageTypeId = MessageTypeId.FromMessage(message);
        var messageContent = MessageRoutingContent.FromMessage(message);

        foreach (var subscription in _subscriptionIndex.FindSubscriptions(messageTypeId, messageContent))
        {
            yield return subscription.ConsumerId;
        }
    }
    ```
    ![screen-short](./images/before-improvement.png)

    3. After improvment, we can see the time of the execution of the `GetConsumers` method is reduced to `4.36ms` on `.NET8.0` and on `.NET9.0` both methods have the same execution time.<br/>

    **GetConsumers method improved**:<br/>
    ```cs
    public IEnumerable<ClientId> GetConsumersImproved(IMessage message)
    {
        var messageTypeId = MessageTypeId.FromMessage(message);
        var messageContent = MessageRoutingContent.FromMessage(message);

        // Use a HashSet to avoid duplicate ConsumerIds
        var consumerIds = new HashSet<ClientId>();

        // replace yield return by Select methode to avoid a explicit loop for more readablitity
        return _subscriptionIndex.FindSubscriptions(messageTypeId, messageContent)
            .Where(subscription => consumerIds.Add(subscription.ConsumerId))
            .Select(s => s.ConsumerId);
    }
    ```
    ![screen-short](./images/after-improvement.png)

## C. Improve SubscriptionIndex performance (Bonus)

- C1. 
  - Did you find a solution where the benchmark executes in less than 10 microseconds?
    
  - If you did, briefly explain your approach (max: 500 words): 
    



------

### Candidate survey (optional)

The questions below are here to help us improve this homework.

1. How did you find this homework? (Easy, Intermediate, Hard)
   

2. How much time did you spend on each questions?
   - A
   - B
   - C

   

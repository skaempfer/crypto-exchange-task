# Crypto Exchange Task

## Task 1

### Problem Statement

Your task is to implement a meta exchange that always gives the user the best possible price if he is buying or selling a certain amount of BTC. Technically, you will be given n order books [from n different
cryptoexchanges], the type of order (buy or sell ), and the amount of BTC that our user wants to buy or sell. Your algorithm needs to output one or more buy or sell orders.

In effect, our user buys the specified amount of BTC for the lowest possible price or sells the specified amount of BTC for the highest possible price.

To make life a bit more complicated, each cryptoexchange has EUR and BTC balance. Your algorithm needs to achieve the best price within these constraints. The algorithm cannot transfer any money or crypto between cryptoexchanges , that means you can only sell what you have stored on that cryptoexchange account (EUR or BTC).

Together with this task, you will receive a bunch of JSON files with order books you can use to test your solution In each file, you will also find the given limit (EUR/BTC) of this cryptoexchange.

Your solution should be a relatively simple .NET Core console mode application, which reads the order books with limits order amounts and order type , and outputs a set of orders to execute against the given order books (exchanges).

### Solution

## Task 2

### Problem Statement

Implement a Web service (Kestrel, .NET Core API), and expose the implemented functionality through it. Implement an endpoint that will receive the required parameters ( order amount, order type and return a JSON response with the "best execution" plan.

### Solution
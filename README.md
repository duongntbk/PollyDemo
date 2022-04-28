This repository holds the sample code for my blog post about the Polly library.

[https://duongnt.com/polly-retry-request](https://duongnt.com/polly-retry-request)

# Usage

## Start the test API

Enter the `DummyApi` folder and run the following command to start the test API.
```
dotnet run
```

## Run Polly policies demo

Enter the `DemoProgram` folder and run the following command to start the demo program.
```
dotnet run
```

You should then see the following result.
```
Retry503: Retry number 1 after 503ms. Original status code: 503
be21236d-6eb7-4a58-8208-56ac1dee4207 did not throw an error.
RetryError: Retry number 1 after 500ms. Original status code: ServiceUnavailable
be21236d-6eb7-4a58-8208-56ac1dee4207 did not throw an error.
RetryDelay: Retry number 1 after 500ms because The delegate executed asynchronously through TimeoutPolicy did not complete within the timeout.
be21236d-6eb7-4a58-8208-56ac1dee4207 was not delayed.
```

## Run custom policies demo

Enter the `DemoCustomPolicy` folder and run the following command to start the demo program.
```
dotnet run
```

You should then see the following result (the exact percentage might differ).
```
######################################
Generating numbers without policy...
######################################
0: 0.5%
1: 0.5%
2: 0.8%
3: 0.4%
// omitted
126: 0.8999999999999999%
127: 0.8%
######################################
Generating numbers with policy...
######################################
91: 10.7%
92: 8.9%
93: 10%
94: 9.1%
95: 9.8%
96: 10.2%
97: 9.6%
98: 10%
99: 11.600000000000001%
100: 10.100000000000001%
######################################
Starting long running task...
######################################
Timeout: Task did not complete within: 500ms
```

# License

https://opensource.org/licenses/MIT

This repository holds the sample code for my blog post about the Polly library.

[https://duongnt.com/polly-retry-request](https://duongnt.com/polly-retry-request)

# Usage

Enter the `DummyApi` and run the following command to start the test API.
```
dotnet run
```

Enter the `DemoProgram` and run the following command to start the demo program.
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

# License

https://opensource.org/licenses/MIT
